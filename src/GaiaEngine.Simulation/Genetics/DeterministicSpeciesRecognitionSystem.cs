using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Foundation.Determinism;

namespace GaiaEngine.Simulation.Genetics;

/// <summary>
/// Recognizes new deterministic species classifications from the current population state.
/// </summary>
public sealed class DeterministicSpeciesRecognitionSystem : ISpeciesRecognitionSystem
{
    private static readonly DevelopmentConditions NeutralDevelopmentConditions = new(18, 500, 500, 0, "Spring");
    private readonly SpeciesRecognitionSettings settings;
    private readonly IEntityIdGenerator idGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicSpeciesRecognitionSystem"/> class.
    /// </summary>
    /// <param name="settings">The immutable recognition settings.</param>
    /// <param name="idGenerator">The deterministic identifier generator.</param>
    public DeterministicSpeciesRecognitionSystem(SpeciesRecognitionSettings settings, IEntityIdGenerator idGenerator)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    }

    /// <inheritdoc />
    public SpeciesRecognitionResult Update(GaiaEngine.Domain.World.World world, OrganismCollection organisms, GenomeCollection genomes, SpeciesCollection species)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(organisms);
        ArgumentNullException.ThrowIfNull(genomes);
        ArgumentNullException.ThrowIfNull(species);

        if (world.TimeState.CurrentTick % settings.EvaluationFrequency != 0)
        {
            return new SpeciesRecognitionResult(organisms, species);
        }

        List<Organism> updatedOrganisms = new(organisms.GetAll());
        List<Species> updatedSpecies = new(species.GetAll());
        ulong nextSpeciesSequence = ResolveNextSpeciesSequence(species);

        for (int index = 0; index < updatedOrganisms.Count; index++)
        {
            Organism organism = updatedOrganisms[index];
            if (!IsEligible(organism))
            {
                continue;
            }

            if (!species.TryGet(organism.SpeciesId, out Species? currentSpecies))
            {
                throw new InvalidOperationException($"The organism '{organism.Id}' references a missing species.");
            }

            if (!genomes.TryGet(organism.GenomeId, out Genome? candidateGenome))
            {
                throw new InvalidOperationException($"The organism '{organism.Id}' references a missing genome.");
            }

            if (!TryResolveReferenceGenome(currentSpecies!, candidateGenome!, organisms, genomes, out Genome? referenceGenome))
            {
                continue;
            }

            SpeciesMetrics metrics = EvaluateMetrics(candidateGenome!, referenceGenome!);
            int failedMetricCount = CountFailedMetrics(metrics);
            if (metrics.ReproductiveCompatibility >= settings.MinimumReproductiveCompatibility
                || failedMetricCount < settings.RequiredFailedMetricCount)
            {
                continue;
            }

            SpeciesId newSpeciesId = idGenerator.CreateSpeciesId(
                new IdentifierGenerationContext(
                    world.Metadata.Seed,
                    world.TimeState.CurrentTick,
                    new EntitySequence(nextSpeciesSequence)));
            nextSpeciesSequence++;

            updatedOrganisms[index] = ReclassifyOrganism(organism, newSpeciesId);
            updatedSpecies.Add(new Species(newSpeciesId, organism.SpeciesId, world.TimeState.CurrentTick, null, new[] { organism.Id }));
        }

        return new SpeciesRecognitionResult(new OrganismCollection(updatedOrganisms.AsReadOnly()), new SpeciesCollection(updatedSpecies.AsReadOnly()));
    }

    private static bool IsEligible(Organism organism)
    {
        return organism.Lifecycle.IsAlive
            && (organism.Lifecycle.Stage == LifecycleStage.Adult || organism.Lifecycle.Stage == LifecycleStage.Elder);
    }

    private static ulong ResolveNextSpeciesSequence(SpeciesCollection species)
    {
        ulong highestSequence = 0;
        foreach (Species entry in species.GetAll())
        {
            if (entry.Id.Sequence.Value > highestSequence)
            {
                highestSequence = entry.Id.Sequence.Value;
            }
        }

        return highestSequence + 1;
    }

    private static bool TryResolveReferenceGenome(Species species, Genome candidateGenome, OrganismCollection organisms, GenomeCollection genomes, out Genome? referenceGenome)
    {
        referenceGenome = null;
        int bestSimilarity = -1;
        foreach (OrganismId founderId in species.GetFounderPopulation())
        {
            if (!organisms.TryGet(founderId, out Organism? founderOrganism))
            {
                continue;
            }

            if (!genomes.TryGet(founderOrganism!.GenomeId, out Genome? founderGenome))
            {
                continue;
            }

            int similarity = ComputeGenomeSimilarity(candidateGenome, founderGenome!);
            if (similarity > bestSimilarity)
            {
                bestSimilarity = similarity;
                referenceGenome = founderGenome;
            }
        }

        return referenceGenome is not null;
    }

    private SpeciesMetrics EvaluateMetrics(Genome candidateGenome, Genome referenceGenome)
    {
        TraitProfile candidateTraits = EvaluateTraits(candidateGenome);
        TraitProfile referenceTraits = EvaluateTraits(referenceGenome);
        BodyPlan candidateBodyPlan = GenerateNeutralBodyPlan(candidateTraits);
        BodyPlan referenceBodyPlan = GenerateNeutralBodyPlan(referenceTraits);

        return new SpeciesMetrics(
            ComputeGenomeSimilarity(candidateGenome, referenceGenome),
            ComputeTraitSimilarity(candidateTraits, referenceTraits),
            ComputeMorphologySimilarity(candidateBodyPlan, referenceBodyPlan),
            ComputeReproductiveCompatibility(candidateGenome.Reproduction, referenceGenome.Reproduction));
    }

    private int CountFailedMetrics(SpeciesMetrics metrics)
    {
        int failed = 0;
        if (metrics.GenomeSimilarity < settings.MinimumGenomeSimilarity)
        {
            failed++;
        }

        if (metrics.TraitSimilarity < settings.MinimumTraitSimilarity)
        {
            failed++;
        }

        if (metrics.MorphologySimilarity < settings.MinimumMorphologySimilarity)
        {
            failed++;
        }

        if (metrics.ReproductiveCompatibility < settings.MinimumReproductiveCompatibility)
        {
            failed++;
        }

        return failed;
    }

    private static Organism ReclassifyOrganism(Organism organism, SpeciesId newSpeciesId)
    {
        return new Organism(
            organism.Id,
            newSpeciesId,
            organism.GenomeId,
            organism.CurrentChunkId,
            organism.Physiology,
            organism.Needs,
            organism.Lifecycle,
            organism.Health);
    }

    private static int ComputeGenomeSimilarity(Genome left, Genome right)
    {
        List<int> scores = new(32);
        AddGeneScores(left.Morphology, right.Morphology, scores);
        AddGeneScores(left.Physiology, right.Physiology, scores);
        AddGeneScores(left.Reproduction, right.Reproduction, scores);
        AddGeneScores(left.Senses, right.Senses, scores);
        AddGeneScores(left.Adaptation, right.Adaptation, scores);
        AddGeneScores(left.Appearance, right.Appearance, scores);
        AddGeneScores(left.BehaviourBias, right.BehaviourBias, scores);
        return Average(scores);
    }

    private static void AddGeneScores(GenomeGeneGroup left, GenomeGeneGroup right, List<int> scores)
    {
        foreach (GenomeGene gene in left.GetGenes())
        {
            if (!right.TryGetGene(gene.Key, out GenomeGene? otherGene))
            {
                scores.Add(0);
                continue;
            }

            scores.Add(ComputeGeneScore(gene, otherGene!));
        }
    }

    private static int ComputeGeneScore(GenomeGene left, GenomeGene right)
    {
        int valueSimilarity = 1000 - Math.Abs(left.Value.ScaledValue - right.Value.ScaledValue);
        int dominanceSimilarity = left.Dominance == right.Dominance ? 1000 : 0;
        int activationSimilarity = left.IsActive == right.IsActive ? 1000 : 0;
        return ((valueSimilarity * 7) + (dominanceSimilarity * 2) + activationSimilarity) / 10;
    }

    private static TraitProfile EvaluateTraits(Genome genome)
    {
        List<ExpressedTrait> traits =
        [
            CreateTrait(TraitKey.BodySize, Compose(genome.Morphology, GenomeGeneKey.BodySize)),
            CreateTrait(TraitKey.SkeletalStrength, Compose(genome.Morphology, GenomeGeneKey.SkeletalDensity)),
            CreateTrait(TraitKey.LimbReach, Average(
                Compose(genome.Morphology, GenomeGeneKey.LimbCount),
                Compose(genome.Morphology, GenomeGeneKey.NeckLength),
                Compose(genome.Morphology, GenomeGeneKey.TailLength))),
            CreateTrait(TraitKey.LocomotionStrength, Average(
                Compose(genome.Morphology, GenomeGeneKey.MuscleDistribution),
                Compose(genome.Morphology, GenomeGeneKey.LimbCount),
                Compose(genome.Adaptation, GenomeGeneKey.MountainAdaptation))),
            CreateTrait(TraitKey.ThermalCovering, Average(
                Compose(genome.Appearance, GenomeGeneKey.FurDensity),
                Compose(genome.Appearance, GenomeGeneKey.ScaleDensity),
                Compose(genome.Adaptation, GenomeGeneKey.ColdAdaptation))),
            CreateTrait(TraitKey.Pigmentation, Average(
                Compose(genome.Appearance, GenomeGeneKey.PrimaryColor),
                Compose(genome.Appearance, GenomeGeneKey.SecondaryColor),
                Compose(genome.Appearance, GenomeGeneKey.Pattern))),
            CreateTrait(TraitKey.SensoryAcuity, Average(
                Compose(genome.Senses, GenomeGeneKey.VisionRange),
                Compose(genome.Senses, GenomeGeneKey.HearingRange),
                Compose(genome.Senses, GenomeGeneKey.SmellSensitivity),
                Compose(genome.Senses, GenomeGeneKey.ThreatDetection))),
            CreateTrait(TraitKey.AquaticLocomotion, Average(
                Compose(genome.Adaptation, GenomeGeneKey.AquaticAffinity),
                Compose(genome.Adaptation, GenomeGeneKey.WetlandAdaptation))),
            CreateTrait(TraitKey.Metabolism, Compose(genome.Physiology, GenomeGeneKey.Metabolism)),
            CreateTrait(TraitKey.Growth, Compose(genome.Physiology, GenomeGeneKey.GrowthRate)),
            CreateTrait(TraitKey.Lifespan, Compose(genome.Physiology, GenomeGeneKey.Lifespan)),
            CreateTrait(TraitKey.HeatTolerance, Average(
                Compose(genome.Physiology, GenomeGeneKey.HeatResistance),
                Compose(genome.Adaptation, GenomeGeneKey.DesertAdaptation))),
            CreateTrait(TraitKey.ColdTolerance, Average(
                Compose(genome.Physiology, GenomeGeneKey.ColdResistance),
                Compose(genome.Adaptation, GenomeGeneKey.ColdAdaptation))),
            CreateTrait(TraitKey.HydrationEfficiency, Compose(genome.Physiology, GenomeGeneKey.WaterEfficiency)),
            CreateTrait(TraitKey.DigestiveEfficiency, Compose(genome.Physiology, GenomeGeneKey.DigestionEfficiency)),
            CreateTrait(TraitKey.Fertility, Compose(genome.Reproduction, GenomeGeneKey.Fertility)),
            CreateTrait(TraitKey.Maturity, Compose(genome.Reproduction, GenomeGeneKey.MaturityAge)),
        ];

        return new TraitProfile(traits);
    }

    private static int ComputeTraitSimilarity(TraitProfile left, TraitProfile right)
    {
        List<int> scores = new(left.GetAll().Count);
        foreach (ExpressedTrait trait in left.GetAll())
        {
            scores.Add(1000 - Math.Abs(trait.Value.ScaledValue - right.GetValue(trait.Key)));
        }

        return Average(scores);
    }

    private static BodyPlan GenerateNeutralBodyPlan(TraitProfile traits)
    {
        int bodySize = traits.GetValue(TraitKey.BodySize);
        int skeletalDensity = traits.GetValue(TraitKey.SkeletalStrength);
        int limbReach = traits.GetValue(TraitKey.LimbReach);
        int locomotionStrength = traits.GetValue(TraitKey.LocomotionStrength);
        int thermalCovering = traits.GetValue(TraitKey.ThermalCovering);
        int pigmentation = traits.GetValue(TraitKey.Pigmentation);
        int sensoryAcuity = traits.GetValue(TraitKey.SensoryAcuity);
        int aquaticAffinity = traits.GetValue(TraitKey.AquaticLocomotion);
        int coldAdaptation = traits.GetValue(TraitKey.ColdTolerance);
        int desertAdaptation = traits.GetValue(TraitKey.HeatTolerance);

        int foodModifier = (NeutralDevelopmentConditions.FoodAvailability - 500) / 4;
        int humidityModifier = (NeutralDevelopmentConditions.Humidity - 500) / 5;
        int altitudeModifier = Math.Clamp(NeutralDevelopmentConditions.Altitude / 8, -250, 250);
        int seasonModifier = 80;
        int thermalModifier = Math.Clamp((20 - NeutralDevelopmentConditions.AverageTemperature) * 8, -240, 240);
        int developmentModifier = Math.Clamp(foodModifier + humidityModifier - altitudeModifier + seasonModifier + thermalModifier, -1000, 1000);

        return new BodyPlan(
            proportions: ClampNormalized((bodySize + limbReach + foodModifier) / 2),
            mass: ClampNormalized((bodySize + skeletalDensity + NeutralDevelopmentConditions.FoodAvailability + (developmentModifier / 2)) / 3),
            limbConfiguration: ClampNormalized((limbReach + locomotionStrength + aquaticAffinity) / 3),
            bodyCovering: ClampNormalized((thermalCovering + coldAdaptation + (1000 - desertAdaptation) + Math.Max(0, thermalModifier)) / 4),
            coloration: ClampNormalized((pigmentation + NeutralDevelopmentConditions.Humidity) / 2),
            sensoryStructures: ClampNormalized(sensoryAcuity),
            locomotionProfile: ClampNormalized((locomotionStrength + limbReach + aquaticAffinity) / 3),
            developmentModifier: developmentModifier);
    }

    private static int ComputeMorphologySimilarity(BodyPlan left, BodyPlan right)
    {
        return Average(
            1000 - Math.Abs(left.Proportions - right.Proportions),
            1000 - Math.Abs(left.Mass - right.Mass),
            1000 - Math.Abs(left.LimbConfiguration - right.LimbConfiguration),
            1000 - Math.Abs(left.BodyCovering - right.BodyCovering),
            1000 - Math.Abs(left.Coloration - right.Coloration),
            1000 - Math.Abs(left.SensoryStructures - right.SensoryStructures),
            1000 - Math.Abs(left.LocomotionProfile - right.LocomotionProfile));
    }

    private static int ComputeReproductiveCompatibility(GenomeGeneGroup left, GenomeGeneGroup right)
    {
        List<int> scores = new(left.Count);
        AddGeneScores(left, right, scores);
        return Average(scores);
    }

    private static ExpressedTrait CreateTrait(TraitKey key, int value)
    {
        return new ExpressedTrait(key, new NormalizedGeneValue(Math.Clamp(value, 0, 1000)));
    }

    private static int Compose(GenomeGeneGroup group, GenomeGeneKey key)
    {
        if (!group.TryGetGene(key, out GenomeGene? gene))
        {
            throw new InvalidOperationException($"The gene '{key}' is required for trait evaluation.");
        }

        if (!gene!.IsActive)
        {
            return 0;
        }

        int dominanceOffset = gene.Dominance switch
        {
            GeneDominance.Dominant => 60,
            GeneDominance.Recessive => -60,
            GeneDominance.CoDominant => 20,
            GeneDominance.Blended => 0,
            _ => 0,
        };

        return Math.Clamp(gene.Value.ScaledValue + dominanceOffset, 0, 1000);
    }

    private static int Average(params int[] values)
    {
        int total = 0;
        foreach (int value in values)
        {
            total += value;
        }

        return values.Length == 0 ? 0 : total / values.Length;
    }

    private static int Average(IReadOnlyList<int> values)
    {
        int total = 0;
        for (int index = 0; index < values.Count; index++)
        {
            total += values[index];
        }

        return values.Count == 0 ? 0 : total / values.Count;
    }

    private static int ClampNormalized(int value)
    {
        return Math.Clamp(value, 0, 1000);
    }

    private sealed record SpeciesMetrics(int GenomeSimilarity, int TraitSimilarity, int MorphologySimilarity, int ReproductiveCompatibility);
}

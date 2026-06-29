using System;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Determinism;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Creates the initial deterministic genome set for a freshly bootstrapped world.
/// </summary>
public sealed class DeterministicGenomeBootstrapFactory : IGenomeBootstrapFactory
{
    private readonly IEntityIdGenerator idGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicGenomeBootstrapFactory"/> class.
    /// </summary>
    /// <param name="idGenerator">The deterministic identifier generator.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="idGenerator"/> is <see langword="null"/>.</exception>
    public DeterministicGenomeBootstrapFactory(IEntityIdGenerator idGenerator)
    {
        this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    }

    /// <summary>
    /// Creates one deterministic genome for the supplied bootstrap context.
    /// </summary>
    /// <param name="worldSeed">The owning world seed.</param>
    /// <param name="chunk">The organism spawn chunk.</param>
    /// <param name="index">The deterministic bootstrap organism index.</param>
    /// <returns>The created immutable genome.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="chunk"/> is <see langword="null"/>.</exception>
    public Genome CreateGenome(WorldSeed worldSeed, Chunk chunk, int index)
    {
        ArgumentNullException.ThrowIfNull(chunk);
        if (index <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "The bootstrap index must be greater than zero.");
        }

        EntitySequence genomeSequence = new((ulong)(2000 + index));
        GenomeId genomeId = idGenerator.CreateGenomeId(new IdentifierGenerationContext(worldSeed, 0, genomeSequence));
        GenomeIdentity identity = new(genomeId, version: 1, parentGenomeA: null, parentGenomeB: null, mutationCount: 0, generation: 0);

        return new Genome(
            identity,
            CreateMorphologyGroup(chunk, index),
            CreatePhysiologyGroup(chunk, index),
            CreateReproductionGroup(chunk, index),
            CreateSensesGroup(chunk, index),
            CreateAdaptationGroup(chunk, index),
            CreateAppearanceGroup(chunk, index),
            CreateBehaviourBiasGroup(chunk, index));
    }

    private static GenomeGeneGroup CreateMorphologyGroup(Chunk chunk, int index)
    {
        int vegetationDensity = chunk.Biome.VegetationProfile.Density;
        int elevationBias = Math.Clamp(Math.Abs(chunk.Terrain.Elevation.RelativeHeight) * 5, 0, 1000);
        return CreateGroup(
            GenomeGroupType.Morphology,
            CreateGene(GenomeGeneKey.BodySize, 380 + (vegetationDensity / 3), index),
            CreateGene(GenomeGeneKey.LimbCount, 500 + ((index * 11) % 180), index + 1),
            CreateGene(GenomeGeneKey.NeckLength, 250 + ((index * 17) % 250), index + 2),
            CreateGene(GenomeGeneKey.TailLength, 280 + ((index * 13) % 280), index + 3),
            CreateGene(GenomeGeneKey.BodyShape, 300 + ((chunk.Terrain.Slope.Gradient * 9) % 300), index + 4),
            CreateGene(GenomeGeneKey.SkeletalDensity, 350 + (elevationBias / 3), index + 5),
            CreateGene(GenomeGeneKey.MuscleDistribution, 320 + ((chunk.Terrain.Slope.TraversalCost * 2) % 360), index + 6));
    }

    private static GenomeGeneGroup CreatePhysiologyGroup(Chunk chunk, int index)
    {
        int averageTemperature = chunk.Biome.ClimateProfile.AverageTemperature + 50;
        int averageRainfall = Math.Clamp(chunk.Biome.ClimateProfile.AverageRainfall * 100, 0, 1000);
        int biomass = chunk.Biome.ResourceProfile.Biomass;
        int waterAvailability = chunk.Resources.GetAll()[1].Availability;
        return CreateGroup(
            GenomeGroupType.Physiology,
            CreateGene(GenomeGeneKey.Metabolism, 280 + (Math.Abs(chunk.Terrain.Elevation.RelativeHeight) * 6), index),
            CreateGene(GenomeGeneKey.GrowthRate, 250 + (biomass / 3), index + 1),
            CreateGene(GenomeGeneKey.Lifespan, 350 + (biomass / 4), index + 2),
            CreateGene(GenomeGeneKey.HeatResistance, averageTemperature * 10, index + 3),
            CreateGene(GenomeGeneKey.ColdResistance, 1000 - (averageTemperature * 10), index + 4),
            CreateGene(GenomeGeneKey.WaterEfficiency, 250 + (waterAvailability / 2), index + 5),
            CreateGene(GenomeGeneKey.DigestionEfficiency, 250 + (chunk.Resources.GetAll()[0].Quality * 7), index + 6));
    }

    private static GenomeGeneGroup CreateReproductionGroup(Chunk chunk, int index)
    {
        int humidity = chunk.Climate.Humidity.RelativeHumidity * 10;
        int vegetationDensity = chunk.Biome.VegetationProfile.Density * 10;
        return CreateGroup(
            GenomeGroupType.Reproduction,
            CreateGene(GenomeGeneKey.MaturityAge, 350 + ((index * 19) % 250), index),
            CreateGene(GenomeGeneKey.Fertility, 300 + (vegetationDensity / 5), index + 1),
            CreateGene(GenomeGeneKey.GestationTime, 400 + ((index * 23) % 220), index + 2),
            CreateGene(GenomeGeneKey.EggCount, 150 + ((humidity + (index * 31)) % 300), index + 3, isActive: index % 2 == 0),
            CreateGene(GenomeGeneKey.BreedingCooldown, 350 + ((index * 29) % 260), index + 4));
    }

    private static GenomeGeneGroup CreateSensesGroup(Chunk chunk, int index)
    {
        int windIntensity = chunk.Biome.ClimateProfile.WindIntensity * 100;
        int plantDiversity = chunk.Biome.SpeciesAffinity.PlantDiversity;
        return CreateGroup(
            GenomeGroupType.Senses,
            CreateGene(GenomeGeneKey.VisionRange, 320 + (plantDiversity * 4), index),
            CreateGene(GenomeGeneKey.NightVision, 180 + ((index * 37) % 360), index + 1),
            CreateGene(GenomeGeneKey.SmellSensitivity, 240 + ((chunk.Biome.ResourceProfile.Food / 4) % 420), index + 2),
            CreateGene(GenomeGeneKey.HearingRange, 260 + (windIntensity / 6), index + 3),
            CreateGene(GenomeGeneKey.ThreatDetection, 220 + ((chunk.Biome.SpeciesAffinity.CarnivoreAffinity * 8) % 420), index + 4));
    }

    private static GenomeGeneGroup CreateAdaptationGroup(Chunk chunk, int index)
    {
        int humidity = chunk.Climate.Humidity.RelativeHumidity * 10;
        int temperature = (chunk.Climate.Temperature.SeasonalAverage + 50) * 10;
        int elevation = Math.Clamp(chunk.Terrain.Elevation.Height * 5, 0, 1000);
        return CreateGroup(
            GenomeGroupType.Adaptation,
            CreateGene(GenomeGeneKey.DesertAdaptation, 1000 - humidity, index),
            CreateGene(GenomeGeneKey.ColdAdaptation, 1000 - temperature, index + 1),
            CreateGene(GenomeGeneKey.WetlandAdaptation, humidity, index + 2),
            CreateGene(GenomeGeneKey.MountainAdaptation, elevation, index + 3),
            CreateGene(GenomeGeneKey.AquaticAffinity, chunk.Biome.SpeciesAffinity.AquaticSuitability * 10, index + 4));
    }

    private static GenomeGeneGroup CreateAppearanceGroup(Chunk chunk, int index)
    {
        int humidity = chunk.Climate.Humidity.RelativeHumidity * 10;
        int vegetationDensity = chunk.Biome.VegetationProfile.Density * 10;
        return CreateGroup(
            GenomeGroupType.Appearance,
            CreateGene(GenomeGeneKey.PrimaryColor, 220 + ((chunk.Metadata.Coordinates.X * 97) % 500), index),
            CreateGene(GenomeGeneKey.SecondaryColor, 180 + ((chunk.Metadata.Coordinates.Y * 131) % 500), index + 1),
            CreateGene(GenomeGeneKey.Pattern, 150 + ((index * 41) % 420), index + 2),
            CreateGene(GenomeGeneKey.FurDensity, 1000 - (chunk.Climate.Temperature.CurrentTemperature + 50) * 10, index + 3),
            CreateGene(GenomeGeneKey.ScaleDensity, 120 + ((humidity + (index * 17)) % 380), index + 4),
            CreateGene(GenomeGeneKey.HornShape, 100 + ((chunk.Terrain.Slope.Gradient * 15) % 300), index + 5),
            CreateGene(GenomeGeneKey.EarShape, 180 + ((vegetationDensity + (index * 13)) % 420), index + 6));
    }

    private static GenomeGeneGroup CreateBehaviourBiasGroup(Chunk chunk, int index)
    {
        int foodAvailability = chunk.Biome.ResourceProfile.Food;
        int herbivoreAffinity = chunk.Biome.SpeciesAffinity.HerbivoreAffinity;
        return CreateGroup(
            GenomeGroupType.BehaviourBias,
            CreateGene(GenomeGeneKey.Curiosity, 180 + ((index * 43) % 400), index),
            CreateGene(GenomeGeneKey.AggressionBias, 120 + ((chunk.Biome.SpeciesAffinity.CarnivoreAffinity * 8) % 420), index + 1),
            CreateGene(GenomeGeneKey.SocialBias, 180 + ((herbivoreAffinity * 6) % 420), index + 2),
            CreateGene(GenomeGeneKey.RiskTolerance, 140 + ((chunk.Terrain.Slope.TraversalCost * 3) % 360), index + 3),
            CreateGene(GenomeGeneKey.ExplorationBias, 200 + ((foodAvailability / 4) % 420), index + 4));
    }

    private static GenomeGeneGroup CreateGroup(GenomeGroupType groupType, params GenomeGene[] genes)
    {
        return new GenomeGeneGroup(groupType, genes);
    }

    private static GenomeGene CreateGene(GenomeGeneKey key, int scaledValue, int seedOffset, bool isActive = true)
    {
        return new GenomeGene(
            key,
            new NormalizedGeneValue(Math.Clamp(scaledValue, 0, 1000)),
            ResolveDominance(seedOffset),
            isActive);
    }

    private static GeneDominance ResolveDominance(int seedOffset)
    {
        return Math.Abs(seedOffset % 4) switch
        {
            0 => GeneDominance.Dominant,
            1 => GeneDominance.Recessive,
            2 => GeneDominance.CoDominant,
            _ => GeneDominance.Blended,
        };
    }
}

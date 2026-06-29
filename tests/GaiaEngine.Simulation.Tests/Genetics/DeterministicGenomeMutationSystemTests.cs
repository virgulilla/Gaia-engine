using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Simulation.Genetics;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Genetics;

public sealed class DeterministicGenomeMutationSystemTests
{
    [Fact]
    public void Mutate_ShouldProduceDeterministicGenomeForIdenticalInputs()
    {
        DeterministicGenomeMutationSystem system = new(CreateSettings());
        Genome genome = CreateGenome();
        WorldSeed seed = new(42);

        Genome first = system.Mutate(genome, seed);
        Genome second = system.Mutate(genome, seed);

        Assert.Equal(first.Identity.MutationCount, second.Identity.MutationCount);
        Assert.Equal(first.MutationVersion, second.MutationVersion);
        Assert.Equal(first.MutationHistory.Count, second.MutationHistory.Count);

        Assert.True(first.Morphology.TryGetGene(GenomeGeneKey.BodySize, out GenomeGene? firstBodySize));
        Assert.True(second.Morphology.TryGetGene(GenomeGeneKey.BodySize, out GenomeGene? secondBodySize));
        Assert.NotNull(firstBodySize);
        Assert.NotNull(secondBodySize);
        Assert.Equal(firstBodySize!.Value.ScaledValue, secondBodySize!.Value.ScaledValue);
        Assert.Equal(firstBodySize.Dominance, secondBodySize.Dominance);
        Assert.Equal(firstBodySize.IsActive, secondBodySize.IsActive);
    }

    [Fact]
    public void Mutate_ShouldRespectMaximumMutationCountAndStoreHistory()
    {
        DeterministicGenomeMutationSystem system = new(CreateSettings(maxMutationsPerGenome: 1));
        Genome genome = CreateGenome();

        Genome mutated = system.Mutate(genome, new WorldSeed(100));

        Assert.True(mutated.Identity.MutationCount <= 1);
        Assert.Equal(mutated.Identity.MutationCount, mutated.MutationHistory.Count);
        Assert.Equal(1, mutated.MutationVersion);
    }

    [Fact]
    public void Mutate_ShouldLeaveGenomeUnchangedWhenGlobalChanceIsZero()
    {
        DeterministicGenomeMutationSystem system = new(CreateSettings(globalMutationChance: 0));
        Genome genome = CreateGenome();

        Genome mutated = system.Mutate(genome, new WorldSeed(7));

        Assert.Equal(0, mutated.Identity.MutationCount);
        Assert.Equal(0, mutated.MutationHistory.Count);

        Assert.True(mutated.Morphology.TryGetGene(GenomeGeneKey.BodySize, out GenomeGene? bodySize));
        Assert.NotNull(bodySize);
        Assert.Equal(520, bodySize!.Value.ScaledValue);

        Assert.True(mutated.Appearance.TryGetGene(GenomeGeneKey.Pattern, out GenomeGene? pattern));
        Assert.NotNull(pattern);
        Assert.False(pattern!.IsActive);
        Assert.Equal(640, pattern.Value.ScaledValue);
    }

    private static GenomeMutationSettings CreateSettings(int globalMutationChance = 1000, int maxMutationsPerGenome = 3)
    {
        return new GenomeMutationSettings(
            globalMutationChance,
            mutationStrength: 180,
            maxMutationsPerGenome,
            parameterMutationWeight: 1000,
            dominanceMutationWeight: 1000,
            activationMutationWeight: 1000,
            structuralMutationWeight: 1000,
            morphologyGroupWeight: 1000,
            physiologyGroupWeight: 1000,
            reproductionGroupWeight: 1000,
            sensesGroupWeight: 1000,
            adaptationGroupWeight: 1000,
            appearanceGroupWeight: 1000,
            behaviourBiasGroupWeight: 1000,
            mutationVersion: 1);
    }

    private static Genome CreateGenome()
    {
        return new Genome(
            new GenomeIdentity(GenomeId.FromSequence(new EntitySequence(500)), version: 1, GenomeId.FromSequence(new EntitySequence(10)), GenomeId.FromSequence(new EntitySequence(20)), mutationCount: 0, generation: 1),
            new GenomeGeneGroup(
                GenomeGroupType.Morphology,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.BodySize, new NormalizedGeneValue(520), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.LimbCount, new NormalizedGeneValue(460), GeneDominance.Recessive, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Physiology,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.Metabolism, new NormalizedGeneValue(430), GeneDominance.CoDominant, true),
                    new GenomeGene(GenomeGeneKey.DigestionEfficiency, new NormalizedGeneValue(450), GeneDominance.Blended, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Reproduction,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.Fertility, new NormalizedGeneValue(390), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.MaturityAge, new NormalizedGeneValue(410), GeneDominance.Recessive, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Senses,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.VisionRange, new NormalizedGeneValue(530), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.HearingRange, new NormalizedGeneValue(490), GeneDominance.Dominant, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Adaptation,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.DesertAdaptation, new NormalizedGeneValue(310), GeneDominance.Recessive, true),
                    new GenomeGene(GenomeGeneKey.ColdAdaptation, new NormalizedGeneValue(620), GeneDominance.Dominant, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Appearance,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.PrimaryColor, new NormalizedGeneValue(350), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.Pattern, new NormalizedGeneValue(640), GeneDominance.Blended, false),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.BehaviourBias,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.Curiosity, new NormalizedGeneValue(500), GeneDominance.Blended, true),
                    new GenomeGene(GenomeGeneKey.RiskTolerance, new NormalizedGeneValue(470), GeneDominance.CoDominant, true),
                }));
    }
}

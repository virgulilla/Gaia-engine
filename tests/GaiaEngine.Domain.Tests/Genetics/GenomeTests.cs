using System;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using Xunit;

namespace GaiaEngine.Domain.Tests.Genetics;

public sealed class GenomeTests
{
    [Fact]
    public void NormalizedGeneValue_ShouldRejectValuesOutsideSupportedRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new NormalizedGeneValue(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => new NormalizedGeneValue(1001));
    }

    [Fact]
    public void GenomeGeneGroup_ShouldRejectGenesAssignedToAnotherGroup()
    {
        GenomeGene gene = new(GenomeGeneKey.Metabolism, new NormalizedGeneValue(400), GeneDominance.Dominant, true);

        Assert.Throws<ArgumentException>(() => new GenomeGeneGroup(GenomeGroupType.Morphology, new[] { gene }));
    }

    [Fact]
    public void GenomeCollection_ShouldSortGenomesByIdentifier()
    {
        Genome later = CreateGenome(5);
        Genome earlier = CreateGenome(2);
        GenomeCollection collection = new(new[] { later, earlier });

        Assert.Equal(earlier.Id, collection.GetAll()[0].Id);
        Assert.Equal(later.Id, collection.GetAll()[1].Id);
    }

    private static Genome CreateGenome(ulong sequence)
    {
        GenomeGeneGroup morphology = new(
            GenomeGroupType.Morphology,
            new[] { new GenomeGene(GenomeGeneKey.BodySize, new NormalizedGeneValue(500), GeneDominance.Dominant, true) });
        GenomeGeneGroup physiology = new(
            GenomeGroupType.Physiology,
            new[] { new GenomeGene(GenomeGeneKey.Metabolism, new NormalizedGeneValue(500), GeneDominance.Dominant, true) });
        GenomeGeneGroup reproduction = new(
            GenomeGroupType.Reproduction,
            new[] { new GenomeGene(GenomeGeneKey.Fertility, new NormalizedGeneValue(500), GeneDominance.Dominant, true) });
        GenomeGeneGroup senses = new(
            GenomeGroupType.Senses,
            new[] { new GenomeGene(GenomeGeneKey.VisionRange, new NormalizedGeneValue(500), GeneDominance.Dominant, true) });
        GenomeGeneGroup adaptation = new(
            GenomeGroupType.Adaptation,
            new[] { new GenomeGene(GenomeGeneKey.DesertAdaptation, new NormalizedGeneValue(500), GeneDominance.Dominant, true) });
        GenomeGeneGroup appearance = new(
            GenomeGroupType.Appearance,
            new[] { new GenomeGene(GenomeGeneKey.PrimaryColor, new NormalizedGeneValue(500), GeneDominance.Dominant, true) });
        GenomeGeneGroup behaviourBias = new(
            GenomeGroupType.BehaviourBias,
            new[] { new GenomeGene(GenomeGeneKey.Curiosity, new NormalizedGeneValue(500), GeneDominance.Dominant, true) });

        return new Genome(
            new GenomeIdentity(GenomeId.FromSequence(new EntitySequence(sequence)), 1, null, null, 0, 0),
            morphology,
            physiology,
            reproduction,
            senses,
            adaptation,
            appearance,
            behaviourBias);
    }
}

using System;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Simulation.Genetics;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Genetics;

public sealed class DeterministicGenomeInheritanceSystemTests
{
    [Fact]
    public void Inherit_ShouldCreateOffspringWithCanonicalParentReferences()
    {
        DeterministicGenomeInheritanceSystem system = new();
        Genome higherIdParent = CreateGenome(
            20,
            bodySize: new GenomeGene(GenomeGeneKey.BodySize, new NormalizedGeneValue(700), GeneDominance.Dominant, true),
            metabolism: new GenomeGene(GenomeGeneKey.Metabolism, new NormalizedGeneValue(420), GeneDominance.Dominant, true));
        Genome lowerIdParent = CreateGenome(
            10,
            bodySize: new GenomeGene(GenomeGeneKey.BodySize, new NormalizedGeneValue(300), GeneDominance.Recessive, true),
            metabolism: new GenomeGene(GenomeGeneKey.Metabolism, new NormalizedGeneValue(620), GeneDominance.CoDominant, true));

        Genome offspring = system.Inherit(higherIdParent, lowerIdParent, GenomeId.FromSequence(new EntitySequence(1000)));

        Assert.Equal(lowerIdParent.Id, offspring.Identity.ParentGenomeA);
        Assert.Equal(higherIdParent.Id, offspring.Identity.ParentGenomeB);
        Assert.Equal(1, offspring.Identity.Generation);
        Assert.Equal(0, offspring.Identity.MutationCount);
    }

    [Fact]
    public void Inherit_ShouldResolveDominantAndAverageRulesDeterministically()
    {
        DeterministicGenomeInheritanceSystem system = new();
        Genome parentA = CreateGenome(
            10,
            bodySize: new GenomeGene(GenomeGeneKey.BodySize, new NormalizedGeneValue(800), GeneDominance.Dominant, true),
            metabolism: new GenomeGene(GenomeGeneKey.Metabolism, new NormalizedGeneValue(400), GeneDominance.CoDominant, true));
        Genome parentB = CreateGenome(
            20,
            bodySize: new GenomeGene(GenomeGeneKey.BodySize, new NormalizedGeneValue(200), GeneDominance.Recessive, true),
            metabolism: new GenomeGene(GenomeGeneKey.Metabolism, new NormalizedGeneValue(600), GeneDominance.Dominant, true));

        Genome offspring = system.Inherit(parentA, parentB, GenomeId.FromSequence(new EntitySequence(1001)));

        Assert.True(offspring.Morphology.TryGetGene(GenomeGeneKey.BodySize, out GenomeGene? bodySizeGene));
        Assert.NotNull(bodySizeGene);
        Assert.Equal(800, bodySizeGene!.Value.ScaledValue);
        Assert.Equal(GeneDominance.Dominant, bodySizeGene.Dominance);

        Assert.True(offspring.Physiology.TryGetGene(GenomeGeneKey.Metabolism, out GenomeGene? metabolismGene));
        Assert.NotNull(metabolismGene);
        Assert.Equal(500, metabolismGene!.Value.ScaledValue);
        Assert.Equal(GeneDominance.CoDominant, metabolismGene.Dominance);
    }

    [Fact]
    public void Inherit_ShouldActivateGeneWhenOnlyOneParentIsActive()
    {
        DeterministicGenomeInheritanceSystem system = new();
        Genome parentA = CreateGenome(
            10,
            bodySize: new GenomeGene(GenomeGeneKey.BodySize, new NormalizedGeneValue(450), GeneDominance.Blended, false),
            metabolism: new GenomeGene(GenomeGeneKey.Metabolism, new NormalizedGeneValue(510), GeneDominance.Dominant, true));
        Genome parentB = CreateGenome(
            20,
            bodySize: new GenomeGene(GenomeGeneKey.BodySize, new NormalizedGeneValue(650), GeneDominance.Dominant, true),
            metabolism: new GenomeGene(GenomeGeneKey.Metabolism, new NormalizedGeneValue(490), GeneDominance.Dominant, true));

        Genome offspring = system.Inherit(parentA, parentB, GenomeId.FromSequence(new EntitySequence(1002)));

        Assert.True(offspring.Morphology.TryGetGene(GenomeGeneKey.BodySize, out GenomeGene? bodySizeGene));
        Assert.NotNull(bodySizeGene);
        Assert.True(bodySizeGene!.IsActive);
        Assert.Equal(650, bodySizeGene.Value.ScaledValue);
    }

    [Fact]
    public void Inherit_ShouldRejectParentsWithDifferentSchemaVersions()
    {
        DeterministicGenomeInheritanceSystem system = new();
        Genome parentA = CreateGenome(
            10,
            bodySize: new GenomeGene(GenomeGeneKey.BodySize, new NormalizedGeneValue(500), GeneDominance.Dominant, true),
            metabolism: new GenomeGene(GenomeGeneKey.Metabolism, new NormalizedGeneValue(500), GeneDominance.Dominant, true),
            version: 1);
        Genome parentB = CreateGenome(
            20,
            bodySize: new GenomeGene(GenomeGeneKey.BodySize, new NormalizedGeneValue(500), GeneDominance.Dominant, true),
            metabolism: new GenomeGene(GenomeGeneKey.Metabolism, new NormalizedGeneValue(500), GeneDominance.Dominant, true),
            version: 2);

        Assert.Throws<ArgumentException>(() => system.Inherit(parentA, parentB, GenomeId.FromSequence(new EntitySequence(1003))));
    }

    private static Genome CreateGenome(ulong sequence, GenomeGene bodySize, GenomeGene metabolism, int version = 1)
    {
        return new Genome(
            new GenomeIdentity(GenomeId.FromSequence(new EntitySequence(sequence)), version, null, null, 0, 0),
            new GenomeGeneGroup(GenomeGroupType.Morphology, new[] { bodySize }),
            new GenomeGeneGroup(GenomeGroupType.Physiology, new[] { metabolism }),
            new GenomeGeneGroup(GenomeGroupType.Reproduction, new[] { new GenomeGene(GenomeGeneKey.Fertility, new NormalizedGeneValue(500), GeneDominance.Dominant, true) }),
            new GenomeGeneGroup(GenomeGroupType.Senses, new[] { new GenomeGene(GenomeGeneKey.VisionRange, new NormalizedGeneValue(500), GeneDominance.Dominant, true) }),
            new GenomeGeneGroup(GenomeGroupType.Adaptation, new[] { new GenomeGene(GenomeGeneKey.DesertAdaptation, new NormalizedGeneValue(500), GeneDominance.Dominant, true) }),
            new GenomeGeneGroup(GenomeGroupType.Appearance, new[] { new GenomeGene(GenomeGeneKey.PrimaryColor, new NormalizedGeneValue(500), GeneDominance.Dominant, true) }),
            new GenomeGeneGroup(GenomeGroupType.BehaviourBias, new[] { new GenomeGene(GenomeGeneKey.Curiosity, new NormalizedGeneValue(500), GeneDominance.Dominant, true) }));
    }
}

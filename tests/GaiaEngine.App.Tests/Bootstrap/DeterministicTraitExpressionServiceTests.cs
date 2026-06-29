using GaiaEngine.App.Bootstrap;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;
using Xunit;

namespace GaiaEngine.App.Tests.Bootstrap;

public sealed class DeterministicTraitExpressionServiceTests
{
    [Fact]
    public void Evaluate_ShouldProduceDeterministicTraitProfile()
    {
        DeterministicTraitExpressionService service = new();
        Genome genome = CreateGenome();

        TraitProfile first = service.Evaluate(genome);
        TraitProfile second = service.Evaluate(genome);

        Assert.Equal(first.GetValue(TraitKey.BodySize), second.GetValue(TraitKey.BodySize));
        Assert.Equal(first.GetValue(TraitKey.SensoryAcuity), second.GetValue(TraitKey.SensoryAcuity));
        Assert.Equal(first.GetValue(TraitKey.Metabolism), second.GetValue(TraitKey.Metabolism));
    }

    [Fact]
    public void Evaluate_ShouldApplyDominanceAndActivityRules()
    {
        DeterministicTraitExpressionService service = new();
        Genome genome = CreateGenome();

        TraitProfile traits = service.Evaluate(genome);

        Assert.True(traits.GetValue(TraitKey.BodySize) > 500);
        Assert.True(traits.GetValue(TraitKey.ThermalCovering) < 500);
    }

    private static Genome CreateGenome()
    {
        return new Genome(
            new GenomeIdentity(GenomeId.FromSequence(new EntitySequence(300)), 1, null, null, 0, 0),
            new GenomeGeneGroup(
                GenomeGroupType.Morphology,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.BodySize, new NormalizedGeneValue(500), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.LimbCount, new NormalizedGeneValue(550), GeneDominance.CoDominant, true),
                    new GenomeGene(GenomeGeneKey.NeckLength, new NormalizedGeneValue(300), GeneDominance.Blended, true),
                    new GenomeGene(GenomeGeneKey.TailLength, new NormalizedGeneValue(280), GeneDominance.Blended, true),
                    new GenomeGene(GenomeGeneKey.SkeletalDensity, new NormalizedGeneValue(430), GeneDominance.Recessive, true),
                    new GenomeGene(GenomeGeneKey.MuscleDistribution, new NormalizedGeneValue(610), GeneDominance.Dominant, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Physiology,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.Metabolism, new NormalizedGeneValue(440), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.GrowthRate, new NormalizedGeneValue(450), GeneDominance.Blended, true),
                    new GenomeGene(GenomeGeneKey.Lifespan, new NormalizedGeneValue(620), GeneDominance.CoDominant, true),
                    new GenomeGene(GenomeGeneKey.HeatResistance, new NormalizedGeneValue(510), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.ColdResistance, new NormalizedGeneValue(480), GeneDominance.Blended, true),
                    new GenomeGene(GenomeGeneKey.WaterEfficiency, new NormalizedGeneValue(530), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.DigestionEfficiency, new NormalizedGeneValue(490), GeneDominance.Blended, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Reproduction,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.Fertility, new NormalizedGeneValue(380), GeneDominance.CoDominant, true),
                    new GenomeGene(GenomeGeneKey.MaturityAge, new NormalizedGeneValue(410), GeneDominance.Dominant, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Senses,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.VisionRange, new NormalizedGeneValue(520), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.HearingRange, new NormalizedGeneValue(510), GeneDominance.Blended, true),
                    new GenomeGene(GenomeGeneKey.SmellSensitivity, new NormalizedGeneValue(480), GeneDominance.Recessive, true),
                    new GenomeGene(GenomeGeneKey.ThreatDetection, new NormalizedGeneValue(560), GeneDominance.CoDominant, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Adaptation,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.MountainAdaptation, new NormalizedGeneValue(340), GeneDominance.Blended, true),
                    new GenomeGene(GenomeGeneKey.WetlandAdaptation, new NormalizedGeneValue(280), GeneDominance.Blended, true),
                    new GenomeGene(GenomeGeneKey.AquaticAffinity, new NormalizedGeneValue(240), GeneDominance.Recessive, true),
                    new GenomeGene(GenomeGeneKey.DesertAdaptation, new NormalizedGeneValue(320), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.ColdAdaptation, new NormalizedGeneValue(470), GeneDominance.Blended, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Appearance,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.FurDensity, new NormalizedGeneValue(420), GeneDominance.Recessive, true),
                    new GenomeGene(GenomeGeneKey.ScaleDensity, new NormalizedGeneValue(300), GeneDominance.Recessive, false),
                    new GenomeGene(GenomeGeneKey.PrimaryColor, new NormalizedGeneValue(350), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.SecondaryColor, new NormalizedGeneValue(610), GeneDominance.Blended, true),
                    new GenomeGene(GenomeGeneKey.Pattern, new NormalizedGeneValue(470), GeneDominance.CoDominant, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.BehaviourBias,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.Curiosity, new NormalizedGeneValue(510), GeneDominance.Blended, true),
                }));
    }
}

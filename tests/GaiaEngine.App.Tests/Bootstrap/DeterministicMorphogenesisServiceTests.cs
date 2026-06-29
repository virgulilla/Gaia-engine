using GaiaEngine.App.Bootstrap;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;
using Xunit;

namespace GaiaEngine.App.Tests.Bootstrap;

public sealed class DeterministicMorphogenesisServiceTests
{
    [Fact]
    public void Generate_ShouldProduceDeterministicResultsForIdenticalInputs()
    {
        DeterministicMorphogenesisService service = new();
        Genome genome = CreateGenome();
        DevelopmentConditions developmentConditions = new(averageTemperature: 18, foodAvailability: 760, humidity: 540, altitude: 140, season: "Spring");

        MorphogenesisResult first = service.Generate(genome, developmentConditions);
        MorphogenesisResult second = service.Generate(genome, developmentConditions);

        Assert.Equal(first, second);
        Assert.Equal(first.BodyPlan, second.BodyPlan);
        Assert.Equal(first.Physiology, second.Physiology);
    }

    [Fact]
    public void Generate_ShouldApplyDevelopmentConditionsWithoutMutatingGenome()
    {
        DeterministicMorphogenesisService service = new();
        Genome genome = CreateGenome();
        DevelopmentConditions scarce = new(averageTemperature: 5, foodAvailability: 220, humidity: 300, altitude: 220, season: "Winter");
        DevelopmentConditions abundant = new(averageTemperature: 20, foodAvailability: 880, humidity: 650, altitude: 50, season: "Spring");

        MorphogenesisResult scarceResult = service.Generate(genome, scarce);
        MorphogenesisResult abundantResult = service.Generate(genome, abundant);

        Assert.NotEqual(scarceResult.BodyPlan.Mass, abundantResult.BodyPlan.Mass);
        Assert.NotEqual(scarceResult.Physiology.GrowthRate, abundantResult.Physiology.GrowthRate);
        Assert.Equal(520, genome.Morphology.GetGenes()[0].Value.ScaledValue);
    }

    private static Genome CreateGenome()
    {
        return new Genome(
            new GenomeIdentity(GenomeId.FromSequence(new EntitySequence(200)), 1, null, null, 0, 0),
            new GenomeGeneGroup(
                GenomeGroupType.Morphology,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.BodySize, new NormalizedGeneValue(520), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.LimbCount, new NormalizedGeneValue(610), GeneDominance.Recessive, true),
                    new GenomeGene(GenomeGeneKey.BodyShape, new NormalizedGeneValue(480), GeneDominance.Blended, true),
                    new GenomeGene(GenomeGeneKey.SkeletalDensity, new NormalizedGeneValue(540), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.MuscleDistribution, new NormalizedGeneValue(570), GeneDominance.CoDominant, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Physiology,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.Metabolism, new NormalizedGeneValue(430), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.GrowthRate, new NormalizedGeneValue(470), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.Lifespan, new NormalizedGeneValue(620), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.HeatResistance, new NormalizedGeneValue(540), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.WaterEfficiency, new NormalizedGeneValue(510), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.DigestionEfficiency, new NormalizedGeneValue(450), GeneDominance.Dominant, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Reproduction,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.Fertility, new NormalizedGeneValue(390), GeneDominance.Blended, true),
                    new GenomeGene(GenomeGeneKey.MaturityAge, new NormalizedGeneValue(410), GeneDominance.Dominant, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Senses,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.VisionRange, new NormalizedGeneValue(530), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.HearingRange, new NormalizedGeneValue(490), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.SmellSensitivity, new NormalizedGeneValue(560), GeneDominance.Dominant, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Adaptation,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.AquaticAffinity, new NormalizedGeneValue(240), GeneDominance.Blended, true),
                    new GenomeGene(GenomeGeneKey.ColdAdaptation, new NormalizedGeneValue(620), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.DesertAdaptation, new NormalizedGeneValue(310), GeneDominance.Recessive, true),
                    new GenomeGene(GenomeGeneKey.MountainAdaptation, new NormalizedGeneValue(400), GeneDominance.Dominant, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.Appearance,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.FurDensity, new NormalizedGeneValue(580), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.PrimaryColor, new NormalizedGeneValue(350), GeneDominance.Dominant, true),
                    new GenomeGene(GenomeGeneKey.SecondaryColor, new NormalizedGeneValue(640), GeneDominance.Blended, true),
                }),
            new GenomeGeneGroup(
                GenomeGroupType.BehaviourBias,
                new[]
                {
                    new GenomeGene(GenomeGeneKey.Curiosity, new NormalizedGeneValue(500), GeneDominance.Blended, true),
                }));
    }
}

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
        TraitProfile traits = CreateTraits();
        DevelopmentConditions developmentConditions = new(averageTemperature: 18, foodAvailability: 760, humidity: 540, altitude: 140, season: "Spring");

        MorphogenesisResult first = service.Generate(traits, developmentConditions);
        MorphogenesisResult second = service.Generate(traits, developmentConditions);

        Assert.Equal(first, second);
        Assert.Equal(first.BodyPlan, second.BodyPlan);
        Assert.Equal(first.Physiology, second.Physiology);
    }

    [Fact]
    public void Generate_ShouldApplyDevelopmentConditionsWithoutMutatingGenome()
    {
        DeterministicMorphogenesisService service = new();
        TraitProfile traits = CreateTraits();
        DevelopmentConditions scarce = new(averageTemperature: 5, foodAvailability: 220, humidity: 300, altitude: 220, season: "Winter");
        DevelopmentConditions abundant = new(averageTemperature: 20, foodAvailability: 880, humidity: 650, altitude: 50, season: "Spring");

        MorphogenesisResult scarceResult = service.Generate(traits, scarce);
        MorphogenesisResult abundantResult = service.Generate(traits, abundant);

        Assert.NotEqual(scarceResult.BodyPlan.Mass, abundantResult.BodyPlan.Mass);
        Assert.NotEqual(scarceResult.Physiology.GrowthRate, abundantResult.Physiology.GrowthRate);
        Assert.Equal(520, traits.GetValue(TraitKey.BodySize));
    }

    private static TraitProfile CreateTraits()
    {
        return new TraitProfile(
            new[]
            {
                new ExpressedTrait(TraitKey.BodySize, new NormalizedGeneValue(520)),
                new ExpressedTrait(TraitKey.SkeletalStrength, new NormalizedGeneValue(540)),
                new ExpressedTrait(TraitKey.LimbReach, new NormalizedGeneValue(430)),
                new ExpressedTrait(TraitKey.LocomotionStrength, new NormalizedGeneValue(570)),
                new ExpressedTrait(TraitKey.ThermalCovering, new NormalizedGeneValue(580)),
                new ExpressedTrait(TraitKey.Pigmentation, new NormalizedGeneValue(490)),
                new ExpressedTrait(TraitKey.SensoryAcuity, new NormalizedGeneValue(530)),
                new ExpressedTrait(TraitKey.AquaticLocomotion, new NormalizedGeneValue(240)),
                new ExpressedTrait(TraitKey.Metabolism, new NormalizedGeneValue(430)),
                new ExpressedTrait(TraitKey.Growth, new NormalizedGeneValue(470)),
                new ExpressedTrait(TraitKey.Lifespan, new NormalizedGeneValue(620)),
                new ExpressedTrait(TraitKey.HeatTolerance, new NormalizedGeneValue(540)),
                new ExpressedTrait(TraitKey.ColdTolerance, new NormalizedGeneValue(620)),
                new ExpressedTrait(TraitKey.HydrationEfficiency, new NormalizedGeneValue(510)),
                new ExpressedTrait(TraitKey.DigestiveEfficiency, new NormalizedGeneValue(450)),
                new ExpressedTrait(TraitKey.Fertility, new NormalizedGeneValue(390)),
                new ExpressedTrait(TraitKey.Maturity, new NormalizedGeneValue(410)),
            });
    }
}

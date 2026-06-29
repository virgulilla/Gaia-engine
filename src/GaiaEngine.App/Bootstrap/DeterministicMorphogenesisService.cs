using System;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Organisms;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Generates deterministic phenotype and physiology data from genomes and development conditions.
/// </summary>
public sealed class DeterministicMorphogenesisService : IMorphogenesisService
{
    /// <summary>
    /// Executes one deterministic morphogenesis pass.
    /// </summary>
    /// <param name="traits">The expressed trait profile to interpret.</param>
    /// <param name="developmentConditions">The environmental development conditions.</param>
    /// <returns>The generated phenotype and initialized physiology.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="traits"/> or <paramref name="developmentConditions"/> is <see langword="null"/>.</exception>
    public MorphogenesisResult Generate(TraitProfile traits, DevelopmentConditions developmentConditions)
    {
        ArgumentNullException.ThrowIfNull(traits);
        ArgumentNullException.ThrowIfNull(developmentConditions);

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

        int foodModifier = (developmentConditions.FoodAvailability - 500) / 4;
        int humidityModifier = (developmentConditions.Humidity - 500) / 5;
        int altitudeModifier = Math.Clamp(developmentConditions.Altitude / 8, -250, 250);
        int seasonModifier = ResolveSeasonModifier(developmentConditions.Season);
        int thermalModifier = Math.Clamp((20 - developmentConditions.AverageTemperature) * 8, -240, 240);
        int developmentModifier = Math.Clamp(foodModifier + humidityModifier - altitudeModifier + seasonModifier + thermalModifier, -1000, 1000);

        BodyPlan bodyPlan = new(
            proportions: ClampNormalized((bodySize + limbReach + foodModifier) / 2),
            mass: ClampNormalized((bodySize + skeletalDensity + developmentConditions.FoodAvailability + (developmentModifier / 2)) / 3),
            limbConfiguration: ClampNormalized((limbReach + locomotionStrength + aquaticAffinity) / 3),
            bodyCovering: ClampNormalized((thermalCovering + coldAdaptation + (1000 - desertAdaptation) + Math.Max(0, thermalModifier)) / 4),
            coloration: ClampNormalized((pigmentation + developmentConditions.Humidity) / 2),
            sensoryStructures: ClampNormalized(sensoryAcuity),
            locomotionProfile: ClampNormalized((locomotionStrength + limbReach + aquaticAffinity) / 3),
            developmentModifier: developmentModifier);

        int metabolismGene = traits.GetValue(TraitKey.Metabolism);
        int growthGene = traits.GetValue(TraitKey.Growth);
        int lifespanGene = traits.GetValue(TraitKey.Lifespan);
        int waterGene = traits.GetValue(TraitKey.HydrationEfficiency);
        int digestionGene = traits.GetValue(TraitKey.DigestiveEfficiency);
        int heatResistance = traits.GetValue(TraitKey.HeatTolerance);
        int fertility = traits.GetValue(TraitKey.Fertility);
        int maturityAge = traits.GetValue(TraitKey.Maturity);

        PhysiologyComponent physiology = new(
            metabolismRate: Math.Max(1, 1 + ((metabolismGene + (bodyPlan.Mass / 2) + Math.Max(0, thermalModifier)) / 250)),
            growthRate: Math.Max(1, (growthGene + developmentConditions.FoodAvailability + Math.Max(0, developmentModifier)) / 450),
            lifespanTicks: Math.Max(120, 240 + ((lifespanGene + heatResistance + coldAdaptation + bodyPlan.BodyCovering) * 2 / 5)),
            waterEfficiency: Math.Clamp((waterGene + developmentConditions.Humidity + (1000 - desertAdaptation)) / 30, 0, 100),
            digestionEfficiency: Math.Clamp((digestionGene + developmentConditions.FoodAvailability + bodyPlan.SensoryStructures) / 30, 0, 100),
            bodyTemperature: developmentConditions.AverageTemperature + ((heatResistance - coldAdaptation) / 100));

        int maturityAgeTicks = Math.Max(60, 40 + ((maturityAge + fertility + bodyPlan.Mass) / 10));
        return new MorphogenesisResult(bodyPlan, physiology, maturityAgeTicks);
    }

    private static int ResolveSeasonModifier(string season)
    {
        return season switch
        {
            "Spring" => 80,
            "Summer" => 40,
            "Autumn" => -20,
            "Winter" => -100,
            _ => 0,
        };
    }

    private static int ClampNormalized(int value)
    {
        return Math.Clamp(value, 0, 1000);
    }
}

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
    /// <param name="genome">The immutable genome to interpret.</param>
    /// <param name="developmentConditions">The environmental development conditions.</param>
    /// <returns>The generated phenotype and initialized physiology.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="genome"/> or <paramref name="developmentConditions"/> is <see langword="null"/>.</exception>
    public MorphogenesisResult Generate(Genome genome, DevelopmentConditions developmentConditions)
    {
        ArgumentNullException.ThrowIfNull(genome);
        ArgumentNullException.ThrowIfNull(developmentConditions);

        int bodySize = GetGeneValue(genome.Morphology, GenomeGeneKey.BodySize);
        int skeletalDensity = GetGeneValue(genome.Morphology, GenomeGeneKey.SkeletalDensity);
        int limbCount = GetGeneValue(genome.Morphology, GenomeGeneKey.LimbCount);
        int muscleDistribution = GetGeneValue(genome.Morphology, GenomeGeneKey.MuscleDistribution);
        int furDensity = GetGeneValue(genome.Appearance, GenomeGeneKey.FurDensity);
        int primaryColor = GetGeneValue(genome.Appearance, GenomeGeneKey.PrimaryColor);
        int secondaryColor = GetGeneValue(genome.Appearance, GenomeGeneKey.SecondaryColor);
        int visionRange = GetGeneValue(genome.Senses, GenomeGeneKey.VisionRange);
        int hearingRange = GetGeneValue(genome.Senses, GenomeGeneKey.HearingRange);
        int smellSensitivity = GetGeneValue(genome.Senses, GenomeGeneKey.SmellSensitivity);
        int aquaticAffinity = GetGeneValue(genome.Adaptation, GenomeGeneKey.AquaticAffinity);
        int coldAdaptation = GetGeneValue(genome.Adaptation, GenomeGeneKey.ColdAdaptation);
        int desertAdaptation = GetGeneValue(genome.Adaptation, GenomeGeneKey.DesertAdaptation);

        int foodModifier = (developmentConditions.FoodAvailability - 500) / 4;
        int humidityModifier = (developmentConditions.Humidity - 500) / 5;
        int altitudeModifier = Math.Clamp(developmentConditions.Altitude / 8, -250, 250);
        int seasonModifier = ResolveSeasonModifier(developmentConditions.Season);
        int thermalModifier = Math.Clamp((20 - developmentConditions.AverageTemperature) * 8, -240, 240);
        int developmentModifier = Math.Clamp(foodModifier + humidityModifier - altitudeModifier + seasonModifier + thermalModifier, -1000, 1000);

        BodyPlan bodyPlan = new(
            proportions: ClampNormalized((bodySize + GetGeneValue(genome.Morphology, GenomeGeneKey.BodyShape) + foodModifier) / 2),
            mass: ClampNormalized((bodySize + skeletalDensity + developmentConditions.FoodAvailability + (developmentModifier / 2)) / 3),
            limbConfiguration: ClampNormalized((limbCount + muscleDistribution + aquaticAffinity) / 3),
            bodyCovering: ClampNormalized((furDensity + coldAdaptation + (1000 - desertAdaptation) + Math.Max(0, thermalModifier)) / 4),
            coloration: ClampNormalized((primaryColor + secondaryColor + developmentConditions.Humidity) / 3),
            sensoryStructures: ClampNormalized((visionRange + hearingRange + smellSensitivity) / 3),
            locomotionProfile: ClampNormalized((muscleDistribution + limbCount + GetGeneValue(genome.Adaptation, GenomeGeneKey.MountainAdaptation)) / 3),
            developmentModifier: developmentModifier);

        int metabolismGene = GetGeneValue(genome.Physiology, GenomeGeneKey.Metabolism);
        int growthGene = GetGeneValue(genome.Physiology, GenomeGeneKey.GrowthRate);
        int lifespanGene = GetGeneValue(genome.Physiology, GenomeGeneKey.Lifespan);
        int waterGene = GetGeneValue(genome.Physiology, GenomeGeneKey.WaterEfficiency);
        int digestionGene = GetGeneValue(genome.Physiology, GenomeGeneKey.DigestionEfficiency);
        int heatResistance = GetGeneValue(genome.Physiology, GenomeGeneKey.HeatResistance);
        int fertility = GetGeneValue(genome.Reproduction, GenomeGeneKey.Fertility);
        int maturityAge = GetGeneValue(genome.Reproduction, GenomeGeneKey.MaturityAge);

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

    private static int GetGeneValue(GenomeGeneGroup group, GenomeGeneKey key)
    {
        foreach (GenomeGene gene in group.GetGenes())
        {
            if (gene.Key == key)
            {
                return gene.IsActive ? gene.Value.ScaledValue : 0;
            }
        }

        throw new InvalidOperationException($"The gene '{key}' is required for morphogenesis.");
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

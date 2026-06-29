using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Genetics;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Evaluates immutable genomes into deterministic expressed trait profiles.
/// </summary>
public sealed class DeterministicTraitExpressionService : ITraitExpressionService
{
    /// <summary>
    /// Evaluates one genome and produces its expressed trait profile.
    /// </summary>
    /// <param name="genome">The immutable genome to interpret.</param>
    /// <returns>The expressed trait profile.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="genome"/> is <see langword="null"/>.</exception>
    public TraitProfile Evaluate(Genome genome)
    {
        ArgumentNullException.ThrowIfNull(genome);

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

    private static ExpressedTrait CreateTrait(TraitKey key, int value)
    {
        return new ExpressedTrait(key, new NormalizedGeneValue(Math.Clamp(value, 0, 1000)));
    }

    private static int Compose(GenomeGeneGroup group, GenomeGeneKey key)
    {
        foreach (GenomeGene gene in group.GetGenes())
        {
            if (gene.Key == key)
            {
                if (!gene.IsActive)
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
        }

        throw new InvalidOperationException($"The gene '{key}' is required for trait evaluation.");
    }

    private static int Average(params int[] values)
    {
        int total = 0;
        foreach (int value in values)
        {
            total += value;
        }

        return total / values.Length;
    }
}

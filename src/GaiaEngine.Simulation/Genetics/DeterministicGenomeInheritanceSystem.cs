using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Simulation.Genetics;

/// <summary>
/// Combines two parent genomes into one deterministic offspring genome.
/// </summary>
public sealed class DeterministicGenomeInheritanceSystem : IGenomeInheritanceSystem
{
    /// <summary>
    /// Creates one deterministic offspring genome from two parent genomes.
    /// </summary>
    /// <param name="parentA">The first parent genome candidate.</param>
    /// <param name="parentB">The second parent genome candidate.</param>
    /// <param name="offspringGenomeId">The new genome identifier to assign to the offspring.</param>
    /// <returns>The combined offspring genome.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="parentA"/> or <paramref name="parentB"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when parent genomes are incompatible.</exception>
    public Genome Inherit(Genome parentA, Genome parentB, GenomeId offspringGenomeId)
    {
        ArgumentNullException.ThrowIfNull(parentA);
        ArgumentNullException.ThrowIfNull(parentB);

        (Genome canonicalA, Genome canonicalB) = OrderParents(parentA, parentB);
        ValidateCompatibility(canonicalA, canonicalB);

        GenomeIdentity identity = new(
            offspringGenomeId,
            canonicalA.Identity.Version,
            canonicalA.Id,
            canonicalB.Id,
            mutationCount: 0,
            generation: Math.Max(canonicalA.Identity.Generation, canonicalB.Identity.Generation) + 1);

        return new Genome(
            identity,
            CombineGroup(canonicalA.Morphology, canonicalB.Morphology),
            CombineGroup(canonicalA.Physiology, canonicalB.Physiology),
            CombineGroup(canonicalA.Reproduction, canonicalB.Reproduction),
            CombineGroup(canonicalA.Senses, canonicalB.Senses),
            CombineGroup(canonicalA.Adaptation, canonicalB.Adaptation),
            CombineGroup(canonicalA.Appearance, canonicalB.Appearance),
            CombineGroup(canonicalA.BehaviourBias, canonicalB.BehaviourBias));
    }

    private static (Genome ParentA, Genome ParentB) OrderParents(Genome parentA, Genome parentB)
    {
        return parentA.Id.Value <= parentB.Id.Value
            ? (parentA, parentB)
            : (parentB, parentA);
    }

    private static void ValidateCompatibility(Genome parentA, Genome parentB)
    {
        if (parentA.Identity.Version != parentB.Identity.Version)
        {
            throw new ArgumentException("Parent genomes must use the same schema version.", nameof(parentB));
        }

        ValidateGroupCompatibility(parentA.Morphology, parentB.Morphology);
        ValidateGroupCompatibility(parentA.Physiology, parentB.Physiology);
        ValidateGroupCompatibility(parentA.Reproduction, parentB.Reproduction);
        ValidateGroupCompatibility(parentA.Senses, parentB.Senses);
        ValidateGroupCompatibility(parentA.Adaptation, parentB.Adaptation);
        ValidateGroupCompatibility(parentA.Appearance, parentB.Appearance);
        ValidateGroupCompatibility(parentA.BehaviourBias, parentB.BehaviourBias);
    }

    private static void ValidateGroupCompatibility(GenomeGeneGroup parentA, GenomeGeneGroup parentB)
    {
        if (parentA.GroupType != parentB.GroupType)
        {
            throw new ArgumentException("Parent genomes must expose matching gene groups.", nameof(parentB));
        }

        IReadOnlyList<GenomeGene> leftGenes = parentA.GetGenes();
        IReadOnlyList<GenomeGene> rightGenes = parentB.GetGenes();
        if (leftGenes.Count != rightGenes.Count)
        {
            throw new ArgumentException($"Parent gene group '{parentA.GroupType}' must contain the same number of genes.", nameof(parentB));
        }

        for (int index = 0; index < leftGenes.Count; index++)
        {
            if (leftGenes[index].Key != rightGenes[index].Key)
            {
                throw new ArgumentException($"Parent gene group '{parentA.GroupType}' must contain the same gene keys.", nameof(parentB));
            }
        }
    }

    private static GenomeGeneGroup CombineGroup(GenomeGeneGroup parentA, GenomeGeneGroup parentB)
    {
        List<GenomeGene> genes = new(parentA.Count);
        foreach (GenomeGene geneA in parentA.GetGenes())
        {
            if (!parentB.TryGetGene(geneA.Key, out GenomeGene? geneB) || geneB is null)
            {
                throw new ArgumentException($"The gene '{geneA.Key}' is missing from the matching parent group.", nameof(parentB));
            }

            genes.Add(CombineGene(geneA, geneB));
        }

        return new GenomeGeneGroup(parentA.GroupType, genes.AsReadOnly());
    }

    private static GenomeGene CombineGene(GenomeGene parentA, GenomeGene parentB)
    {
        bool isActive = ResolveActivation(parentA, parentB);
        GeneDominance dominance = ResolveDominance(parentA.Dominance, parentB.Dominance);
        int value = ResolveValue(parentA, parentB, isActive);

        return new GenomeGene(
            parentA.Key,
            new NormalizedGeneValue(value),
            dominance,
            isActive);
    }

    private static bool ResolveActivation(GenomeGene parentA, GenomeGene parentB)
    {
        return parentA.IsActive || parentB.IsActive;
    }

    private static GeneDominance ResolveDominance(GeneDominance left, GeneDominance right)
    {
        if (left == right)
        {
            return left;
        }

        if (left == GeneDominance.Blended || right == GeneDominance.Blended)
        {
            return GeneDominance.Blended;
        }

        if (left == GeneDominance.CoDominant || right == GeneDominance.CoDominant)
        {
            return GeneDominance.CoDominant;
        }

        if ((left == GeneDominance.Dominant && right == GeneDominance.Recessive) ||
            (left == GeneDominance.Recessive && right == GeneDominance.Dominant))
        {
            return GeneDominance.Dominant;
        }

        return GeneDominance.Blended;
    }

    private static int ResolveValue(GenomeGene parentA, GenomeGene parentB, bool isActive)
    {
        if (!parentA.IsActive && !parentB.IsActive)
        {
            return Average(parentA.Value.ScaledValue, parentB.Value.ScaledValue);
        }

        if (parentA.IsActive && !parentB.IsActive)
        {
            return parentA.Value.ScaledValue;
        }

        if (!parentA.IsActive && parentB.IsActive)
        {
            return parentB.Value.ScaledValue;
        }

        if (parentA.Value.ScaledValue == parentB.Value.ScaledValue)
        {
            return parentA.Value.ScaledValue;
        }

        if (parentA.Dominance == GeneDominance.Dominant && parentB.Dominance == GeneDominance.Recessive)
        {
            return parentA.Value.ScaledValue;
        }

        if (parentA.Dominance == GeneDominance.Recessive && parentB.Dominance == GeneDominance.Dominant)
        {
            return parentB.Value.ScaledValue;
        }

        return isActive
            ? Average(parentA.Value.ScaledValue, parentB.Value.ScaledValue)
            : Average(parentA.Value.ScaledValue, parentB.Value.ScaledValue);
    }

    private static int Average(int left, int right)
    {
        return (left + right) / 2;
    }
}

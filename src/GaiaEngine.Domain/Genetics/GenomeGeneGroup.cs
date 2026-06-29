using System;
using System.Collections.Generic;

namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Represents one immutable deterministic genome group.
/// </summary>
public sealed class GenomeGeneGroup
{
    private readonly IReadOnlyList<GenomeGene> genes;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenomeGeneGroup"/> class.
    /// </summary>
    /// <param name="groupType">The group category.</param>
    /// <param name="genes">The genes assigned to the group.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="genes"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when a gene does not belong to the supplied group or when duplicate keys are present.</exception>
    public GenomeGeneGroup(GenomeGroupType groupType, IReadOnlyList<GenomeGene> genes)
    {
        ArgumentNullException.ThrowIfNull(genes);

        HashSet<GenomeGeneKey> uniqueKeys = new();
        List<GenomeGene> orderedGenes = new(genes.Count);
        foreach (GenomeGene gene in genes)
        {
            ArgumentNullException.ThrowIfNull(gene);

            if (ResolveGroupType(gene.Key) != groupType)
            {
                throw new ArgumentException($"The gene '{gene.Key}' does not belong to the '{groupType}' group.", nameof(genes));
            }

            if (!uniqueKeys.Add(gene.Key))
            {
                throw new ArgumentException($"The gene '{gene.Key}' is duplicated inside the '{groupType}' group.", nameof(genes));
            }

            orderedGenes.Add(gene);
        }

        orderedGenes.Sort(static (left, right) => left.Key.CompareTo(right.Key));
        GroupType = groupType;
        this.genes = orderedGenes.AsReadOnly();
    }

    /// <summary>
    /// Gets the deterministic group category.
    /// </summary>
    public GenomeGroupType GroupType { get; }

    /// <summary>
    /// Gets the number of genes stored in the group.
    /// </summary>
    public int Count => genes.Count;

    /// <summary>
    /// Gets the stored genes in deterministic order.
    /// </summary>
    /// <returns>The stored genes.</returns>
    public IReadOnlyList<GenomeGene> GetGenes()
    {
        return genes;
    }

    /// <summary>
    /// Resolves the owning group for a supplied gene key.
    /// </summary>
    /// <param name="key">The gene key to resolve.</param>
    /// <returns>The owning group type.</returns>
    public static GenomeGroupType ResolveGroupType(GenomeGeneKey key)
    {
        int value = (int)key;
        return value switch
        {
            >= 0 and < 100 => GenomeGroupType.Morphology,
            >= 100 and < 200 => GenomeGroupType.Physiology,
            >= 200 and < 300 => GenomeGroupType.Reproduction,
            >= 300 and < 400 => GenomeGroupType.Senses,
            >= 400 and < 500 => GenomeGroupType.Adaptation,
            >= 500 and < 600 => GenomeGroupType.Appearance,
            >= 600 and < 700 => GenomeGroupType.BehaviourBias,
            _ => throw new ArgumentOutOfRangeException(nameof(key), key, "The supplied gene key is outside the supported group ranges."),
        };
    }
}

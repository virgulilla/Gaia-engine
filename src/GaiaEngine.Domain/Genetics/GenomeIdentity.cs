using System;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Stores the immutable metadata that identifies one genome lineage.
/// </summary>
public sealed record GenomeIdentity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenomeIdentity"/> class.
    /// </summary>
    /// <param name="genomeId">The immutable genome identifier.</param>
    /// <param name="version">The deterministic schema version for the genome payload.</param>
    /// <param name="parentGenomeA">The first optional parent genome identifier.</param>
    /// <param name="parentGenomeB">The second optional parent genome identifier.</param>
    /// <param name="mutationCount">The accumulated mutation count.</param>
    /// <param name="generation">The lineage generation number.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="version"/>, <paramref name="mutationCount"/>, or <paramref name="generation"/> is negative.</exception>
    public GenomeIdentity(
        GenomeId genomeId,
        int version,
        GenomeId? parentGenomeA,
        GenomeId? parentGenomeB,
        int mutationCount,
        int generation)
    {
        if (version < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(version), "The genome version must be zero or greater.");
        }

        if (mutationCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(mutationCount), "The mutation count must be zero or greater.");
        }

        if (generation < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(generation), "The generation must be zero or greater.");
        }

        GenomeId = genomeId;
        Version = version;
        ParentGenomeA = parentGenomeA;
        ParentGenomeB = parentGenomeB;
        MutationCount = mutationCount;
        Generation = generation;
    }

    /// <summary>
    /// Gets the immutable genome identifier.
    /// </summary>
    public GenomeId GenomeId { get; }

    /// <summary>
    /// Gets the deterministic schema version for the genome payload.
    /// </summary>
    public int Version { get; }

    /// <summary>
    /// Gets the first optional parent genome identifier.
    /// </summary>
    public GenomeId? ParentGenomeA { get; }

    /// <summary>
    /// Gets the second optional parent genome identifier.
    /// </summary>
    public GenomeId? ParentGenomeB { get; }

    /// <summary>
    /// Gets the accumulated mutation count.
    /// </summary>
    public int MutationCount { get; }

    /// <summary>
    /// Gets the lineage generation number.
    /// </summary>
    public int Generation { get; }
}

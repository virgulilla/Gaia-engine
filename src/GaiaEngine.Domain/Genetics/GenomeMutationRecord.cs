using System;

namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Stores one deterministic mutation applied during genome generation.
/// </summary>
public sealed record GenomeMutationRecord
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenomeMutationRecord"/> class.
    /// </summary>
    /// <param name="sequence">The deterministic mutation sequence inside the generated genome.</param>
    /// <param name="groupType">The affected gene group.</param>
    /// <param name="geneKey">The affected gene key.</param>
    /// <param name="category">The mutation category.</param>
    /// <param name="previousValue">The previous normalized gene value.</param>
    /// <param name="newValue">The new normalized gene value.</param>
    /// <param name="previousDominance">The previous dominance mode.</param>
    /// <param name="newDominance">The new dominance mode.</param>
    /// <param name="previousIsActive">The previous activation state.</param>
    /// <param name="newIsActive">The new activation state.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="sequence"/> is negative.</exception>
    public GenomeMutationRecord(
        int sequence,
        GenomeGroupType groupType,
        GenomeGeneKey geneKey,
        MutationCategory category,
        int previousValue,
        int newValue,
        GeneDominance previousDominance,
        GeneDominance newDominance,
        bool previousIsActive,
        bool newIsActive)
    {
        if (sequence < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(sequence), "The mutation sequence must be zero or greater.");
        }

        Sequence = sequence;
        GroupType = groupType;
        GeneKey = geneKey;
        Category = category;
        PreviousValue = previousValue;
        NewValue = newValue;
        PreviousDominance = previousDominance;
        NewDominance = newDominance;
        PreviousIsActive = previousIsActive;
        NewIsActive = newIsActive;
    }

    /// <summary>
    /// Gets the deterministic mutation sequence inside the generated genome.
    /// </summary>
    public int Sequence { get; }

    /// <summary>
    /// Gets the affected gene group.
    /// </summary>
    public GenomeGroupType GroupType { get; }

    /// <summary>
    /// Gets the affected gene key.
    /// </summary>
    public GenomeGeneKey GeneKey { get; }

    /// <summary>
    /// Gets the mutation category.
    /// </summary>
    public MutationCategory Category { get; }

    /// <summary>
    /// Gets the previous normalized gene value.
    /// </summary>
    public int PreviousValue { get; }

    /// <summary>
    /// Gets the new normalized gene value.
    /// </summary>
    public int NewValue { get; }

    /// <summary>
    /// Gets the previous dominance mode.
    /// </summary>
    public GeneDominance PreviousDominance { get; }

    /// <summary>
    /// Gets the new dominance mode.
    /// </summary>
    public GeneDominance NewDominance { get; }

    /// <summary>
    /// Gets the previous activation state.
    /// </summary>
    public bool PreviousIsActive { get; }

    /// <summary>
    /// Gets the new activation state.
    /// </summary>
    public bool NewIsActive { get; }
}

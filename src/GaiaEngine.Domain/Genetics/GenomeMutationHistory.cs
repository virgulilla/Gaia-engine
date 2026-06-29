using System;
using System.Collections.Generic;

namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Stores the deterministic mutation history of one genome.
/// </summary>
public sealed class GenomeMutationHistory
{
    private readonly IReadOnlyList<GenomeMutationRecord> orderedRecords;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenomeMutationHistory"/> class.
    /// </summary>
    /// <param name="records">The mutation records to store.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="records"/> is <see langword="null"/>.</exception>
    public GenomeMutationHistory(IReadOnlyList<GenomeMutationRecord> records)
    {
        ArgumentNullException.ThrowIfNull(records);

        List<GenomeMutationRecord> ordered = new(records.Count);
        foreach (GenomeMutationRecord record in records)
        {
            ArgumentNullException.ThrowIfNull(record);
            ordered.Add(record);
        }

        ordered.Sort(static (left, right) => left.Sequence.CompareTo(right.Sequence));
        orderedRecords = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty mutation history instance.
    /// </summary>
    public static GenomeMutationHistory Empty { get; } = new(Array.Empty<GenomeMutationRecord>());

    /// <summary>
    /// Gets the number of stored mutation records.
    /// </summary>
    public int Count => orderedRecords.Count;

    /// <summary>
    /// Gets the stored mutation records in deterministic order.
    /// </summary>
    /// <returns>The ordered mutation records.</returns>
    public IReadOnlyList<GenomeMutationRecord> GetAll()
    {
        return orderedRecords;
    }
}

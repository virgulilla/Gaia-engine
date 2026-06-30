using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Domain.AI;

/// <summary>
/// Represents the deterministic memory set owned by one organism.
/// </summary>
public sealed class OrganismMemory
{
    private readonly IReadOnlyList<MemoryEntry> orderedEntries;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganismMemory"/> class.
    /// </summary>
    /// <param name="organismId">The organism that owns the memory set.</param>
    /// <param name="entries">The remembered entries.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entries"/> is <see langword="null"/>.</exception>
    public OrganismMemory(OrganismId organismId, IReadOnlyList<MemoryEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        List<MemoryEntry> ordered = new(entries.Count);
        foreach (MemoryEntry entry in entries)
        {
            ordered.Add(entry ?? throw new ArgumentNullException(nameof(entries), "The organism memory cannot contain null entries."));
        }

        ordered.Sort(CompareEntries);
        OrganismId = organismId;
        orderedEntries = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the owning organism identifier.
    /// </summary>
    public OrganismId OrganismId { get; }

    /// <summary>
    /// Gets the number of stored memory entries.
    /// </summary>
    public int Count => orderedEntries.Count;

    /// <summary>
    /// Returns the memory entries in deterministic order.
    /// </summary>
    /// <returns>The stored memory entries.</returns>
    public IReadOnlyList<MemoryEntry> GetAll()
    {
        return orderedEntries;
    }

    private static int CompareEntries(MemoryEntry left, MemoryEntry right)
    {
        int categoryComparison = left.Category.CompareTo(right.Category);
        if (categoryComparison != 0)
        {
            return categoryComparison;
        }

        int identifierComparison = left.Identifier.CompareTo(right.Identifier);
        if (identifierComparison != 0)
        {
            return identifierComparison;
        }

        int yComparison = left.Position.Y.CompareTo(right.Position.Y);
        if (yComparison != 0)
        {
            return yComparison;
        }

        return left.Position.X.CompareTo(right.Position.X);
    }
}

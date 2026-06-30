using System;
using System.Collections.Generic;

namespace GaiaEngine.Gameplay.Encyclopedia;

/// <summary>
/// Represents the deterministic encyclopedia owned by a player profile.
/// </summary>
public sealed class EncyclopediaCollection
{
    private readonly Dictionary<string, EncyclopediaEntry> entriesById;
    private readonly IReadOnlyList<EncyclopediaEntry> orderedEntries;

    /// <summary>
    /// Initializes a new instance of the <see cref="EncyclopediaCollection"/> class.
    /// </summary>
    /// <param name="entries">The encyclopedia entries to store.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entries"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when duplicated entry identifiers are detected.</exception>
    public EncyclopediaCollection(IReadOnlyList<EncyclopediaEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        entriesById = new Dictionary<string, EncyclopediaEntry>(entries.Count, StringComparer.Ordinal);
        List<EncyclopediaEntry> ordered = new(entries.Count);
        foreach (EncyclopediaEntry entry in entries)
        {
            ArgumentNullException.ThrowIfNull(entry);
            if (!entriesById.TryAdd(entry.EntryId, entry))
            {
                throw new ArgumentException($"The encyclopedia entry '{entry.EntryId}' is duplicated.", nameof(entries));
            }

            ordered.Add(entry);
        }

        ordered.Sort(static (left, right) => string.CompareOrdinal(left.EntryId, right.EntryId));
        orderedEntries = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty encyclopedia instance.
    /// </summary>
    public static EncyclopediaCollection Empty { get; } = new(Array.Empty<EncyclopediaEntry>());

    /// <summary>
    /// Gets the number of stored encyclopedia entries.
    /// </summary>
    public int Count => orderedEntries.Count;

    /// <summary>
    /// Returns the encyclopedia entries in deterministic order.
    /// </summary>
    /// <returns>The ordered encyclopedia entries.</returns>
    public IReadOnlyList<EncyclopediaEntry> GetAll()
    {
        return orderedEntries;
    }

    /// <summary>
    /// Determines whether the supplied entry identifier exists.
    /// </summary>
    /// <param name="entryId">The entry identifier to test.</param>
    /// <returns><see langword="true"/> when the entry exists; otherwise <see langword="false"/>.</returns>
    public bool Contains(string entryId)
    {
        return !string.IsNullOrWhiteSpace(entryId) && entriesById.ContainsKey(entryId);
    }
}

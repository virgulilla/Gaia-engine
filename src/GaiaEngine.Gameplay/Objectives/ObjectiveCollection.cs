using System;
using System.Collections.Generic;

namespace GaiaEngine.Gameplay.Objectives;

/// <summary>
/// Represents the deterministic set of objectives owned by one player profile.
/// </summary>
public sealed class ObjectiveCollection
{
    private readonly Dictionary<string, ObjectiveEntry> entriesById;
    private readonly IReadOnlyList<ObjectiveEntry> orderedEntries;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectiveCollection"/> class.
    /// </summary>
    /// <param name="entries">The stored objective entries.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entries"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when duplicate objective identifiers are detected.</exception>
    public ObjectiveCollection(IReadOnlyList<ObjectiveEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        entriesById = new Dictionary<string, ObjectiveEntry>(entries.Count, StringComparer.Ordinal);
        List<ObjectiveEntry> ordered = new(entries.Count);
        foreach (ObjectiveEntry entry in entries)
        {
            ArgumentNullException.ThrowIfNull(entry);
            if (!entriesById.TryAdd(entry.ObjectiveId, entry))
            {
                throw new ArgumentException($"The objective '{entry.ObjectiveId}' is duplicated.", nameof(entries));
            }

            ordered.Add(entry);
        }

        ordered.Sort(static (left, right) => string.CompareOrdinal(left.ObjectiveId, right.ObjectiveId));
        orderedEntries = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty objective collection.
    /// </summary>
    public static ObjectiveCollection Empty { get; } = new(Array.Empty<ObjectiveEntry>());

    /// <summary>
    /// Gets the total number of stored objective entries.
    /// </summary>
    public int Count => orderedEntries.Count;

    /// <summary>
    /// Returns the objectives in deterministic order.
    /// </summary>
    /// <returns>The ordered objective entries.</returns>
    public IReadOnlyList<ObjectiveEntry> GetAll()
    {
        return orderedEntries;
    }

    /// <summary>
    /// Determines whether the supplied objective identifier exists.
    /// </summary>
    /// <param name="objectiveId">The objective identifier to test.</param>
    /// <returns><see langword="true"/> when the objective exists; otherwise <see langword="false"/>.</returns>
    public bool Contains(string objectiveId)
    {
        return !string.IsNullOrWhiteSpace(objectiveId) && entriesById.ContainsKey(objectiveId);
    }

    /// <summary>
    /// Attempts to return one stored objective entry.
    /// </summary>
    /// <param name="objectiveId">The objective identifier to resolve.</param>
    /// <param name="entry">The resolved entry when found.</param>
    /// <returns><see langword="true"/> when the objective exists; otherwise <see langword="false"/>.</returns>
    public bool TryGet(string objectiveId, out ObjectiveEntry? entry)
    {
        if (string.IsNullOrWhiteSpace(objectiveId))
        {
            entry = null;
            return false;
        }

        return entriesById.TryGetValue(objectiveId, out entry);
    }
}

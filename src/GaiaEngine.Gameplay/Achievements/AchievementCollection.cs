using System;
using System.Collections.Generic;

namespace GaiaEngine.Gameplay.Achievements;

/// <summary>
/// Represents the deterministic set of achievements owned by one player profile.
/// </summary>
public sealed class AchievementCollection
{
    private readonly Dictionary<string, AchievementEntry> entriesById;
    private readonly IReadOnlyList<AchievementEntry> orderedEntries;

    /// <summary>
    /// Initializes a new instance of the <see cref="AchievementCollection"/> class.
    /// </summary>
    /// <param name="entries">The stored achievement entries.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="entries"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when a duplicate achievement identifier is detected.</exception>
    public AchievementCollection(IReadOnlyList<AchievementEntry> entries)
    {
        ArgumentNullException.ThrowIfNull(entries);

        entriesById = new Dictionary<string, AchievementEntry>(entries.Count, StringComparer.Ordinal);
        List<AchievementEntry> ordered = new(entries.Count);
        foreach (AchievementEntry entry in entries)
        {
            ArgumentNullException.ThrowIfNull(entry);
            if (!entriesById.TryAdd(entry.AchievementId, entry))
            {
                throw new ArgumentException($"The achievement '{entry.AchievementId}' is duplicated.", nameof(entries));
            }

            ordered.Add(entry);
        }

        ordered.Sort(static (left, right) => string.CompareOrdinal(left.AchievementId, right.AchievementId));
        orderedEntries = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty achievement collection.
    /// </summary>
    public static AchievementCollection Empty { get; } = new(Array.Empty<AchievementEntry>());

    /// <summary>
    /// Gets the number of stored achievement entries.
    /// </summary>
    public int Count => orderedEntries.Count;

    /// <summary>
    /// Returns the achievement entries in deterministic order.
    /// </summary>
    /// <returns>The ordered achievement entries.</returns>
    public IReadOnlyList<AchievementEntry> GetAll()
    {
        return orderedEntries;
    }

    /// <summary>
    /// Attempts to return one stored achievement entry.
    /// </summary>
    /// <param name="achievementId">The achievement identifier to resolve.</param>
    /// <param name="entry">The resolved entry when found.</param>
    /// <returns><see langword="true"/> when the achievement exists; otherwise <see langword="false"/>.</returns>
    public bool TryGet(string achievementId, out AchievementEntry? entry)
    {
        if (string.IsNullOrWhiteSpace(achievementId))
        {
            entry = null;
            return false;
        }

        return entriesById.TryGetValue(achievementId, out entry);
    }
}

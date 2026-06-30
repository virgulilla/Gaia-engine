using System;
using System.Collections.Generic;

namespace GaiaEngine.Gameplay.Progression;

/// <summary>
/// Represents the deterministic set of progression unlock identifiers owned by one player profile.
/// </summary>
public sealed class ProgressionUnlockCollection
{
    private readonly HashSet<string> unlocksById;
    private readonly IReadOnlyList<string> orderedUnlocks;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgressionUnlockCollection"/> class.
    /// </summary>
    /// <param name="unlockIds">The stored unlock identifiers.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="unlockIds"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when a duplicate unlock identifier is detected.</exception>
    public ProgressionUnlockCollection(IReadOnlyList<string> unlockIds)
    {
        ArgumentNullException.ThrowIfNull(unlockIds);

        unlocksById = new HashSet<string>(unlockIds.Count, StringComparer.Ordinal);
        List<string> ordered = new(unlockIds.Count);
        foreach (string unlockId in unlockIds)
        {
            if (string.IsNullOrWhiteSpace(unlockId))
            {
                throw new ArgumentException("One progression unlock identifier is empty.", nameof(unlockIds));
            }

            if (!unlocksById.Add(unlockId))
            {
                throw new ArgumentException($"The progression unlock '{unlockId}' is duplicated.", nameof(unlockIds));
            }

            ordered.Add(unlockId);
        }

        ordered.Sort(StringComparer.Ordinal);
        orderedUnlocks = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty unlock collection.
    /// </summary>
    public static ProgressionUnlockCollection Empty { get; } = new(Array.Empty<string>());

    /// <summary>
    /// Gets the number of stored unlock identifiers.
    /// </summary>
    public int Count => orderedUnlocks.Count;

    /// <summary>
    /// Returns the unlock identifiers in deterministic order.
    /// </summary>
    /// <returns>The ordered unlock identifiers.</returns>
    public IReadOnlyList<string> GetAll()
    {
        return orderedUnlocks;
    }

    /// <summary>
    /// Determines whether one unlock identifier exists.
    /// </summary>
    /// <param name="unlockId">The unlock identifier to test.</param>
    /// <returns><see langword="true"/> when the identifier exists; otherwise <see langword="false"/>.</returns>
    public bool Contains(string unlockId)
    {
        return !string.IsNullOrWhiteSpace(unlockId) && unlocksById.Contains(unlockId);
    }
}

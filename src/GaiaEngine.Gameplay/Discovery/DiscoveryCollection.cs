using System;
using System.Collections.Generic;

namespace GaiaEngine.Gameplay.Discovery;

/// <summary>
/// Represents the deterministic set of discoveries owned by one player profile.
/// </summary>
public sealed class DiscoveryCollection
{
    private readonly Dictionary<string, DiscoveryEntry> entriesById;
    private readonly IReadOnlyList<DiscoveryEntry> orderedEntries;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscoveryCollection"/> class.
    /// </summary>
    /// <param name="discoveries">The owned discoveries.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="discoveries"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when duplicated discoveries are detected.</exception>
    public DiscoveryCollection(IReadOnlyList<DiscoveryEntry> discoveries)
    {
        ArgumentNullException.ThrowIfNull(discoveries);

        entriesById = new Dictionary<string, DiscoveryEntry>(discoveries.Count, StringComparer.Ordinal);
        List<DiscoveryEntry> ordered = new(discoveries.Count);
        foreach (DiscoveryEntry entry in discoveries)
        {
            ArgumentNullException.ThrowIfNull(entry);
            if (!entriesById.TryAdd(entry.DiscoveryId, entry))
            {
                throw new ArgumentException($"The discovery '{entry.DiscoveryId}' is duplicated.", nameof(discoveries));
            }

            ordered.Add(entry);
        }

        ordered.Sort(static (left, right) => string.CompareOrdinal(left.DiscoveryId, right.DiscoveryId));
        orderedEntries = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty discovery collection instance.
    /// </summary>
    public static DiscoveryCollection Empty { get; } = new(Array.Empty<DiscoveryEntry>());

    /// <summary>
    /// Gets the total number of unlocked discoveries.
    /// </summary>
    public int Count => orderedEntries.Count;

    /// <summary>
    /// Returns the discoveries in deterministic order.
    /// </summary>
    /// <returns>The ordered discovery set.</returns>
    public IReadOnlyList<DiscoveryEntry> GetAll()
    {
        return orderedEntries;
    }

    /// <summary>
    /// Determines whether the supplied discovery identifier already exists.
    /// </summary>
    /// <param name="discoveryId">The discovery identifier to test.</param>
    /// <returns><see langword="true"/> when the discovery exists; otherwise <see langword="false"/>.</returns>
    public bool Contains(string discoveryId)
    {
        return !string.IsNullOrWhiteSpace(discoveryId) && entriesById.ContainsKey(discoveryId);
    }
}

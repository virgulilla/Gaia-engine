using System;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Gameplay.Discovery;

/// <summary>
/// Represents one permanent discovery unlocked by a player profile.
/// </summary>
public sealed record DiscoveryEntry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DiscoveryEntry"/> class.
    /// </summary>
    /// <param name="discoveryId">The stable discovery identifier.</param>
    /// <param name="category">The discovery category.</param>
    /// <param name="name">The player-facing discovery name.</param>
    /// <param name="description">The player-facing discovery description.</param>
    /// <param name="unlockTick">The simulation tick when the discovery was unlocked.</param>
    /// <param name="worldId">The world where the discovery was unlocked.</param>
    /// <param name="playerId">The owning player identifier.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="discoveryId"/>, <paramref name="name"/>, <paramref name="description"/>, or <paramref name="playerId"/> is empty.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="unlockTick"/> is negative.</exception>
    public DiscoveryEntry(
        string discoveryId,
        DiscoveryCategory category,
        string name,
        string description,
        long unlockTick,
        WorldId worldId,
        string playerId)
    {
        if (string.IsNullOrWhiteSpace(discoveryId))
        {
            throw new ArgumentException("The discovery identifier must contain a value.", nameof(discoveryId));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The discovery name must contain a value.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("The discovery description must contain a value.", nameof(description));
        }

        if (unlockTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unlockTick), "The unlock tick must be zero or greater.");
        }

        if (string.IsNullOrWhiteSpace(playerId))
        {
            throw new ArgumentException("The player identifier must contain a value.", nameof(playerId));
        }

        DiscoveryId = discoveryId;
        Category = category;
        Name = name;
        Description = description;
        UnlockTick = unlockTick;
        WorldId = worldId;
        PlayerId = playerId;
    }

    /// <summary>
    /// Gets the stable discovery identifier.
    /// </summary>
    public string DiscoveryId { get; }

    /// <summary>
    /// Gets the discovery category.
    /// </summary>
    public DiscoveryCategory Category { get; }

    /// <summary>
    /// Gets the player-facing discovery name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the player-facing discovery description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the simulation tick when the discovery was unlocked.
    /// </summary>
    public long UnlockTick { get; }

    /// <summary>
    /// Gets the world where the discovery was unlocked.
    /// </summary>
    public WorldId WorldId { get; }

    /// <summary>
    /// Gets the owning player identifier.
    /// </summary>
    public string PlayerId { get; }
}

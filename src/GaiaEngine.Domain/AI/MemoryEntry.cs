using System;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Domain.AI;

/// <summary>
/// Represents one deterministic remembered observation.
/// </summary>
public sealed record MemoryEntry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryEntry"/> class.
    /// </summary>
    /// <param name="identifier">The remembered object identifier.</param>
    /// <param name="category">The memory category.</param>
    /// <param name="position">The last known chunk position.</param>
    /// <param name="confidence">
    /// The normalized confidence scaled to the inclusive range [0, 1000],
    /// which represents the specification range [0.0, 1.0].
    /// </param>
    /// <param name="creationTick">The tick when the memory was first created.</param>
    /// <param name="lastUpdateTick">The tick when the memory was last refreshed.</param>
    /// <param name="expirationTick">The tick when the memory expires.</param>
    /// <param name="estimatedAvailability">
    /// The optional remembered availability scaled to the inclusive range [0, 1000],
    /// which represents the specification range [0.0, 1.0].
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when confidence is outside [0, 1000], when ticks are negative,
    /// when expiration is earlier than creation, or when availability is outside [0, 1000].
    /// </exception>
    public MemoryEntry(
        ulong identifier,
        MemoryCategory category,
        ChunkCoordinates position,
        int confidence,
        long creationTick,
        long lastUpdateTick,
        long expirationTick,
        int? estimatedAvailability)
    {
        if (confidence < 0 || confidence > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(confidence), "The memory confidence must be between 0 and 1000.");
        }

        if (creationTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(creationTick), "The memory creation tick must be zero or greater.");
        }

        if (lastUpdateTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lastUpdateTick), "The memory last update tick must be zero or greater.");
        }

        if (expirationTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(expirationTick), "The memory expiration tick must be zero or greater.");
        }

        if (lastUpdateTick < creationTick)
        {
            throw new ArgumentOutOfRangeException(nameof(lastUpdateTick), "The memory last update tick cannot be earlier than the creation tick.");
        }

        if (expirationTick < creationTick)
        {
            throw new ArgumentOutOfRangeException(nameof(expirationTick), "The memory expiration tick cannot be earlier than the creation tick.");
        }

        if (estimatedAvailability is < 0 or > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(estimatedAvailability), "The estimated availability must be between 0 and 1000.");
        }

        Identifier = identifier;
        Category = category;
        Position = position;
        Confidence = confidence;
        CreationTick = creationTick;
        LastUpdateTick = lastUpdateTick;
        ExpirationTick = expirationTick;
        EstimatedAvailability = estimatedAvailability;
    }

    /// <summary>
    /// Gets the remembered object identifier.
    /// </summary>
    public ulong Identifier { get; }

    /// <summary>
    /// Gets the memory category.
    /// </summary>
    public MemoryCategory Category { get; }

    /// <summary>
    /// Gets the last known chunk position.
    /// </summary>
    public ChunkCoordinates Position { get; }

    /// <summary>
    /// Gets the normalized confidence scaled to the inclusive range [0, 1000].
    /// </summary>
    public int Confidence { get; }

    /// <summary>
    /// Gets the tick when the memory was first created.
    /// </summary>
    public long CreationTick { get; }

    /// <summary>
    /// Gets the tick when the memory was last refreshed.
    /// </summary>
    public long LastUpdateTick { get; }

    /// <summary>
    /// Gets the tick when the memory expires.
    /// </summary>
    public long ExpirationTick { get; }

    /// <summary>
    /// Gets the optional remembered availability scaled to the inclusive range [0, 1000].
    /// </summary>
    public int? EstimatedAvailability { get; }
}

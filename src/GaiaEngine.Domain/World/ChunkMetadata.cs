using System;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents immutable metadata associated with a chunk.
/// </summary>
public sealed record ChunkMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChunkMetadata"/> class.
    /// </summary>
    /// <param name="chunkId">The immutable chunk identifier.</param>
    /// <param name="worldId">The immutable owner world identifier.</param>
    /// <param name="coordinates">The immutable chunk coordinates.</param>
    /// <param name="seed">The deterministic chunk seed.</param>
    /// <param name="size">The immutable chunk size.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="size"/> is not positive.</exception>
    public ChunkMetadata(ChunkId chunkId, WorldId worldId, ChunkCoordinates coordinates, WorldSeed seed, int size)
    {
        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), "The chunk size must be greater than zero.");
        }

        ChunkId = chunkId;
        WorldId = worldId;
        Coordinates = coordinates;
        Seed = seed;
        Size = size;
    }

    /// <summary>
    /// Gets the immutable chunk identifier.
    /// </summary>
    public ChunkId ChunkId { get; }

    /// <summary>
    /// Gets the immutable owner world identifier.
    /// </summary>
    public WorldId WorldId { get; }

    /// <summary>
    /// Gets the immutable chunk coordinates.
    /// </summary>
    public ChunkCoordinates Coordinates { get; }

    /// <summary>
    /// Gets the deterministic chunk seed.
    /// </summary>
    public WorldSeed Seed { get; }

    /// <summary>
    /// Gets the immutable chunk size.
    /// </summary>
    public int Size { get; }
}

using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Entities;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the passive world aggregate that owns every chunk in the simulation.
/// </summary>
public sealed class World : IAggregateRoot<WorldId>
{
    private readonly SortedDictionary<ChunkCoordinates, Chunk> chunks;

    /// <summary>
    /// Initializes a new instance of the <see cref="World"/> class.
    /// </summary>
    /// <param name="metadata">The immutable world metadata.</param>
    /// <param name="dimensions">The immutable world dimensions.</param>
    /// <param name="timeState">The stored world time state.</param>
    /// <param name="chunks">The chunks owned by the world.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="metadata"/>, <paramref name="dimensions"/>, <paramref name="timeState"/>,
    /// or <paramref name="chunks"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when a chunk belongs to another world or duplicates coordinates.</exception>
    public World(
        WorldMetadata metadata,
        WorldDimensions dimensions,
        WorldTimeState timeState,
        IReadOnlyList<Chunk> chunks)
    {
        Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        Dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
        TimeState = timeState ?? throw new ArgumentNullException(nameof(timeState));

        ArgumentNullException.ThrowIfNull(chunks);

        this.chunks = new SortedDictionary<ChunkCoordinates, Chunk>(Comparer<ChunkCoordinates>.Create(CompareCoordinates));

        foreach (Chunk chunk in chunks)
        {
            ArgumentNullException.ThrowIfNull(chunk);

            if (chunk.Metadata.WorldId != Metadata.WorldId)
            {
                throw new ArgumentException("Every chunk must belong to the owning world.", nameof(chunks));
            }

            if (!this.chunks.TryAdd(chunk.Metadata.Coordinates, chunk))
            {
                throw new ArgumentException("Chunk coordinates must be unique within a world.", nameof(chunks));
            }
        }
    }

    /// <summary>
    /// Gets the immutable world identifier.
    /// </summary>
    public WorldId Id => Metadata.WorldId;

    /// <summary>
    /// Gets the immutable world metadata.
    /// </summary>
    public WorldMetadata Metadata { get; }

    /// <summary>
    /// Gets the immutable world dimensions.
    /// </summary>
    public WorldDimensions Dimensions { get; }

    /// <summary>
    /// Gets the stored world time state.
    /// </summary>
    public WorldTimeState TimeState { get; }

    /// <summary>
    /// Gets the number of chunks owned by the world.
    /// </summary>
    public int ChunkCount => chunks.Count;

    /// <summary>
    /// Tries to get a chunk by coordinates.
    /// </summary>
    /// <param name="coordinates">The chunk coordinates.</param>
    /// <param name="chunk">The resolved chunk when present.</param>
    /// <returns><see langword="true"/> when the chunk exists; otherwise <see langword="false"/>.</returns>
    public bool TryGetChunk(ChunkCoordinates coordinates, out Chunk? chunk)
    {
        return chunks.TryGetValue(coordinates, out chunk);
    }

    /// <summary>
    /// Gets a chunk by coordinates.
    /// </summary>
    /// <param name="coordinates">The chunk coordinates.</param>
    /// <returns>The resolved chunk.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the chunk does not exist.</exception>
    public Chunk GetChunk(ChunkCoordinates coordinates)
    {
        if (!TryGetChunk(coordinates, out Chunk? chunk))
        {
            throw new KeyNotFoundException("The requested chunk does not exist in the world.");
        }

        return chunk!;
    }

    /// <summary>
    /// Returns the chunks owned by the world in deterministic coordinate order.
    /// </summary>
    /// <returns>The world chunks in deterministic coordinate order.</returns>
    public IReadOnlyList<Chunk> GetChunks()
    {
        List<Chunk> orderedChunks = new(chunks.Count);
        foreach (KeyValuePair<ChunkCoordinates, Chunk> pair in chunks)
        {
            orderedChunks.Add(pair.Value);
        }

        return orderedChunks.AsReadOnly();
    }

    private static int CompareCoordinates(ChunkCoordinates left, ChunkCoordinates right)
    {
        int yComparison = left.Y.CompareTo(right.Y);
        if (yComparison != 0)
        {
            return yComparison;
        }

        return left.X.CompareTo(right.X);
    }
}

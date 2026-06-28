using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Entities;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents a passive chunk entity owned by a world aggregate.
/// </summary>
public sealed class Chunk : IEntity<ChunkId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Chunk"/> class.
    /// </summary>
    /// <param name="metadata">The immutable chunk metadata.</param>
    /// <param name="state">The runtime chunk state.</param>
    /// <param name="organismIds">The organism references currently contained by the chunk.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="metadata"/> or <paramref name="organismIds"/> is <see langword="null"/>.</exception>
    public Chunk(ChunkMetadata metadata, ChunkState state, IReadOnlyList<OrganismId> organismIds)
    {
        Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        OrganismIds = organismIds ?? throw new ArgumentNullException(nameof(organismIds));
        State = state;
    }

    /// <summary>
    /// Gets the immutable chunk identifier.
    /// </summary>
    public ChunkId Id => Metadata.ChunkId;

    /// <summary>
    /// Gets the immutable chunk metadata.
    /// </summary>
    public ChunkMetadata Metadata { get; }

    /// <summary>
    /// Gets the runtime chunk state.
    /// </summary>
    public ChunkState State { get; }

    /// <summary>
    /// Gets the immutable organism references currently contained by the chunk.
    /// </summary>
    public IReadOnlyList<OrganismId> OrganismIds { get; }
}

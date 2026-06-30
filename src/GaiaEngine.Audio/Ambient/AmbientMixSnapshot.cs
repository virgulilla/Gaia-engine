using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Audio.Ambient;

/// <summary>
/// Represents one deterministic ambient mix snapshot reconstructed from current world state.
/// </summary>
public sealed record AmbientMixSnapshot
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AmbientMixSnapshot"/> class.
    /// </summary>
    /// <param name="tick">The world tick represented by this snapshot.</param>
    /// <param name="focusChunkId">The focus chunk identifier used to resolve local ambience.</param>
    /// <param name="layers">The active ambient layers in deterministic blending order.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="layers"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="tick"/> is negative.</exception>
    public AmbientMixSnapshot(long tick, ChunkId focusChunkId, IReadOnlyList<AmbientLayerState> layers)
    {
        if (tick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tick), "The ambient snapshot tick must be zero or greater.");
        }

        Tick = tick;
        FocusChunkId = focusChunkId;
        Layers = layers ?? throw new ArgumentNullException(nameof(layers));
    }

    /// <summary>
    /// Gets the world tick represented by this snapshot.
    /// </summary>
    public long Tick { get; }

    /// <summary>
    /// Gets the focus chunk identifier used to resolve local ambience.
    /// </summary>
    public ChunkId FocusChunkId { get; }

    /// <summary>
    /// Gets the active ambient layers in deterministic blending order.
    /// </summary>
    public IReadOnlyList<AmbientLayerState> Layers { get; }
}

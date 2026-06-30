using System;
using GaiaEngine.Audio.Events;

namespace GaiaEngine.Audio.Ambient;

/// <summary>
/// Represents one active ambient audio layer resolved from world state.
/// </summary>
public sealed record AmbientLayerState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AmbientLayerState"/> class.
    /// </summary>
    /// <param name="layerId">The stable logical layer identifier.</param>
    /// <param name="kind">The resolved ambient layer kind.</param>
    /// <param name="spatialScope">The resolved spatial scope.</param>
    /// <param name="priority">The deterministic blending priority.</param>
    /// <param name="audioClipId">The logical audio clip identifier.</param>
    /// <param name="volume">The normalized target volume in the inclusive range [0, 1].</param>
    /// <param name="transitionTicks">The preferred transition duration measured in ticks.</param>
    /// <param name="spatialProfile">The optional spatial playback profile.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="layerId"/> or <paramref name="audioClipId"/> is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="priority"/>, <paramref name="transitionTicks"/>, or <paramref name="volume"/> is out of range.
    /// </exception>
    public AmbientLayerState(
        string layerId,
        AmbientLayerKind kind,
        AmbientSpatialScope spatialScope,
        int priority,
        string audioClipId,
        float volume,
        int transitionTicks,
        AudioSpatialProfile? spatialProfile)
    {
        if (string.IsNullOrWhiteSpace(layerId))
        {
            throw new ArgumentException("The ambient layer identifier must contain a value.", nameof(layerId));
        }

        if (string.IsNullOrWhiteSpace(audioClipId))
        {
            throw new ArgumentException("The ambient clip identifier must contain a value.", nameof(audioClipId));
        }

        if (priority < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(priority), "The ambient priority must be zero or greater.");
        }

        if (volume < 0f || volume > 1f)
        {
            throw new ArgumentOutOfRangeException(nameof(volume), "The ambient volume must be between 0 and 1.");
        }

        if (transitionTicks < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(transitionTicks), "The transition duration must be zero or greater.");
        }

        LayerId = layerId;
        Kind = kind;
        SpatialScope = spatialScope;
        Priority = priority;
        AudioClipId = audioClipId;
        Volume = volume;
        TransitionTicks = transitionTicks;
        SpatialProfile = spatialProfile;
    }

    /// <summary>
    /// Gets the stable logical layer identifier.
    /// </summary>
    public string LayerId { get; }

    /// <summary>
    /// Gets the resolved ambient layer kind.
    /// </summary>
    public AmbientLayerKind Kind { get; }

    /// <summary>
    /// Gets the resolved spatial scope.
    /// </summary>
    public AmbientSpatialScope SpatialScope { get; }

    /// <summary>
    /// Gets the deterministic blending priority.
    /// </summary>
    public int Priority { get; }

    /// <summary>
    /// Gets the logical audio clip identifier.
    /// </summary>
    public string AudioClipId { get; }

    /// <summary>
    /// Gets the normalized target volume in the inclusive range [0, 1].
    /// </summary>
    public float Volume { get; }

    /// <summary>
    /// Gets the preferred transition duration measured in ticks.
    /// </summary>
    public int TransitionTicks { get; }

    /// <summary>
    /// Gets the optional spatial playback profile.
    /// </summary>
    public AudioSpatialProfile? SpatialProfile { get; }
}

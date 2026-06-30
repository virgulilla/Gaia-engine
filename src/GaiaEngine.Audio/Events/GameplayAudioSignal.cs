using System;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Audio.Events;

/// <summary>
/// Represents one deterministic gameplay audio input signal.
/// </summary>
public sealed record GameplayAudioSignal
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameplayAudioSignal"/> class.
    /// </summary>
    /// <param name="eventId">The immutable source event identifier.</param>
    /// <param name="kind">The gameplay signal kind.</param>
    /// <param name="timestamp">The deterministic timestamp.</param>
    /// <param name="spatialProfile">The optional spatial playback profile.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="timestamp"/> is negative.</exception>
    public GameplayAudioSignal(EventId eventId, GameplayAudioSignalKind kind, long timestamp, AudioSpatialProfile? spatialProfile = null)
    {
        if (timestamp < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(timestamp), "The gameplay audio timestamp must be zero or greater.");
        }

        EventId = eventId;
        Kind = kind;
        Timestamp = timestamp;
        SpatialProfile = spatialProfile;
    }

    /// <summary>
    /// Gets the immutable source event identifier.
    /// </summary>
    public EventId EventId { get; }

    /// <summary>
    /// Gets the gameplay signal kind.
    /// </summary>
    public GameplayAudioSignalKind Kind { get; }

    /// <summary>
    /// Gets the deterministic timestamp.
    /// </summary>
    public long Timestamp { get; }

    /// <summary>
    /// Gets the optional spatial playback profile.
    /// </summary>
    public AudioSpatialProfile? SpatialProfile { get; }
}

using System;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Audio.Events;

/// <summary>
/// Represents one runtime audio event translated from simulation or gameplay activity.
/// </summary>
public sealed record AudioEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioEvent"/> class.
    /// </summary>
    /// <param name="eventId">The immutable event identifier.</param>
    /// <param name="category">The audio event category.</param>
    /// <param name="priority">The playback priority.</param>
    /// <param name="spatialProfile">The optional spatial playback profile.</param>
    /// <param name="timestamp">The deterministic timestamp.</param>
    /// <param name="audioClipId">The logical audio clip identifier.</param>
    /// <param name="playbackRules">The deterministic playback rules.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="audioClipId"/> is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="playbackRules"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="timestamp"/> is negative.</exception>
    public AudioEvent(
        EventId eventId,
        AudioEventCategory category,
        AudioEventPriority priority,
        AudioSpatialProfile? spatialProfile,
        long timestamp,
        string audioClipId,
        AudioPlaybackRules playbackRules)
    {
        if (timestamp < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(timestamp), "The audio timestamp must be zero or greater.");
        }

        if (string.IsNullOrWhiteSpace(audioClipId))
        {
            throw new ArgumentException("The audio clip identifier must contain a value.", nameof(audioClipId));
        }

        EventId = eventId;
        Category = category;
        Priority = priority;
        SpatialProfile = spatialProfile;
        Timestamp = timestamp;
        AudioClipId = audioClipId;
        PlaybackRules = playbackRules ?? throw new ArgumentNullException(nameof(playbackRules));
    }

    /// <summary>
    /// Gets the immutable event identifier.
    /// </summary>
    public EventId EventId { get; }

    /// <summary>
    /// Gets the audio event category.
    /// </summary>
    public AudioEventCategory Category { get; }

    /// <summary>
    /// Gets the playback priority.
    /// </summary>
    public AudioEventPriority Priority { get; }

    /// <summary>
    /// Gets the optional spatial playback profile.
    /// </summary>
    public AudioSpatialProfile? SpatialProfile { get; }

    /// <summary>
    /// Gets the deterministic timestamp.
    /// </summary>
    public long Timestamp { get; }

    /// <summary>
    /// Gets the logical audio clip identifier.
    /// </summary>
    public string AudioClipId { get; }

    /// <summary>
    /// Gets the deterministic playback rules.
    /// </summary>
    public AudioPlaybackRules PlaybackRules { get; }
}

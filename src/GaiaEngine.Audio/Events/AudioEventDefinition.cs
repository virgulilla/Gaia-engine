using System;

namespace GaiaEngine.Audio.Events;

/// <summary>
/// Represents one configurable translation rule from a source key into an audio event.
/// </summary>
public sealed record AudioEventDefinition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioEventDefinition"/> class.
    /// </summary>
    /// <param name="sourceKey">The stable source key to match.</param>
    /// <param name="category">The audio event category.</param>
    /// <param name="priority">The playback priority.</param>
    /// <param name="audioClipId">The logical audio clip identifier.</param>
    /// <param name="spatial">Whether the event should be spatialized.</param>
    /// <param name="playbackRules">The deterministic playback rules.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="sourceKey"/> or <paramref name="audioClipId"/> is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="playbackRules"/> is <see langword="null"/>.</exception>
    public AudioEventDefinition(
        string sourceKey,
        AudioEventCategory category,
        AudioEventPriority priority,
        string audioClipId,
        bool spatial,
        AudioPlaybackRules playbackRules)
    {
        if (string.IsNullOrWhiteSpace(sourceKey))
        {
            throw new ArgumentException("The audio source key must contain a value.", nameof(sourceKey));
        }

        if (string.IsNullOrWhiteSpace(audioClipId))
        {
            throw new ArgumentException("The audio clip identifier must contain a value.", nameof(audioClipId));
        }

        SourceKey = sourceKey;
        Category = category;
        Priority = priority;
        AudioClipId = audioClipId;
        Spatial = spatial;
        PlaybackRules = playbackRules ?? throw new ArgumentNullException(nameof(playbackRules));
    }

    /// <summary>
    /// Gets the stable source key to match.
    /// </summary>
    public string SourceKey { get; }

    /// <summary>
    /// Gets the audio event category.
    /// </summary>
    public AudioEventCategory Category { get; }

    /// <summary>
    /// Gets the playback priority.
    /// </summary>
    public AudioEventPriority Priority { get; }

    /// <summary>
    /// Gets the logical audio clip identifier.
    /// </summary>
    public string AudioClipId { get; }

    /// <summary>
    /// Gets a value indicating whether the event should be spatialized.
    /// </summary>
    public bool Spatial { get; }

    /// <summary>
    /// Gets the deterministic playback rules.
    /// </summary>
    public AudioPlaybackRules PlaybackRules { get; }
}

using System;
using System.Collections.Generic;

namespace GaiaEngine.Audio.Events;

/// <summary>
/// Represents the deterministic result of one audio event translation pass.
/// </summary>
public sealed record AudioEventBatchResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioEventBatchResult"/> class.
    /// </summary>
    /// <param name="audioEvents">The translated audio events.</param>
    /// <param name="playbackRequests">The forwarded playback requests.</param>
    /// <param name="mergedEventCount">The number of merged duplicate events.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="audioEvents"/> or <paramref name="playbackRequests"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="mergedEventCount"/> is negative.</exception>
    public AudioEventBatchResult(
        IReadOnlyList<AudioEvent> audioEvents,
        IReadOnlyList<AudioPlaybackRequest> playbackRequests,
        int mergedEventCount)
    {
        if (mergedEventCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(mergedEventCount), "The merged event count must be zero or greater.");
        }

        AudioEvents = audioEvents ?? throw new ArgumentNullException(nameof(audioEvents));
        PlaybackRequests = playbackRequests ?? throw new ArgumentNullException(nameof(playbackRequests));
        MergedEventCount = mergedEventCount;
    }

    /// <summary>
    /// Gets the translated audio events.
    /// </summary>
    public IReadOnlyList<AudioEvent> AudioEvents { get; }

    /// <summary>
    /// Gets the forwarded playback requests.
    /// </summary>
    public IReadOnlyList<AudioPlaybackRequest> PlaybackRequests { get; }

    /// <summary>
    /// Gets the number of merged duplicate events.
    /// </summary>
    public int MergedEventCount { get; }
}

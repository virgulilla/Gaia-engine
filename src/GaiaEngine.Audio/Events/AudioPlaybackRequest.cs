using System;

namespace GaiaEngine.Audio.Events;

/// <summary>
/// Represents one runtime playback request forwarded to the audio system.
/// </summary>
public sealed record AudioPlaybackRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioPlaybackRequest"/> class.
    /// </summary>
    /// <param name="audioEvent">The translated audio event to forward.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="audioEvent"/> is <see langword="null"/>.</exception>
    public AudioPlaybackRequest(AudioEvent audioEvent)
    {
        AudioEvent = audioEvent ?? throw new ArgumentNullException(nameof(audioEvent));
    }

    /// <summary>
    /// Gets the translated audio event to forward.
    /// </summary>
    public AudioEvent AudioEvent { get; }
}

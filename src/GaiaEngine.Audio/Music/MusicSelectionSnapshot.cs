using System;

namespace GaiaEngine.Audio.Music;

/// <summary>
/// Represents one deterministic music selection snapshot.
/// </summary>
public sealed record MusicSelectionSnapshot
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MusicSelectionSnapshot"/> class.
    /// </summary>
    /// <param name="timestamp">The deterministic timestamp associated with the selection.</param>
    /// <param name="context">The presentation context that requested the selection.</param>
    /// <param name="theme">The resolved music theme.</param>
    /// <param name="trackId">The logical track identifier.</param>
    /// <param name="reason">The short deterministic reason for the selection.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="theme"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="trackId"/> or <paramref name="reason"/> is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="timestamp"/> is negative.</exception>
    public MusicSelectionSnapshot(long timestamp, MusicPresentationContext context, MusicThemeDefinition theme, string trackId, string reason)
    {
        if (timestamp < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(timestamp), "The music selection timestamp must be zero or greater.");
        }

        if (string.IsNullOrWhiteSpace(trackId))
        {
            throw new ArgumentException("The music track identifier must contain a value.", nameof(trackId));
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("The music selection reason must contain a value.", nameof(reason));
        }

        Timestamp = timestamp;
        Context = context;
        Theme = theme ?? throw new ArgumentNullException(nameof(theme));
        TrackId = trackId;
        Reason = reason;
    }

    /// <summary>
    /// Gets the deterministic timestamp associated with the selection.
    /// </summary>
    public long Timestamp { get; }

    /// <summary>
    /// Gets the presentation context that requested the selection.
    /// </summary>
    public MusicPresentationContext Context { get; }

    /// <summary>
    /// Gets the resolved music theme.
    /// </summary>
    public MusicThemeDefinition Theme { get; }

    /// <summary>
    /// Gets the resolved logical track identifier.
    /// </summary>
    public string TrackId { get; }

    /// <summary>
    /// Gets the short deterministic reason for the selection.
    /// </summary>
    public string Reason { get; }
}

using System;

namespace GaiaEngine.Audio.Music;

/// <summary>
/// Represents one deterministic music theme definition.
/// </summary>
public sealed record MusicThemeDefinition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MusicThemeDefinition"/> class.
    /// </summary>
    /// <param name="themeId">The stable theme identifier.</param>
    /// <param name="themeKind">The music theme kind.</param>
    /// <param name="primaryState">The primary music state represented by the theme.</param>
    /// <param name="priority">The playback priority.</param>
    /// <param name="trackPrefix">The logical track prefix used to resolve adaptive variants.</param>
    /// <param name="playbackRules">The deterministic playback rules.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="themeId"/> or <paramref name="trackPrefix"/> is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="playbackRules"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="priority"/> is negative.</exception>
    public MusicThemeDefinition(
        string themeId,
        MusicThemeKind themeKind,
        MusicPrimaryState primaryState,
        int priority,
        string trackPrefix,
        MusicPlaybackRules playbackRules)
    {
        if (string.IsNullOrWhiteSpace(themeId))
        {
            throw new ArgumentException("The music theme identifier must contain a value.", nameof(themeId));
        }

        if (string.IsNullOrWhiteSpace(trackPrefix))
        {
            throw new ArgumentException("The music track prefix must contain a value.", nameof(trackPrefix));
        }

        if (priority < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(priority), "The music priority must be zero or greater.");
        }

        ThemeId = themeId;
        ThemeKind = themeKind;
        PrimaryState = primaryState;
        Priority = priority;
        TrackPrefix = trackPrefix;
        PlaybackRules = playbackRules ?? throw new ArgumentNullException(nameof(playbackRules));
    }

    /// <summary>
    /// Gets the stable theme identifier.
    /// </summary>
    public string ThemeId { get; }

    /// <summary>
    /// Gets the music theme kind.
    /// </summary>
    public MusicThemeKind ThemeKind { get; }

    /// <summary>
    /// Gets the primary music state represented by the theme.
    /// </summary>
    public MusicPrimaryState PrimaryState { get; }

    /// <summary>
    /// Gets the playback priority.
    /// </summary>
    public int Priority { get; }

    /// <summary>
    /// Gets the logical track prefix used to resolve adaptive variants.
    /// </summary>
    public string TrackPrefix { get; }

    /// <summary>
    /// Gets the deterministic playback rules.
    /// </summary>
    public MusicPlaybackRules PlaybackRules { get; }
}

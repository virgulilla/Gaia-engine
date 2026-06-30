namespace GaiaEngine.Audio.Events;

/// <summary>
/// Defines the supported audio playback priorities.
/// </summary>
public enum AudioEventPriority
{
    /// <summary>
    /// Represents ambient playback.
    /// </summary>
    Ambient = 0,

    /// <summary>
    /// Represents normal playback.
    /// </summary>
    Normal = 1,

    /// <summary>
    /// Represents important playback.
    /// </summary>
    Important = 2,

    /// <summary>
    /// Represents critical playback.
    /// </summary>
    Critical = 3,
}

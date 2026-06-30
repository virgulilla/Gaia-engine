namespace GaiaEngine.Audio.Music;

/// <summary>
/// Defines the supported primary music states.
/// </summary>
public enum MusicPrimaryState
{
    /// <summary>
    /// Identifies silent playback.
    /// </summary>
    Silent = 0,

    /// <summary>
    /// Identifies ambient background playback.
    /// </summary>
    Ambient = 1,

    /// <summary>
    /// Identifies exploration playback.
    /// </summary>
    Exploration = 2,

    /// <summary>
    /// Identifies discovery playback.
    /// </summary>
    Discovery = 3,

    /// <summary>
    /// Identifies tension playback.
    /// </summary>
    Tension = 4,

    /// <summary>
    /// Identifies event playback.
    /// </summary>
    Event = 5,

    /// <summary>
    /// Identifies menu playback.
    /// </summary>
    Menu = 6,
}

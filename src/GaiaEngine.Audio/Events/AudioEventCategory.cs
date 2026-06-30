namespace GaiaEngine.Audio.Events;

/// <summary>
/// Defines the supported audio event categories.
/// </summary>
public enum AudioEventCategory
{
    /// <summary>
    /// Represents organism-related audio events.
    /// </summary>
    Organisms = 0,

    /// <summary>
    /// Represents environment-related audio events.
    /// </summary>
    Environment = 1,

    /// <summary>
    /// Represents weather-related audio events.
    /// </summary>
    Weather = 2,

    /// <summary>
    /// Represents gameplay-related audio events.
    /// </summary>
    Gameplay = 3,

    /// <summary>
    /// Represents user-interface-related audio events.
    /// </summary>
    UI = 4,

    /// <summary>
    /// Represents system-related audio events.
    /// </summary>
    System = 5,
}

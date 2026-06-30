namespace GaiaEngine.Audio.SoundEffects;

/// <summary>
/// Defines the supported sound mixer groups.
/// </summary>
public enum SoundMixerGroup
{
    /// <summary>
    /// Represents the master mixer group.
    /// </summary>
    Master = 0,

    /// <summary>
    /// Represents the music mixer group.
    /// </summary>
    Music = 1,

    /// <summary>
    /// Represents the ambience mixer group.
    /// </summary>
    Ambience = 2,

    /// <summary>
    /// Represents the user-interface mixer group.
    /// </summary>
    UI = 3,

    /// <summary>
    /// Represents the creatures mixer group.
    /// </summary>
    Creatures = 4,

    /// <summary>
    /// Represents the environment mixer group.
    /// </summary>
    Environment = 5,
}

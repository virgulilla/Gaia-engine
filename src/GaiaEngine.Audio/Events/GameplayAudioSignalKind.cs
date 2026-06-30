namespace GaiaEngine.Audio.Events;

/// <summary>
/// Defines the supported gameplay-origin audio signal kinds.
/// </summary>
public enum GameplayAudioSignalKind
{
    /// <summary>
    /// Represents one discovery signal.
    /// </summary>
    Discovery = 0,

    /// <summary>
    /// Represents one completed objective signal.
    /// </summary>
    ObjectiveComplete = 1,

    /// <summary>
    /// Represents one unlocked achievement signal.
    /// </summary>
    AchievementUnlocked = 2,

    /// <summary>
    /// Represents one progression unlock signal.
    /// </summary>
    UnlockGranted = 3,

    /// <summary>
    /// Represents one warning signal.
    /// </summary>
    Warning = 4,
}

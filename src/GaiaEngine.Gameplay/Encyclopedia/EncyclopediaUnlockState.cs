namespace GaiaEngine.Gameplay.Encyclopedia;

/// <summary>
/// Defines the unlock state of one encyclopedia entry.
/// </summary>
public enum EncyclopediaUnlockState
{
    /// <summary>
    /// Represents an entry that remains hidden.
    /// </summary>
    Hidden = 0,

    /// <summary>
    /// Represents an entry that has been discovered.
    /// </summary>
    Discovered = 1,

    /// <summary>
    /// Represents an entry that has been fully completed.
    /// </summary>
    Complete = 2,
}

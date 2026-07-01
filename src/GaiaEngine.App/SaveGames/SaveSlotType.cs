namespace GaiaEngine.App.SaveGames;

/// <summary>
/// Defines the supported persistent save slot types.
/// </summary>
public enum SaveSlotType
{
    /// <summary>
    /// Represents a manual save slot.
    /// </summary>
    Manual = 0,

    /// <summary>
    /// Represents an auto save slot.
    /// </summary>
    Auto = 1,

    /// <summary>
    /// Represents the shared quick save slot.
    /// </summary>
    Quick = 2,
}

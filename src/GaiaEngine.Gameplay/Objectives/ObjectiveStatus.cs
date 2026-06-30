namespace GaiaEngine.Gameplay.Objectives;

/// <summary>
/// Defines the supported objective lifecycle states.
/// </summary>
public enum ObjectiveStatus
{
    /// <summary>
    /// Represents an objective that exists but is not yet available.
    /// </summary>
    Locked = 0,

    /// <summary>
    /// Represents an objective that is currently tracking progress.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Represents an objective that has been completed.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Represents an objective that can no longer be completed.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// Represents an objective that remains hidden until triggered.
    /// </summary>
    Hidden = 4,
}

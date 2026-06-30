namespace GaiaEngine.Godot.UI.Notifications;

/// <summary>
/// Defines the supported HUD notification categories.
/// </summary>
public enum HudNotificationCategory
{
    /// <summary>
    /// Identifies information notifications.
    /// </summary>
    Information = 0,

    /// <summary>
    /// Identifies discovery notifications.
    /// </summary>
    Discovery = 1,

    /// <summary>
    /// Identifies objective notifications.
    /// </summary>
    Objective = 2,

    /// <summary>
    /// Identifies achievement notifications.
    /// </summary>
    Achievement = 3,

    /// <summary>
    /// Identifies warning notifications.
    /// </summary>
    Warning = 4,

    /// <summary>
    /// Identifies critical notifications.
    /// </summary>
    Critical = 5,

    /// <summary>
    /// Identifies system notifications.
    /// </summary>
    System = 6,
}

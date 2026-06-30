namespace GaiaEngine.Godot.UI.Notifications;

/// <summary>
/// Defines the supported HUD notification priorities.
/// </summary>
public enum HudNotificationPriority
{
    /// <summary>
    /// Identifies low-priority notifications.
    /// </summary>
    Low = 0,

    /// <summary>
    /// Identifies normal-priority notifications.
    /// </summary>
    Normal = 1,

    /// <summary>
    /// Identifies high-priority notifications.
    /// </summary>
    High = 2,

    /// <summary>
    /// Identifies critical notifications.
    /// </summary>
    Critical = 3,
}

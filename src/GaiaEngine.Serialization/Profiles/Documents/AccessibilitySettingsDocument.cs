namespace GaiaEngine.Serialization.Profiles.Documents;

/// <summary>
/// Represents the serialized accessibility settings section.
/// </summary>
internal sealed class AccessibilitySettingsDocument
{
    /// <summary>
    /// Gets or sets a value indicating whether high contrast mode is enabled.
    /// </summary>
    public bool HighContrastMode { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether large text mode is enabled.
    /// </summary>
    public bool LargeText { get; set; }

    /// <summary>
    /// Gets or sets the UI scale percentage.
    /// </summary>
    public int UiScalePercent { get; set; }

    /// <summary>
    /// Gets or sets the selected color profile.
    /// </summary>
    public string ColorProfile { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether reduced motion is enabled.
    /// </summary>
    public bool ReducedMotion { get; set; }

    /// <summary>
    /// Gets or sets the subtitle size percentage.
    /// </summary>
    public int SubtitleSizePercent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether simplified notifications are enabled.
    /// </summary>
    public bool SimplifiedNotifications { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether visual event indicators are enabled.
    /// </summary>
    public bool VisualEventIndicators { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether large touch targets are enabled.
    /// </summary>
    public bool LargeTouchTargets { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether toggle input is preferred over hold input.
    /// </summary>
    public bool ToggleInsteadOfHold { get; set; }

    /// <summary>
    /// Gets or sets the hold duration in milliseconds.
    /// </summary>
    public int HoldDurationMilliseconds { get; set; }
}

namespace GaiaEngine.Serialization.Profiles.Documents;

/// <summary>
/// Represents the serialized player settings section.
/// </summary>
internal sealed class PlayerSettingsDocument
{
    /// <summary>
    /// Gets or sets the preferred language code.
    /// </summary>
    public string Language { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the serialized accessibility section.
    /// </summary>
    public AccessibilitySettingsDocument? Accessibility { get; set; }

    /// <summary>
    /// Gets or sets the preferred brightness percentage.
    /// </summary>
    public int BrightnessPercent { get; set; }

    /// <summary>
    /// Gets or sets the master volume percentage.
    /// </summary>
    public int MasterVolumePercent { get; set; }

    /// <summary>
    /// Gets or sets the music volume percentage.
    /// </summary>
    public int MusicVolumePercent { get; set; }

    /// <summary>
    /// Gets or sets the effects volume percentage.
    /// </summary>
    public int EffectsVolumePercent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether controller support hints are enabled.
    /// </summary>
    public bool ControllerSupportEnabled { get; set; }
}

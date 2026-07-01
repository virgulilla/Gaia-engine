using System;

namespace GaiaEngine.Gameplay.Player;

/// <summary>
/// Represents presentation and input preferences stored in one player profile.
/// </summary>
public sealed record PlayerSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerSettings"/> class.
    /// </summary>
    /// <param name="language">The preferred language code.</param>
    /// <param name="accessibility">The accessibility preferences.</param>
    /// <param name="brightnessPercent">The preferred brightness percentage.</param>
    /// <param name="masterVolumePercent">The master volume percentage.</param>
    /// <param name="musicVolumePercent">The music volume percentage.</param>
    /// <param name="effectsVolumePercent">The effects volume percentage.</param>
    /// <param name="controllerSupportEnabled">Whether controller support hints are enabled.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="language"/> is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="accessibility"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one percentage is outside the supported range.</exception>
    public PlayerSettings(
        string language,
        AccessibilitySettings accessibility,
        int brightnessPercent,
        int masterVolumePercent,
        int musicVolumePercent,
        int effectsVolumePercent,
        bool controllerSupportEnabled)
    {
        if (string.IsNullOrWhiteSpace(language))
        {
            throw new ArgumentException("The preferred language must contain a value.", nameof(language));
        }

        if (brightnessPercent < 50 || brightnessPercent > 150)
        {
            throw new ArgumentOutOfRangeException(nameof(brightnessPercent), "The brightness percentage must be between 50 and 150.");
        }

        if (masterVolumePercent < 0 || masterVolumePercent > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(masterVolumePercent), "The master volume percentage must be between 0 and 100.");
        }

        if (musicVolumePercent < 0 || musicVolumePercent > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(musicVolumePercent), "The music volume percentage must be between 0 and 100.");
        }

        if (effectsVolumePercent < 0 || effectsVolumePercent > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(effectsVolumePercent), "The effects volume percentage must be between 0 and 100.");
        }

        Language = language;
        Accessibility = accessibility ?? throw new ArgumentNullException(nameof(accessibility));
        BrightnessPercent = brightnessPercent;
        MasterVolumePercent = masterVolumePercent;
        MusicVolumePercent = musicVolumePercent;
        EffectsVolumePercent = effectsVolumePercent;
        ControllerSupportEnabled = controllerSupportEnabled;
    }

    /// <summary>
    /// Gets the shared default player settings.
    /// </summary>
    public static PlayerSettings Default { get; } = new(
        language: "en",
        accessibility: AccessibilitySettings.Default,
        brightnessPercent: 100,
        masterVolumePercent: 100,
        musicVolumePercent: 80,
        effectsVolumePercent: 90,
        controllerSupportEnabled: true);

    /// <summary>
    /// Gets the preferred language code.
    /// </summary>
    public string Language { get; }

    /// <summary>
    /// Gets the accessibility preferences.
    /// </summary>
    public AccessibilitySettings Accessibility { get; }

    /// <summary>
    /// Gets the preferred brightness percentage.
    /// </summary>
    public int BrightnessPercent { get; }

    /// <summary>
    /// Gets the master volume percentage.
    /// </summary>
    public int MasterVolumePercent { get; }

    /// <summary>
    /// Gets the music volume percentage.
    /// </summary>
    public int MusicVolumePercent { get; }

    /// <summary>
    /// Gets the effects volume percentage.
    /// </summary>
    public int EffectsVolumePercent { get; }

    /// <summary>
    /// Gets a value indicating whether controller support hints are enabled.
    /// </summary>
    public bool ControllerSupportEnabled { get; }
}

using System;

namespace GaiaEngine.Gameplay.Player;

/// <summary>
/// Represents presentation-only accessibility preferences owned by one player profile.
/// </summary>
public sealed record AccessibilitySettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AccessibilitySettings"/> class.
    /// </summary>
    /// <param name="highContrastMode">Whether high contrast mode is enabled.</param>
    /// <param name="largeText">Whether large text mode is enabled.</param>
    /// <param name="uiScalePercent">The requested user interface scale percentage.</param>
    /// <param name="colorProfile">The selected accessibility color profile.</param>
    /// <param name="reducedMotion">Whether reduced motion is enabled.</param>
    /// <param name="subtitleSizePercent">The subtitle size percentage.</param>
    /// <param name="simplifiedNotifications">Whether simplified notifications are enabled.</param>
    /// <param name="visualEventIndicators">Whether audio cues should always have visual indicators.</param>
    /// <param name="largeTouchTargets">Whether large touch targets are enabled.</param>
    /// <param name="toggleInsteadOfHold">Whether toggle input is preferred over hold input.</param>
    /// <param name="holdDurationMilliseconds">The required hold duration in milliseconds.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one numeric preference is outside the supported range.</exception>
    public AccessibilitySettings(
        bool highContrastMode,
        bool largeText,
        int uiScalePercent,
        AccessibilityColorProfile colorProfile,
        bool reducedMotion,
        int subtitleSizePercent,
        bool simplifiedNotifications,
        bool visualEventIndicators,
        bool largeTouchTargets,
        bool toggleInsteadOfHold,
        int holdDurationMilliseconds)
    {
        if (uiScalePercent is not (75 or 100 or 125 or 150 or 200))
        {
            throw new ArgumentOutOfRangeException(nameof(uiScalePercent), "The UI scale must match one of the supported accessibility scale percentages.");
        }

        if (subtitleSizePercent is not (75 or 100 or 125 or 150 or 200))
        {
            throw new ArgumentOutOfRangeException(nameof(subtitleSizePercent), "The subtitle size must match one of the supported accessibility scale percentages.");
        }

        if (holdDurationMilliseconds < 0 || holdDurationMilliseconds > 2000)
        {
            throw new ArgumentOutOfRangeException(nameof(holdDurationMilliseconds), "The hold duration must be between 0 and 2000 milliseconds.");
        }

        HighContrastMode = highContrastMode;
        LargeText = largeText;
        UiScalePercent = uiScalePercent;
        ColorProfile = colorProfile;
        ReducedMotion = reducedMotion;
        SubtitleSizePercent = subtitleSizePercent;
        SimplifiedNotifications = simplifiedNotifications;
        VisualEventIndicators = visualEventIndicators;
        LargeTouchTargets = largeTouchTargets;
        ToggleInsteadOfHold = toggleInsteadOfHold;
        HoldDurationMilliseconds = holdDurationMilliseconds;
    }

    /// <summary>
    /// Gets the shared default accessibility settings.
    /// </summary>
    public static AccessibilitySettings Default { get; } = new(
        highContrastMode: false,
        largeText: false,
        uiScalePercent: 100,
        colorProfile: AccessibilityColorProfile.None,
        reducedMotion: false,
        subtitleSizePercent: 100,
        simplifiedNotifications: false,
        visualEventIndicators: true,
        largeTouchTargets: false,
        toggleInsteadOfHold: false,
        holdDurationMilliseconds: 250);

    /// <summary>
    /// Gets a value indicating whether high contrast mode is enabled.
    /// </summary>
    public bool HighContrastMode { get; }

    /// <summary>
    /// Gets a value indicating whether large text mode is enabled.
    /// </summary>
    public bool LargeText { get; }

    /// <summary>
    /// Gets the requested user interface scale percentage.
    /// </summary>
    public int UiScalePercent { get; }

    /// <summary>
    /// Gets the selected accessibility color profile.
    /// </summary>
    public AccessibilityColorProfile ColorProfile { get; }

    /// <summary>
    /// Gets a value indicating whether reduced motion is enabled.
    /// </summary>
    public bool ReducedMotion { get; }

    /// <summary>
    /// Gets the subtitle size percentage.
    /// </summary>
    public int SubtitleSizePercent { get; }

    /// <summary>
    /// Gets a value indicating whether simplified notifications are enabled.
    /// </summary>
    public bool SimplifiedNotifications { get; }

    /// <summary>
    /// Gets a value indicating whether audio cues should always expose visual indicators.
    /// </summary>
    public bool VisualEventIndicators { get; }

    /// <summary>
    /// Gets a value indicating whether large touch targets are enabled.
    /// </summary>
    public bool LargeTouchTargets { get; }

    /// <summary>
    /// Gets a value indicating whether toggle input is preferred over hold input.
    /// </summary>
    public bool ToggleInsteadOfHold { get; }

    /// <summary>
    /// Gets the requested hold duration in milliseconds.
    /// </summary>
    public int HoldDurationMilliseconds { get; }
}

namespace GaiaEngine.Gameplay.Player;

/// <summary>
/// Defines the supported accessibility color profiles for presentation-only adaptation.
/// </summary>
public enum AccessibilityColorProfile
{
    /// <summary>
    /// Uses the default presentation palette.
    /// </summary>
    None = 0,

    /// <summary>
    /// Uses a palette adapted for protanopia.
    /// </summary>
    Protanopia = 1,

    /// <summary>
    /// Uses a palette adapted for deuteranopia.
    /// </summary>
    Deuteranopia = 2,

    /// <summary>
    /// Uses a palette adapted for tritanopia.
    /// </summary>
    Tritanopia = 3,
}

namespace GaiaEngine.Domain.World;

/// <summary>
/// Defines the supported deterministic terrain modifiers.
/// </summary>
public enum TerrainModifierType
{
    /// <summary>
    /// Identifies landslide effects.
    /// </summary>
    Landslide,

    /// <summary>
    /// Identifies flood damage.
    /// </summary>
    FloodDamage,

    /// <summary>
    /// Identifies erosion effects.
    /// </summary>
    Erosion,

    /// <summary>
    /// Identifies lava flow effects.
    /// </summary>
    LavaFlow,

    /// <summary>
    /// Identifies crater effects.
    /// </summary>
    Crater,
}

namespace GaiaEngine.Domain.World;

/// <summary>
/// Defines the supported deterministic surface layers used by terrain data.
/// </summary>
public enum SurfaceType
{
    /// <summary>
    /// Identifies rock surface.
    /// </summary>
    Rock,

    /// <summary>
    /// Identifies sand surface.
    /// </summary>
    Sand,

    /// <summary>
    /// Identifies grass surface.
    /// </summary>
    Grass,

    /// <summary>
    /// Identifies mud surface.
    /// </summary>
    Mud,

    /// <summary>
    /// Identifies snow surface.
    /// </summary>
    Snow,

    /// <summary>
    /// Identifies ice surface.
    /// </summary>
    Ice,
}

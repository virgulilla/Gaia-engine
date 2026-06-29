namespace GaiaEngine.Domain.World;

/// <summary>
/// Defines the supported deterministic dominant vegetation types used by biome data.
/// </summary>
public enum VegetationType
{
    /// <summary>
    /// Identifies no dominant vegetation.
    /// </summary>
    None,

    /// <summary>
    /// Identifies forest vegetation.
    /// </summary>
    Forest,

    /// <summary>
    /// Identifies grassland vegetation.
    /// </summary>
    Grassland,

    /// <summary>
    /// Identifies shrub vegetation.
    /// </summary>
    Shrubs,

    /// <summary>
    /// Identifies moss vegetation.
    /// </summary>
    Moss,
}

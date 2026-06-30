namespace GaiaEngine.Gameplay.Discovery;

/// <summary>
/// Defines the supported discovery categories available to player knowledge.
/// </summary>
public enum DiscoveryCategory
{
    /// <summary>
    /// Represents species discoveries.
    /// </summary>
    Species = 0,

    /// <summary>
    /// Represents trait discoveries.
    /// </summary>
    Traits = 1,

    /// <summary>
    /// Represents biome discoveries.
    /// </summary>
    Biomes = 2,

    /// <summary>
    /// Represents resource discoveries.
    /// </summary>
    Resources = 3,

    /// <summary>
    /// Represents behaviour discoveries.
    /// </summary>
    Behaviours = 4,

    /// <summary>
    /// Represents climate discoveries.
    /// </summary>
    Climate = 5,

    /// <summary>
    /// Represents world event discoveries.
    /// </summary>
    WorldEvents = 6,
}

namespace GaiaEngine.Audio.Ambient;

/// <summary>
/// Defines the supported ambient audio layer kinds.
/// </summary>
public enum AmbientLayerKind
{
    /// <summary>
    /// Identifies the always-on global ambience layer.
    /// </summary>
    Global = 0,

    /// <summary>
    /// Identifies the biome ambience layer.
    /// </summary>
    Biome = 1,

    /// <summary>
    /// Identifies the weather ambience layer.
    /// </summary>
    Weather = 2,

    /// <summary>
    /// Identifies the water ambience layer.
    /// </summary>
    Water = 3,

    /// <summary>
    /// Identifies the wildlife ambience layer.
    /// </summary>
    Wildlife = 4,

    /// <summary>
    /// Identifies the time-of-day ambience layer.
    /// </summary>
    Time = 5,
}

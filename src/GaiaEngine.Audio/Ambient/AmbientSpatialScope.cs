namespace GaiaEngine.Audio.Ambient;

/// <summary>
/// Defines the supported ambient spatial scopes.
/// </summary>
public enum AmbientSpatialScope
{
    /// <summary>
    /// Identifies globally mixed ambience.
    /// </summary>
    Global = 0,

    /// <summary>
    /// Identifies regionally mixed ambience.
    /// </summary>
    Regional = 1,

    /// <summary>
    /// Identifies locally spatialized ambience.
    /// </summary>
    Local = 2,
}

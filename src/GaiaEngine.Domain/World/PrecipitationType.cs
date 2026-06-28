namespace GaiaEngine.Domain.World;

/// <summary>
/// Defines the supported deterministic precipitation types.
/// </summary>
public enum PrecipitationType
{
    /// <summary>
    /// Identifies the absence of precipitation.
    /// </summary>
    None,

    /// <summary>
    /// Identifies rain precipitation.
    /// </summary>
    Rain,

    /// <summary>
    /// Identifies snow precipitation.
    /// </summary>
    Snow,
}

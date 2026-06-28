namespace GaiaEngine.Domain.World;

/// <summary>
/// Defines the supported deterministic temporary weather states.
/// </summary>
public enum WeatherState
{
    /// <summary>
    /// Identifies clear weather.
    /// </summary>
    Clear,

    /// <summary>
    /// Identifies cloudy weather.
    /// </summary>
    Cloudy,

    /// <summary>
    /// Identifies rain weather.
    /// </summary>
    Rain,

    /// <summary>
    /// Identifies storm weather.
    /// </summary>
    Storm,

    /// <summary>
    /// Identifies snow weather.
    /// </summary>
    Snow,

    /// <summary>
    /// Identifies fog weather.
    /// </summary>
    Fog,

    /// <summary>
    /// Identifies drought weather.
    /// </summary>
    Drought,
}

using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the typical deterministic climate profile of a biome.
/// </summary>
public sealed record BiomeClimateProfile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BiomeClimateProfile"/> class.
    /// </summary>
    /// <param name="averageTemperature">The average temperature.</param>
    /// <param name="averageRainfall">The average rainfall intensity.</param>
    /// <param name="humidity">The humidity value in the inclusive range [0, 100].</param>
    /// <param name="windIntensity">The wind intensity.</param>
    /// <param name="seasonalVariation">The seasonal variation.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="humidity"/> is outside [0, 100].</exception>
    public BiomeClimateProfile(int averageTemperature, int averageRainfall, int humidity, int windIntensity, int seasonalVariation)
    {
        if (humidity < 0 || humidity > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(humidity), "The biome humidity must be between 0 and 100.");
        }

        AverageTemperature = averageTemperature;
        AverageRainfall = averageRainfall;
        Humidity = humidity;
        WindIntensity = windIntensity;
        SeasonalVariation = seasonalVariation;
    }

    /// <summary>
    /// Gets the average temperature.
    /// </summary>
    public int AverageTemperature { get; }

    /// <summary>
    /// Gets the average rainfall intensity.
    /// </summary>
    public int AverageRainfall { get; }

    /// <summary>
    /// Gets the humidity value.
    /// </summary>
    public int Humidity { get; }

    /// <summary>
    /// Gets the wind intensity.
    /// </summary>
    public int WindIntensity { get; }

    /// <summary>
    /// Gets the seasonal variation.
    /// </summary>
    public int SeasonalVariation { get; }
}

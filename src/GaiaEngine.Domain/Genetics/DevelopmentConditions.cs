using System;

namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Represents the deterministic environmental conditions used during morphogenesis.
/// </summary>
public sealed record DevelopmentConditions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DevelopmentConditions"/> class.
    /// </summary>
    /// <param name="averageTemperature">The average environmental temperature in degrees Celsius.</param>
    /// <param name="foodAvailability">The food availability value scaled to the inclusive range [0, 1000].</param>
    /// <param name="humidity">The humidity value scaled to the inclusive range [0, 1000].</param>
    /// <param name="altitude">The deterministic altitude value.</param>
    /// <param name="season">The deterministic season label.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="foodAvailability"/> or <paramref name="humidity"/> is outside the supported range.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="season"/> is empty.</exception>
    public DevelopmentConditions(int averageTemperature, int foodAvailability, int humidity, int altitude, string season)
    {
        if (foodAvailability < 0 || foodAvailability > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(foodAvailability), "The food availability must be between 0 and 1000.");
        }

        if (humidity < 0 || humidity > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(humidity), "The humidity must be between 0 and 1000.");
        }

        if (string.IsNullOrWhiteSpace(season))
        {
            throw new ArgumentException("The season must contain a value.", nameof(season));
        }

        AverageTemperature = averageTemperature;
        FoodAvailability = foodAvailability;
        Humidity = humidity;
        Altitude = altitude;
        Season = season;
    }

    /// <summary>
    /// Gets the average environmental temperature in degrees Celsius.
    /// </summary>
    public int AverageTemperature { get; }

    /// <summary>
    /// Gets the food availability value scaled to the inclusive range [0, 1000].
    /// </summary>
    public int FoodAvailability { get; }

    /// <summary>
    /// Gets the humidity value scaled to the inclusive range [0, 1000].
    /// </summary>
    public int Humidity { get; }

    /// <summary>
    /// Gets the deterministic altitude value.
    /// </summary>
    public int Altitude { get; }

    /// <summary>
    /// Gets the deterministic season label.
    /// </summary>
    public string Season { get; }
}

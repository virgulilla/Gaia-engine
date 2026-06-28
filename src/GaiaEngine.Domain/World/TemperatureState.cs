using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the passive temperature values stored for one chunk climate state.
/// </summary>
public sealed record TemperatureState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TemperatureState"/> class.
    /// </summary>
    /// <param name="currentTemperature">The current temperature.</param>
    /// <param name="dailyAverage">The daily average temperature.</param>
    /// <param name="seasonalAverage">The seasonal average temperature.</param>
    /// <param name="dailyVariation">The deterministic daily variation.</param>
    public TemperatureState(int currentTemperature, int dailyAverage, int seasonalAverage, int dailyVariation)
    {
        CurrentTemperature = currentTemperature;
        DailyAverage = dailyAverage;
        SeasonalAverage = seasonalAverage;
        DailyVariation = dailyVariation;
    }

    /// <summary>
    /// Gets the current temperature.
    /// </summary>
    public int CurrentTemperature { get; }

    /// <summary>
    /// Gets the daily average temperature.
    /// </summary>
    public int DailyAverage { get; }

    /// <summary>
    /// Gets the seasonal average temperature.
    /// </summary>
    public int SeasonalAverage { get; }

    /// <summary>
    /// Gets the deterministic daily variation.
    /// </summary>
    public int DailyVariation { get; }
}

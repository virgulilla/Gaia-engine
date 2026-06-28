using System;

namespace GaiaEngine.Simulation.World.Climate;

/// <summary>
/// Defines the explicit deterministic settings used by the Climate System.
/// </summary>
public sealed record ClimateSystemSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClimateSystemSettings"/> class.
    /// </summary>
    /// <param name="ticksPerDay">The number of ticks contained in one world day.</param>
    /// <param name="baseTemperature">The base temperature used for temperate climates.</param>
    /// <param name="seasonalTemperatureDelta">The seasonal temperature adjustment magnitude.</param>
    /// <param name="baseHumidity">The base humidity percentage used for temperate climates.</param>
    /// <param name="basePressure">The base atmospheric pressure value.</param>
    /// <param name="baseWindSpeed">The base wind speed.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any required numeric value is outside its valid range.</exception>
    public ClimateSystemSettings(
        int ticksPerDay,
        int baseTemperature,
        int seasonalTemperatureDelta,
        int baseHumidity,
        int basePressure,
        int baseWindSpeed)
    {
        if (ticksPerDay <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(ticksPerDay), "The ticks-per-day value must be greater than zero.");
        }

        if (baseHumidity < 0 || baseHumidity > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(baseHumidity), "The base humidity must be between 0 and 100.");
        }

        if (baseWindSpeed < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(baseWindSpeed), "The base wind speed must be zero or greater.");
        }

        TicksPerDay = ticksPerDay;
        BaseTemperature = baseTemperature;
        SeasonalTemperatureDelta = seasonalTemperatureDelta;
        BaseHumidity = baseHumidity;
        BasePressure = basePressure;
        BaseWindSpeed = baseWindSpeed;
    }

    /// <summary>
    /// Gets the number of ticks contained in one world day.
    /// </summary>
    public int TicksPerDay { get; }

    /// <summary>
    /// Gets the base temperature used for temperate climates.
    /// </summary>
    public int BaseTemperature { get; }

    /// <summary>
    /// Gets the seasonal temperature adjustment magnitude.
    /// </summary>
    public int SeasonalTemperatureDelta { get; }

    /// <summary>
    /// Gets the base humidity percentage used for temperate climates.
    /// </summary>
    public int BaseHumidity { get; }

    /// <summary>
    /// Gets the base atmospheric pressure value.
    /// </summary>
    public int BasePressure { get; }

    /// <summary>
    /// Gets the base wind speed.
    /// </summary>
    public int BaseWindSpeed { get; }
}

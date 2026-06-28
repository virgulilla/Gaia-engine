using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the passive climate state stored for one world chunk.
/// </summary>
public sealed record ClimateState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ClimateState"/> class.
    /// </summary>
    /// <param name="zone">The deterministic climate zone.</param>
    /// <param name="weatherState">The deterministic temporary weather state.</param>
    /// <param name="temperature">The passive temperature state.</param>
    /// <param name="humidity">The passive humidity state.</param>
    /// <param name="wind">The passive wind state.</param>
    /// <param name="precipitation">The passive precipitation state.</param>
    /// <param name="pressure">The passive pressure state.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="temperature"/>, <paramref name="humidity"/>, <paramref name="wind"/>,
    /// <paramref name="precipitation"/>, or <paramref name="pressure"/> is <see langword="null"/>.
    /// </exception>
    public ClimateState(
        ClimateZone zone,
        WeatherState weatherState,
        TemperatureState temperature,
        HumidityState humidity,
        WindState wind,
        PrecipitationState precipitation,
        PressureState pressure)
    {
        Zone = zone;
        WeatherState = weatherState;
        Temperature = temperature ?? throw new ArgumentNullException(nameof(temperature));
        Humidity = humidity ?? throw new ArgumentNullException(nameof(humidity));
        Wind = wind ?? throw new ArgumentNullException(nameof(wind));
        Precipitation = precipitation ?? throw new ArgumentNullException(nameof(precipitation));
        Pressure = pressure ?? throw new ArgumentNullException(nameof(pressure));
    }

    /// <summary>
    /// Gets the deterministic climate zone.
    /// </summary>
    public ClimateZone Zone { get; }

    /// <summary>
    /// Gets the deterministic temporary weather state.
    /// </summary>
    public WeatherState WeatherState { get; }

    /// <summary>
    /// Gets the passive temperature state.
    /// </summary>
    public TemperatureState Temperature { get; }

    /// <summary>
    /// Gets the passive humidity state.
    /// </summary>
    public HumidityState Humidity { get; }

    /// <summary>
    /// Gets the passive wind state.
    /// </summary>
    public WindState Wind { get; }

    /// <summary>
    /// Gets the passive precipitation state.
    /// </summary>
    public PrecipitationState Precipitation { get; }

    /// <summary>
    /// Gets the passive pressure state.
    /// </summary>
    public PressureState Pressure { get; }
}

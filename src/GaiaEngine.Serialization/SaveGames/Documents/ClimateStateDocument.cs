namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized climate state of a chunk.
/// </summary>
internal sealed class ClimateStateDocument
{
    public string Zone { get; set; } = string.Empty;

    public string WeatherState { get; set; } = string.Empty;

    public int CurrentTemperature { get; set; }

    public int DailyAverageTemperature { get; set; }

    public int SeasonalAverageTemperature { get; set; }

    public int DailyTemperatureVariation { get; set; }

    public int RelativeHumidity { get; set; }

    public int EvaporationRate { get; set; }

    public int CondensationRate { get; set; }

    public int WindDirection { get; set; }

    public int WindSpeed { get; set; }

    public int WindGustStrength { get; set; }

    public string PrecipitationType { get; set; } = string.Empty;

    public int PrecipitationIntensity { get; set; }

    public int PrecipitationDuration { get; set; }

    public int PrecipitationCoverage { get; set; }

    public int Pressure { get; set; }
}

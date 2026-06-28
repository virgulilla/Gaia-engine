namespace GaiaEngine.App.Configuration;

/// <summary>
/// Represents the raw JSON document used to deserialize simulation configuration.
/// </summary>
internal sealed class SimulationConfigurationDocument
{
    /// <summary>
    /// Gets or sets the number of ticks contained in one world day.
    /// </summary>
    public int TicksPerDay { get; set; }

    /// <summary>
    /// Gets or sets the number of days contained in one season.
    /// </summary>
    public int DaysPerSeason { get; set; }

    /// <summary>
    /// Gets or sets the initial world day.
    /// </summary>
    public int StartingDay { get; set; }

    /// <summary>
    /// Gets or sets the initial season name.
    /// </summary>
    public string StartingSeason { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the initial world year.
    /// </summary>
    public int StartingYear { get; set; }
}

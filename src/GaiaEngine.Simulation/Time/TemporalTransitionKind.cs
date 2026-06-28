namespace GaiaEngine.Simulation.Time;

/// <summary>
/// Defines the supported temporal transition kinds emitted by the Time System.
/// </summary>
public enum TemporalTransitionKind
{
    /// <summary>
    /// Identifies the start of a new day.
    /// </summary>
    NewDay,

    /// <summary>
    /// Identifies the start of a new season.
    /// </summary>
    NewSeason,

    /// <summary>
    /// Identifies the start of a new year.
    /// </summary>
    NewYear,
}

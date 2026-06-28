using System;

namespace GaiaEngine.Simulation.Diagnostics;

/// <summary>
/// Represents a deterministic diagnostics snapshot captured during the post-update phase of a simulation tick.
/// </summary>
public sealed record SimulationTickDiagnostics
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationTickDiagnostics"/> class.
    /// </summary>
    /// <param name="tick">The current simulation tick.</param>
    /// <param name="day">The current world day.</param>
    /// <param name="season">The current world season.</param>
    /// <param name="year">The current world year.</param>
    /// <param name="executedPhaseCount">The number of executed phases observed when the snapshot was captured.</param>
    /// <param name="scheduledSystemCount">The number of scheduled systems for the current tick.</param>
    /// <param name="publishedEventCount">The number of events published during the current tick.</param>
    /// <param name="processedEventCount">The number of events processed during the current tick.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any numeric value is negative.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="season"/> is empty.</exception>
    public SimulationTickDiagnostics(
        long tick,
        int day,
        string season,
        int year,
        int executedPhaseCount,
        int scheduledSystemCount,
        int publishedEventCount,
        int processedEventCount)
    {
        if (tick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tick), "The tick value must be zero or greater.");
        }

        if (day < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(day), "The day value must be zero or greater.");
        }

        if (string.IsNullOrWhiteSpace(season))
        {
            throw new ArgumentException("The season value must contain a value.", nameof(season));
        }

        if (year < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(year), "The year value must be zero or greater.");
        }

        if (executedPhaseCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(executedPhaseCount), "The executed phase count must be zero or greater.");
        }

        if (scheduledSystemCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(scheduledSystemCount), "The scheduled system count must be zero or greater.");
        }

        if (publishedEventCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(publishedEventCount), "The published event count must be zero or greater.");
        }

        if (processedEventCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(processedEventCount), "The processed event count must be zero or greater.");
        }

        Tick = tick;
        Day = day;
        Season = season;
        Year = year;
        ExecutedPhaseCount = executedPhaseCount;
        ScheduledSystemCount = scheduledSystemCount;
        PublishedEventCount = publishedEventCount;
        ProcessedEventCount = processedEventCount;
    }

    /// <summary>
    /// Gets the current simulation tick.
    /// </summary>
    public long Tick { get; }

    /// <summary>
    /// Gets the current world day.
    /// </summary>
    public int Day { get; }

    /// <summary>
    /// Gets the current world season.
    /// </summary>
    public string Season { get; }

    /// <summary>
    /// Gets the current world year.
    /// </summary>
    public int Year { get; }

    /// <summary>
    /// Gets the number of executed phases observed when the snapshot was captured.
    /// </summary>
    public int ExecutedPhaseCount { get; }

    /// <summary>
    /// Gets the number of scheduled systems for the current tick.
    /// </summary>
    public int ScheduledSystemCount { get; }

    /// <summary>
    /// Gets the number of events published during the current tick.
    /// </summary>
    public int PublishedEventCount { get; }

    /// <summary>
    /// Gets the number of events processed during the current tick.
    /// </summary>
    public int ProcessedEventCount { get; }
}

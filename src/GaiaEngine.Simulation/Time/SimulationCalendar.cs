using System;

namespace GaiaEngine.Simulation.Time;

/// <summary>
/// Defines the deterministic calendar parameters used by the Time System.
/// </summary>
public sealed record SimulationCalendar
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationCalendar"/> class.
    /// </summary>
    /// <param name="ticksPerDay">The number of simulation ticks contained in one world day.</param>
    /// <param name="daysPerSeason">The number of world days contained in one season.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="ticksPerDay"/> or <paramref name="daysPerSeason"/> is zero or negative.
    /// </exception>
    public SimulationCalendar(int ticksPerDay, int daysPerSeason)
    {
        if (ticksPerDay <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(ticksPerDay), "The ticks-per-day value must be greater than zero.");
        }

        if (daysPerSeason <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(daysPerSeason), "The days-per-season value must be greater than zero.");
        }

        TicksPerDay = ticksPerDay;
        DaysPerSeason = daysPerSeason;
    }

    /// <summary>
    /// Gets the number of simulation ticks contained in one world day.
    /// </summary>
    public int TicksPerDay { get; }

    /// <summary>
    /// Gets the number of world days contained in one season.
    /// </summary>
    public int DaysPerSeason { get; }
}

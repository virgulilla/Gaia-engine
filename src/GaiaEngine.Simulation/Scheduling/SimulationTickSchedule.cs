using System;
using System.Collections.Generic;
using GaiaEngine.Simulation.Pipeline;

namespace GaiaEngine.Simulation.Scheduling;

/// <summary>
/// Represents the deterministic schedule of systems selected for one simulation tick.
/// </summary>
public sealed record SimulationTickSchedule
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationTickSchedule"/> class.
    /// </summary>
    /// <param name="executingTick">The simulation tick that owns the schedule.</param>
    /// <param name="systems">The deterministic systems scheduled for the tick.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="executingTick"/> is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="systems"/> is <see langword="null"/>.</exception>
    public SimulationTickSchedule(long executingTick, IReadOnlyList<ScheduledSimulationSystem> systems)
    {
        if (executingTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(executingTick), "The executing tick must be zero or greater.");
        }

        ExecutingTick = executingTick;
        Systems = systems ?? throw new ArgumentNullException(nameof(systems));
    }

    /// <summary>
    /// Gets the simulation tick that owns the schedule.
    /// </summary>
    public long ExecutingTick { get; }

    /// <summary>
    /// Gets the deterministic systems scheduled for the tick.
    /// </summary>
    public IReadOnlyList<ScheduledSimulationSystem> Systems { get; }

    /// <summary>
    /// Gets the scheduled systems associated with the supplied phase.
    /// </summary>
    /// <param name="phase">The phase to inspect.</param>
    /// <returns>The deterministic list of systems scheduled for the phase.</returns>
    public IReadOnlyList<ScheduledSimulationSystem> GetSystemsForPhase(SimulationTickPhase phase)
    {
        List<ScheduledSimulationSystem> systems = new();
        foreach (ScheduledSimulationSystem system in Systems)
        {
            if (system.Phase == phase)
            {
                systems.Add(system);
            }
        }

        return systems.AsReadOnly();
    }
}

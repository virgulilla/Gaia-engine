using System;
using GaiaEngine.Simulation.Pipeline;

namespace GaiaEngine.Simulation.Scheduling;

/// <summary>
/// Defines the deterministic scheduling rule associated with one simulation system.
/// </summary>
public sealed record ScheduledSimulationSystemDefinition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScheduledSimulationSystemDefinition"/> class.
    /// </summary>
    /// <param name="systemName">The logical simulation system name.</param>
    /// <param name="phase">The tick phase that owns the system execution.</param>
    /// <param name="frequency">The number of ticks between executions.</param>
    /// <param name="priority">The deterministic execution priority inside the phase.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="systemName"/> is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="frequency"/> is not greater than zero or <paramref name="priority"/> is negative.
    /// </exception>
    public ScheduledSimulationSystemDefinition(
        string systemName,
        SimulationTickPhase phase,
        int frequency,
        int priority)
    {
        if (string.IsNullOrWhiteSpace(systemName))
        {
            throw new ArgumentException("The system name must contain a value.", nameof(systemName));
        }

        if (frequency <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(frequency), "The system frequency must be greater than zero.");
        }

        if (priority < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(priority), "The system priority must be zero or greater.");
        }

        SystemName = systemName;
        Phase = phase;
        Frequency = frequency;
        Priority = priority;
    }

    /// <summary>
    /// Gets the logical simulation system name.
    /// </summary>
    public string SystemName { get; }

    /// <summary>
    /// Gets the tick phase that owns the system execution.
    /// </summary>
    public SimulationTickPhase Phase { get; }

    /// <summary>
    /// Gets the number of ticks between executions.
    /// </summary>
    public int Frequency { get; }

    /// <summary>
    /// Gets the deterministic execution priority inside the phase.
    /// </summary>
    public int Priority { get; }
}

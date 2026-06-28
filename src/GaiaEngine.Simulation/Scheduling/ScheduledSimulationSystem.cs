using GaiaEngine.Simulation.Pipeline;

namespace GaiaEngine.Simulation.Scheduling;

/// <summary>
/// Represents one concrete simulation system execution scheduled for a deterministic tick.
/// </summary>
public sealed record ScheduledSimulationSystem
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ScheduledSimulationSystem"/> class.
    /// </summary>
    /// <param name="systemName">The logical simulation system name.</param>
    /// <param name="phase">The phase that will execute the system.</param>
    /// <param name="priority">The deterministic phase-local execution priority.</param>
    /// <param name="executingTick">The simulation tick that owns the execution.</param>
    public ScheduledSimulationSystem(
        string systemName,
        SimulationTickPhase phase,
        int priority,
        long executingTick)
    {
        SystemName = systemName;
        Phase = phase;
        Priority = priority;
        ExecutingTick = executingTick;
    }

    /// <summary>
    /// Gets the logical simulation system name.
    /// </summary>
    public string SystemName { get; }

    /// <summary>
    /// Gets the phase that will execute the system.
    /// </summary>
    public SimulationTickPhase Phase { get; }

    /// <summary>
    /// Gets the deterministic phase-local execution priority.
    /// </summary>
    public int Priority { get; }

    /// <summary>
    /// Gets the simulation tick that owns the execution.
    /// </summary>
    public long ExecutingTick { get; }
}

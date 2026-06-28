using System;
using System.Collections.Generic;

namespace GaiaEngine.Simulation.Scheduling;

/// <summary>
/// Creates deterministic per-tick schedules from explicit system definitions.
/// </summary>
public sealed class DeterministicSimulationScheduler : ISimulationScheduler
{
    private readonly IReadOnlyList<ScheduledSimulationSystemDefinition> definitions;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicSimulationScheduler"/> class.
    /// </summary>
    /// <param name="definitions">The explicit system scheduling definitions.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="definitions"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when duplicate system names are supplied.</exception>
    public DeterministicSimulationScheduler(IReadOnlyList<ScheduledSimulationSystemDefinition> definitions)
    {
        ArgumentNullException.ThrowIfNull(definitions);

        HashSet<string> names = new(StringComparer.Ordinal);
        foreach (ScheduledSimulationSystemDefinition definition in definitions)
        {
            ArgumentNullException.ThrowIfNull(definition);

            if (!names.Add(definition.SystemName))
            {
                throw new ArgumentException("Simulation system names must be unique inside the scheduler.", nameof(definitions));
            }
        }

        this.definitions = definitions;
    }

    /// <summary>
    /// Creates the deterministic schedule for the supplied simulation tick.
    /// </summary>
    /// <param name="executingTick">The simulation tick to schedule.</param>
    /// <returns>The deterministic schedule for the supplied tick.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="executingTick"/> is negative.</exception>
    public SimulationTickSchedule CreateSchedule(long executingTick)
    {
        if (executingTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(executingTick), "The executing tick must be zero or greater.");
        }

        List<ScheduledSimulationSystem> systems = new();
        foreach (ScheduledSimulationSystemDefinition definition in definitions)
        {
            if ((executingTick % definition.Frequency) == 0)
            {
                systems.Add(new ScheduledSimulationSystem(definition.SystemName, definition.Phase, definition.Priority, executingTick));
            }
        }

        systems.Sort(CompareSystems);
        return new SimulationTickSchedule(executingTick, systems.AsReadOnly());
    }

    private static int CompareSystems(ScheduledSimulationSystem left, ScheduledSimulationSystem right)
    {
        int phaseComparison = left.Phase.CompareTo(right.Phase);
        if (phaseComparison != 0)
        {
            return phaseComparison;
        }

        int priorityComparison = left.Priority.CompareTo(right.Priority);
        if (priorityComparison != 0)
        {
            return priorityComparison;
        }

        return StringComparer.Ordinal.Compare(left.SystemName, right.SystemName);
    }
}

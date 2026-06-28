using System;
using System.Collections.Generic;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Scheduling;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Executes the fixed deterministic phase order defined by the Gaia Engine simulation specifications.
/// </summary>
public sealed class DeterministicSimulationTickPipeline : ISimulationTickPipeline
{
    private static readonly SimulationTickPhase[] RequiredPhaseOrder =
    {
        SimulationTickPhase.InputCollection,
        SimulationTickPhase.PreUpdate,
        SimulationTickPhase.WorldUpdate,
        SimulationTickPhase.OrganismUpdate,
        SimulationTickPhase.InteractionSystems,
        SimulationTickPhase.EnvironmentUpdate,
        SimulationTickPhase.EventDispatch,
        SimulationTickPhase.PostUpdate,
    };

    private readonly IReadOnlyList<ISimulationTickPhase> phases;
    private readonly ISimulationScheduler scheduler;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicSimulationTickPipeline"/> class.
    /// </summary>
    /// <param name="phases">The ordered deterministic phases executed by every simulation tick.</param>
    /// <param name="scheduler">The scheduler responsible for selecting systems for the current tick.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="phases"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when the supplied phase order is incomplete or invalid.</exception>
    public DeterministicSimulationTickPipeline(IReadOnlyList<ISimulationTickPhase> phases, ISimulationScheduler scheduler)
    {
        ArgumentNullException.ThrowIfNull(phases);
        this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));

        if (phases.Count != RequiredPhaseOrder.Length)
        {
            throw new ArgumentException("The simulation tick pipeline must contain the full deterministic phase set.", nameof(phases));
        }

        for (int index = 0; index < RequiredPhaseOrder.Length; index++)
        {
            ISimulationTickPhase phase = phases[index] ?? throw new ArgumentNullException(nameof(phases), "The simulation tick pipeline cannot contain null phases.");
            if (phase.Phase != RequiredPhaseOrder[index])
            {
                throw new ArgumentException("The simulation tick pipeline phase order must match the approved deterministic specification.", nameof(phases));
            }
        }

        this.phases = phases;
    }

    /// <summary>
    /// Executes one deterministic simulation tick starting from the supplied world state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <returns>The deterministic result of the tick pipeline execution.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="world"/> is <see langword="null"/>.</exception>
    public SimulationTickResult Execute(GaiaEngine.Domain.World.World world, ulong nextEventSequence)
    {
        SimulationTickContext context = new(world ?? throw new ArgumentNullException(nameof(world)), nextEventSequence);
        List<SimulationTickPhase> executedPhases = new(phases.Count);

        foreach (ISimulationTickPhase phase in phases)
        {
            phase.Execute(context);
            context.RegisterExecutedPhase(phase.Phase);
            executedPhases.Add(phase.Phase);

            if (phase.Phase == SimulationTickPhase.WorldUpdate
                && context.Schedule.ExecutingTick != context.CurrentTimeState.CurrentTick)
            {
                context.ApplySchedule(scheduler.CreateSchedule(context.CurrentTimeState.CurrentTick));
            }
        }

        return new SimulationTickResult(
            context.CurrentWorld,
            context.CurrentTimeState,
            executedPhases.AsReadOnly(),
            context.Schedule,
            context.EventPublicationResult,
            context.EventDispatchResult,
            context.Diagnostics,
            context.NextEventSequence,
            context.TimeAdvanceResult);
    }
}

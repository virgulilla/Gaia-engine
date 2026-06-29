using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Interactions.Feeding;
using GaiaEngine.Simulation.Interactions.Hydration;
using GaiaEngine.Simulation.Interactions.Movement;
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
        return Execute(world, OrganismCollection.Empty, MovementRequestCollection.Empty, FeedingRequestCollection.Empty, HydrationRequestCollection.Empty, nextEventSequence);
    }

    /// <summary>
    /// Executes one deterministic simulation tick starting from the supplied world and organism state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <returns>The deterministic result of the tick pipeline execution.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/> or <paramref name="organisms"/> is <see langword="null"/>.
    /// </exception>
    public SimulationTickResult Execute(GaiaEngine.Domain.World.World world, OrganismCollection organisms, ulong nextEventSequence)
    {
        return Execute(world, organisms, MovementRequestCollection.Empty, FeedingRequestCollection.Empty, HydrationRequestCollection.Empty, nextEventSequence);
    }

    /// <summary>
    /// Executes one deterministic simulation tick starting from the supplied world, organism and movement request state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="movementRequests">The current movement request state.</param>
    /// <param name="feedingRequests">The current feeding request state.</param>
    /// <param name="hydrationRequests">The current hydration request state.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <returns>The deterministic result of the tick pipeline execution.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/>, <paramref name="organisms"/>, or <paramref name="movementRequests"/> is <see langword="null"/>.
    /// </exception>
    public SimulationTickResult Execute(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        MovementRequestCollection movementRequests,
        FeedingRequestCollection feedingRequests,
        HydrationRequestCollection hydrationRequests,
        ulong nextEventSequence)
    {
        SimulationTickContext context = new(
            world ?? throw new ArgumentNullException(nameof(world)),
            organisms ?? throw new ArgumentNullException(nameof(organisms)),
            movementRequests ?? throw new ArgumentNullException(nameof(movementRequests)),
            feedingRequests ?? throw new ArgumentNullException(nameof(feedingRequests)),
            hydrationRequests ?? throw new ArgumentNullException(nameof(hydrationRequests)),
            nextEventSequence);
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
            context.CurrentOrganisms,
            context.CurrentMovementRequests,
            context.CurrentFeedingRequests,
            context.CurrentHydrationRequests,
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

using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Interactions.Feeding;
using GaiaEngine.Simulation.Interactions.Hydration;
using GaiaEngine.Simulation.Interactions.Movement;
using GaiaEngine.Simulation.Scheduling;
using GaiaEngine.Simulation.Time;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Represents the deterministic result produced by one simulation tick pipeline execution.
/// </summary>
public sealed record SimulationTickResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationTickResult"/> class.
    /// </summary>
    /// <param name="world">The resulting world state.</param>
    /// <param name="organisms">The resulting organism state.</param>
    /// <param name="species">The resulting species state.</param>
    /// <param name="actionRequests">The resulting common action request state.</param>
    /// <param name="movementRequests">The resulting movement request state.</param>
    /// <param name="feedingRequests">The resulting feeding request state.</param>
    /// <param name="hydrationRequests">The resulting hydration request state.</param>
    /// <param name="timeState">The resulting world time state.</param>
    /// <param name="executedPhases">The deterministic list of executed phases.</param>
    /// <param name="schedule">The deterministic schedule selected for the tick.</param>
    /// <param name="eventPublicationResult">The deterministic event publication result produced during the tick.</param>
    /// <param name="eventDispatchResult">The deterministic event dispatch result produced during the tick, when available.</param>
    /// <param name="diagnostics">The deterministic diagnostics snapshot captured during the tick, when available.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <param name="timeAdvanceResult">The time advancement produced during the tick, when available.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="timeState"/> or <paramref name="executedPhases"/> is <see langword="null"/>.
    /// </exception>
    public SimulationTickResult(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        SpeciesCollection species,
        SimulationActionRequestCollection actionRequests,
        MovementRequestCollection movementRequests,
        FeedingRequestCollection feedingRequests,
        HydrationRequestCollection hydrationRequests,
        WorldTimeState timeState,
        IReadOnlyList<SimulationTickPhase> executedPhases,
        SimulationTickSchedule schedule,
        SimulationEventPublicationResult eventPublicationResult,
        EventDispatchResult? eventDispatchResult,
        SimulationTickDiagnostics? diagnostics,
        ulong nextEventSequence,
        TimeAdvanceResult? timeAdvanceResult)
    {
        World = world ?? throw new ArgumentNullException(nameof(world));
        Organisms = organisms ?? throw new ArgumentNullException(nameof(organisms));
        Species = species ?? throw new ArgumentNullException(nameof(species));
        ActionRequests = actionRequests ?? throw new ArgumentNullException(nameof(actionRequests));
        MovementRequests = movementRequests ?? throw new ArgumentNullException(nameof(movementRequests));
        FeedingRequests = feedingRequests ?? throw new ArgumentNullException(nameof(feedingRequests));
        HydrationRequests = hydrationRequests ?? throw new ArgumentNullException(nameof(hydrationRequests));
        TimeState = timeState ?? throw new ArgumentNullException(nameof(timeState));
        ExecutedPhases = executedPhases ?? throw new ArgumentNullException(nameof(executedPhases));
        Schedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
        EventPublicationResult = eventPublicationResult ?? throw new ArgumentNullException(nameof(eventPublicationResult));
        EventDispatchResult = eventDispatchResult;
        Diagnostics = diagnostics;
        if (nextEventSequence == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(nextEventSequence), "The next event sequence value must be greater than zero.");
        }

        NextEventSequence = nextEventSequence;
        TimeAdvanceResult = timeAdvanceResult;
    }

    /// <summary>
    /// Gets the resulting world state.
    /// </summary>
    public GaiaEngine.Domain.World.World World { get; }

    /// <summary>
    /// Gets the resulting organism state.
    /// </summary>
    public OrganismCollection Organisms { get; }

    /// <summary>
    /// Gets the resulting species state.
    /// </summary>
    public SpeciesCollection Species { get; }

    /// <summary>
    /// Gets the resulting common action request state.
    /// </summary>
    public SimulationActionRequestCollection ActionRequests { get; }

    /// <summary>
    /// Gets the resulting world time state.
    /// </summary>
    public WorldTimeState TimeState { get; }

    /// <summary>
    /// Gets the resulting movement request state.
    /// </summary>
    public MovementRequestCollection MovementRequests { get; }

    /// <summary>
    /// Gets the resulting feeding request state.
    /// </summary>
    public FeedingRequestCollection FeedingRequests { get; }

    /// <summary>
    /// Gets the resulting hydration request state.
    /// </summary>
    public HydrationRequestCollection HydrationRequests { get; }

    /// <summary>
    /// Gets the deterministic list of executed phases.
    /// </summary>
    public IReadOnlyList<SimulationTickPhase> ExecutedPhases { get; }

    /// <summary>
    /// Gets the deterministic schedule selected for the tick.
    /// </summary>
    public SimulationTickSchedule Schedule { get; }

    /// <summary>
    /// Gets the deterministic event publication result produced during the tick.
    /// </summary>
    public SimulationEventPublicationResult EventPublicationResult { get; }

    /// <summary>
    /// Gets the deterministic event dispatch result produced during the tick, when available.
    /// </summary>
    public EventDispatchResult? EventDispatchResult { get; }

    /// <summary>
    /// Gets the deterministic diagnostics snapshot captured during the tick, when available.
    /// </summary>
    public SimulationTickDiagnostics? Diagnostics { get; }

    /// <summary>
    /// Gets the next deterministic event sequence value to use.
    /// </summary>
    public ulong NextEventSequence { get; }

    /// <summary>
    /// Gets the time advancement produced during the tick, when available.
    /// </summary>
    public TimeAdvanceResult? TimeAdvanceResult { get; }
}

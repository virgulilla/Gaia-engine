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
/// Represents the mutable runtime context shared across deterministic tick phases.
/// </summary>
public sealed class SimulationTickContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationTickContext"/> class.
    /// </summary>
    /// <param name="world">The initial world state for the tick.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="world"/> is <see langword="null"/>.</exception>
    public SimulationTickContext(GaiaEngine.Domain.World.World world, ulong nextEventSequence)
        : this(world, OrganismCollection.Empty, SpeciesCollection.Empty, SimulationActionRequestCollection.Empty, MovementRequestCollection.Empty, FeedingRequestCollection.Empty, HydrationRequestCollection.Empty, nextEventSequence)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationTickContext"/> class.
    /// </summary>
    /// <param name="world">The initial world state for the tick.</param>
    /// <param name="organisms">The initial organism state for the tick.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/> or <paramref name="organisms"/> is <see langword="null"/>.
    /// </exception>
    public SimulationTickContext(GaiaEngine.Domain.World.World world, OrganismCollection organisms, ulong nextEventSequence)
        : this(world, organisms, SpeciesCollection.Empty, SimulationActionRequestCollection.Empty, MovementRequestCollection.Empty, FeedingRequestCollection.Empty, HydrationRequestCollection.Empty, nextEventSequence)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationTickContext"/> class.
    /// </summary>
    /// <param name="world">The initial world state for the tick.</param>
    /// <param name="organisms">The initial organism state for the tick.</param>
    /// <param name="species">The initial species state for the tick.</param>
    /// <param name="actionRequests">The initial common action request state for the tick.</param>
    /// <param name="movementRequests">The initial movement request state for the tick.</param>
    /// <param name="feedingRequests">The initial feeding request state for the tick.</param>
    /// <param name="hydrationRequests">The initial hydration request state for the tick.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/>, <paramref name="organisms"/>, <paramref name="movementRequests"/>, <paramref name="feedingRequests"/>,
    /// or <paramref name="hydrationRequests"/> is <see langword="null"/>.
    /// </exception>
    public SimulationTickContext(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        SpeciesCollection species,
        SimulationActionRequestCollection actionRequests,
        MovementRequestCollection movementRequests,
        FeedingRequestCollection feedingRequests,
        HydrationRequestCollection hydrationRequests,
        ulong nextEventSequence)
    {
        CurrentWorld = world ?? throw new ArgumentNullException(nameof(world));
        CurrentOrganisms = organisms ?? throw new ArgumentNullException(nameof(organisms));
        CurrentSpecies = species ?? throw new ArgumentNullException(nameof(species));
        CurrentActionRequests = actionRequests ?? throw new ArgumentNullException(nameof(actionRequests));
        CurrentMovementRequests = movementRequests ?? throw new ArgumentNullException(nameof(movementRequests));
        CurrentFeedingRequests = feedingRequests ?? throw new ArgumentNullException(nameof(feedingRequests));
        CurrentHydrationRequests = hydrationRequests ?? throw new ArgumentNullException(nameof(hydrationRequests));
        Schedule = new SimulationTickSchedule(world.TimeState.CurrentTick, Array.Empty<ScheduledSimulationSystem>());
        if (nextEventSequence == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(nextEventSequence), "The next event sequence value must be greater than zero.");
        }

        NextEventSequence = nextEventSequence;
        EventPublicationResult = new SimulationEventPublicationResult(nextEventSequence, Array.Empty<IEvent>());
    }

    /// <summary>
    /// Gets the current world state being updated by the pipeline.
    /// </summary>
    public GaiaEngine.Domain.World.World CurrentWorld { get; private set; }

    /// <summary>
    /// Gets the current organism state being updated by the pipeline.
    /// </summary>
    public OrganismCollection CurrentOrganisms { get; private set; }

    /// <summary>
    /// Gets the current species state being updated by the pipeline.
    /// </summary>
    public SpeciesCollection CurrentSpecies { get; private set; }

    /// <summary>
    /// Gets the current common action request state being updated by the pipeline.
    /// </summary>
    public SimulationActionRequestCollection CurrentActionRequests { get; private set; }

    /// <summary>
    /// Gets the current movement request state being updated by the pipeline.
    /// </summary>
    public MovementRequestCollection CurrentMovementRequests { get; private set; }

    /// <summary>
    /// Gets the current feeding request state being updated by the pipeline.
    /// </summary>
    public FeedingRequestCollection CurrentFeedingRequests { get; private set; }

    /// <summary>
    /// Gets the current hydration request state being updated by the pipeline.
    /// </summary>
    public HydrationRequestCollection CurrentHydrationRequests { get; private set; }

    /// <summary>
    /// Gets the current world time state being updated by the pipeline.
    /// </summary>
    public WorldTimeState CurrentTimeState => CurrentWorld.TimeState;

    /// <summary>
    /// Gets the time advancement produced during the current tick, when available.
    /// </summary>
    public TimeAdvanceResult? TimeAdvanceResult { get; private set; }

    /// <summary>
    /// Gets the deterministic event publication result produced during the current tick.
    /// </summary>
    public SimulationEventPublicationResult EventPublicationResult { get; private set; }

    /// <summary>
    /// Gets the deterministic event dispatch result produced during the current tick, when available.
    /// </summary>
    public EventDispatchResult? EventDispatchResult { get; private set; }

    /// <summary>
    /// Gets the deterministic diagnostics snapshot captured during the current tick, when available.
    /// </summary>
    public SimulationTickDiagnostics? Diagnostics { get; private set; }

    /// <summary>
    /// Gets the deterministic schedule selected for the current tick, when available.
    /// </summary>
    public SimulationTickSchedule Schedule { get; private set; }

    /// <summary>
    /// Gets the next deterministic event sequence value to use.
    /// </summary>
    public ulong NextEventSequence { get; private set; }

    /// <summary>
    /// Gets the deterministic list of phases already executed during the current tick.
    /// </summary>
    public IReadOnlyList<SimulationTickPhase> ExecutedPhases => executedPhases.AsReadOnly();

    private readonly List<SimulationTickPhase> executedPhases = new();

    /// <summary>
    /// Applies the time advancement produced by the Time System to the current context.
    /// </summary>
    /// <param name="result">The time advancement result to apply.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public void ApplyTimeAdvance(TimeAdvanceResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        TimeAdvanceResult = result;
        CurrentWorld = new GaiaEngine.Domain.World.World(CurrentWorld.Metadata, CurrentWorld.Dimensions, result.TimeState, CurrentWorld.GetChunks());
    }

    /// <summary>
    /// Applies the deterministic schedule created for the current tick.
    /// </summary>
    /// <param name="schedule">The schedule to apply.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="schedule"/> is <see langword="null"/>.</exception>
    public void ApplySchedule(SimulationTickSchedule schedule)
    {
        Schedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
    }

    /// <summary>
    /// Applies the deterministic event publication result produced during the current tick.
    /// </summary>
    /// <param name="result">The event publication result to apply.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public void ApplyEventPublicationResult(SimulationEventPublicationResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        EventPublicationResult = result;
        NextEventSequence = result.NextEventSequence;
    }

    /// <summary>
    /// Applies the deterministic event dispatch result produced during the current tick.
    /// </summary>
    /// <param name="result">The event dispatch result to apply.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public void ApplyEventDispatchResult(EventDispatchResult result)
    {
        EventDispatchResult = result ?? throw new ArgumentNullException(nameof(result));
    }

    /// <summary>
    /// Registers one executed phase in deterministic order.
    /// </summary>
    /// <param name="phase">The executed phase.</param>
    public void RegisterExecutedPhase(SimulationTickPhase phase)
    {
        executedPhases.Add(phase);
    }

    /// <summary>
    /// Applies the deterministic diagnostics snapshot captured during the current tick.
    /// </summary>
    /// <param name="diagnostics">The diagnostics snapshot to apply.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="diagnostics"/> is <see langword="null"/>.</exception>
    public void ApplyDiagnostics(SimulationTickDiagnostics diagnostics)
    {
        Diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
    }

    /// <summary>
    /// Applies the updated world state produced during the current tick.
    /// </summary>
    /// <param name="world">The updated world state.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="world"/> is <see langword="null"/>.</exception>
    public void ApplyWorld(GaiaEngine.Domain.World.World world)
    {
        CurrentWorld = world ?? throw new ArgumentNullException(nameof(world));
    }

    /// <summary>
    /// Applies the updated organism state produced during the current tick.
    /// </summary>
    /// <param name="organisms">The updated organism state.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="organisms"/> is <see langword="null"/>.</exception>
    public void ApplyOrganisms(OrganismCollection organisms)
    {
        CurrentOrganisms = organisms ?? throw new ArgumentNullException(nameof(organisms));
    }

    /// <summary>
    /// Applies the updated species state produced during the current tick.
    /// </summary>
    /// <param name="species">The updated species state.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="species"/> is <see langword="null"/>.</exception>
    public void ApplySpecies(SpeciesCollection species)
    {
        CurrentSpecies = species ?? throw new ArgumentNullException(nameof(species));
    }

    /// <summary>
    /// Applies the updated common action request state produced during the current tick.
    /// </summary>
    /// <param name="actionRequests">The updated common action request state.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="actionRequests"/> is <see langword="null"/>.</exception>
    public void ApplyActionRequests(SimulationActionRequestCollection actionRequests)
    {
        CurrentActionRequests = actionRequests ?? throw new ArgumentNullException(nameof(actionRequests));
    }

    /// <summary>
    /// Applies the updated movement request state produced during the current tick.
    /// </summary>
    /// <param name="movementRequests">The updated movement request state.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="movementRequests"/> is <see langword="null"/>.</exception>
    public void ApplyMovementRequests(MovementRequestCollection movementRequests)
    {
        CurrentMovementRequests = movementRequests ?? throw new ArgumentNullException(nameof(movementRequests));
    }

    /// <summary>
    /// Applies the updated feeding request state produced during the current tick.
    /// </summary>
    /// <param name="feedingRequests">The updated feeding request state.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="feedingRequests"/> is <see langword="null"/>.</exception>
    public void ApplyFeedingRequests(FeedingRequestCollection feedingRequests)
    {
        CurrentFeedingRequests = feedingRequests ?? throw new ArgumentNullException(nameof(feedingRequests));
    }

    /// <summary>
    /// Applies the updated hydration request state produced during the current tick.
    /// </summary>
    /// <param name="hydrationRequests">The updated hydration request state.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="hydrationRequests"/> is <see langword="null"/>.</exception>
    public void ApplyHydrationRequests(HydrationRequestCollection hydrationRequests)
    {
        CurrentHydrationRequests = hydrationRequests ?? throw new ArgumentNullException(nameof(hydrationRequests));
    }
}

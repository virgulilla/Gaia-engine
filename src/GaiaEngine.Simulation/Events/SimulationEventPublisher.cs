using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Engine.Events;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Simulation.Time;

namespace GaiaEngine.Simulation.Events;

/// <summary>
/// Publishes deterministic simulation time events through the engine event bus.
/// </summary>
public sealed class SimulationEventPublisher : ISimulationEventPublisher
{
    private readonly IEventBus eventBus;
    private readonly IEntityIdGenerator idGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationEventPublisher"/> class.
    /// </summary>
    /// <param name="eventBus">The engine event bus used for deterministic publication.</param>
    /// <param name="idGenerator">The deterministic identifier generator used for event identifiers.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="eventBus"/> or <paramref name="idGenerator"/> is <see langword="null"/>.
    /// </exception>
    public SimulationEventPublisher(IEventBus eventBus, IEntityIdGenerator idGenerator)
    {
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    }

    /// <summary>
    /// Publishes the simulation events derived from a time advance result.
    /// </summary>
    /// <param name="result">The deterministic time advance result.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <returns>The deterministic publication result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="nextEventSequence"/> is zero.</exception>
    public SimulationEventPublicationResult PublishTimeEvents(TimeAdvanceResult result, ulong nextEventSequence)
    {
        ArgumentNullException.ThrowIfNull(result);

        if (nextEventSequence == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(nextEventSequence), "The next event sequence value must be greater than zero.");
        }

        List<IEvent> publishedEvents = new(result.Transitions.Count);
        ulong currentSequence = nextEventSequence;

        foreach (TemporalTransition transition in result.Transitions)
        {
            IEvent eventInstance = CreateTimeEvent(transition, currentSequence);
            eventBus.Publish(eventInstance);
            publishedEvents.Add(eventInstance);
            currentSequence++;
        }

        return new SimulationEventPublicationResult(currentSequence, publishedEvents.AsReadOnly());
    }

    private IEvent CreateTimeEvent(TemporalTransition transition, ulong sequence)
    {
        EventId eventId = idGenerator.CreateEventId(
            new IdentifierGenerationContext(
                new WorldSeed(0),
                transition.TimeState.CurrentTick,
                new EntitySequence(sequence)));

        return transition.Kind switch
        {
            TemporalTransitionKind.NewDay => new NewDaySimulationEvent(
                eventId,
                transition.TimeState.CurrentTick,
                transition.TimeState.CurrentTick,
                transition.TimeState.CurrentDay,
                transition.TimeState.CurrentSeason,
                transition.TimeState.CurrentYear),
            TemporalTransitionKind.NewSeason => new NewSeasonSimulationEvent(
                eventId,
                transition.TimeState.CurrentTick,
                transition.TimeState.CurrentTick,
                transition.TimeState.CurrentDay,
                transition.TimeState.CurrentSeason,
                transition.TimeState.CurrentYear),
            TemporalTransitionKind.NewYear => new NewYearSimulationEvent(
                eventId,
                transition.TimeState.CurrentTick,
                transition.TimeState.CurrentTick,
                transition.TimeState.CurrentDay,
                transition.TimeState.CurrentSeason,
                transition.TimeState.CurrentYear),
            _ => throw new ArgumentOutOfRangeException(nameof(transition), "The supplied temporal transition kind is not supported."),
        };
    }
}

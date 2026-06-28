using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Pipeline;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Pipeline;

public sealed class EventDispatchPhaseTests
{
    [Fact]
    public void Execute_ShouldDispatchQueuedEventsInDeterministicPriorityOrder()
    {
        EventBus eventBus = new();
        List<string> receivedEvents = new();
        eventBus.Subscribe<NewDaySimulationEvent>(eventInstance => receivedEvents.Add(eventInstance.EventType));
        eventBus.Subscribe<NewSeasonSimulationEvent>(eventInstance => receivedEvents.Add(eventInstance.EventType));
        eventBus.Subscribe<NewYearSimulationEvent>(eventInstance => receivedEvents.Add(eventInstance.EventType));

        eventBus.Publish(new NewDaySimulationEvent(EventId.FromSequence(new EntitySequence(1)), 8, 8, 1, "Winter", 0));
        eventBus.Publish(new NewSeasonSimulationEvent(EventId.FromSequence(new EntitySequence(2)), 8, 8, 0, "Spring", 0));
        eventBus.Publish(new NewYearSimulationEvent(EventId.FromSequence(new EntitySequence(3)), 8, 8, 0, "Spring", 1));

        EventDispatchPhase phase = new(eventBus);
        SimulationTickContext context = new(new WorldTimeState(8, 0, "Spring", 1), 4);

        phase.Execute(context);

        Assert.Equal(3, context.EventDispatchResult!.ProcessedEventCount);
        Assert.Equal(
            new[]
            {
                nameof(NewYearSimulationEvent),
                nameof(NewSeasonSimulationEvent),
                nameof(NewDaySimulationEvent),
            },
            receivedEvents);
        Assert.Equal(0, eventBus.PendingEventCount);
    }
}

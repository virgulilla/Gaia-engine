using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Time;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Events;

public sealed class SimulationEventPublisherTests
{
    [Fact]
    public void PublishTimeEvents_ShouldCreateOrderedEventsAndAdvanceSequence()
    {
        EventBus eventBus = new();
        SimulationEventPublisher publisher = new(eventBus, new DeterministicEntityIdGenerator());
        TimeAdvanceResult result = new(
            new WorldTimeState(8, 0, "Spring", 1),
            new TemporalTransition[]
            {
                new(TemporalTransitionKind.NewDay, new WorldTimeState(8, 1, "Winter", 0)),
                new(TemporalTransitionKind.NewSeason, new WorldTimeState(8, 0, "Spring", 0)),
                new(TemporalTransitionKind.NewYear, new WorldTimeState(8, 0, "Spring", 1)),
            });

        SimulationEventPublicationResult publicationResult = publisher.PublishTimeEvents(result, 10);

        Assert.Equal(13UL, publicationResult.NextEventSequence);
        Assert.Equal(3, publicationResult.PublishedEvents.Count);
        Assert.IsType<NewDaySimulationEvent>(publicationResult.PublishedEvents[0]);
        Assert.IsType<NewSeasonSimulationEvent>(publicationResult.PublishedEvents[1]);
        Assert.IsType<NewYearSimulationEvent>(publicationResult.PublishedEvents[2]);
        Assert.Equal(3, eventBus.PendingEventCount);
        Assert.Equal(new EntitySequence(10), publicationResult.PublishedEvents[0].EventId.Sequence);
    }
}

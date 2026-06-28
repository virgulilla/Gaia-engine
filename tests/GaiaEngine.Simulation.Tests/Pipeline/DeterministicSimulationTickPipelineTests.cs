using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Pipeline;
using GaiaEngine.Simulation.Scheduling;
using GaiaEngine.Simulation.Time;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Pipeline;

public sealed class DeterministicSimulationTickPipelineTests
{
    [Fact]
    public void Execute_ShouldRunAllPhasesInApprovedOrder()
    {
        DeterministicTimeSystem timeSystem = new(new SimulationCalendar(4, 3));
        EventBus eventBus = new();
        DeterministicSimulationScheduler scheduler = new(
            new[]
            {
                new ScheduledSimulationSystemDefinition(SimulationSystemNames.Statistics, SimulationTickPhase.PostUpdate, 100, 0),
            });
        DeterministicSimulationTickPipeline pipeline = CreatePipeline(
            timeSystem,
            scheduler,
            CreateEventPublisher(eventBus),
            eventBus,
            new SimulationDiagnosticsCollector());

        SimulationTickResult result = pipeline.Execute(new WorldTimeState(99, 0, "Spring", 0), 1);

        Assert.Equal(8, result.ExecutedPhases.Count);
        Assert.Equal(SimulationTickPhase.InputCollection, result.ExecutedPhases[0]);
        Assert.Equal(SimulationTickPhase.PreUpdate, result.ExecutedPhases[1]);
        Assert.Equal(SimulationTickPhase.WorldUpdate, result.ExecutedPhases[2]);
        Assert.Equal(SimulationTickPhase.OrganismUpdate, result.ExecutedPhases[3]);
        Assert.Equal(SimulationTickPhase.InteractionSystems, result.ExecutedPhases[4]);
        Assert.Equal(SimulationTickPhase.EnvironmentUpdate, result.ExecutedPhases[5]);
        Assert.Equal(SimulationTickPhase.EventDispatch, result.ExecutedPhases[6]);
        Assert.Equal(SimulationTickPhase.PostUpdate, result.ExecutedPhases[7]);
        Assert.Equal(100, result.TimeState.CurrentTick);
        Assert.Single(result.Schedule.Systems);
        Assert.Single(result.EventPublicationResult.PublishedEvents);
        Assert.NotNull(result.EventDispatchResult);
        Assert.NotNull(result.Diagnostics);
    }

    [Fact]
    public void Execute_ShouldExposeTimeAdvanceProducedByWorldUpdate()
    {
        DeterministicTimeSystem timeSystem = new(new SimulationCalendar(4, 3));
        EventBus eventBus = new();
        DeterministicSimulationScheduler scheduler = new(
            new[]
            {
                new ScheduledSimulationSystemDefinition("Climate", SimulationTickPhase.WorldUpdate, 4, 1),
                new ScheduledSimulationSystemDefinition("Terrain", SimulationTickPhase.WorldUpdate, 2, 0),
                new ScheduledSimulationSystemDefinition(SimulationSystemNames.Statistics, SimulationTickPhase.PostUpdate, 100, 0),
            });
        List<string> receivedEvents = new();
        eventBus.Subscribe<NewDaySimulationEvent>(eventInstance => receivedEvents.Add(eventInstance.EventType));
        DeterministicSimulationTickPipeline pipeline = CreatePipeline(timeSystem, scheduler, CreateEventPublisher(eventBus), eventBus, new SimulationDiagnosticsCollector());

        SimulationTickResult result = pipeline.Execute(new WorldTimeState(3, 0, "Spring", 0), 1);

        Assert.NotNull(result.TimeAdvanceResult);
        Assert.Equal(4, result.TimeAdvanceResult!.TimeState.CurrentTick);
        Assert.Single(result.TimeAdvanceResult.Transitions);
        Assert.Equal(2, result.Schedule.Systems.Count);
        Assert.Equal(4, result.Schedule.ExecutingTick);
        Assert.Single(result.EventPublicationResult.PublishedEvents);
        Assert.Equal(1, result.EventDispatchResult!.ProcessedEventCount);
        Assert.Single(receivedEvents);
        Assert.Null(result.Diagnostics);
    }

    [Fact]
    public void Constructor_ShouldRejectInvalidPhaseOrder()
    {
        Assert.Throws<ArgumentException>(
            () => new DeterministicSimulationTickPipeline(
                new ISimulationTickPhase[]
                {
                    new NoOpSimulationTickPhase(SimulationTickPhase.PreUpdate),
                    new NoOpSimulationTickPhase(SimulationTickPhase.InputCollection),
                    new NoOpSimulationTickPhase(SimulationTickPhase.WorldUpdate),
                    new NoOpSimulationTickPhase(SimulationTickPhase.OrganismUpdate),
                    new NoOpSimulationTickPhase(SimulationTickPhase.InteractionSystems),
                    new NoOpSimulationTickPhase(SimulationTickPhase.EnvironmentUpdate),
                    new EventDispatchPhase(new EventBus()),
                    new NoOpSimulationTickPhase(SimulationTickPhase.PostUpdate),
                },
                new DeterministicSimulationScheduler(Array.Empty<ScheduledSimulationSystemDefinition>())));
    }

    private static DeterministicSimulationTickPipeline CreatePipeline(
        ITimeSystem timeSystem,
        ISimulationScheduler scheduler,
        ISimulationEventPublisher eventPublisher,
        IEventBus eventBus,
        ISimulationDiagnosticsCollector diagnosticsCollector)
    {
        return new DeterministicSimulationTickPipeline(
            new ISimulationTickPhase[]
            {
                new NoOpSimulationTickPhase(SimulationTickPhase.InputCollection),
                new NoOpSimulationTickPhase(SimulationTickPhase.PreUpdate),
                new WorldUpdateTimePhase(timeSystem, eventPublisher),
                new NoOpSimulationTickPhase(SimulationTickPhase.OrganismUpdate),
                new NoOpSimulationTickPhase(SimulationTickPhase.InteractionSystems),
                new NoOpSimulationTickPhase(SimulationTickPhase.EnvironmentUpdate),
                new EventDispatchPhase(eventBus),
                new PostUpdateStatisticsPhase(diagnosticsCollector),
            },
            scheduler);
    }

    private static ISimulationEventPublisher CreateEventPublisher(IEventBus eventBus)
    {
        return new SimulationEventPublisher(eventBus, new DeterministicEntityIdGenerator());
    }
}

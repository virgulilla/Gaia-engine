using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Pipeline;
using GaiaEngine.Simulation.Scheduling;
using GaiaEngine.Simulation.Time;
using GaiaEngine.Simulation.World.Climate;
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
                new ScheduledSimulationSystemDefinition(SimulationSystemNames.Climate, SimulationTickPhase.WorldUpdate, 10, 0),
                new ScheduledSimulationSystemDefinition(SimulationSystemNames.Statistics, SimulationTickPhase.PostUpdate, 100, 0),
            });
        DeterministicSimulationTickPipeline pipeline = CreatePipeline(
            timeSystem,
            scheduler,
            CreateEventPublisher(eventBus),
            eventBus,
            new SimulationDiagnosticsCollector(),
            new DeterministicClimateSystem(new ClimateSystemSettings(300, 18, 10, 55, 1012, 4)));

        SimulationTickResult result = pipeline.Execute(CreateWorld(99, 0, "Spring", 0), 1);

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
        Assert.Equal(2, result.Schedule.Systems.Count);
        Assert.Single(result.EventPublicationResult.PublishedEvents);
        Assert.NotNull(result.EventDispatchResult);
        Assert.NotNull(result.Diagnostics);
        Assert.NotEqual(18, result.World.GetChunks()[0].Climate.Temperature.CurrentTemperature);
    }

    [Fact]
    public void Execute_ShouldExposeTimeAdvanceProducedByWorldUpdate()
    {
        DeterministicTimeSystem timeSystem = new(new SimulationCalendar(4, 3));
        EventBus eventBus = new();
        DeterministicSimulationScheduler scheduler = new(
            new[]
            {
                new ScheduledSimulationSystemDefinition(SimulationSystemNames.Climate, SimulationTickPhase.WorldUpdate, 4, 0),
                new ScheduledSimulationSystemDefinition("Terrain", SimulationTickPhase.WorldUpdate, 2, 0),
                new ScheduledSimulationSystemDefinition(SimulationSystemNames.Statistics, SimulationTickPhase.PostUpdate, 100, 0),
            });
        List<string> receivedEvents = new();
        eventBus.Subscribe<NewDaySimulationEvent>(eventInstance => receivedEvents.Add(eventInstance.EventType));
        DeterministicSimulationTickPipeline pipeline = CreatePipeline(
            timeSystem,
            scheduler,
            CreateEventPublisher(eventBus),
            eventBus,
            new SimulationDiagnosticsCollector(),
            new DeterministicClimateSystem(new ClimateSystemSettings(300, 18, 10, 55, 1012, 4)));

        SimulationTickResult result = pipeline.Execute(CreateWorld(3, 0, "Spring", 0), 1);

        Assert.NotNull(result.TimeAdvanceResult);
        Assert.Equal(4, result.TimeAdvanceResult!.TimeState.CurrentTick);
        Assert.Single(result.TimeAdvanceResult.Transitions);
        Assert.Equal(2, result.Schedule.Systems.Count);
        Assert.Equal(4, result.Schedule.ExecutingTick);
        Assert.Single(result.EventPublicationResult.PublishedEvents);
        Assert.Equal(1, result.EventDispatchResult!.ProcessedEventCount);
        Assert.Single(receivedEvents);
        Assert.Null(result.Diagnostics);
        Assert.NotEqual(18, result.World.GetChunks()[0].Climate.Temperature.CurrentTemperature);
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
        ISimulationDiagnosticsCollector diagnosticsCollector,
        IClimateSystem climateSystem)
    {
        return new DeterministicSimulationTickPipeline(
            new ISimulationTickPhase[]
            {
                new NoOpSimulationTickPhase(SimulationTickPhase.InputCollection),
                new NoOpSimulationTickPhase(SimulationTickPhase.PreUpdate),
                new WorldUpdateTimePhase(timeSystem, scheduler, climateSystem, eventPublisher),
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

    private static GaiaEngine.Domain.World.World CreateWorld(long tick, int day, string season, int year)
    {
        WorldId worldId = WorldId.FromSequence(new EntitySequence(1));
        return new GaiaEngine.Domain.World.World(
            new WorldMetadata(
                worldId,
                "Gaia",
                new WorldSeed(42),
                "2026-06-28",
                new EngineVersion(1, 0, 0),
                new ConfigurationVersion("2026.06.28")),
            new WorldDimensions(32, 32, 16, 4, 200),
            new WorldTimeState(tick, day, season, year),
            new[]
            {
                CreateChunk(worldId, 2, new ChunkCoordinates(0, 0), ClimateZone.Temperate),
                CreateChunk(worldId, 3, new ChunkCoordinates(1, 0), ClimateZone.Arid),
                CreateChunk(worldId, 4, new ChunkCoordinates(0, 1), ClimateZone.Continental),
                CreateChunk(worldId, 5, new ChunkCoordinates(1, 1), ClimateZone.Polar),
            });
    }

    private static Chunk CreateChunk(WorldId worldId, ulong sequence, ChunkCoordinates coordinates, ClimateZone zone)
    {
        return new Chunk(
            new ChunkMetadata(
                ChunkId.FromSequence(new EntitySequence(sequence)),
                worldId,
                coordinates,
                new WorldSeed((long)sequence * 10),
                16),
            ChunkState.Active,
            new ClimateState(
                zone,
                WeatherState.Clear,
                new TemperatureState(18, 18, 18, 0),
                new HumidityState(55, 3, 2),
                new WindState(90, 4, 6),
                new PrecipitationState(PrecipitationType.None, 0, 0, 0),
                new PressureState(1012)),
            Array.Empty<OrganismId>());
    }
}

using System;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Pipeline;
using GaiaEngine.Simulation.Runtime;
using GaiaEngine.Simulation.Scheduling;
using GaiaEngine.Simulation.Time;
using GaiaEngine.Simulation.World.Climate;
using GaiaEngine.Simulation.World.Resources;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Runtime;

public sealed class DeterministicSimulationSessionTests
{
    [Fact]
    public void AdvanceTick_ShouldUpdateCurrentTimeState()
    {
        DeterministicTimeSystem timeSystem = new(new SimulationCalendar(4, 3));
        EventBus eventBus = new();
        SimulationEventPublisher eventPublisher = new(eventBus, new DeterministicEntityIdGenerator());
        DeterministicSimulationScheduler scheduler = new(
            new[]
            {
                new ScheduledSimulationSystemDefinition(SimulationSystemNames.Climate, SimulationTickPhase.WorldUpdate, 10, 0),
                new ScheduledSimulationSystemDefinition(SimulationSystemNames.Resources, SimulationTickPhase.WorldUpdate, 20, 0),
                new ScheduledSimulationSystemDefinition(SimulationSystemNames.Statistics, SimulationTickPhase.PostUpdate, 100, 0),
            });
        DeterministicSimulationTickPipeline pipeline = new(
            new ISimulationTickPhase[]
            {
                new NoOpSimulationTickPhase(SimulationTickPhase.InputCollection),
                new NoOpSimulationTickPhase(SimulationTickPhase.PreUpdate),
                new WorldUpdateTimePhase(
                    timeSystem,
                    scheduler,
                    new DeterministicClimateSystem(new ClimateSystemSettings(300, 18, 10, 55, 1012, 4)),
                    new DeterministicResourceSystem(new ResourceSystemSettings(3, 2, 3, 4)),
                    eventPublisher),
                new NoOpSimulationTickPhase(SimulationTickPhase.OrganismUpdate),
                new NoOpSimulationTickPhase(SimulationTickPhase.InteractionSystems),
                new NoOpSimulationTickPhase(SimulationTickPhase.EnvironmentUpdate),
                new EventDispatchPhase(eventBus),
                new PostUpdateStatisticsPhase(new SimulationDiagnosticsCollector()),
            },
            scheduler);
        DeterministicSimulationSession session = new(pipeline, CreateWorld(99, 0, "Spring", 0));

        SimulationTickResult result = session.AdvanceTick();

        Assert.Equal(result.TimeState, session.CurrentTimeState);
        Assert.Equal(100, session.CurrentTimeState.CurrentTick);
        Assert.Equal(8, result.ExecutedPhases.Count);
        Assert.Equal(2UL, result.NextEventSequence);
        Assert.NotNull(result.Diagnostics);
        Assert.Equal(100, session.CurrentWorld.TimeState.CurrentTick);
        Assert.True(session.CurrentWorld.GetChunks()[0].Resources.TryGet(ResourceType.Vegetation, out ResourceState? vegetation));
        Assert.NotNull(vegetation);
    }

    [Fact]
    public void AdvanceTick_ShouldAdvanceEventSequenceWhenEventsArePublished()
    {
        DeterministicTimeSystem timeSystem = new(new SimulationCalendar(4, 3));
        EventBus eventBus = new();
        SimulationEventPublisher eventPublisher = new(eventBus, new DeterministicEntityIdGenerator());
        DeterministicSimulationScheduler scheduler = new(
            new[]
            {
                new ScheduledSimulationSystemDefinition(SimulationSystemNames.Climate, SimulationTickPhase.WorldUpdate, 10, 0),
                new ScheduledSimulationSystemDefinition(SimulationSystemNames.Resources, SimulationTickPhase.WorldUpdate, 20, 0),
                new ScheduledSimulationSystemDefinition(SimulationSystemNames.Statistics, SimulationTickPhase.PostUpdate, 100, 0),
            });
        DeterministicSimulationTickPipeline pipeline = new(
            new ISimulationTickPhase[]
            {
                new NoOpSimulationTickPhase(SimulationTickPhase.InputCollection),
                new NoOpSimulationTickPhase(SimulationTickPhase.PreUpdate),
                new WorldUpdateTimePhase(
                    timeSystem,
                    scheduler,
                    new DeterministicClimateSystem(new ClimateSystemSettings(300, 18, 10, 55, 1012, 4)),
                    new DeterministicResourceSystem(new ResourceSystemSettings(3, 2, 3, 4)),
                    eventPublisher),
                new NoOpSimulationTickPhase(SimulationTickPhase.OrganismUpdate),
                new NoOpSimulationTickPhase(SimulationTickPhase.InteractionSystems),
                new NoOpSimulationTickPhase(SimulationTickPhase.EnvironmentUpdate),
                new EventDispatchPhase(eventBus),
                new PostUpdateStatisticsPhase(new SimulationDiagnosticsCollector()),
            },
            scheduler);
        DeterministicSimulationSession session = new(pipeline, CreateWorld(3, 0, "Spring", 0));

        SimulationTickResult first = session.AdvanceTick();
        SimulationTickResult second = session.AdvanceTick();

        Assert.Equal(2UL, first.NextEventSequence);
        Assert.Equal(2UL, second.NextEventSequence);
        Assert.Null(first.Diagnostics);
        Assert.Null(second.Diagnostics);
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
                CreateChunk(worldId, 2, new ChunkCoordinates(0, 0)),
                CreateChunk(worldId, 3, new ChunkCoordinates(1, 0)),
                CreateChunk(worldId, 4, new ChunkCoordinates(0, 1)),
                CreateChunk(worldId, 5, new ChunkCoordinates(1, 1)),
            });
    }

    private static Chunk CreateChunk(WorldId worldId, ulong sequence, ChunkCoordinates coordinates)
    {
        return new Chunk(
            new ChunkMetadata(
                ChunkId.FromSequence(new EntitySequence(sequence)),
                worldId,
                coordinates,
                new WorldSeed((long)sequence * 10),
                16),
            ChunkState.Active,
            CreateTerrain(sequence),
            CreateBiome(sequence),
            new ClimateState(
                ClimateZone.Temperate,
                WeatherState.Clear,
                new TemperatureState(18, 18, 18, 0),
                new HumidityState(55, 3, 2),
                new WindState(90, 4, 6),
                new PrecipitationState(PrecipitationType.None, 0, 0, 0),
                new PressureState(1012)),
            CreateResources(sequence),
            Array.Empty<OrganismId>());
    }

    private static BiomeState CreateBiome(ulong sequence)
    {
        return new BiomeState(
            BiomeId.FromSequence(new EntitySequence((sequence * 10) + 4)),
            "Grassland",
            BiomeCategory.Plains,
            "Open plains with moderate fertility and dominant grass vegetation.",
            new BiomeClimateProfile(18, 2, 55, 4, 8),
            new BiomeTerrainProfile(40, 80, SoilType.Loam, SurfaceType.Grass, 60),
            new BiomeResourceProfile(750, 800, 500, 800),
            new BiomeVegetationProfile(VegetationType.Grassland, 62),
            new BiomeSpeciesAffinityProfile(72, 46, 60, 20));
    }

    private static TerrainState CreateTerrain(ulong sequence)
    {
        return new TerrainState(
            new ElevationState(50 + (int)sequence, (int)sequence, (int)sequence),
            new SlopeState(9, (int)((sequence * 41) % 360), 118),
            new SoilState(SoilType.Loam, 74, 61, 69, 66),
            SurfaceType.Grass,
            GeologyType.Granite,
            Array.Empty<TerrainModifierState>());
    }

    private static ChunkResources CreateResources(ulong sequence)
    {
        return new ChunkResources(
            new ResourceState[]
            {
                new(
                    ResourceId.FromSequence(new EntitySequence((sequence * 10) + 1)),
                    ResourceType.Vegetation,
                    ResourceCategory.Renewable,
                    400,
                    500,
                    4,
                    70,
                    800),
                new(
                    ResourceId.FromSequence(new EntitySequence((sequence * 10) + 2)),
                    ResourceType.FreshWater,
                    ResourceCategory.Renewable,
                    300,
                    400,
                    3,
                    80,
                    750),
                new(
                    ResourceId.FromSequence(new EntitySequence((sequence * 10) + 3)),
                    ResourceType.Minerals,
                    ResourceCategory.NonRenewable,
                    250,
                    250,
                    0,
                    65,
                    500),
            });
    }
}

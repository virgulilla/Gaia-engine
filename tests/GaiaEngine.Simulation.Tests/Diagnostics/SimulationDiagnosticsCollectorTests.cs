using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Pipeline;
using GaiaEngine.Simulation.Scheduling;
using GaiaEngine.Simulation.Time;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Diagnostics;

public sealed class SimulationDiagnosticsCollectorTests
{
    [Fact]
    public void Capture_ShouldCreateDeterministicSnapshotFromTickContext()
    {
        SimulationTickContext context = new(CreateWorldState(100, 2, "Summer", 1), 4);
        context.ApplySchedule(
            new SimulationTickSchedule(
                100,
                new[]
                {
                    new ScheduledSimulationSystem("Statistics", SimulationTickPhase.PostUpdate, 0, 100),
                }));
        context.ApplyEventPublicationResult(
            new SimulationEventPublicationResult(
                6,
                new IEvent[]
                {
                    new NewDaySimulationEvent(GaiaEngine.Domain.Identifiers.EventId.FromSequence(new GaiaEngine.Domain.Identifiers.EntitySequence(4)), 100, 100, 2, "Summer", 1),
                }));
        context.ApplyEventDispatchResult(new EventDispatchResult(1, 0, System.Array.Empty<EventDispatchFailure>()));
        context.RegisterExecutedPhase(SimulationTickPhase.InputCollection);
        context.RegisterExecutedPhase(SimulationTickPhase.PreUpdate);
        context.RegisterExecutedPhase(SimulationTickPhase.WorldUpdate);
        context.RegisterExecutedPhase(SimulationTickPhase.EventDispatch);

        SimulationDiagnosticsCollector collector = new();
        SimulationTickDiagnostics diagnostics = collector.Capture(context);

        Assert.Equal(100, diagnostics.Tick);
        Assert.Equal(2, diagnostics.Day);
        Assert.Equal("Summer", diagnostics.Season);
        Assert.Equal(1, diagnostics.Year);
        Assert.Equal(4, diagnostics.ExecutedPhaseCount);
        Assert.Equal(1, diagnostics.ScheduledSystemCount);
        Assert.Equal(1, diagnostics.PublishedEventCount);
        Assert.Equal(1, diagnostics.ProcessedEventCount);
    }

    private static GaiaEngine.Domain.World.World CreateWorldState(long tick, int day, string season, int year)
    {
        GaiaEngine.Domain.Identifiers.WorldId worldId = GaiaEngine.Domain.Identifiers.WorldId.FromSequence(new GaiaEngine.Domain.Identifiers.EntitySequence(1));
        return new GaiaEngine.Domain.World.World(
            new GaiaEngine.Domain.World.WorldMetadata(
                worldId,
                "Gaia",
                new GaiaEngine.Foundation.Determinism.WorldSeed(42),
                "2026-06-28",
                new GaiaEngine.Foundation.Versioning.EngineVersion(1, 0, 0),
                new GaiaEngine.Foundation.Configuration.ConfigurationVersion("2026.06.28")),
            new GaiaEngine.Domain.World.WorldDimensions(32, 32, 16, 1, 200),
            new WorldTimeState(tick, day, season, year),
            new[]
            {
                new GaiaEngine.Domain.World.Chunk(
                    new GaiaEngine.Domain.World.ChunkMetadata(
                        GaiaEngine.Domain.Identifiers.ChunkId.FromSequence(new GaiaEngine.Domain.Identifiers.EntitySequence(2)),
                        worldId,
                        new GaiaEngine.Domain.World.ChunkCoordinates(0, 0),
                        new GaiaEngine.Foundation.Determinism.WorldSeed(100),
                        16),
                    GaiaEngine.Domain.World.ChunkState.Active,
                    CreateTerrain(),
                    CreateBiome(),
                    new GaiaEngine.Domain.World.ClimateState(
                        GaiaEngine.Domain.World.ClimateZone.Temperate,
                        GaiaEngine.Domain.World.WeatherState.Clear,
                        new GaiaEngine.Domain.World.TemperatureState(18, 18, 18, 0),
                        new GaiaEngine.Domain.World.HumidityState(55, 3, 2),
                        new GaiaEngine.Domain.World.WindState(90, 4, 6),
                        new GaiaEngine.Domain.World.PrecipitationState(GaiaEngine.Domain.World.PrecipitationType.None, 0, 0, 0),
                        new GaiaEngine.Domain.World.PressureState(1012)),
                    CreateWater(),
                    CreateResources(2),
                    System.Array.Empty<GaiaEngine.Domain.Identifiers.OrganismId>()),
            });
    }

    private static GaiaEngine.Domain.World.BiomeState CreateBiome()
    {
        return new GaiaEngine.Domain.World.BiomeState(
            GaiaEngine.Domain.Identifiers.BiomeId.FromSequence(new GaiaEngine.Domain.Identifiers.EntitySequence(24)),
            "Grassland",
            GaiaEngine.Domain.World.BiomeCategory.Plains,
            "Open plains biome.",
            new GaiaEngine.Domain.World.BiomeClimateProfile(18, 2, 55, 4, 8),
            new GaiaEngine.Domain.World.BiomeTerrainProfile(0, 20, GaiaEngine.Domain.World.SoilType.Loam, GaiaEngine.Domain.World.SurfaceType.Grass, 60),
            new GaiaEngine.Domain.World.BiomeResourceProfile(750, 800, 500, 800),
            new GaiaEngine.Domain.World.BiomeVegetationProfile(GaiaEngine.Domain.World.VegetationType.Grassland, 62),
            new GaiaEngine.Domain.World.BiomeSpeciesAffinityProfile(72, 46, 60, 20));
    }

    private static GaiaEngine.Domain.World.TerrainState CreateTerrain()
    {
        return new GaiaEngine.Domain.World.TerrainState(
            new GaiaEngine.Domain.World.ElevationState(62, 2, 2),
            new GaiaEngine.Domain.World.SlopeState(7, 90, 114),
            new GaiaEngine.Domain.World.SoilState(GaiaEngine.Domain.World.SoilType.Loam, 76, 63, 71, 69),
            GaiaEngine.Domain.World.SurfaceType.Grass,
            GaiaEngine.Domain.World.GeologyType.Limestone,
            System.Array.Empty<GaiaEngine.Domain.World.TerrainModifierState>());
    }

    private static GaiaEngine.Domain.World.WaterState CreateWater()
    {
        return new GaiaEngine.Domain.World.WaterState(
            new GaiaEngine.Domain.World.SurfaceWaterState(220, 3, 90, 400),
            new GaiaEngine.Domain.World.GroundWaterState(42, 58, 6, 0),
            null,
            null,
            null);
    }

    private static GaiaEngine.Domain.World.ChunkResources CreateResources(ulong sequence)
    {
        return new GaiaEngine.Domain.World.ChunkResources(
            new GaiaEngine.Domain.World.ResourceState[]
            {
                new(
                    GaiaEngine.Domain.Identifiers.ResourceId.FromSequence(new GaiaEngine.Domain.Identifiers.EntitySequence((sequence * 10) + 1)),
                    GaiaEngine.Domain.World.ResourceType.Vegetation,
                    GaiaEngine.Domain.World.ResourceCategory.Renewable,
                    400,
                    500,
                    4,
                    70,
                    800),
                new(
                    GaiaEngine.Domain.Identifiers.ResourceId.FromSequence(new GaiaEngine.Domain.Identifiers.EntitySequence((sequence * 10) + 2)),
                    GaiaEngine.Domain.World.ResourceType.FreshWater,
                    GaiaEngine.Domain.World.ResourceCategory.Renewable,
                    300,
                    400,
                    3,
                    80,
                    750),
                new(
                    GaiaEngine.Domain.Identifiers.ResourceId.FromSequence(new GaiaEngine.Domain.Identifiers.EntitySequence((sequence * 10) + 3)),
                    GaiaEngine.Domain.World.ResourceType.Minerals,
                    GaiaEngine.Domain.World.ResourceCategory.NonRenewable,
                    250,
                    250,
                    0,
                    65,
                    500),
            });
    }
}

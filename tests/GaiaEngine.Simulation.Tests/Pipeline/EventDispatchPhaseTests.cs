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
        SimulationTickContext context = new(CreateWorldState(8, "Spring", 1), 4);

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

    private static GaiaEngine.Domain.World.World CreateWorldState(long tick, string season, int year)
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
            new WorldTimeState(tick, 0, season, year),
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

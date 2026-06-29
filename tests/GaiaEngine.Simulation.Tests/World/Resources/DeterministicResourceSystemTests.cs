using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.World.Resources;
using Xunit;

namespace GaiaEngine.Simulation.Tests.World.Resources;

public sealed class DeterministicResourceSystemTests
{
    [Fact]
    public void UpdateWorld_ShouldProduceDeterministicResourcesForIdenticalInput()
    {
        DeterministicResourceSystem resourceSystem = new(new ResourceSystemSettings(3, 2, 3, 4));
        GaiaEngine.Domain.World.World world = CreateWorld("Spring", WeatherState.Rain);

        GaiaEngine.Domain.World.World first = resourceSystem.UpdateWorld(world);
        GaiaEngine.Domain.World.World second = resourceSystem.UpdateWorld(world);

        Assert.Equal(first.GetChunks()[0].Resources.GetAll(), second.GetChunks()[0].Resources.GetAll());
    }

    [Fact]
    public void UpdateWorld_ShouldIncreaseVegetationDuringWetSpringConditions()
    {
        DeterministicResourceSystem resourceSystem = new(new ResourceSystemSettings(3, 2, 3, 4));
        GaiaEngine.Domain.World.World world = resourceSystem.UpdateWorld(CreateWorld("Spring", WeatherState.Rain));

        ResourceState vegetation = world.GetChunks()[0].Resources.GetAll()[0];

        Assert.Equal(ResourceType.Vegetation, vegetation.Type);
        Assert.True(vegetation.CurrentAmount > 400);
        Assert.True(vegetation.Availability >= 800);
    }

    [Fact]
    public void UpdateWorld_ShouldPreserveNonRenewableMinerals()
    {
        DeterministicResourceSystem resourceSystem = new(new ResourceSystemSettings(3, 2, 3, 4));
        GaiaEngine.Domain.World.World world = resourceSystem.UpdateWorld(CreateWorld("Winter", WeatherState.Snow));

        ResourceState minerals = world.GetChunks()[0].Resources.GetAll()[2];

        Assert.Equal(ResourceType.Minerals, minerals.Type);
        Assert.Equal(250, minerals.CurrentAmount);
        Assert.Equal(0, minerals.RegenerationRate);
    }

    private static GaiaEngine.Domain.World.World CreateWorld(string season, WeatherState weatherState)
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
            new WorldDimensions(100, 100, 16, 1, 200),
            new WorldTimeState(40, 1, season, 0),
            new List<Chunk>
            {
                new(
                    new ChunkMetadata(
                        ChunkId.FromSequence(new EntitySequence(10)),
                        worldId,
                        new ChunkCoordinates(0, 0),
                        new WorldSeed(100),
                        16),
                    ChunkState.Active,
                    CreateTerrain(),
                    CreateBiome(),
                    new ClimateState(
                        ClimateZone.Temperate,
                        weatherState,
                        new TemperatureState(18, 18, 18, 0),
                        new HumidityState(72, 6, 3),
                        new WindState(90, 4, 6),
                        new PrecipitationState(
                            weatherState == WeatherState.Snow ? PrecipitationType.Snow : PrecipitationType.Rain,
                            9,
                            3,
                            80),
                        new PressureState(1005)),
                    CreateWater(),
                    CreateResources(10),
                    System.Array.Empty<OrganismId>()),
            });
    }

    private static BiomeState CreateBiome()
    {
        return new BiomeState(
            BiomeId.FromSequence(new EntitySequence(104)),
            "Grassland",
            BiomeCategory.Plains,
            "Open plains with moderate fertility and dominant grass vegetation.",
            new BiomeClimateProfile(18, 2, 55, 4, 8),
            new BiomeTerrainProfile(40, 80, SoilType.Loam, SurfaceType.Grass, 60),
            new BiomeResourceProfile(750, 800, 500, 800),
            new BiomeVegetationProfile(VegetationType.Grassland, 62),
            new BiomeSpeciesAffinityProfile(72, 46, 60, 20));
    }

    private static TerrainState CreateTerrain()
    {
        return new TerrainState(
            new ElevationState(64, 4, 4),
            new SlopeState(8, 120, 116),
            new SoilState(SoilType.Loam, 78, 62, 74, 68),
            SurfaceType.Grass,
            GeologyType.Limestone,
            System.Array.Empty<TerrainModifierState>());
    }

    private static WaterState CreateWater()
    {
        return new WaterState(
            new SurfaceWaterState(420, 4, 90, 700),
            new GroundWaterState(48, 72, 8, 0),
            null,
            null,
            null);
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

using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.World.Climate;
using Xunit;

namespace GaiaEngine.Simulation.Tests.World.Climate;

public sealed class DeterministicClimateSystemTests
{
    [Fact]
    public void UpdateWorld_ShouldProduceDeterministicClimateForIdenticalInput()
    {
        DeterministicClimateSystem climateSystem = new(new ClimateSystemSettings(300, 18, 10, 55, 1012, 4));
        GaiaEngine.Domain.World.World world = CreateWorld("Summer");

        GaiaEngine.Domain.World.World first = climateSystem.UpdateWorld(world);
        GaiaEngine.Domain.World.World second = climateSystem.UpdateWorld(world);

        Assert.Equal(first.GetChunks()[0].Climate, second.GetChunks()[0].Climate);
        Assert.Equal(first.GetChunks()[1].Climate, second.GetChunks()[1].Climate);
    }

    [Fact]
    public void UpdateWorld_ShouldMakeSummerWarmerThanWinterForTheSameChunk()
    {
        DeterministicClimateSystem climateSystem = new(new ClimateSystemSettings(300, 18, 10, 55, 1012, 4));
        GaiaEngine.Domain.World.World summerWorld = climateSystem.UpdateWorld(CreateWorld("Summer"));
        GaiaEngine.Domain.World.World winterWorld = climateSystem.UpdateWorld(CreateWorld("Winter"));

        Assert.True(
            summerWorld.GetChunks()[0].Climate.Temperature.CurrentTemperature >
            winterWorld.GetChunks()[0].Climate.Temperature.CurrentTemperature);
    }

    [Fact]
    public void UpdateWorld_ShouldUpdateEveryChunkAndPreserveOwnership()
    {
        DeterministicClimateSystem climateSystem = new(new ClimateSystemSettings(300, 18, 10, 55, 1012, 4));
        GaiaEngine.Domain.World.World world = climateSystem.UpdateWorld(CreateWorld("Spring"));

        Assert.Equal(2, world.ChunkCount);
        Assert.Equal(WeatherState.Clear, world.GetChunks()[0].Climate.WeatherState);
        Assert.NotEqual(0, world.GetChunks()[1].Climate.Wind.Direction);
        Assert.Equal(world.Id, world.GetChunks()[0].Metadata.WorldId);
        Assert.Equal(world.Id, world.GetChunks()[1].Metadata.WorldId);
    }

    private static GaiaEngine.Domain.World.World CreateWorld(string season)
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
            new WorldDimensions(100, 100, 16, 2, 200),
            new WorldTimeState(150, 1, season, 0),
            new List<Chunk>
            {
                CreateChunk(worldId, 10, new ChunkCoordinates(0, 0), ClimateZone.Temperate),
                CreateChunk(worldId, 11, new ChunkCoordinates(1, 0), ClimateZone.Arid),
            });
    }

    private static Chunk CreateChunk(WorldId worldId, ulong sequence, ChunkCoordinates coordinates, ClimateZone zone)
    {
        return new Chunk(
            new ChunkMetadata(
                ChunkId.FromSequence(new EntitySequence(sequence)),
                worldId,
                coordinates,
                new WorldSeed((long)sequence),
                16),
            ChunkState.Active,
            CreateTerrain(sequence),
            CreateBiome(sequence),
            new ClimateState(
                zone,
                WeatherState.Clear,
                new TemperatureState(18, 18, 18, 0),
                new HumidityState(55, 3, 2),
                new WindState(90, 4, 6),
                new PrecipitationState(PrecipitationType.None, 0, 0, 0),
                new PressureState(1012)),
            CreateWater(sequence),
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
            new ElevationState(58 + (int)sequence, (int)sequence - 10, (int)sequence - 10),
            new SlopeState(14, (int)((sequence * 29) % 360), 128),
            new SoilState(SoilType.Loam, 76, 60, 71, 67),
            SurfaceType.Grass,
            GeologyType.Limestone,
            Array.Empty<TerrainModifierState>());
    }

    private static WaterState CreateWater(ulong sequence)
    {
        return new WaterState(
            new SurfaceWaterState(220 + (int)sequence, 3, 90, 400 + ((int)sequence * 10)),
            new GroundWaterState(42, 58, 6, 0),
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

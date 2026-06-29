using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.World.Water;
using Xunit;

namespace GaiaEngine.Simulation.Tests.World.Water;

public sealed class DeterministicWaterSystemTests
{
    [Fact]
    public void UpdateWorld_ShouldProduceDeterministicWaterForIdenticalInput()
    {
        DeterministicWaterSystem waterSystem = new(new WaterSystemSettings(8, 4, 12, 6));
        GaiaEngine.Domain.World.World world = CreateWorld();

        GaiaEngine.Domain.World.World first = waterSystem.UpdateWorld(world);
        GaiaEngine.Domain.World.World second = waterSystem.UpdateWorld(world);

        Assert.Equal(first.GetChunks()[0].Water, second.GetChunks()[0].Water);
    }

    [Fact]
    public void UpdateWorld_ShouldIncreaseSurfaceWaterDuringRain()
    {
        DeterministicWaterSystem waterSystem = new(new WaterSystemSettings(8, 4, 12, 6));
        GaiaEngine.Domain.World.World updatedWorld = waterSystem.UpdateWorld(CreateWorld());

        Assert.True(updatedWorld.GetChunks()[0].Water.SurfaceWater.WaterLevel > 220);
    }

    [Fact]
    public void UpdateWorld_ShouldCreateRiverWhenSlopeAndWaterAreHigh()
    {
        DeterministicWaterSystem waterSystem = new(new WaterSystemSettings(8, 4, 12, 6));
        GaiaEngine.Domain.World.World updatedWorld = waterSystem.UpdateWorld(CreateWorld());

        Assert.NotNull(updatedWorld.GetChunks()[0].Water.River);
    }

    private static GaiaEngine.Domain.World.World CreateWorld()
    {
        WorldId worldId = WorldId.FromSequence(new EntitySequence(1));
        return new GaiaEngine.Domain.World.World(
            new WorldMetadata(
                worldId,
                "Gaia",
                new WorldSeed(42),
                "2026-06-29",
                new EngineVersion(1, 0, 0),
                new ConfigurationVersion("2026.06.29")),
            new WorldDimensions(32, 32, 16, 2, 200),
            new WorldTimeState(4, 1, "Spring", 0),
            new List<Chunk>
            {
                CreateChunk(worldId, 2, new ChunkCoordinates(0, 0), 64, 18),
                CreateChunk(worldId, 3, new ChunkCoordinates(1, 0), 72, 12),
            });
    }

    private static Chunk CreateChunk(WorldId worldId, ulong sequence, ChunkCoordinates coordinates, int elevation, int slope)
    {
        return new Chunk(
            new ChunkMetadata(
                ChunkId.FromSequence(new EntitySequence(sequence)),
                worldId,
                coordinates,
                new WorldSeed((long)sequence * 10),
                16),
            ChunkState.Active,
            new TerrainState(
                new ElevationState(elevation, elevation - 60, elevation - 60),
                new SlopeState(slope, 90, 100 + slope),
                new SoilState(SoilType.Loam, 76, 40, 72, 68),
                SurfaceType.Grass,
                GeologyType.Granite,
                System.Array.Empty<TerrainModifierState>()),
            new BiomeState(
                BiomeId.FromSequence(new EntitySequence((sequence * 10) + 4)),
                "Grassland",
                BiomeCategory.Plains,
                "Open plains biome.",
                new BiomeClimateProfile(18, 6, 72, 4, 8),
                new BiomeTerrainProfile(40, 80, SoilType.Loam, SurfaceType.Grass, 40),
                new BiomeResourceProfile(750, 800, 500, 800),
                new BiomeVegetationProfile(VegetationType.Grassland, 62),
                new BiomeSpeciesAffinityProfile(72, 46, 60, 20)),
            new ClimateState(
                ClimateZone.Temperate,
                WeatherState.Rain,
                new TemperatureState(18, 18, 18, 0),
                new HumidityState(72, 6, 3),
                new WindState(90, 4, 6),
                new PrecipitationState(PrecipitationType.Rain, 9, 3, 80),
                new PressureState(1005)),
            new WaterState(
                new SurfaceWaterState(340, 4, 90, 640),
                new GroundWaterState(42, 58, 6, 0),
                null,
                null,
                null),
            new ChunkResources(
                new ResourceState[]
                {
                    new(ResourceId.FromSequence(new EntitySequence((sequence * 10) + 1)), ResourceType.Vegetation, ResourceCategory.Renewable, 400, 500, 4, 70, 800),
                    new(ResourceId.FromSequence(new EntitySequence((sequence * 10) + 2)), ResourceType.FreshWater, ResourceCategory.Renewable, 300, 400, 3, 80, 750),
                    new(ResourceId.FromSequence(new EntitySequence((sequence * 10) + 3)), ResourceType.Minerals, ResourceCategory.NonRenewable, 250, 250, 0, 65, 500),
                }),
            System.Array.Empty<OrganismId>());
    }
}

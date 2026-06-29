using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.World.Queries;
using Xunit;

namespace GaiaEngine.Simulation.Tests.World.Queries;

public sealed class DeterministicSpatialQueryServiceTests
{
    [Fact]
    public void GetAdjacentChunks_ShouldReturnNeighboursInDeterministicOrder()
    {
        DeterministicSpatialQueryService service = new();
        GaiaEngine.Domain.World.World world = CreateWorld();

        IReadOnlyList<Chunk> chunks = service.GetAdjacentChunks(world, new ChunkCoordinates(1, 1));

        Assert.Equal(8, chunks.Count);
        Assert.Equal(new ChunkCoordinates(0, 0), chunks[0].Metadata.Coordinates);
        Assert.Equal(new ChunkCoordinates(2, 2), chunks[^1].Metadata.Coordinates);
    }

    [Fact]
    public void GetChunksInArea_ShouldReturnAreaInDeterministicOrder()
    {
        DeterministicSpatialQueryService service = new();
        GaiaEngine.Domain.World.World world = CreateWorld();

        IReadOnlyList<Chunk> chunks = service.GetChunksInArea(world, new ChunkCoordinates(2, 1), new ChunkCoordinates(1, 0));

        Assert.Equal(4, chunks.Count);
        Assert.Equal(new ChunkCoordinates(1, 0), chunks[0].Metadata.Coordinates);
        Assert.Equal(new ChunkCoordinates(2, 1), chunks[^1].Metadata.Coordinates);
    }

    [Fact]
    public void GetOrganismsInChunk_ShouldResolveReferencedOrganisms()
    {
        DeterministicSpatialQueryService service = new();
        GaiaEngine.Domain.World.World world = CreateWorld();
        OrganismCollection organisms = CreateOrganisms();
        Chunk chunk = world.GetChunk(new ChunkCoordinates(0, 0));

        IReadOnlyList<Organism> resolved = service.GetOrganismsInChunk(chunk, organisms);

        Assert.Single(resolved);
        Assert.Equal(OrganismId.FromSequence(new EntitySequence(100)), resolved[0].Id);
    }

    [Fact]
    public void GetOrganismsInChunk_ShouldRejectMissingReferencedOrganisms()
    {
        DeterministicSpatialQueryService service = new();
        GaiaEngine.Domain.World.World world = CreateWorld();
        Chunk chunk = world.GetChunk(new ChunkCoordinates(0, 0));

        Assert.Throws<InvalidOperationException>(() => service.GetOrganismsInChunk(chunk, OrganismCollection.Empty));
    }

    [Fact]
    public void FindNearestChunkWithResource_ShouldReturnNearestChunkUsingStableTieBreaking()
    {
        DeterministicSpatialQueryService service = new();
        GaiaEngine.Domain.World.World world = CreateWorld();

        Chunk? nearest = service.FindNearestChunkWithResource(world, new ChunkCoordinates(1, 1), ResourceType.Minerals);

        Assert.NotNull(nearest);
        Assert.Equal(new ChunkCoordinates(1, 0), nearest!.Metadata.Coordinates);
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
            new WorldDimensions(48, 48, 16, 9, 200),
            new WorldTimeState(0, 0, "Spring", 0),
            new[]
            {
                CreateChunk(worldId, 2, 0, 0, new[] { OrganismId.FromSequence(new EntitySequence(100)) }),
                CreateChunk(worldId, 3, 1, 0, Array.Empty<OrganismId>()),
                CreateChunk(worldId, 4, 2, 0, Array.Empty<OrganismId>()),
                CreateChunk(worldId, 5, 0, 1, Array.Empty<OrganismId>()),
                CreateChunk(worldId, 6, 1, 1, new[] { OrganismId.FromSequence(new EntitySequence(101)) }),
                CreateChunk(worldId, 7, 2, 1, Array.Empty<OrganismId>()),
                CreateChunk(worldId, 8, 0, 2, Array.Empty<OrganismId>()),
                CreateChunk(worldId, 9, 1, 2, Array.Empty<OrganismId>()),
                CreateChunk(worldId, 10, 2, 2, Array.Empty<OrganismId>()),
            });
    }

    private static Chunk CreateChunk(WorldId worldId, ulong sequence, int x, int y, IReadOnlyList<OrganismId> organismIds)
    {
        return new Chunk(
            new ChunkMetadata(
                ChunkId.FromSequence(new EntitySequence(sequence)),
                worldId,
                new ChunkCoordinates(x, y),
                new WorldSeed(100 + x + (y * 10)),
                16),
            ChunkState.Active,
            new TerrainState(
                new ElevationState(50 + x + y, 0, 0),
                new SlopeState(4, 90, 110),
                new SoilState(SoilType.Loam, 70, 60, 70, 65),
                SurfaceType.Grass,
                GeologyType.Granite,
                Array.Empty<TerrainModifierState>()),
            new BiomeState(
                BiomeId.FromSequence(new EntitySequence((sequence * 10) + 4)),
                "Grassland",
                BiomeCategory.Plains,
                "Open plains biome.",
                new BiomeClimateProfile(18, 2, 55, 4, 8),
                new BiomeTerrainProfile(40, 80, SoilType.Loam, SurfaceType.Grass, 60),
                new BiomeResourceProfile(750, 800, 500, 800),
                new BiomeVegetationProfile(VegetationType.Grassland, 62),
                new BiomeSpeciesAffinityProfile(72, 46, 60, 20)),
            new ClimateState(
                ClimateZone.Temperate,
                WeatherState.Clear,
                new TemperatureState(18, 18, 18, 0),
                new HumidityState(55, 3, 2),
                new WindState(90, 4, 6),
                new PrecipitationState(PrecipitationType.None, 0, 0, 0),
                new PressureState(1012)),
            new WaterState(
                new SurfaceWaterState(220, 3, 90, 400),
                new GroundWaterState(42, 58, 6, 0),
                null,
                null,
                null),
            CreateResources(sequence),
            organismIds);
    }

    private static ChunkResources CreateResources(ulong sequence)
    {
        int mineralAmount = sequence == 3 || sequence == 5 ? 250 : 0;
        return new ChunkResources(
            new ResourceState[]
            {
                new(ResourceId.FromSequence(new EntitySequence((sequence * 10) + 1)), ResourceType.Vegetation, ResourceCategory.Renewable, 400, 500, 4, 70, 800),
                new(ResourceId.FromSequence(new EntitySequence((sequence * 10) + 2)), ResourceType.FreshWater, ResourceCategory.Renewable, 300, 400, 3, 80, 750),
                new(ResourceId.FromSequence(new EntitySequence((sequence * 10) + 3)), ResourceType.Minerals, ResourceCategory.NonRenewable, mineralAmount, 250, 0, 65, 500),
            });
    }

    private static OrganismCollection CreateOrganisms()
    {
        return new OrganismCollection(
            new[]
            {
                CreateOrganism(100, 2),
                CreateOrganism(101, 6),
            });
    }

    private static Organism CreateOrganism(ulong organismSequence, ulong chunkSequence)
    {
        return new Organism(
            OrganismId.FromSequence(new EntitySequence(organismSequence)),
            SpeciesId.FromSequence(new EntitySequence(1)),
            GenomeId.FromSequence(new EntitySequence(200 + organismSequence)),
            ChunkId.FromSequence(new EntitySequence(chunkSequence)),
            new PhysiologyComponent(3, 2, 500, 60, 55, 18),
            new NeedsComponent(100, 100, 100, 0),
            new LifecycleComponent(0, 0, 100, LifecycleStage.Juvenile, true),
            new HealthComponent(100, 100));
    }
}

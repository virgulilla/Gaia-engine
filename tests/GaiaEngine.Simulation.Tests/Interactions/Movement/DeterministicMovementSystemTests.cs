using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.Interactions.Movement;
using GaiaEngine.Simulation.World.Queries;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Interactions.Movement;

public sealed class DeterministicMovementSystemTests
{
    [Fact]
    public void Execute_ShouldMoveOrganismToAdjacentChunk()
    {
        GaiaEngine.Domain.World.World world = CreateWorld();
        OrganismCollection organisms = CreateOrganisms();
        DeterministicMovementSystem system = new(new DeterministicSpatialQueryService());
        MovementRequestCollection requests = new(
            new[]
            {
                new MovementRequest(
                    OrganismId.FromSequence(new EntitySequence(100)),
                    ChunkId.FromSequence(new EntitySequence(3)),
                    startTick: 0,
                    expectedDuration: 1,
                    priority: 0),
            });

        MovementSystemResult result = system.Execute(world, organisms, requests);

        Assert.Empty(result.RemainingRequests.GetAll());
        Assert.Equal(ChunkId.FromSequence(new EntitySequence(3)), result.Organisms.Get(OrganismId.FromSequence(new EntitySequence(100))).CurrentChunkId);
        Assert.Empty(result.World.GetChunk(new ChunkCoordinates(0, 0)).OrganismIds);
        Assert.Single(result.World.GetChunk(new ChunkCoordinates(1, 0)).OrganismIds);
    }

    [Fact]
    public void Execute_ShouldKeepFutureRequestsPending()
    {
        GaiaEngine.Domain.World.World world = CreateWorld();
        OrganismCollection organisms = CreateOrganisms();
        DeterministicMovementSystem system = new(new DeterministicSpatialQueryService());
        MovementRequestCollection requests = new(
            new[]
            {
                new MovementRequest(
                    OrganismId.FromSequence(new EntitySequence(100)),
                    ChunkId.FromSequence(new EntitySequence(3)),
                    startTick: 10,
                    expectedDuration: 1,
                    priority: 0),
            });

        MovementSystemResult result = system.Execute(world, organisms, requests);

        Assert.Single(result.RemainingRequests.GetAll());
        Assert.Equal(ChunkId.FromSequence(new EntitySequence(2)), result.Organisms.Get(OrganismId.FromSequence(new EntitySequence(100))).CurrentChunkId);
    }

    [Fact]
    public void Execute_ShouldIgnoreNonAdjacentTargets()
    {
        GaiaEngine.Domain.World.World world = CreateWorld();
        OrganismCollection organisms = CreateOrganisms();
        DeterministicMovementSystem system = new(new DeterministicSpatialQueryService());
        MovementRequestCollection requests = new(
            new[]
            {
                new MovementRequest(
                    OrganismId.FromSequence(new EntitySequence(100)),
                    ChunkId.FromSequence(new EntitySequence(7)),
                    startTick: 0,
                    expectedDuration: 1,
                    priority: 0),
            });

        MovementSystemResult result = system.Execute(world, organisms, requests);

        Assert.Equal(ChunkId.FromSequence(new EntitySequence(2)), result.Organisms.Get(OrganismId.FromSequence(new EntitySequence(100))).CurrentChunkId);
        Assert.Single(result.World.GetChunk(new ChunkCoordinates(0, 0)).OrganismIds);
        Assert.Empty(result.World.GetChunk(new ChunkCoordinates(0, 1)).OrganismIds);
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
            new WorldDimensions(48, 32, 16, 6, 200),
            new WorldTimeState(0, 0, "Spring", 0),
            new[]
            {
                CreateChunk(worldId, 2, 0, 0, new[] { OrganismId.FromSequence(new EntitySequence(100)) }, ocean: false),
                CreateChunk(worldId, 3, 1, 0, Array.Empty<OrganismId>(), ocean: false),
                CreateChunk(worldId, 4, 2, 0, Array.Empty<OrganismId>(), ocean: false),
                CreateChunk(worldId, 5, 0, 1, Array.Empty<OrganismId>(), ocean: false),
                CreateChunk(worldId, 6, 1, 1, Array.Empty<OrganismId>(), ocean: false),
                CreateChunk(worldId, 7, 2, 1, Array.Empty<OrganismId>(), ocean: false),
            });
    }

    private static Chunk CreateChunk(WorldId worldId, ulong sequence, int x, int y, IReadOnlyList<OrganismId> organismIds, bool ocean)
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
                ocean ? new OceanState(40, 350, 18) : null),
            new ChunkResources(
                new ResourceState[]
                {
                    new(ResourceId.FromSequence(new EntitySequence((sequence * 10) + 1)), ResourceType.Vegetation, ResourceCategory.Renewable, 400, 500, 4, 70, 800),
                    new(ResourceId.FromSequence(new EntitySequence((sequence * 10) + 2)), ResourceType.FreshWater, ResourceCategory.Renewable, 300, 400, 3, 80, 750),
                    new(ResourceId.FromSequence(new EntitySequence((sequence * 10) + 3)), ResourceType.Minerals, ResourceCategory.NonRenewable, 250, 250, 0, 65, 500),
                }),
            organismIds);
    }

    private static OrganismCollection CreateOrganisms()
    {
        return new OrganismCollection(
            new[]
            {
                new Organism(
                    OrganismId.FromSequence(new EntitySequence(100)),
                    SpeciesId.FromSequence(new EntitySequence(1)),
                    GenomeId.FromSequence(new EntitySequence(200)),
                    ChunkId.FromSequence(new EntitySequence(2)),
                    new PhysiologyComponent(3, 2, 500, 60, 55, 18),
                    new NeedsComponent(100, 100, 100, 0),
                    new LifecycleComponent(0, 0, 100, LifecycleStage.Juvenile, true),
                    new HealthComponent(100, 100)),
            });
    }
}

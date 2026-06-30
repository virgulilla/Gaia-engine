using System;
using GaiaEngine.Domain.AI;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.AI.Memory;
using GaiaEngine.Simulation.AI.Perception;
using GaiaEngine.Simulation.World.Queries;
using Xunit;

namespace GaiaEngine.Simulation.Tests.AI.Memory;

public sealed class DeterministicMemorySystemTests
{
    [Fact]
    public void Update_ShouldCreateMemoriesFromObservedOrganismsAndResources()
    {
        DeterministicMemorySystem system = CreateSystem(MemorySettings.Default);
        GaiaEngine.Domain.World.World world = CreateWorld(25);
        OrganismCollection organisms = CreateOrganisms();

        MemoryCollection result = system.Update(world, organisms, MemoryCollection.Empty);

        Assert.Equal(2, result.Count);
        Assert.True(result.TryGet(OrganismId.FromSequence(new EntitySequence(100)), out OrganismMemory? memory));
        Assert.NotNull(memory);
        Assert.Contains(memory.GetAll(), entry => entry.Category == MemoryCategory.Organism && entry.Identifier == OrganismId.FromSequence(new EntitySequence(101)).Value);
        Assert.Contains(memory.GetAll(), entry => entry.Category == MemoryCategory.Resource && entry.Identifier == ResourceId.FromSequence(new EntitySequence(21)).Value);
    }

    [Fact]
    public void Update_ShouldRemoveExpiredAndLowConfidenceMemories()
    {
        MemorySettings settings = new(
            organismCapacity: 16,
            resourceCapacity: 16,
            locationCapacity: 8,
            hazardCapacity: 8,
            eventCapacity: 8,
            organismExpirationTicks: 30,
            resourceExpirationTicks: 30,
            locationExpirationTicks: 30,
            hazardExpirationTicks: 30,
            eventExpirationTicks: 30,
            organismDecayPerTick: 50,
            resourceDecayPerTick: 50,
            locationDecayPerTick: 50,
            hazardDecayPerTick: 50,
            eventDecayPerTick: 50,
            confidenceRefreshBonus: 0,
            minimumConfidence: 200);
        DeterministicMemorySystem system = CreateSystem(settings);
        GaiaEngine.Domain.World.World world = CreateWorld(40);
        OrganismCollection organisms = CreateOrganisms();
        MemoryCollection existing = new(
            new[]
            {
                new OrganismMemory(
                    OrganismId.FromSequence(new EntitySequence(100)),
                    new[]
                    {
                        new MemoryEntry(
                            identifier: 999,
                            MemoryCategory.Resource,
                            new ChunkCoordinates(1, 1),
                            confidence: 210,
                            creationTick: 0,
                            lastUpdateTick: 39,
                            expirationTick: 40,
                            estimatedAvailability: 300),
                    }),
            });

        MemoryCollection result = system.Update(world, organisms, existing);

        Assert.True(result.TryGet(OrganismId.FromSequence(new EntitySequence(100)), out OrganismMemory? memory));
        Assert.NotNull(memory);
        Assert.DoesNotContain(memory.GetAll(), entry => entry.Identifier == 999);
    }

    [Fact]
    public void Update_ShouldRespectCategoryCapacityUsingDeterministicPriority()
    {
        MemorySettings settings = new(
            organismCapacity: 16,
            resourceCapacity: 1,
            locationCapacity: 8,
            hazardCapacity: 8,
            eventCapacity: 8,
            organismExpirationTicks: 100,
            resourceExpirationTicks: 100,
            locationExpirationTicks: 100,
            hazardExpirationTicks: 100,
            eventExpirationTicks: 100,
            organismDecayPerTick: 0,
            resourceDecayPerTick: 0,
            locationDecayPerTick: 0,
            hazardDecayPerTick: 0,
            eventDecayPerTick: 0,
            confidenceRefreshBonus: 0,
            minimumConfidence: 0);
        DeterministicMemorySystem system = CreateSystem(settings);
        GaiaEngine.Domain.World.World world = CreateWorld(25);
        OrganismCollection organisms = CreateOrganisms();

        MemoryCollection result = system.Update(world, organisms, MemoryCollection.Empty);

        Assert.True(result.TryGet(OrganismId.FromSequence(new EntitySequence(100)), out OrganismMemory? memory));
        Assert.NotNull(memory);
        Assert.Single(memory.GetAll(), entry => entry.Category == MemoryCategory.Resource);
    }

    private static DeterministicMemorySystem CreateSystem(MemorySettings settings)
    {
        DeterministicPerceptionSystem perceptionSystem = new(
            new PerceptionSettings(visionRange: 1, hearingRange: 1, smellRange: 1, touchRange: 0, minimumConfidence: 0),
            new DeterministicSpatialQueryService());
        return new DeterministicMemorySystem(settings, perceptionSystem);
    }

    private static GaiaEngine.Domain.World.World CreateWorld(long currentTick)
    {
        WorldId worldId = WorldId.FromSequence(new EntitySequence(1));
        return new GaiaEngine.Domain.World.World(
            new WorldMetadata(
                worldId,
                "Gaia",
                new WorldSeed(42),
                "2026-06-30",
                new EngineVersion(1, 0, 0),
                new ConfigurationVersion("2026.06.30")),
            new WorldDimensions(32, 32, 16, 2, 200),
            new WorldTimeState(currentTick, 0, "Spring", 0),
            new[]
            {
                CreateChunk(worldId, 2, 0, 0, new[] { OrganismId.FromSequence(new EntitySequence(100)), OrganismId.FromSequence(new EntitySequence(101)) }),
                CreateChunk(worldId, 3, 1, 0, Array.Empty<OrganismId>()),
            });
    }

    private static Chunk CreateChunk(WorldId worldId, ulong chunkSequence, int x, int y, OrganismId[] organismIds)
    {
        return new Chunk(
            new ChunkMetadata(
                ChunkId.FromSequence(new EntitySequence(chunkSequence)),
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
                BiomeId.FromSequence(new EntitySequence((chunkSequence * 10) + 4)),
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
            new ChunkResources(
                new ResourceState[]
                {
                    new(ResourceId.FromSequence(new EntitySequence(21)), ResourceType.Vegetation, ResourceCategory.Renewable, 400, 500, 4, 70, 800),
                    new(ResourceId.FromSequence(new EntitySequence(22)), ResourceType.FreshWater, ResourceCategory.Renewable, 300, 400, 3, 80, 750),
                }),
            organismIds);
    }

    private static OrganismCollection CreateOrganisms()
    {
        return new OrganismCollection(
            new[]
            {
                CreateOrganism(100, 2),
                CreateOrganism(101, 2),
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
            new LifecycleComponent(0, 0, 100, LifecycleStage.Adult, true),
            new HealthComponent(100, 100));
    }
}

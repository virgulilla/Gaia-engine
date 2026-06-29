using System;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.Interactions.Hydration;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Interactions.Hydration;

public sealed class DeterministicHydrationSystemTests
{
    [Fact]
    public void Execute_ShouldConsumeFreshWaterAndReduceHydrationNeed()
    {
        GaiaEngine.Domain.World.World world = CreateWorld(freshWaterAmount: 300);
        OrganismCollection organisms = CreateOrganisms();
        DeterministicHydrationSystem system = new();
        HydrationRequestCollection requests = new(
            new[]
            {
                new HydrationRequest(
                    OrganismId.FromSequence(new EntitySequence(100)),
                    ChunkId.FromSequence(new EntitySequence(2)),
                    startTick: 0,
                    expectedDuration: 1,
                    priority: 0),
            });

        HydrationSystemResult result = system.Execute(world, organisms, requests);

        Assert.Empty(result.RemainingRequests.GetAll());
        Assert.Equal(380, result.Organisms.Get(OrganismId.FromSequence(new EntitySequence(100))).Needs.Hydration);
        Assert.True(result.World.GetChunk(new ChunkCoordinates(0, 0)).Resources.TryGet(ResourceType.FreshWater, out ResourceState? freshWater));
        Assert.NotNull(freshWater);
        Assert.Equal(290, freshWater!.CurrentAmount);
    }

    [Fact]
    public void Execute_ShouldKeepFutureRequestsPending()
    {
        GaiaEngine.Domain.World.World world = CreateWorld(freshWaterAmount: 300);
        OrganismCollection organisms = CreateOrganisms();
        DeterministicHydrationSystem system = new();
        HydrationRequestCollection requests = new(
            new[]
            {
                new HydrationRequest(
                    OrganismId.FromSequence(new EntitySequence(100)),
                    ChunkId.FromSequence(new EntitySequence(2)),
                    startTick: 10,
                    expectedDuration: 1,
                    priority: 0),
            });

        HydrationSystemResult result = system.Execute(world, organisms, requests);

        Assert.Single(result.RemainingRequests.GetAll());
        Assert.Equal(600, result.Organisms.Get(OrganismId.FromSequence(new EntitySequence(100))).Needs.Hydration);
    }

    [Fact]
    public void Execute_ShouldIgnoreRequestsWithoutEnoughFreshWater()
    {
        GaiaEngine.Domain.World.World world = CreateWorld(freshWaterAmount: 5);
        OrganismCollection organisms = CreateOrganisms();
        DeterministicHydrationSystem system = new();
        HydrationRequestCollection requests = new(
            new[]
            {
                new HydrationRequest(
                    OrganismId.FromSequence(new EntitySequence(100)),
                    ChunkId.FromSequence(new EntitySequence(2)),
                    startTick: 0,
                    expectedDuration: 1,
                    priority: 0),
            });

        HydrationSystemResult result = system.Execute(world, organisms, requests);

        Assert.Equal(600, result.Organisms.Get(OrganismId.FromSequence(new EntitySequence(100))).Needs.Hydration);
        Assert.True(result.World.GetChunk(new ChunkCoordinates(0, 0)).Resources.TryGet(ResourceType.FreshWater, out ResourceState? freshWater));
        Assert.NotNull(freshWater);
        Assert.Equal(5, freshWater!.CurrentAmount);
    }

    private static GaiaEngine.Domain.World.World CreateWorld(int freshWaterAmount)
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
            new WorldDimensions(16, 16, 16, 1, 200),
            new WorldTimeState(0, 0, "Spring", 0),
            new[]
            {
                new Chunk(
                    new ChunkMetadata(
                        ChunkId.FromSequence(new EntitySequence(2)),
                        worldId,
                        new ChunkCoordinates(0, 0),
                        new WorldSeed(100),
                        16),
                    ChunkState.Active,
                    new TerrainState(
                        new ElevationState(50, 0, 0),
                        new SlopeState(4, 90, 110),
                        new SoilState(SoilType.Loam, 70, 60, 70, 65),
                        SurfaceType.Grass,
                        GeologyType.Granite,
                        Array.Empty<TerrainModifierState>()),
                    new BiomeState(
                        BiomeId.FromSequence(new EntitySequence(5)),
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
                            new(ResourceId.FromSequence(new EntitySequence(22)), ResourceType.FreshWater, ResourceCategory.Renewable, freshWaterAmount, 400, 3, 80, 750),
                            new(ResourceId.FromSequence(new EntitySequence(23)), ResourceType.Minerals, ResourceCategory.NonRenewable, 250, 250, 0, 65, 500),
                        }),
                    new[] { OrganismId.FromSequence(new EntitySequence(100)) }),
            });
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
                    new NeedsComponent(100, 600, 100, 0),
                    new LifecycleComponent(0, 0, 100, LifecycleStage.Juvenile, true),
                    new HealthComponent(100, 100)),
            });
    }
}

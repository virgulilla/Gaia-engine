using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.Organisms;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Organisms;

public sealed class DeterministicOrganismUpdateSystemTests
{
    [Fact]
    public void Update_ShouldAdvanceAgeAndNeedsForAliveOrganisms()
    {
        GaiaEngine.Domain.World.World world = CreateWorld();
        Organism organism = CreateOrganism(ageTicks: 10, health: 100, hunger: 100, hydration: 100, rest: 100);
        DeterministicOrganismUpdateSystem system = new();

        OrganismCollection updated = system.Update(world, new OrganismCollection(new[] { organism }));
        Organism result = updated.GetAll()[0];

        Assert.Equal(11, result.Lifecycle.AgeTicks);
        Assert.True(result.Needs.Hunger > organism.Needs.Hunger);
        Assert.True(result.Needs.Hydration > organism.Needs.Hydration);
        Assert.True(result.Needs.Rest > organism.Needs.Rest);
        Assert.True(result.Health.CurrentValue <= organism.Health.CurrentValue);
    }

    [Fact]
    public void Update_ShouldMarkOrganismDeadWhenLifespanIsReached()
    {
        GaiaEngine.Domain.World.World world = CreateWorld();
        Organism organism = CreateOrganism(ageTicks: 499, health: 100, hunger: 100, hydration: 100, rest: 100);
        DeterministicOrganismUpdateSystem system = new();

        OrganismCollection updated = system.Update(world, new OrganismCollection(new[] { organism }));
        Organism result = updated.GetAll()[0];

        Assert.False(result.Lifecycle.IsAlive);
        Assert.Equal(LifecycleStage.Dead, result.Lifecycle.Stage);
        Assert.Equal(0, result.Needs.ReproductionUrge);
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
                        System.Array.Empty<TerrainModifierState>()),
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
                            new(ResourceId.FromSequence(new EntitySequence(22)), ResourceType.FreshWater, ResourceCategory.Renewable, 300, 400, 3, 80, 750),
                            new(ResourceId.FromSequence(new EntitySequence(23)), ResourceType.Minerals, ResourceCategory.NonRenewable, 250, 250, 0, 65, 500),
                        }),
                    new[] { OrganismId.FromSequence(new EntitySequence(100)) }),
            });
    }

    private static Organism CreateOrganism(long ageTicks, int health, int hunger, int hydration, int rest)
    {
        return new Organism(
            OrganismId.FromSequence(new EntitySequence(100)),
            SpeciesId.FromSequence(new EntitySequence(1)),
            GenomeId.FromSequence(new EntitySequence(200)),
            ChunkId.FromSequence(new EntitySequence(2)),
            new PhysiologyComponent(3, 2, 500, 60, 55, 18),
            new NeedsComponent(hunger, hydration, rest, 0),
            new LifecycleComponent(0, ageTicks, 100, LifecycleStage.Juvenile, true),
            new HealthComponent(health, 100));
    }
}

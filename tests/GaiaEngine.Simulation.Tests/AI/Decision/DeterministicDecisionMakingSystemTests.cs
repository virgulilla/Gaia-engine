using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.AI.Decision;
using GaiaEngine.Simulation.AI.Utility;
using Xunit;

namespace GaiaEngine.Simulation.Tests.AI.Decision;

public sealed class DeterministicDecisionMakingSystemTests
{
    [Fact]
    public void Select_ShouldChooseHighestValidUtilityCandidate()
    {
        DeterministicDecisionMakingSystem system = new(new DeterministicEntityIdGenerator());
        GaiaEngine.Domain.World.World world = CreateWorld();
        Organism organism = CreateOrganism();
        UtilityEvaluationResult utilityResult = new(
            organism.Id,
            50,
            new[]
            {
                CreateCandidate(20, SimulationActionType.Drink, "144115188075855874", score: 700, cost: 10, duration: 1, isValid: true),
                CreateCandidate(10, SimulationActionType.Eat, "144115188075855874", score: 900, cost: 20, duration: 1, isValid: true),
            });

        SelectedDecision result = system.Select(world, new OrganismCollection(new[] { organism }), utilityResult, organism.Id);

        Assert.Equal(SimulationActionType.Eat, result.ActionType);
        Assert.False(result.IsIdleFallback);
        Assert.Equal(900, result.UtilityScore);
    }

    [Fact]
    public void Select_ShouldBreakTiesByLowestCostThenDurationThenActionId()
    {
        DeterministicDecisionMakingSystem system = new(new DeterministicEntityIdGenerator());
        GaiaEngine.Domain.World.World world = CreateWorld();
        Organism organism = CreateOrganism();
        UtilityEvaluationResult utilityResult = new(
            organism.Id,
            50,
            new[]
            {
                CreateCandidate(30, SimulationActionType.Drink, "144115188075855874", score: 800, cost: 20, duration: 1, isValid: true),
                CreateCandidate(20, SimulationActionType.Eat, "144115188075855874", score: 800, cost: 10, duration: 2, isValid: true),
                CreateCandidate(10, SimulationActionType.Move, "144115188075855875", score: 800, cost: 10, duration: 1, isValid: true),
            });

        SelectedDecision result = system.Select(world, new OrganismCollection(new[] { organism }), utilityResult, organism.Id);

        Assert.Equal(SimulationActionType.Move, result.ActionType);
        Assert.Equal(ActionId.FromSequence(new EntitySequence(10)), result.ActionId);
    }

    [Fact]
    public void Select_ShouldReturnIdleWhenNoValidCandidateExists()
    {
        DeterministicDecisionMakingSystem system = new(new DeterministicEntityIdGenerator());
        GaiaEngine.Domain.World.World world = CreateWorld();
        Organism organism = CreateOrganism();
        UtilityEvaluationResult utilityResult = new(
            organism.Id,
            50,
            new[]
            {
                CreateCandidate(20, SimulationActionType.Drink, "144115188075855874", score: 0, cost: 0, duration: 0, isValid: false),
                CreateCandidate(10, SimulationActionType.Eat, "144115188075855874", score: 0, cost: 0, duration: 0, isValid: false),
            });

        SelectedDecision result = system.Select(world, new OrganismCollection(new[] { organism }), utilityResult, organism.Id);

        Assert.Equal(SimulationActionType.Idle, result.ActionType);
        Assert.True(result.IsIdleFallback);
        Assert.Equal(ChunkId.FromSequence(new EntitySequence(2)).ToString(), result.Target.TargetId);
    }

    private static UtilityActionEvaluation CreateCandidate(ulong actionSequence, SimulationActionType actionType, string chunkTargetId, int score, int cost, int duration, bool isValid)
    {
        return new UtilityActionEvaluation(
            ActionId.FromSequence(new EntitySequence(actionSequence)),
            actionType,
            new SimulationActionTarget(ActionTargetKind.Chunk, chunkTargetId),
            score,
            cost,
            duration,
            isValid);
    }

    private static GaiaEngine.Domain.World.World CreateWorld()
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
            new WorldDimensions(16, 16, 16, 1, 200),
            new WorldTimeState(50, 0, "Spring", 0),
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
                    new ChunkResources(System.Array.Empty<ResourceState>()),
                    new[] { OrganismId.FromSequence(new EntitySequence(100)) }),
            });
    }

    private static Organism CreateOrganism()
    {
        return new Organism(
            OrganismId.FromSequence(new EntitySequence(100)),
            SpeciesId.FromSequence(new EntitySequence(1)),
            GenomeId.FromSequence(new EntitySequence(200)),
            ChunkId.FromSequence(new EntitySequence(2)),
            new PhysiologyComponent(3, 2, 500, 60, 55, 18),
            new NeedsComponent(100, 100, 100, 0),
            new LifecycleComponent(0, 0, 100, LifecycleStage.Adult, true),
            new HealthComponent(100, 100));
    }
}

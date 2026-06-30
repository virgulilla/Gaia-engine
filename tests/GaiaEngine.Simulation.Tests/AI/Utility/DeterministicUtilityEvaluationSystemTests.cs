using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.AI.Perception;
using GaiaEngine.Simulation.AI.Utility;
using GaiaEngine.Simulation.World.Queries;
using Xunit;

namespace GaiaEngine.Simulation.Tests.AI.Utility;

public sealed class DeterministicUtilityEvaluationSystemTests
{
    [Fact]
    public void Evaluate_ShouldScoreEatAboveDrinkWhenHungerIsMoreUrgent()
    {
        DeterministicPerceptionSystem perceptionSystem = CreatePerceptionSystem();
        DeterministicUtilityEvaluationSystem utilitySystem = CreateUtilitySystem();
        GaiaEngine.Domain.World.World world = CreateWorld(currentVegetationAmount: 400, currentWaterAmount: 300, adjacentVegetationAmount: 0, adjacentWaterAmount: 0);
        Organism organism = CreateOrganism(hunger: 900, hydration: 300);
        OrganismCollection organisms = new(new[] { organism });
        PerceptionResult perception = perceptionSystem.Evaluate(world, organisms, organism.Id);

        UtilityEvaluationResult result = utilitySystem.Evaluate(world, organisms, perception, organism.Id);

        Assert.Equal(SimulationActionType.Eat, result.Candidates[0].ActionType);
        Assert.True(result.Candidates[0].IsValid);
        Assert.True(result.Candidates[0].UtilityScore > result.Candidates[1].UtilityScore);
    }

    [Fact]
    public void Evaluate_ShouldInvalidateEatWhenFoodIsNotPerceivedInCurrentChunk()
    {
        DeterministicPerceptionSystem perceptionSystem = CreatePerceptionSystem();
        DeterministicUtilityEvaluationSystem utilitySystem = CreateUtilitySystem();
        GaiaEngine.Domain.World.World world = CreateWorld(currentVegetationAmount: 0, currentWaterAmount: 300, adjacentVegetationAmount: 250, adjacentWaterAmount: 0);
        Organism organism = CreateOrganism(hunger: 900, hydration: 300);
        OrganismCollection organisms = new(new[] { organism });
        PerceptionResult perception = perceptionSystem.Evaluate(world, organisms, organism.Id);

        UtilityEvaluationResult result = utilitySystem.Evaluate(world, organisms, perception, organism.Id);
        UtilityActionEvaluation eat = FindCandidate(result, SimulationActionType.Eat);

        Assert.False(eat.IsValid);
        Assert.Equal(0, eat.UtilityScore);
    }

    [Fact]
    public void Evaluate_ShouldRecommendMoveToAdjacentFoodChunkWhenCurrentChunkLacksFood()
    {
        DeterministicPerceptionSystem perceptionSystem = CreatePerceptionSystem();
        DeterministicUtilityEvaluationSystem utilitySystem = CreateUtilitySystem();
        GaiaEngine.Domain.World.World world = CreateWorld(currentVegetationAmount: 0, currentWaterAmount: 0, adjacentVegetationAmount: 300, adjacentWaterAmount: 0);
        Organism organism = CreateOrganism(hunger: 850, hydration: 100);
        OrganismCollection organisms = new(new[] { organism });
        PerceptionResult perception = perceptionSystem.Evaluate(world, organisms, organism.Id);

        UtilityEvaluationResult result = utilitySystem.Evaluate(world, organisms, perception, organism.Id);
        UtilityActionEvaluation move = FindCandidate(result, SimulationActionType.Move);

        Assert.True(move.IsValid);
        Assert.Equal(ChunkId.FromSequence(new EntitySequence(3)).ToString(), move.Target.TargetId);
        Assert.True(move.UtilityScore > 0);
    }

    private static DeterministicPerceptionSystem CreatePerceptionSystem()
    {
        return new DeterministicPerceptionSystem(new PerceptionSettings(visionRange: 1, hearingRange: 1, smellRange: 1, touchRange: 0, minimumConfidence: 250), new DeterministicSpatialQueryService());
    }

    private static DeterministicUtilityEvaluationSystem CreateUtilitySystem()
    {
        return new DeterministicUtilityEvaluationSystem(UtilityEvaluationSettings.Default, new DeterministicUtilityCurveEvaluator(), new DeterministicEntityIdGenerator());
    }

    private static UtilityActionEvaluation FindCandidate(UtilityEvaluationResult result, SimulationActionType actionType)
    {
        foreach (UtilityActionEvaluation candidate in result.Candidates)
        {
            if (candidate.ActionType == actionType)
            {
                return candidate;
            }
        }

        throw new InvalidOperationException("The requested utility candidate was not produced.");
    }

    private static GaiaEngine.Domain.World.World CreateWorld(int currentVegetationAmount, int currentWaterAmount, int adjacentVegetationAmount, int adjacentWaterAmount)
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
            new WorldTimeState(40, 0, "Spring", 0),
            new[]
            {
                CreateChunk(worldId, 2, 0, 0, currentVegetationAmount, currentWaterAmount, new[] { OrganismId.FromSequence(new EntitySequence(100)) }),
                CreateChunk(worldId, 3, 1, 0, adjacentVegetationAmount, adjacentWaterAmount, Array.Empty<OrganismId>()),
            });
    }

    private static Chunk CreateChunk(WorldId worldId, ulong chunkSequence, int x, int y, int vegetationAmount, int waterAmount, IReadOnlyList<OrganismId> organismIds)
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
                new BiomeVegetationProfile(VegetationType.Grassland, 30),
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
                    new(ResourceId.FromSequence(new EntitySequence((chunkSequence * 10) + 1)), ResourceType.Vegetation, ResourceCategory.Renewable, vegetationAmount, 500, 4, 70, vegetationAmount > 0 ? 800 : 0),
                    new(ResourceId.FromSequence(new EntitySequence((chunkSequence * 10) + 2)), ResourceType.FreshWater, ResourceCategory.Renewable, waterAmount, 400, 3, 80, waterAmount > 0 ? 750 : 0),
                }),
            organismIds);
    }

    private static Organism CreateOrganism(int hunger, int hydration)
    {
        return new Organism(
            OrganismId.FromSequence(new EntitySequence(100)),
            SpeciesId.FromSequence(new EntitySequence(1)),
            GenomeId.FromSequence(new EntitySequence(300)),
            ChunkId.FromSequence(new EntitySequence(2)),
            new PhysiologyComponent(3, 2, 500, 60, 55, 18),
            new NeedsComponent(hunger, hydration, 100, 0),
            new LifecycleComponent(0, 0, 100, LifecycleStage.Adult, true),
            new HealthComponent(100, 100));
    }
}

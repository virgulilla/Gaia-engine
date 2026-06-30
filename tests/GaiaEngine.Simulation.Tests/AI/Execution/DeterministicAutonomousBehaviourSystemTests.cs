using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.AI.Decision;
using GaiaEngine.Simulation.AI.Execution;
using GaiaEngine.Simulation.AI.Perception;
using GaiaEngine.Simulation.AI.Utility;
using GaiaEngine.Simulation.World.Queries;
using Xunit;

namespace GaiaEngine.Simulation.Tests.AI.Execution;

public sealed class DeterministicAutonomousBehaviourSystemTests
{
    [Fact]
    public void Update_ShouldCreateDeterministicActionRequestsForAliveOrganisms()
    {
        DeterministicAutonomousBehaviourSystem system = CreateSystem();
        GaiaEngine.Domain.World.World world = CreateWorld();
        OrganismCollection organisms = new(
            new[]
            {
                CreateOrganism(100, hunger: 900, hydration: 200, isAlive: true),
                CreateOrganism(101, hunger: 100, hydration: 900, isAlive: true),
                CreateOrganism(102, hunger: 900, hydration: 900, isAlive: false),
            });

        SimulationActionRequestCollection result = system.Update(world, organisms, SimulationActionRequestCollection.Empty);

        Assert.Equal(2, result.Count);
        Assert.Contains(result.GetAll(), request => request.OrganismId == OrganismId.FromSequence(new EntitySequence(100)) && request.ActionType == SimulationActionType.Eat);
        Assert.Contains(result.GetAll(), request => request.OrganismId == OrganismId.FromSequence(new EntitySequence(101)) && request.ActionType == SimulationActionType.Drink);
        Assert.DoesNotContain(result.GetAll(), request => request.OrganismId == OrganismId.FromSequence(new EntitySequence(102)));
    }

    private static DeterministicAutonomousBehaviourSystem CreateSystem()
    {
        DeterministicSpatialQueryService spatialQueryService = new();
        return new DeterministicAutonomousBehaviourSystem(
            new DeterministicPerceptionSystem(new PerceptionSettings(visionRange: 0, hearingRange: 0, smellRange: 0, touchRange: 0, minimumConfidence: 250), spatialQueryService),
            new DeterministicUtilityEvaluationSystem(UtilityEvaluationSettings.Default, new DeterministicUtilityCurveEvaluator(), new DeterministicEntityIdGenerator()),
            new DeterministicDecisionMakingSystem(new DeterministicEntityIdGenerator()),
            new DeterministicBehaviourExecutionSystem());
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
            new WorldDimensions(32, 32, 16, 1, 200),
            new WorldTimeState(50, 0, "Spring", 0),
            new[]
            {
                CreateChunk(worldId, new[] { OrganismId.FromSequence(new EntitySequence(100)), OrganismId.FromSequence(new EntitySequence(101)), OrganismId.FromSequence(new EntitySequence(102)) }),
            });
    }

    private static Chunk CreateChunk(WorldId worldId, IReadOnlyList<OrganismId> organismIds)
    {
        return new Chunk(
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
                    new(ResourceId.FromSequence(new EntitySequence(22)), ResourceType.FreshWater, ResourceCategory.Renewable, 300, 400, 3, 80, 750),
                }),
            organismIds);
    }

    private static Organism CreateOrganism(ulong organismSequence, int hunger, int hydration, bool isAlive)
    {
        return new Organism(
            OrganismId.FromSequence(new EntitySequence(organismSequence)),
            SpeciesId.FromSequence(new EntitySequence(1)),
            GenomeId.FromSequence(new EntitySequence(200 + organismSequence)),
            ChunkId.FromSequence(new EntitySequence(2)),
            new PhysiologyComponent(3, 2, 500, 60, 55, 18),
            new NeedsComponent(hunger, hydration, 100, 0),
            new LifecycleComponent(0, 0, 100, LifecycleStage.Adult, isAlive),
            new HealthComponent(100, 100));
    }
}

using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.AI.Perception;
using GaiaEngine.Simulation.World.Queries;
using Xunit;

namespace GaiaEngine.Simulation.Tests.AI.Perception;

public sealed class DeterministicPerceptionSystemTests
{
    [Fact]
    public void Evaluate_ShouldReturnDeterministicObservationsAcrossSupportedSensors()
    {
        DeterministicPerceptionSystem system = new(CreateSettings(), new DeterministicSpatialQueryService());
        GaiaEngine.Domain.World.World world = CreateWorld(WeatherState.Clear, vegetationDensity: 20);
        OrganismCollection organisms = CreateOrganisms();

        PerceptionResult result = system.Evaluate(world, organisms, OrganismId.FromSequence(new EntitySequence(100)));

        Assert.NotEmpty(result.Observations);
        Assert.Equal(world.TimeState.CurrentTick, result.DetectionTick);
        Assert.Contains(result.Observations, observation => observation.ObjectKind == PerceivedObjectKind.Organism && observation.SensorType == SensorType.Touch);
        Assert.Contains(result.Observations, observation => observation.ObjectKind == PerceivedObjectKind.Resource && observation.SensorType == SensorType.Vision);
        Assert.Contains(result.Observations, observation => observation.ObjectKind == PerceivedObjectKind.Water && observation.SensorType == SensorType.Hearing);
        AssertSensorOrdering(result.Observations);
    }

    [Fact]
    public void Evaluate_ShouldFilterObjectsOutsideConfiguredSensorRange()
    {
        DeterministicPerceptionSystem system = new(new PerceptionSettings(visionRange: 0, hearingRange: 0, smellRange: 0, touchRange: 0, minimumConfidence: 250), new DeterministicSpatialQueryService());
        GaiaEngine.Domain.World.World world = CreateWorld(WeatherState.Clear, vegetationDensity: 20);
        OrganismCollection organisms = CreateOrganisms();

        PerceptionResult result = system.Evaluate(world, organisms, OrganismId.FromSequence(new EntitySequence(100)));

        Assert.All(result.Observations, observation => Assert.Equal(0, observation.Distance));
        Assert.DoesNotContain(result.Observations, observation => observation.ObjectId == ResourceId.FromSequence(new EntitySequence(31)).Value);
    }

    [Fact]
    public void Evaluate_ShouldReduceVisionConfidenceUnderFogAndDenseVegetation()
    {
        DeterministicPerceptionSystem system = new(new PerceptionSettings(visionRange: 1, hearingRange: 1, smellRange: 1, touchRange: 0, minimumConfidence: 0), new DeterministicSpatialQueryService());
        OrganismId observerId = OrganismId.FromSequence(new EntitySequence(100));
        ResourceId localResourceId = ResourceId.FromSequence(new EntitySequence(21));
        OrganismCollection organisms = CreateOrganisms();

        PerceptionResult clearResult = system.Evaluate(CreateWorld(WeatherState.Clear, vegetationDensity: 10), organisms, observerId);
        PerceptionResult fogResult = system.Evaluate(CreateWorld(WeatherState.Fog, vegetationDensity: 90), organisms, observerId);

        int clearConfidence = FindObservation(clearResult.Observations, localResourceId.Value, PerceivedObjectKind.Resource, SensorType.Vision).Confidence;
        int fogConfidence = FindObservation(fogResult.Observations, localResourceId.Value, PerceivedObjectKind.Resource, SensorType.Vision).Confidence;

        Assert.True(fogConfidence < clearConfidence);
    }

    private static PerceptionSettings CreateSettings()
    {
        return new PerceptionSettings(visionRange: 1, hearingRange: 1, smellRange: 1, touchRange: 0, minimumConfidence: 250);
    }

    private static void AssertSensorOrdering(IReadOnlyList<PerceivedObject> observations)
    {
        for (int index = 1; index < observations.Count; index++)
        {
            Assert.True(observations[index - 1].SensorType <= observations[index].SensorType);
        }
    }

    private static PerceivedObject FindObservation(IReadOnlyList<PerceivedObject> observations, ulong objectId, PerceivedObjectKind objectKind, SensorType sensorType)
    {
        foreach (PerceivedObject observation in observations)
        {
            if (observation.ObjectId == objectId && observation.ObjectKind == objectKind && observation.SensorType == sensorType)
            {
                return observation;
            }
        }

        throw new InvalidOperationException("The expected observation was not found.");
    }

    private static GaiaEngine.Domain.World.World CreateWorld(WeatherState weatherState, int vegetationDensity)
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
            new WorldTimeState(25, 0, "Spring", 0),
            new[]
            {
                CreateChunk(worldId, 2, 0, 0, new[] { OrganismId.FromSequence(new EntitySequence(100)), OrganismId.FromSequence(new EntitySequence(101)) }, 300, vegetationDensity, weatherState),
                CreateChunk(worldId, 3, 1, 0, Array.Empty<OrganismId>(), 260, vegetationDensity, weatherState),
                CreateChunk(worldId, 4, 0, 1, Array.Empty<OrganismId>(), 0, vegetationDensity, weatherState),
                CreateChunk(worldId, 5, 1, 1, Array.Empty<OrganismId>(), 0, vegetationDensity, weatherState),
            });
    }

    private static Chunk CreateChunk(
        WorldId worldId,
        ulong chunkSequence,
        int x,
        int y,
        IReadOnlyList<OrganismId> organismIds,
        int surfaceWaterAmount,
        int vegetationDensity,
        WeatherState weatherState)
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
                new ElevationState(50 + x + y, 0, y == 0 ? 0 : 4),
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
                new BiomeVegetationProfile(VegetationType.Grassland, vegetationDensity),
                new BiomeSpeciesAffinityProfile(72, 46, 60, 20)),
            new ClimateState(
                ClimateZone.Temperate,
                weatherState,
                new TemperatureState(18, 18, 18, 0),
                new HumidityState(55, 3, 2),
                new WindState(90, 4, 6),
                new PrecipitationState(weatherState == WeatherState.Clear ? PrecipitationType.None : PrecipitationType.Rain, weatherState == WeatherState.Clear ? 0 : 10, 0, weatherState == WeatherState.Clear ? 0 : 60),
                new PressureState(1012)),
            new WaterState(
                new SurfaceWaterState(surfaceWaterAmount, 3, 90, 400),
                new GroundWaterState(42, 58, 6, 0),
                null,
                null,
                null),
            CreateResources(chunkSequence),
            organismIds);
    }

    private static ChunkResources CreateResources(ulong chunkSequence)
    {
        return new ChunkResources(
            new ResourceState[]
            {
                new(ResourceId.FromSequence(new EntitySequence((chunkSequence * 10) + 1)), ResourceType.Vegetation, ResourceCategory.Renewable, 400, 500, 4, 70, 800),
                new(ResourceId.FromSequence(new EntitySequence((chunkSequence * 10) + 2)), ResourceType.FreshWater, ResourceCategory.Renewable, 300, 400, 3, 80, 750),
            });
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

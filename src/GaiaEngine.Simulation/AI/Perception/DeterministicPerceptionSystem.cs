using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.World.Queries;

namespace GaiaEngine.Simulation.AI.Perception;

/// <summary>
/// Produces deterministic perceived observations by filtering nearby world data through sensor rules.
/// </summary>
public sealed class DeterministicPerceptionSystem : IPerceptionSystem
{
    private readonly PerceptionSettings settings;
    private readonly ISpatialQueryService spatialQueryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicPerceptionSystem"/> class.
    /// </summary>
    /// <param name="settings">The deterministic perception settings.</param>
    /// <param name="spatialQueryService">The deterministic spatial query service.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="settings"/> or <paramref name="spatialQueryService"/> is <see langword="null"/>.
    /// </exception>
    public DeterministicPerceptionSystem(PerceptionSettings settings, ISpatialQueryService spatialQueryService)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        this.spatialQueryService = spatialQueryService ?? throw new ArgumentNullException(nameof(spatialQueryService));
    }

    /// <summary>
    /// Evaluates the current world state from the perspective of one organism.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="observerId">The observing organism identifier.</param>
    /// <returns>The generated deterministic observations.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="world"/> or <paramref name="organisms"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the observer does not exist or when its current chunk cannot be resolved in the world state.
    /// </exception>
    public PerceptionResult Evaluate(GaiaEngine.Domain.World.World world, OrganismCollection organisms, OrganismId observerId)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(organisms);

        if (!organisms.TryGet(observerId, out Organism? observer))
        {
            throw new InvalidOperationException("The observing organism does not exist in the organism collection.");
        }

        Chunk observerChunk = ResolveChunkById(world, observer!.CurrentChunkId);
        ChunkCoordinates origin = observerChunk.Metadata.Coordinates;
        IReadOnlyList<Chunk> nearbyChunks = spatialQueryService.GetChunksInArea(
            world,
            new ChunkCoordinates(origin.X - settings.MaximumRange, origin.Y - settings.MaximumRange),
            new ChunkCoordinates(origin.X + settings.MaximumRange, origin.Y + settings.MaximumRange));

        List<PerceivedObject> observations = new();
        foreach (Chunk chunk in nearbyChunks)
        {
            int distance = GetChunkDistance(origin, chunk.Metadata.Coordinates);
            AppendOrganismObservations(observations, chunk, organisms, observerId, distance, world.TimeState.CurrentTick);
            AppendResourceObservations(observations, chunk, distance, world.TimeState.CurrentTick);
            AppendWaterObservations(observations, chunk, distance, world.TimeState.CurrentTick);
        }

        observations.Sort(static (left, right) => CompareObservations(left, right));
        return new PerceptionResult(observerId, world.TimeState.CurrentTick, observations.AsReadOnly());
    }

    private void AppendOrganismObservations(
        List<PerceivedObject> observations,
        Chunk chunk,
        OrganismCollection organisms,
        OrganismId observerId,
        int distance,
        long detectionTick)
    {
        foreach (Organism organism in spatialQueryService.GetOrganismsInChunk(chunk, organisms))
        {
            if (organism.Id == observerId)
            {
                continue;
            }

            AppendObservationForSensors(
                observations,
                organism.Id.Value,
                PerceivedObjectKind.Organism,
                chunk,
                distance,
                detectionTick,
                hearingBaseConfidence: 720,
                smellBaseConfidence: 680);
        }
    }

    private void AppendResourceObservations(List<PerceivedObject> observations, Chunk chunk, int distance, long detectionTick)
    {
        foreach (ResourceState resource in spatialQueryService.GetResourcesInChunk(chunk))
        {
            if (resource.CurrentAmount <= 0)
            {
                continue;
            }

            AppendObservationForSensors(
                observations,
                resource.ResourceId.Value,
                PerceivedObjectKind.Resource,
                chunk,
                distance,
                detectionTick,
                hearingBaseConfidence: 420,
                smellBaseConfidence: 760);
        }
    }

    private void AppendWaterObservations(List<PerceivedObject> observations, Chunk chunk, int distance, long detectionTick)
    {
        if (chunk.Water.SurfaceWater.WaterLevel <= 0 && chunk.Water.Lake is null && chunk.Water.River is null && chunk.Water.Ocean is null)
        {
            return;
        }

        AppendObservationForSensors(
            observations,
            chunk.Id.Value,
            PerceivedObjectKind.Water,
            chunk,
            distance,
            detectionTick,
            hearingBaseConfidence: 800,
            smellBaseConfidence: 580);
    }

    private void AppendObservationForSensors(
        List<PerceivedObject> observations,
        ulong objectId,
        PerceivedObjectKind objectKind,
        Chunk chunk,
        int distance,
        long detectionTick,
        int hearingBaseConfidence,
        int smellBaseConfidence)
    {
        TryAppendObservation(observations, objectId, objectKind, SensorType.Vision, ComputeVisionConfidence(chunk, distance), distance, detectionTick);
        TryAppendObservation(observations, objectId, objectKind, SensorType.Hearing, ComputeHearingConfidence(chunk, distance, hearingBaseConfidence), distance, detectionTick);
        TryAppendObservation(observations, objectId, objectKind, SensorType.Smell, ComputeSmellConfidence(chunk, distance, smellBaseConfidence), distance, detectionTick);
        TryAppendObservation(observations, objectId, objectKind, SensorType.Touch, ComputeTouchConfidence(distance), distance, detectionTick);
    }

    private void TryAppendObservation(
        List<PerceivedObject> observations,
        ulong objectId,
        PerceivedObjectKind objectKind,
        SensorType sensorType,
        int confidence,
        int distance,
        long detectionTick)
    {
        if (confidence < settings.MinimumConfidence)
        {
            return;
        }

        observations.Add(new PerceivedObject(objectId, objectKind, sensorType, confidence, distance, detectionTick));
    }

    private int ComputeVisionConfidence(Chunk chunk, int distance)
    {
        if (distance > settings.VisionRange)
        {
            return 0;
        }

        int confidence = 1000;
        confidence -= distance * 220;
        confidence -= chunk.Biome.VegetationProfile.Density * 3;
        confidence -= Math.Abs(chunk.Terrain.Elevation.SeaLevelOffset) * 2;
        confidence -= chunk.Climate.Precipitation.Coverage * 2;
        confidence -= chunk.Climate.WeatherState switch
        {
            WeatherState.Clear => 0,
            WeatherState.Cloudy => 50,
            WeatherState.Rain => 180,
            WeatherState.Storm => 300,
            WeatherState.Snow => 220,
            WeatherState.Fog => 320,
            WeatherState.Drought => 0,
            _ => 0,
        };

        return ClampConfidence(confidence);
    }

    private int ComputeHearingConfidence(Chunk chunk, int distance, int baseConfidence)
    {
        if (distance > settings.HearingRange)
        {
            return 0;
        }

        int confidence = baseConfidence;
        confidence -= distance * 180;
        confidence -= chunk.Climate.Wind.Speed * 20;
        confidence -= chunk.Climate.Wind.GustStrength * 10;
        confidence -= chunk.Terrain.Slope.Gradient * 8;
        return ClampConfidence(confidence);
    }

    private int ComputeSmellConfidence(Chunk chunk, int distance, int baseConfidence)
    {
        if (distance > settings.SmellRange)
        {
            return 0;
        }

        int confidence = baseConfidence;
        confidence -= distance * 200;
        confidence += chunk.Climate.Humidity.RelativeHumidity * 2;
        confidence -= chunk.Climate.Wind.Speed * 10;
        confidence -= chunk.Climate.Precipitation.Coverage;
        return ClampConfidence(confidence);
    }

    private int ComputeTouchConfidence(int distance)
    {
        if (distance > settings.TouchRange)
        {
            return 0;
        }

        return distance == 0 ? 1000 : 0;
    }

    private static Chunk ResolveChunkById(GaiaEngine.Domain.World.World world, ChunkId chunkId)
    {
        foreach (Chunk chunk in world.GetChunks())
        {
            if (chunk.Id == chunkId)
            {
                return chunk;
            }
        }

        throw new InvalidOperationException("The organism references a chunk that does not exist in the world state.");
    }

    private static int GetChunkDistance(ChunkCoordinates left, ChunkCoordinates right)
    {
        return Math.Max(Math.Abs(left.X - right.X), Math.Abs(left.Y - right.Y));
    }

    private static int ClampConfidence(int value)
    {
        return Math.Clamp(value, 0, 1000);
    }

    private static int CompareObservations(PerceivedObject left, PerceivedObject right)
    {
        int sensorComparison = left.SensorType.CompareTo(right.SensorType);
        if (sensorComparison != 0)
        {
            return sensorComparison;
        }

        int distanceComparison = left.Distance.CompareTo(right.Distance);
        if (distanceComparison != 0)
        {
            return distanceComparison;
        }

        int objectKindComparison = left.ObjectKind.CompareTo(right.ObjectKind);
        if (objectKindComparison != 0)
        {
            return objectKindComparison;
        }

        return left.ObjectId.CompareTo(right.ObjectId);
    }
}

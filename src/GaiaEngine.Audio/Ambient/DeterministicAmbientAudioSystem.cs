using System;
using System.Collections.Generic;
using GaiaEngine.Audio.Events;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Audio.Ambient;

/// <summary>
/// Reconstructs layered ambient audio from the current deterministic world state.
/// </summary>
public sealed class DeterministicAmbientAudioSystem : IAmbientAudioSystem
{
    private readonly AmbientAudioSettings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicAmbientAudioSystem"/> class.
    /// </summary>
    /// <param name="settings">The deterministic ambient evaluation settings.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="settings"/> is <see langword="null"/>.</exception>
    public DeterministicAmbientAudioSystem(AmbientAudioSettings settings)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    /// <inheritdoc />
    public AmbientMixSnapshot Evaluate(GaiaEngine.Domain.World.World world, OrganismCollection organisms, ChunkId focusChunkId)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(organisms);

        Chunk focusChunk = ResolveChunk(world, focusChunkId);
        List<Chunk> nearbyChunks = GetNearbyChunks(world, focusChunk.Metadata.Coordinates);
        List<AmbientLayerState> layers = new();

        layers.Add(CreateGlobalLayer(focusChunk));

        AmbientLayerState? weatherLayer = CreateWeatherLayer(focusChunk);
        if (weatherLayer is not null)
        {
            layers.Add(weatherLayer);
        }

        AmbientLayerState primaryBiomeLayer = CreatePrimaryBiomeLayer(focusChunk);
        layers.Add(primaryBiomeLayer);

        AmbientLayerState? transitionLayer = CreateBiomeTransitionLayer(focusChunk, nearbyChunks);
        if (transitionLayer is not null)
        {
            layers.Add(transitionLayer);
        }

        AmbientLayerState timeLayer = CreateTimeLayer(world.TimeState);
        layers.Add(timeLayer);

        AmbientLayerState? waterLayer = CreateWaterLayer(focusChunk, nearbyChunks);
        if (waterLayer is not null)
        {
            layers.Add(waterLayer);
        }

        AmbientLayerState? wildlifeLayer = CreateWildlifeLayer(world.TimeState, organisms, nearbyChunks);
        if (wildlifeLayer is not null)
        {
            layers.Add(wildlifeLayer);
        }

        layers.Sort(CompareLayers);
        return new AmbientMixSnapshot(world.TimeState.CurrentTick, focusChunkId, layers.AsReadOnly());
    }

    private static Chunk ResolveChunk(GaiaEngine.Domain.World.World world, ChunkId focusChunkId)
    {
        foreach (Chunk chunk in world.GetChunks())
        {
            if (chunk.Id == focusChunkId)
            {
                return chunk;
            }
        }

        throw new KeyNotFoundException("The supplied focus chunk does not exist in the current world.");
    }

    private List<Chunk> GetNearbyChunks(GaiaEngine.Domain.World.World world, ChunkCoordinates origin)
    {
        List<Chunk> chunks = new();
        for (int y = origin.Y - settings.NearbyChunkRange; y <= origin.Y + settings.NearbyChunkRange; y++)
        {
            for (int x = origin.X - settings.NearbyChunkRange; x <= origin.X + settings.NearbyChunkRange; x++)
            {
                ChunkCoordinates coordinates = new(x, y);
                if (world.TryGetChunk(coordinates, out Chunk? chunk))
                {
                    chunks.Add(chunk!);
                }
            }
        }

        return chunks;
    }

    private AmbientLayerState CreateGlobalLayer(Chunk focusChunk)
    {
        string clipId = focusChunk.Climate.Zone switch
        {
            ClimateZone.Arid => "ambient.global.arid",
            ClimateZone.Polar => "ambient.global.polar",
            ClimateZone.Tropical => "ambient.global.tropical",
            ClimateZone.Continental => "ambient.global.continental",
            _ => "ambient.global.temperate",
        };

        float volume = Clamp01(0.25f + (focusChunk.Climate.Wind.Speed / 40f));
        return new AmbientLayerState(
            "global.primary",
            AmbientLayerKind.Global,
            AmbientSpatialScope.Global,
            priority: 5,
            clipId,
            volume,
            settings.TransitionTicks,
            spatialProfile: null);
    }

    private AmbientLayerState CreatePrimaryBiomeLayer(Chunk focusChunk)
    {
        return new AmbientLayerState(
            "biome.primary",
            AmbientLayerKind.Biome,
            AmbientSpatialScope.Regional,
            priority: 4,
            GetBiomeClipId(focusChunk.Biome.Category),
            volume: 0.55f,
            settings.TransitionTicks,
            spatialProfile: null);
    }

    private AmbientLayerState? CreateBiomeTransitionLayer(Chunk focusChunk, IReadOnlyList<Chunk> nearbyChunks)
    {
        Dictionary<BiomeCategory, int> counts = new();
        foreach (Chunk chunk in nearbyChunks)
        {
            if (chunk.Id == focusChunk.Id || chunk.Biome.Category == focusChunk.Biome.Category)
            {
                continue;
            }

            counts.TryGetValue(chunk.Biome.Category, out int count);
            counts[chunk.Biome.Category] = count + 1;
        }

        if (counts.Count == 0)
        {
            return null;
        }

        BiomeCategory selectedCategory = default;
        int selectedCount = -1;
        foreach ((BiomeCategory category, int count) in counts)
        {
            if (count > selectedCount || (count == selectedCount && category < selectedCategory))
            {
                selectedCategory = category;
                selectedCount = count;
            }
        }

        float volume = Clamp01(0.18f + (selectedCount * 0.08f));
        return new AmbientLayerState(
            "biome.transition",
            AmbientLayerKind.Biome,
            AmbientSpatialScope.Regional,
            priority: 4,
            GetBiomeClipId(selectedCategory),
            volume,
            settings.TransitionTicks,
            spatialProfile: null);
    }

    private AmbientLayerState? CreateWeatherLayer(Chunk focusChunk)
    {
        string? clipId = focusChunk.Climate.WeatherState switch
        {
            WeatherState.Clear => null,
            WeatherState.Cloudy => "ambient.weather.cloudy",
            WeatherState.Rain => "ambient.weather.rain",
            WeatherState.Storm => "ambient.weather.storm",
            WeatherState.Snow => "ambient.weather.snow",
            WeatherState.Fog => "ambient.weather.fog",
            WeatherState.Drought => "ambient.weather.drought",
            _ => null,
        };

        if (clipId is null)
        {
            return null;
        }

        float volume = focusChunk.Climate.WeatherState switch
        {
            WeatherState.Cloudy => 0.35f,
            WeatherState.Fog => 0.40f,
            WeatherState.Rain => 0.75f,
            WeatherState.Snow => 0.60f,
            WeatherState.Storm => 0.90f,
            WeatherState.Drought => 0.50f,
            _ => 0.35f,
        };

        return new AmbientLayerState(
            "weather.primary",
            AmbientLayerKind.Weather,
            AmbientSpatialScope.Global,
            priority: 0,
            clipId,
            volume,
            settings.TransitionTicks,
            spatialProfile: null);
    }

    private AmbientLayerState? CreateWaterLayer(Chunk focusChunk, IReadOnlyList<Chunk> nearbyChunks)
    {
        WaterCandidate? bestCandidate = null;
        foreach (Chunk chunk in nearbyChunks)
        {
            int distance = GetChunkDistance(focusChunk.Metadata.Coordinates, chunk.Metadata.Coordinates);
            WaterCandidate? candidate = CreateWaterCandidate(chunk, distance);
            if (candidate is null)
            {
                continue;
            }

            if (bestCandidate is null
                || candidate.Priority < bestCandidate.Priority
                || (candidate.Priority == bestCandidate.Priority && candidate.Distance < bestCandidate.Distance)
                || (candidate.Priority == bestCandidate.Priority && candidate.Distance == bestCandidate.Distance && chunk.Id.Value < bestCandidate.SourceChunk.Id.Value))
            {
                bestCandidate = candidate;
            }
        }

        if (bestCandidate is null)
        {
            return null;
        }

        float volume = Clamp01(bestCandidate.BaseVolume - (bestCandidate.Distance * 0.18f));
        if (volume <= 0f)
        {
            return null;
        }

        AmbientSpatialScope scope = bestCandidate.Distance == 0 ? AmbientSpatialScope.Local : AmbientSpatialScope.Regional;
        AudioSpatialProfile? spatialProfile = scope == AmbientSpatialScope.Local
            ? CreateSpatialProfile(bestCandidate.SourceChunk)
            : null;

        return new AmbientLayerState(
            "water.primary",
            AmbientLayerKind.Water,
            scope,
            priority: 1,
            bestCandidate.ClipId,
            volume,
            settings.TransitionTicks,
            spatialProfile);
    }

    private AmbientLayerState? CreateWildlifeLayer(WorldTimeState timeState, OrganismCollection organisms, IReadOnlyList<Chunk> nearbyChunks)
    {
        int nearbyPopulation = CountNearbyPopulation(organisms, nearbyChunks);
        if (nearbyPopulation <= 0)
        {
            return null;
        }

        bool isNight = IsNight(timeState.CurrentTick);
        float volume = Clamp01(0.20f + (nearbyPopulation * 0.10f));
        return new AmbientLayerState(
            "wildlife.primary",
            AmbientLayerKind.Wildlife,
            AmbientSpatialScope.Regional,
            priority: 2,
            isNight ? "ambient.wildlife.night" : "ambient.wildlife.day",
            volume,
            settings.TransitionTicks,
            spatialProfile: null);
    }

    private AmbientLayerState CreateTimeLayer(WorldTimeState timeState)
    {
        bool isNight = IsNight(timeState.CurrentTick);
        return new AmbientLayerState(
            "time.primary",
            AmbientLayerKind.Time,
            AmbientSpatialScope.Global,
            priority: 3,
            isNight ? "ambient.time.night" : "ambient.time.day",
            isNight ? 0.32f : 0.24f,
            settings.TransitionTicks,
            spatialProfile: null);
    }

    private WaterCandidate? CreateWaterCandidate(Chunk chunk, int distance)
    {
        if (chunk.Water.Ocean is not null)
        {
            return new WaterCandidate(chunk, "ambient.water.ocean", BaseVolume: 0.85f, Priority: 0, Distance: distance);
        }

        if (chunk.Water.River is not null)
        {
            return new WaterCandidate(chunk, "ambient.water.river", BaseVolume: 0.72f, Priority: 1, Distance: distance);
        }

        if (chunk.Water.Lake is not null)
        {
            return new WaterCandidate(chunk, "ambient.water.lake", BaseVolume: 0.62f, Priority: 2, Distance: distance);
        }

        if (chunk.Water.SurfaceWater.WaterLevel >= 250)
        {
            return new WaterCandidate(chunk, "ambient.water.shore", BaseVolume: 0.45f, Priority: 3, Distance: distance);
        }

        return null;
    }

    private int CountNearbyPopulation(OrganismCollection organisms, IReadOnlyList<Chunk> nearbyChunks)
    {
        HashSet<ChunkId> chunkIds = new();
        foreach (Chunk chunk in nearbyChunks)
        {
            chunkIds.Add(chunk.Id);
        }

        int count = 0;
        foreach (Organism organism in organisms.GetAll())
        {
            if (chunkIds.Contains(organism.CurrentChunkId) && organism.Lifecycle.IsAlive)
            {
                count++;
            }
        }

        return count;
    }

    private bool IsNight(long tick)
    {
        long tickOfDay = tick % settings.TicksPerDay;
        int midpoint = settings.TicksPerDay / 2;
        return tickOfDay >= midpoint;
    }

    private AudioSpatialProfile CreateSpatialProfile(Chunk chunk)
    {
        float x = (chunk.Metadata.Coordinates.X * chunk.Metadata.Size) + (chunk.Metadata.Size / 2f);
        float z = (chunk.Metadata.Coordinates.Y * chunk.Metadata.Size) + (chunk.Metadata.Size / 2f);
        return new AudioSpatialProfile(new AudioPosition(x, 0f, z), settings.LocalSpatialRadius, volumeFalloff: 1f);
    }

    private static int GetChunkDistance(ChunkCoordinates left, ChunkCoordinates right)
    {
        return Math.Abs(left.X - right.X) + Math.Abs(left.Y - right.Y);
    }

    private static string GetBiomeClipId(BiomeCategory category)
    {
        return category switch
        {
            BiomeCategory.Aquatic => "ambient.biome.aquatic",
            BiomeCategory.Coastal => "ambient.biome.coastal",
            BiomeCategory.Plains => "ambient.biome.plains",
            BiomeCategory.Forest => "ambient.biome.forest",
            BiomeCategory.Wetland => "ambient.biome.wetland",
            BiomeCategory.Arid => "ambient.biome.arid",
            BiomeCategory.Cold => "ambient.biome.cold",
            BiomeCategory.Mountain => "ambient.biome.mountain",
            BiomeCategory.Volcanic => "ambient.biome.volcanic",
            _ => "ambient.biome.unknown",
        };
    }

    private static float Clamp01(float value)
    {
        if (value < 0f)
        {
            return 0f;
        }

        return value > 1f ? 1f : value;
    }

    private static int CompareLayers(AmbientLayerState left, AmbientLayerState right)
    {
        int priorityComparison = left.Priority.CompareTo(right.Priority);
        if (priorityComparison != 0)
        {
            return priorityComparison;
        }

        int kindComparison = left.Kind.CompareTo(right.Kind);
        if (kindComparison != 0)
        {
            return kindComparison;
        }

        return string.CompareOrdinal(left.LayerId, right.LayerId);
    }

    private sealed record WaterCandidate(Chunk SourceChunk, string ClipId, float BaseVolume, int Priority, int Distance);
}

using System;
using System.Collections.Generic;
using GaiaEngine.Audio.Ambient;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using Xunit;

namespace GaiaEngine.Audio.Tests.Ambient;

public sealed class DeterministicAmbientAudioSystemTests
{
    [Fact]
    public void Evaluate_ShouldCreateLayeredAmbienceFromCurrentWorldState()
    {
        DeterministicAmbientAudioSystem system = new(new AmbientAudioSettings(ticksPerDay: 100, transitionTicks: 12, nearbyChunkRange: 1, localSpatialRadius: 24f));
        GaiaEngine.Domain.World.World world = CreateWorld(currentTick: 75, focusWeather: WeatherState.Rain, focusBiome: BiomeCategory.Forest, focusRiver: true, eastBiome: BiomeCategory.Arid);
        OrganismCollection organisms = CreateOrganisms((100, 2, true), (101, 2, true), (102, 3, true));

        AmbientMixSnapshot result = system.Evaluate(world, organisms, ChunkId.FromSequence(new EntitySequence(2)));

        Assert.Equal(75, result.Tick);
        Assert.Equal(ChunkId.FromSequence(new EntitySequence(2)), result.FocusChunkId);
        Assert.Equal("weather.primary", result.Layers[0].LayerId);
        Assert.Contains(result.Layers, layer => layer.LayerId == "water.primary" && layer.AudioClipId == "ambient.water.river" && layer.SpatialProfile is not null);
        Assert.Contains(result.Layers, layer => layer.LayerId == "wildlife.primary" && layer.AudioClipId == "ambient.wildlife.night");
        Assert.Contains(result.Layers, layer => layer.LayerId == "biome.primary" && layer.AudioClipId == "ambient.biome.forest");
        Assert.Contains(result.Layers, layer => layer.LayerId == "time.primary" && layer.AudioClipId == "ambient.time.night");
        Assert.Contains(result.Layers, layer => layer.LayerId == "global.primary" && layer.AudioClipId == "ambient.global.temperate");
    }

    [Fact]
    public void Evaluate_ShouldAddBiomeTransitionLayerWhenNeighbouringBiomeChanges()
    {
        DeterministicAmbientAudioSystem system = new(new AmbientAudioSettings(ticksPerDay: 100, transitionTicks: 8, nearbyChunkRange: 1, localSpatialRadius: 24f));
        GaiaEngine.Domain.World.World world = CreateWorld(currentTick: 20, focusWeather: WeatherState.Clear, focusBiome: BiomeCategory.Forest, focusRiver: false, eastBiome: BiomeCategory.Arid);

        AmbientMixSnapshot result = system.Evaluate(world, OrganismCollection.Empty, ChunkId.FromSequence(new EntitySequence(2)));

        Assert.Contains(result.Layers, layer => layer.LayerId == "biome.transition" && layer.AudioClipId == "ambient.biome.arid");
        Assert.DoesNotContain(result.Layers, layer => layer.LayerId == "weather.primary");
        Assert.DoesNotContain(result.Layers, layer => layer.LayerId == "wildlife.primary");
    }

    private static GaiaEngine.Domain.World.World CreateWorld(long currentTick, WeatherState focusWeather, BiomeCategory focusBiome, bool focusRiver, BiomeCategory eastBiome)
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
            new WorldDimensions(32, 16, 16, 4, 200),
            new WorldTimeState(currentTick, 0, "Spring", 0),
            new[]
            {
                CreateChunk(worldId, 2, 0, 0, focusBiome, focusWeather, focusRiver, Array.Empty<OrganismId>()),
                CreateChunk(worldId, 3, 1, 0, eastBiome, WeatherState.Clear, river: false, Array.Empty<OrganismId>()),
                CreateChunk(worldId, 4, 0, 1, focusBiome, WeatherState.Clear, river: false, Array.Empty<OrganismId>()),
                CreateChunk(worldId, 5, 1, 1, focusBiome, WeatherState.Clear, river: false, Array.Empty<OrganismId>()),
            });
    }

    private static Chunk CreateChunk(
        WorldId worldId,
        ulong chunkSequence,
        int x,
        int y,
        BiomeCategory biomeCategory,
        WeatherState weatherState,
        bool river,
        IReadOnlyList<OrganismId> organismIds)
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
                biomeCategory.ToString(),
                biomeCategory,
                $"Biome {biomeCategory}.",
                new BiomeClimateProfile(18, 2, 55, 4, 8),
                new BiomeTerrainProfile(40, 80, SoilType.Loam, SurfaceType.Grass, 60),
                new BiomeResourceProfile(750, 800, 500, 800),
                new BiomeVegetationProfile(VegetationType.Forest, 62),
                new BiomeSpeciesAffinityProfile(72, 46, 60, 20)),
            new ClimateState(
                ClimateZone.Temperate,
                weatherState,
                new TemperatureState(18, 18, 18, 0),
                new HumidityState(55, 3, 2),
                new WindState(90, 4, 6),
                new PrecipitationState(weatherState == WeatherState.Rain ? PrecipitationType.Rain : PrecipitationType.None, weatherState == WeatherState.Rain ? 12 : 0, 0, weatherState == WeatherState.Rain ? 70 : 0),
                new PressureState(1012)),
            new WaterState(
                new SurfaceWaterState(river ? 300 : 100, 3, 90, 400),
                new GroundWaterState(42, 58, 6, 0),
                river ? new RiverState($"river-{chunkSequence}", 4, 3, 20, 6) : null,
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

    private static OrganismCollection CreateOrganisms(params (ulong OrganismSequence, ulong ChunkSequence, bool IsAlive)[] organisms)
    {
        Organism[] resolved = new Organism[organisms.Length];
        for (int index = 0; index < organisms.Length; index++)
        {
            resolved[index] = CreateOrganism(organisms[index].OrganismSequence, organisms[index].ChunkSequence, organisms[index].IsAlive);
        }

        return new OrganismCollection(resolved);
    }

    private static Organism CreateOrganism(ulong organismSequence, ulong chunkSequence, bool isAlive)
    {
        return new Organism(
            OrganismId.FromSequence(new EntitySequence(organismSequence)),
            SpeciesId.FromSequence(new EntitySequence(1)),
            GenomeId.FromSequence(new EntitySequence(200 + organismSequence)),
            ChunkId.FromSequence(new EntitySequence(chunkSequence)),
            new PhysiologyComponent(3, 2, 500, 60, 55, 18),
            new NeedsComponent(100, 100, 100, 0),
            new LifecycleComponent(0, 0, 100, LifecycleStage.Adult, isAlive),
            new HealthComponent(100, 100));
    }
}

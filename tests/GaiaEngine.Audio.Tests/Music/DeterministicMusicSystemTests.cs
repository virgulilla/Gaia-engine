using System;
using System.Collections.Generic;
using GaiaEngine.Audio.Events;
using GaiaEngine.Audio.Music;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.Events;
using Xunit;

namespace GaiaEngine.Audio.Tests.Music;

public sealed class DeterministicMusicSystemTests
{
    [Fact]
    public void Evaluate_ShouldPreferCriticalWorldEventsOverOtherInputs()
    {
        DeterministicMusicSystem system = new(DefaultMusicCatalogFactory.Create());
        GaiaEngine.Domain.World.World world = CreateWorld(20, WeatherState.Clear, BiomeCategory.Forest);

        MusicSelectionSnapshot result = system.Evaluate(
            MusicPresentationContext.InGame,
            world,
            CreateOrganisms((100, 2)),
            new IEvent[]
            {
                new NewYearSimulationEvent(
                    EventId.FromSequence(new EntitySequence(1)),
                    tick: 20,
                    timestamp: 20,
                    currentDay: 0,
                    currentSeason: "Spring",
                    currentYear: 1),
            },
            new[]
            {
                new GameplayAudioSignal(EventId.FromSequence(new EntitySequence(2)), GameplayAudioSignalKind.Discovery, 20),
            });

        Assert.Equal(MusicThemeKind.WorldEvent, result.Theme.ThemeKind);
        Assert.Equal(MusicPrimaryState.Event, result.Theme.PrimaryState);
        Assert.Equal("music.event.critical", result.TrackId);
    }

    [Fact]
    public void Evaluate_ShouldSelectDiscoveryThemeWhenDiscoverySignalsExist()
    {
        DeterministicMusicSystem system = new(DefaultMusicCatalogFactory.Create());
        GaiaEngine.Domain.World.World world = CreateWorld(20, WeatherState.Clear, BiomeCategory.Forest);

        MusicSelectionSnapshot result = system.Evaluate(
            MusicPresentationContext.InGame,
            world,
            CreateOrganisms((100, 2)),
            Array.Empty<IEvent>(),
            new[]
            {
                new GameplayAudioSignal(EventId.FromSequence(new EntitySequence(3)), GameplayAudioSignalKind.AchievementUnlocked, 20),
            });

        Assert.Equal(MusicThemeKind.Discovery, result.Theme.ThemeKind);
        Assert.Equal("music.discovery.highlight", result.TrackId);
        Assert.Equal(MusicTransitionKind.Crossfade, result.Theme.PlaybackRules.TransitionKind);
    }

    [Fact]
    public void Evaluate_ShouldSelectTensionThemeForWarningsOrSevereWeather()
    {
        DeterministicMusicSystem system = new(DefaultMusicCatalogFactory.Create());
        GaiaEngine.Domain.World.World world = CreateWorld(60, WeatherState.Storm, BiomeCategory.Cold);

        MusicSelectionSnapshot result = system.Evaluate(
            MusicPresentationContext.InGame,
            world,
            CreateOrganisms((100, 2)),
            Array.Empty<IEvent>(),
            Array.Empty<GameplayAudioSignal>());

        Assert.Equal(MusicThemeKind.Tension, result.Theme.ThemeKind);
        Assert.Equal(MusicPrimaryState.Tension, result.Theme.PrimaryState);
        Assert.Equal("music.tension.danger", result.TrackId);
    }

    [Fact]
    public void Evaluate_ShouldSelectExplorationThemeByDefaultAndPreserveAdaptiveReason()
    {
        DeterministicMusicSystem system = new(DefaultMusicCatalogFactory.Create());
        GaiaEngine.Domain.World.World world = CreateWorld(60, WeatherState.Clear, BiomeCategory.Forest);

        MusicSelectionSnapshot result = system.Evaluate(
            MusicPresentationContext.InGame,
            world,
            CreateOrganisms((100, 2)),
            Array.Empty<IEvent>(),
            Array.Empty<GameplayAudioSignal>());

        Assert.Equal(MusicThemeKind.Exploration, result.Theme.ThemeKind);
        Assert.Equal("music.exploration", result.TrackId);
        Assert.Equal("exploration-forest-night", result.Reason);
    }
    
    [Fact]
    public void Evaluate_ShouldResolvePresentationThemesOutsideGameplay()
    {
        DeterministicMusicSystem system = new(DefaultMusicCatalogFactory.Create());
        GaiaEngine.Domain.World.World world = CreateWorld(0, WeatherState.Clear, BiomeCategory.Plains);

        MusicSelectionSnapshot menuResult = system.Evaluate(
            MusicPresentationContext.Menu,
            world,
            OrganismCollection.Empty,
            Array.Empty<IEvent>(),
            Array.Empty<GameplayAudioSignal>());
        MusicSelectionSnapshot loadingResult = system.Evaluate(
            MusicPresentationContext.Loading,
            world,
            OrganismCollection.Empty,
            Array.Empty<IEvent>(),
            Array.Empty<GameplayAudioSignal>());
        MusicSelectionSnapshot creditsResult = system.Evaluate(
            MusicPresentationContext.Credits,
            world,
            OrganismCollection.Empty,
            Array.Empty<IEvent>(),
            Array.Empty<GameplayAudioSignal>());

        Assert.Equal("music.menu.relax", menuResult.TrackId);
        Assert.Equal(MusicThemeKind.MainTheme, loadingResult.Theme.ThemeKind);
        Assert.Equal("music.credits.ending", creditsResult.TrackId);
    }

    private static GaiaEngine.Domain.World.World CreateWorld(long currentTick, WeatherState weatherState, BiomeCategory biomeCategory)
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
            new WorldTimeState(currentTick, 0, "Spring", 0),
            new[]
            {
                CreateChunk(worldId, 2, biomeCategory, weatherState),
            });
    }

    private static Chunk CreateChunk(WorldId worldId, ulong chunkSequence, BiomeCategory biomeCategory, WeatherState weatherState)
    {
        return new Chunk(
            new ChunkMetadata(
                ChunkId.FromSequence(new EntitySequence(chunkSequence)),
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
                new PrecipitationState(weatherState == WeatherState.Storm ? PrecipitationType.Rain : PrecipitationType.None, weatherState == WeatherState.Storm ? 12 : 0, 0, weatherState == WeatherState.Storm ? 70 : 0),
                new PressureState(1012)),
            new WaterState(
                new SurfaceWaterState(100, 3, 90, 400),
                new GroundWaterState(42, 58, 6, 0),
                null,
                null,
                null),
            CreateResources(chunkSequence),
            Array.Empty<OrganismId>());
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

    private static OrganismCollection CreateOrganisms(params (ulong OrganismSequence, ulong ChunkSequence)[] organisms)
    {
        List<Organism> resolved = new(organisms.Length);
        foreach ((ulong organismSequence, ulong chunkSequence) in organisms)
        {
            resolved.Add(
                new Organism(
                    OrganismId.FromSequence(new EntitySequence(organismSequence)),
                    SpeciesId.FromSequence(new EntitySequence(1)),
                    GenomeId.FromSequence(new EntitySequence(200 + organismSequence)),
                    ChunkId.FromSequence(new EntitySequence(chunkSequence)),
                    new PhysiologyComponent(3, 2, 500, 60, 55, 18),
                    new NeedsComponent(100, 100, 100, 0),
                    new LifecycleComponent(0, 0, 100, LifecycleStage.Adult, true),
                    new HealthComponent(100, 100)));
        }

        return new OrganismCollection(resolved.ToArray());
    }
}

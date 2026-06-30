using System;
using System.Collections.Generic;
using GaiaEngine.Audio.Events;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Events;

namespace GaiaEngine.Audio.Music;

/// <summary>
/// Selects one deterministic primary music theme from current presentation, world, and event state.
/// </summary>
public sealed class DeterministicMusicSystem : IMusicSystem
{
    private readonly MusicCatalog catalog;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicMusicSystem"/> class.
    /// </summary>
    /// <param name="catalog">The deterministic music catalog.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="catalog"/> is <see langword="null"/>.</exception>
    public DeterministicMusicSystem(MusicCatalog catalog)
    {
        this.catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
    }

    /// <inheritdoc />
    public MusicSelectionSnapshot Evaluate(
        MusicPresentationContext context,
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        IReadOnlyList<IEvent> simulationEvents,
        IReadOnlyList<GameplayAudioSignal> gameplaySignals)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(organisms);
        ArgumentNullException.ThrowIfNull(simulationEvents);
        ArgumentNullException.ThrowIfNull(gameplaySignals);

        return context switch
        {
            MusicPresentationContext.Menu => CreateSelection(world.TimeState.CurrentTick, context, catalog.Get(MusicThemeKind.Menu), "menu"),
            MusicPresentationContext.Loading => CreateSelection(world.TimeState.CurrentTick, context, catalog.Get(MusicThemeKind.MainTheme), "loading"),
            MusicPresentationContext.Credits => CreateSelection(world.TimeState.CurrentTick, context, catalog.Get(MusicThemeKind.Credits), "credits"),
            _ => EvaluateInGame(world, organisms, simulationEvents, gameplaySignals),
        };
    }

    private MusicSelectionSnapshot EvaluateInGame(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        IReadOnlyList<IEvent> simulationEvents,
        IReadOnlyList<GameplayAudioSignal> gameplaySignals)
    {
        if (HasCriticalWorldEvent(simulationEvents))
        {
            return CreateSelection(world.TimeState.CurrentTick, MusicPresentationContext.InGame, catalog.Get(MusicThemeKind.WorldEvent), "critical-world-event");
        }

        if (HasDiscoverySignal(gameplaySignals))
        {
            return CreateSelection(world.TimeState.CurrentTick, MusicPresentationContext.InGame, catalog.Get(MusicThemeKind.Discovery), "discovery");
        }

        if (HasTension(world, gameplaySignals))
        {
            return CreateSelection(world.TimeState.CurrentTick, MusicPresentationContext.InGame, catalog.Get(MusicThemeKind.Tension), "tension");
        }

        if (organisms.Count == 0)
        {
            MusicThemeDefinition explorationTheme = catalog.Get(MusicThemeKind.Exploration);
            return new MusicSelectionSnapshot(
                world.TimeState.CurrentTick,
                MusicPresentationContext.InGame,
                explorationTheme,
                "music.silence.ambient",
                "ambient-silence");
        }

        return CreateSelection(world.TimeState.CurrentTick, MusicPresentationContext.InGame, catalog.Get(MusicThemeKind.Exploration), BuildExplorationReason(world));
    }

    private static bool HasCriticalWorldEvent(IReadOnlyList<IEvent> simulationEvents)
    {
        foreach (IEvent simulationEvent in simulationEvents)
        {
            if (simulationEvent is NewYearSimulationEvent)
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasDiscoverySignal(IReadOnlyList<GameplayAudioSignal> gameplaySignals)
    {
        foreach (GameplayAudioSignal signal in gameplaySignals)
        {
            if (signal.Kind is GameplayAudioSignalKind.Discovery or GameplayAudioSignalKind.AchievementUnlocked)
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasTension(GaiaEngine.Domain.World.World world, IReadOnlyList<GameplayAudioSignal> gameplaySignals)
    {
        foreach (GameplayAudioSignal signal in gameplaySignals)
        {
            if (signal.Kind == GameplayAudioSignalKind.Warning)
            {
                return true;
            }
        }

        foreach (Chunk chunk in world.GetChunks())
        {
            if (chunk.Climate.WeatherState is WeatherState.Storm or WeatherState.Drought)
            {
                return true;
            }
        }

        return false;
    }

    private MusicSelectionSnapshot CreateSelection(long timestamp, MusicPresentationContext context, MusicThemeDefinition theme, string reason)
    {
        return new MusicSelectionSnapshot(timestamp, context, theme, ResolveTrackId(theme), reason);
    }

    private string ResolveTrackId(MusicThemeDefinition theme)
    {
        return theme.ThemeKind switch
        {
            MusicThemeKind.MainTheme => $"{theme.TrackPrefix}.intro",
            MusicThemeKind.Menu => $"{theme.TrackPrefix}.relax",
            MusicThemeKind.Credits => $"{theme.TrackPrefix}.ending",
            MusicThemeKind.WorldEvent => $"{theme.TrackPrefix}.critical",
            MusicThemeKind.Discovery => $"{theme.TrackPrefix}.highlight",
            MusicThemeKind.Tension => $"{theme.TrackPrefix}.danger",
            _ => theme.TrackPrefix,
        };
    }

    private string BuildExplorationReason(GaiaEngine.Domain.World.World world)
    {
        Chunk primaryChunk = world.GetChunks()[0];
        string dayPeriod = IsNight(world.TimeState.CurrentTick) ? "night" : "day";
        return $"exploration-{primaryChunk.Biome.Category.ToString().ToLowerInvariant()}-{dayPeriod}";
    }

    private static bool IsNight(long tick)
    {
        return (tick % 100) >= 50;
    }
}

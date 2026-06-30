using System;
using System.Collections.Generic;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Events;

namespace GaiaEngine.Audio.Events;

/// <summary>
/// Translates simulation and gameplay activity into deterministic runtime audio requests.
/// </summary>
public sealed class DeterministicAudioEventSystem : IAudioEventSystem
{
    private readonly AudioEventCatalog catalog;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicAudioEventSystem"/> class.
    /// </summary>
    /// <param name="catalog">The configurable audio translation catalog.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="catalog"/> is <see langword="null"/>.</exception>
    public DeterministicAudioEventSystem(AudioEventCatalog catalog)
    {
        this.catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
    }

    /// <inheritdoc />
    public AudioEventBatchResult Translate(IReadOnlyList<IEvent> simulationEvents, IReadOnlyList<GameplayAudioSignal> gameplaySignals)
    {
        ArgumentNullException.ThrowIfNull(simulationEvents);
        ArgumentNullException.ThrowIfNull(gameplaySignals);

        List<AudioEvent> translatedEvents = new(simulationEvents.Count + gameplaySignals.Count);
        foreach (IEvent simulationEvent in simulationEvents)
        {
            ArgumentNullException.ThrowIfNull(simulationEvent);
            if (TryTranslateSimulationEvent(simulationEvent, out AudioEvent? audioEvent) && audioEvent is not null)
            {
                translatedEvents.Add(audioEvent);
            }
        }

        foreach (GameplayAudioSignal gameplaySignal in gameplaySignals)
        {
            ArgumentNullException.ThrowIfNull(gameplaySignal);
            if (TryTranslateGameplaySignal(gameplaySignal, out AudioEvent? audioEvent) && audioEvent is not null)
            {
                translatedEvents.Add(audioEvent);
            }
        }

        translatedEvents.Sort(static (left, right) => CompareAudioEvents(left, right));

        HashSet<string> deduplicationKeys = new(StringComparer.Ordinal);
        List<AudioEvent> uniqueEvents = new(translatedEvents.Count);
        List<AudioPlaybackRequest> playbackRequests = new(translatedEvents.Count);
        int mergedEventCount = 0;
        foreach (AudioEvent audioEvent in translatedEvents)
        {
            string deduplicationKey = CreateDeduplicationKey(audioEvent);
            if (!deduplicationKeys.Add(deduplicationKey))
            {
                mergedEventCount++;
                continue;
            }

            uniqueEvents.Add(audioEvent);
            playbackRequests.Add(new AudioPlaybackRequest(audioEvent));
        }

        return new AudioEventBatchResult(uniqueEvents.AsReadOnly(), playbackRequests.AsReadOnly(), mergedEventCount);
    }

    private bool TryTranslateSimulationEvent(IEvent simulationEvent, out AudioEvent? audioEvent)
    {
        string sourceKey = ResolveSimulationSourceKey(simulationEvent);
        if (!catalog.TryResolveSimulation(sourceKey, out AudioEventDefinition? definition) || definition is null)
        {
            audioEvent = null;
            return false;
        }

        AudioSpatialProfile? spatialProfile = definition.Spatial ? CreateDefaultSpatialProfile() : null;
        audioEvent = new AudioEvent(
            simulationEvent.EventId,
            definition.Category,
            definition.Priority,
            spatialProfile,
            simulationEvent.Timestamp,
            definition.AudioClipId,
            definition.PlaybackRules);
        return true;
    }

    private bool TryTranslateGameplaySignal(GameplayAudioSignal gameplaySignal, out AudioEvent? audioEvent)
    {
        if (!catalog.TryResolveGameplay(gameplaySignal.Kind, out AudioEventDefinition? definition) || definition is null)
        {
            audioEvent = null;
            return false;
        }

        AudioSpatialProfile? spatialProfile = definition.Spatial ? gameplaySignal.SpatialProfile : null;
        audioEvent = new AudioEvent(
            gameplaySignal.EventId,
            definition.Category,
            definition.Priority,
            spatialProfile,
            gameplaySignal.Timestamp,
            definition.AudioClipId,
            definition.PlaybackRules);
        return true;
    }

    private static string ResolveSimulationSourceKey(IEvent simulationEvent)
    {
        return simulationEvent switch
        {
            ActionCompletedSimulationEvent completed => $"ActionCompleted:{completed.ActionType}",
            _ => simulationEvent.EventType,
        };
    }

    private static AudioSpatialProfile CreateDefaultSpatialProfile()
    {
        return new AudioSpatialProfile(new AudioPosition(0, 0, 0), maximumDistance: 32, volumeFalloff: 1);
    }

    private static int CompareAudioEvents(AudioEvent left, AudioEvent right)
    {
        int timestampComparison = left.Timestamp.CompareTo(right.Timestamp);
        if (timestampComparison != 0)
        {
            return timestampComparison;
        }

        int priorityComparison = right.Priority.CompareTo(left.Priority);
        if (priorityComparison != 0)
        {
            return priorityComparison;
        }

        int clipComparison = string.CompareOrdinal(left.AudioClipId, right.AudioClipId);
        if (clipComparison != 0)
        {
            return clipComparison;
        }

        return left.EventId.Value.CompareTo(right.EventId.Value);
    }

    private static string CreateDeduplicationKey(AudioEvent audioEvent)
    {
        string spatialKey = audioEvent.SpatialProfile is null
            ? "none"
            : $"{audioEvent.SpatialProfile.Position.X}|{audioEvent.SpatialProfile.Position.Y}|{audioEvent.SpatialProfile.Position.Z}|{audioEvent.SpatialProfile.MaximumDistance}|{audioEvent.SpatialProfile.VolumeFalloff}";
        return $"{audioEvent.Category}|{audioEvent.Priority}|{audioEvent.AudioClipId}|{audioEvent.Timestamp}|{spatialKey}";
    }
}

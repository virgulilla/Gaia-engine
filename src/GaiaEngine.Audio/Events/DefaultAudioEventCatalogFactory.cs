using System.Collections.Generic;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.Events;

namespace GaiaEngine.Audio.Events;

/// <summary>
/// Creates the default deterministic audio translation catalog for the current audio slice.
/// </summary>
public static class DefaultAudioEventCatalogFactory
{
    /// <summary>
    /// Creates the ordered default audio translation catalog.
    /// </summary>
    /// <returns>The default audio translation catalog.</returns>
    public static AudioEventCatalog Create()
    {
        Dictionary<string, AudioEventDefinition> simulationDefinitions = new(System.StringComparer.Ordinal)
        {
            [CreateActionKey(SimulationActionType.Move)] = new(
                CreateActionKey(SimulationActionType.Move),
                AudioEventCategory.Organisms,
                AudioEventPriority.Normal,
                "organism.footstep",
                spatial: true,
                new AudioPlaybackRules(cooldownTicks: 1, maximumInstances: 8, looping: false, variationKey: "footstep")),
            [CreateActionKey(SimulationActionType.Eat)] = new(
                CreateActionKey(SimulationActionType.Eat),
                AudioEventCategory.Organisms,
                AudioEventPriority.Normal,
                "organism.eat",
                spatial: true,
                new AudioPlaybackRules(cooldownTicks: 2, maximumInstances: 4, looping: false, variationKey: "eat")),
            [CreateActionKey(SimulationActionType.Drink)] = new(
                CreateActionKey(SimulationActionType.Drink),
                AudioEventCategory.Organisms,
                AudioEventPriority.Normal,
                "organism.drink",
                spatial: true,
                new AudioPlaybackRules(cooldownTicks: 2, maximumInstances: 4, looping: false, variationKey: "drink")),
            [nameof(NewSeasonSimulationEvent)] = new(
                nameof(NewSeasonSimulationEvent),
                AudioEventCategory.Environment,
                AudioEventPriority.Important,
                "environment.season.change",
                spatial: false,
                new AudioPlaybackRules(cooldownTicks: 0, maximumInstances: 1, looping: false, variationKey: null)),
            [nameof(NewYearSimulationEvent)] = new(
                nameof(NewYearSimulationEvent),
                AudioEventCategory.System,
                AudioEventPriority.Critical,
                "system.new.year",
                spatial: false,
                new AudioPlaybackRules(cooldownTicks: 0, maximumInstances: 1, looping: false, variationKey: null)),
        };

        Dictionary<GameplayAudioSignalKind, AudioEventDefinition> gameplayDefinitions = new()
        {
            [GameplayAudioSignalKind.Discovery] = new(
                GameplayAudioSignalKind.Discovery.ToString(),
                AudioEventCategory.Gameplay,
                AudioEventPriority.Important,
                "gameplay.discovery",
                spatial: false,
                new AudioPlaybackRules(cooldownTicks: 0, maximumInstances: 2, looping: false, variationKey: null)),
            [GameplayAudioSignalKind.ObjectiveComplete] = new(
                GameplayAudioSignalKind.ObjectiveComplete.ToString(),
                AudioEventCategory.Gameplay,
                AudioEventPriority.Important,
                "gameplay.objective.complete",
                spatial: false,
                new AudioPlaybackRules(cooldownTicks: 0, maximumInstances: 1, looping: false, variationKey: null)),
            [GameplayAudioSignalKind.AchievementUnlocked] = new(
                GameplayAudioSignalKind.AchievementUnlocked.ToString(),
                AudioEventCategory.Gameplay,
                AudioEventPriority.Critical,
                "gameplay.achievement.unlocked",
                spatial: false,
                new AudioPlaybackRules(cooldownTicks: 0, maximumInstances: 1, looping: false, variationKey: null)),
            [GameplayAudioSignalKind.UnlockGranted] = new(
                GameplayAudioSignalKind.UnlockGranted.ToString(),
                AudioEventCategory.Gameplay,
                AudioEventPriority.Important,
                "gameplay.unlock.granted",
                spatial: false,
                new AudioPlaybackRules(cooldownTicks: 0, maximumInstances: 1, looping: false, variationKey: null)),
            [GameplayAudioSignalKind.Warning] = new(
                GameplayAudioSignalKind.Warning.ToString(),
                AudioEventCategory.Gameplay,
                AudioEventPriority.Critical,
                "gameplay.warning",
                spatial: false,
                new AudioPlaybackRules(cooldownTicks: 0, maximumInstances: 1, looping: false, variationKey: null)),
        };

        return new AudioEventCatalog(simulationDefinitions, gameplayDefinitions);
    }

    private static string CreateActionKey(SimulationActionType actionType)
    {
        return $"ActionCompleted:{actionType}";
    }
}

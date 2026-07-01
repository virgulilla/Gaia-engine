using System;
using System.Collections.Generic;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Player;

namespace GaiaEngine.Gameplay.Achievements;

/// <summary>
/// Evaluates player achievements deterministically from current profile state.
/// </summary>
public sealed class DeterministicAchievementSystem : IAchievementSystem
{
    private readonly IReadOnlyList<AchievementDefinition> definitions;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicAchievementSystem"/> class.
    /// </summary>
    /// <param name="definitions">The ordered achievement definitions to evaluate.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="definitions"/> is <see langword="null"/>.</exception>
    public DeterministicAchievementSystem(IReadOnlyList<AchievementDefinition> definitions)
    {
        this.definitions = definitions ?? throw new ArgumentNullException(nameof(definitions));
    }

    /// <inheritdoc />
    public AchievementEvaluationResult Evaluate(PlayerProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);

        List<AchievementEntry> updatedEntries = new(definitions.Count);
        List<AchievementEntry> unlockedEntries = new();
        string unlockDate = profile.Identity.CreationDate;

        foreach (AchievementDefinition definition in definitions)
        {
            int progressValue = ResolveProgressValue(definition, profile);
            bool shouldUnlock = progressValue >= definition.TargetValue;
            AchievementEntry currentEntry = profile.Achievements.TryGet(definition.AchievementId, out AchievementEntry? existingEntry) && existingEntry is not null
                ? existingEntry
                : CreateEntry(definition, progressValue, unlockDate: null);

            AchievementEntry updatedEntry = currentEntry.IsUnlocked
                ? new AchievementEntry(
                    currentEntry.AchievementId,
                    currentEntry.Category,
                    currentEntry.Title,
                    currentEntry.Description,
                    currentEntry.TargetValue,
                    progressValue,
                    currentEntry.Reward,
                    currentEntry.Hidden,
                    currentEntry.UnlockDate)
                : new AchievementEntry(
                    currentEntry.AchievementId,
                    currentEntry.Category,
                    currentEntry.Title,
                    currentEntry.Description,
                    currentEntry.TargetValue,
                    progressValue,
                    currentEntry.Reward,
                    currentEntry.Hidden,
                    shouldUnlock ? unlockDate : null);

            if (!currentEntry.IsUnlocked && updatedEntry.IsUnlocked)
            {
                unlockedEntries.Add(updatedEntry);
            }

            updatedEntries.Add(updatedEntry);
        }

        PlayerProfile updatedProfile = new(
            profile.Identity,
            profile.Knowledge,
            profile.Objectives,
            profile.Progression,
            new AchievementCollection(updatedEntries.AsReadOnly()),
            profile.Statistics,
            profile.Settings);
        return new AchievementEvaluationResult(updatedProfile, unlockedEntries.AsReadOnly());
    }

    private static AchievementEntry CreateEntry(AchievementDefinition definition, int progressValue, string? unlockDate)
    {
        return new AchievementEntry(
            definition.AchievementId,
            definition.Category,
            definition.Title,
            definition.Description,
            definition.TargetValue,
            progressValue,
            definition.Reward,
            definition.Hidden,
            unlockDate);
    }

    private static int ResolveProgressValue(AchievementDefinition definition, PlayerProfile profile)
    {
        return definition.RequirementType switch
        {
            AchievementRequirementType.TotalDiscoveries => profile.Knowledge.Discoveries.Count,
            AchievementRequirementType.CategoryDiscoveries => CountDiscoveries(profile, definition.DiscoveryCategory),
            AchievementRequirementType.EncyclopediaEntries => profile.Knowledge.Encyclopedia.Count,
            AchievementRequirementType.CompletedObjectives => profile.Progression.CompletedObjectives,
            AchievementRequirementType.UnlockLevel => profile.Progression.UnlockLevel,
            _ => 0,
        };
    }

    private static int CountDiscoveries(PlayerProfile profile, DiscoveryCategory? category)
    {
        if (!category.HasValue)
        {
            return 0;
        }

        int count = 0;
        foreach (DiscoveryEntry entry in profile.Knowledge.Discoveries.GetAll())
        {
            if (entry.Category == category.Value)
            {
                count++;
            }
        }

        return count;
    }
}

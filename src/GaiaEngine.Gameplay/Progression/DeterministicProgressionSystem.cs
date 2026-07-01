using System;
using System.Collections.Generic;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Player;

namespace GaiaEngine.Gameplay.Progression;

/// <summary>
/// Evaluates player progression deterministically from current profile state.
/// </summary>
public sealed class DeterministicProgressionSystem : IProgressionSystem
{
    private readonly ProgressionCatalog catalog;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicProgressionSystem"/> class.
    /// </summary>
    /// <param name="catalog">The configurable progression catalog.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="catalog"/> is <see langword="null"/>.</exception>
    public DeterministicProgressionSystem(ProgressionCatalog catalog)
    {
        this.catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
    }

    /// <inheritdoc />
    public ProgressionEvaluationResult Evaluate(PlayerProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);

        int currentLevel = ResolveCurrentLevel(profile.Progression.Experience);
        List<string> unlockIds = ResolveUnlockIds(currentLevel);
        List<string> milestoneIds = ResolveMilestoneIds(profile);

        List<string> newUnlockIds = new();
        foreach (string unlockId in unlockIds)
        {
            if (!profile.Progression.Unlocks.Contains(unlockId))
            {
                newUnlockIds.Add(unlockId);
            }
        }

        List<string> newMilestoneIds = new();
        foreach (string milestoneId in milestoneIds)
        {
            if (!profile.Progression.CompletedMilestones.Contains(milestoneId))
            {
                newMilestoneIds.Add(milestoneId);
            }
        }

        PlayerProfile updatedProfile = new(
            profile.Identity,
            profile.Knowledge,
            profile.Objectives,
            new PlayerProgression(
                profile.Progression.Experience,
                profile.Progression.Discoveries,
                currentLevel,
                profile.Progression.CompletedObjectives,
                new ProgressionUnlockCollection(unlockIds.AsReadOnly()),
                new ProgressionMilestoneCollection(milestoneIds.AsReadOnly())),
            profile.Achievements,
            profile.Statistics,
            profile.Settings);
        return new ProgressionEvaluationResult(updatedProfile, newUnlockIds.AsReadOnly(), newMilestoneIds.AsReadOnly());
    }

    private int ResolveCurrentLevel(int experience)
    {
        int currentLevel = 0;
        foreach (ProgressionLevelDefinition level in catalog.Levels)
        {
            if (experience < level.RequiredExperience)
            {
                continue;
            }

            currentLevel = Math.Max(currentLevel, level.Level);
        }

        return currentLevel;
    }

    private List<string> ResolveUnlockIds(int currentLevel)
    {
        HashSet<string> uniqueUnlocks = new(StringComparer.Ordinal);
        List<string> unlockIds = new();

        foreach (ProgressionLevelDefinition level in catalog.Levels)
        {
            if (level.Level > currentLevel)
            {
                continue;
            }

            foreach (ProgressionUnlockDefinition unlock in level.Unlocks)
            {
                if (!uniqueUnlocks.Add(unlock.UnlockId))
                {
                    continue;
                }

                unlockIds.Add(unlock.UnlockId);
            }
        }

        unlockIds.Sort(StringComparer.Ordinal);
        return unlockIds;
    }

    private List<string> ResolveMilestoneIds(PlayerProfile profile)
    {
        List<string> milestoneIds = new();
        foreach (ProgressionMilestoneDefinition milestone in catalog.Milestones)
        {
            if (IsMilestoneCompleted(milestone, profile))
            {
                milestoneIds.Add(milestone.MilestoneId);
            }
        }

        milestoneIds.Sort(StringComparer.Ordinal);
        return milestoneIds;
    }

    private static bool IsMilestoneCompleted(ProgressionMilestoneDefinition milestone, PlayerProfile profile)
    {
        return milestone.RequirementType switch
        {
            ProgressionMilestoneRequirementType.Experience => profile.Progression.Experience >= milestone.TargetValue,
            ProgressionMilestoneRequirementType.TotalDiscoveries => profile.Knowledge.Discoveries.Count >= milestone.TargetValue,
            ProgressionMilestoneRequirementType.CategoryDiscoveries => CountDiscoveries(profile, milestone.DiscoveryCategory) >= milestone.TargetValue,
            ProgressionMilestoneRequirementType.CompletedObjectives => profile.Progression.CompletedObjectives >= milestone.TargetValue,
            ProgressionMilestoneRequirementType.EncyclopediaEntries => profile.Knowledge.Encyclopedia.Count >= milestone.TargetValue,
            _ => false,
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

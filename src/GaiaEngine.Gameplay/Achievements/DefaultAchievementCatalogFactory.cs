using System.Collections.Generic;
using GaiaEngine.Gameplay.Discovery;

namespace GaiaEngine.Gameplay.Achievements;

/// <summary>
/// Creates the default deterministic achievement catalog for the current gameplay slice.
/// </summary>
public static class DefaultAchievementCatalogFactory
{
    /// <summary>
    /// Creates the ordered default achievement catalog.
    /// </summary>
    /// <returns>The ordered default achievement definitions.</returns>
    public static IReadOnlyList<AchievementDefinition> Create()
    {
        return new List<AchievementDefinition>
        {
            new(
                "achievement.discovery.first-species",
                AchievementCategory.Discovery,
                "First Species",
                "Discover your first species.",
                AchievementRequirementType.CategoryDiscoveries,
                targetValue: 1,
                new AchievementRewardDefinition(
                    "reward.badge.first-species",
                    AchievementRewardCategory.ProfileBadge,
                    "First Species Badge"),
                hidden: false,
                discoveryCategory: DiscoveryCategory.Species),
            new(
                "achievement.ecology.first-biome",
                AchievementCategory.Ecology,
                "Observe Every Biome",
                "Discover a first biome while observing the world.",
                AchievementRequirementType.CategoryDiscoveries,
                targetValue: 1,
                new AchievementRewardDefinition(
                    "reward.statistics.field-observer",
                    AchievementRewardCategory.StatisticsEntry,
                    "Field Observer Entry"),
                hidden: false,
                discoveryCategory: DiscoveryCategory.Biomes),
            new(
                "achievement.mastery.objective-initiate",
                AchievementCategory.Mastery,
                "Complete Every Objective",
                "Complete your first gameplay objective.",
                AchievementRequirementType.CompletedObjectives,
                targetValue: 1,
                new AchievementRewardDefinition(
                    "reward.cosmetic.objective-initiate",
                    AchievementRewardCategory.CosmeticUnlock,
                    "Objective Initiate"),
                hidden: false),
            new(
                "achievement.hidden.rising-scholar",
                AchievementCategory.Hidden,
                "Rising Scholar",
                "Reach progression level two.",
                AchievementRequirementType.UnlockLevel,
                targetValue: 2,
                new AchievementRewardDefinition(
                    "reward.encyclopedia.rising-scholar",
                    AchievementRewardCategory.EncyclopediaDecoration,
                    "Scholar Decoration"),
                hidden: true),
        }.AsReadOnly();
    }
}

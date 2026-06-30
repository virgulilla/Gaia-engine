using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Gameplay.Achievements;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Encyclopedia;
using GaiaEngine.Gameplay.Objectives;
using GaiaEngine.Gameplay.Player;
using GaiaEngine.Gameplay.Progression;
using Xunit;

namespace GaiaEngine.Gameplay.Tests.Achievements;

public sealed class DeterministicAchievementSystemTests
{
    [Fact]
    public void Evaluate_ShouldUnlockVisibleAndHiddenAchievementsDeterministically()
    {
        DeterministicAchievementSystem system = new(DefaultAchievementCatalogFactory.Create());
        PlayerProfile profile = new(
            new PlayerIdentity("player-001", "Oscar", "2026-06-30"),
            new PlayerKnowledge(
                new DiscoveryCollection(
                    new[]
                    {
                        new DiscoveryEntry(
                            "species.herbivore.a",
                            DiscoveryCategory.Species,
                            "Herbivore A",
                            "Observed a new herbivore species.",
                            10,
                            WorldId.FromSequence(new EntitySequence(1)),
                            "player-001"),
                        new DiscoveryEntry(
                            "biome.grassland",
                            DiscoveryCategory.Biomes,
                            "Grassland",
                            "Observed a biome.",
                            10,
                            WorldId.FromSequence(new EntitySequence(1)),
                            "player-001"),
                    }),
                new EncyclopediaCollection(
                    new[]
                    {
                        new EncyclopediaEntry(
                            "species.herbivore.a",
                            EncyclopediaCategory.Species,
                            "Herbivore A",
                            "Observed a new herbivore species.",
                            EncyclopediaUnlockState.Discovered,
                            "10",
                            System.Array.Empty<string>(),
                            new[] { new EncyclopediaStatistic("TimesObserved", 1) }),
                    })),
            ObjectiveCollection.Empty,
            new PlayerProgression(
                50,
                2,
                2,
                1,
                new ProgressionUnlockCollection(new[] { "analysis.organism-inspector" }),
                new ProgressionMilestoneCollection(new[] { "milestone.first-discovery" })),
            AchievementCollection.Empty,
            new PlayerStatistics(2, 0));

        AchievementEvaluationResult result = system.Evaluate(profile);

        Assert.Equal(4, result.Profile.Achievements.Count);
        Assert.Equal(4, result.UnlockedAchievements.Count);
        Assert.Contains(result.UnlockedAchievements, achievement => achievement.Hidden);
    }

    [Fact]
    public void Evaluate_ShouldPreserveUnlockedAchievementAndRefreshProgress()
    {
        DeterministicAchievementSystem system = new(DefaultAchievementCatalogFactory.Create());
        PlayerProfile profile = new(
            new PlayerIdentity("player-001", "Oscar", "2026-06-30"),
            new PlayerKnowledge(DiscoveryCollection.Empty, EncyclopediaCollection.Empty),
            ObjectiveCollection.Empty,
            new PlayerProgression(0, 0, 0, 0),
            new AchievementCollection(
                new[]
                {
                    new AchievementEntry(
                        "achievement.discovery.first-species",
                        AchievementCategory.Discovery,
                        "First Species",
                        "Discover your first species.",
                        1,
                        1,
                        new AchievementRewardDefinition(
                            "reward.badge.first-species",
                            AchievementRewardCategory.ProfileBadge,
                            "First Species Badge"),
                        hidden: false,
                        unlockDate: "2026-06-30"),
                }),
            new PlayerStatistics(0, 0));

        AchievementEvaluationResult result = system.Evaluate(profile);

        Assert.Empty(result.UnlockedAchievements);
        Assert.Equal("2026-06-30", result.Profile.Achievements.GetAll()[0].UnlockDate);
    }
}

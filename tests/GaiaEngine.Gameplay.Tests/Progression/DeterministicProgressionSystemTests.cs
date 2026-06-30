using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Encyclopedia;
using GaiaEngine.Gameplay.Objectives;
using GaiaEngine.Gameplay.Player;
using GaiaEngine.Gameplay.Progression;
using Xunit;

namespace GaiaEngine.Gameplay.Tests.Progression;

public sealed class DeterministicProgressionSystemTests
{
    [Fact]
    public void Evaluate_ShouldResolveLevelUnlocksAndMilestonesFromProfileState()
    {
        DeterministicProgressionSystem system = new(DefaultProgressionCatalogFactory.Create());
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
            new PlayerProgression(50, 1, 0, 1),
            new PlayerStatistics(1, 0));

        ProgressionEvaluationResult result = system.Evaluate(profile);

        Assert.Equal(2, result.Profile.Progression.UnlockLevel);
        Assert.Equal(4, result.Profile.Progression.Unlocks.Count);
        Assert.Equal(4, result.Profile.Progression.CompletedMilestones.Count);
        Assert.Equal(4, result.NewUnlockIds.Count);
        Assert.Equal(4, result.NewMilestoneIds.Count);
    }

    [Fact]
    public void Evaluate_ShouldNotDuplicatePreviouslyGrantedUnlocksOrMilestones()
    {
        DeterministicProgressionSystem system = new(DefaultProgressionCatalogFactory.Create());
        PlayerProfile profile = new(
            new PlayerIdentity("player-001", "Oscar", "2026-06-30"),
            new PlayerKnowledge(DiscoveryCollection.Empty, EncyclopediaCollection.Empty),
            ObjectiveCollection.Empty,
            new PlayerProgression(
                25,
                0,
                1,
                0,
                new ProgressionUnlockCollection(
                    new[]
                    {
                        "analysis.organism-inspector",
                        "simulation.time-acceleration",
                    }),
                ProgressionMilestoneCollection.Empty),
            new PlayerStatistics(0, 0));

        ProgressionEvaluationResult result = system.Evaluate(profile);

        Assert.Empty(result.NewUnlockIds);
        Assert.Empty(result.NewMilestoneIds);
        Assert.Equal(2, result.Profile.Progression.Unlocks.Count);
    }
}

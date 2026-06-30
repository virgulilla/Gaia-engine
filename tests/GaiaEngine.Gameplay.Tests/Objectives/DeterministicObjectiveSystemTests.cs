using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Encyclopedia;
using GaiaEngine.Gameplay.Objectives;
using GaiaEngine.Gameplay.Player;
using GaiaEngine.Gameplay.Achievements;
using Xunit;

namespace GaiaEngine.Gameplay.Tests.Objectives;

public sealed class DeterministicObjectiveSystemTests
{
    [Fact]
    public void Evaluate_ShouldCompleteCounterObjectiveAndGrantExperience()
    {
        DeterministicObjectiveSystem system = new(
            new[]
            {
                new ObjectiveDefinition(
                    "objective.discovery.first-species",
                    ObjectiveCategory.Discovery,
                    "Discover your first species",
                    "Unlock one species discovery.",
                    new[]
                    {
                        new ObjectiveRequirementDefinition(
                            "requirement.discovery.first-species",
                            ObjectiveRequirementType.Counter,
                            targetCount: 1,
                            discoveryCategory: DiscoveryCategory.Species),
                    },
                    new ObjectiveRewardDefinition(25, System.Array.Empty<string>()),
                    ObjectiveStatus.Active),
            });
        PlayerProfile profile = CreateProfile(
            new PlayerKnowledge(
                new DiscoveryCollection(
                    new[]
                    {
                        new DiscoveryEntry(
                            "species.herbivore.a",
                            DiscoveryCategory.Species,
                            "Herbivore A",
                            "Observed a new herbivore species.",
                            20,
                            WorldId.FromSequence(new EntitySequence(1)),
                            "player-001"),
                    }),
                EncyclopediaCollection.Empty));

        ObjectiveEvaluationResult result = system.Evaluate(profile, 20, System.Array.Empty<ObjectiveSignal>());

        ObjectiveEntry completed = Assert.Single(result.CompletedObjectives);
        Assert.Equal(ObjectiveStatus.Completed, completed.Status);
        Assert.Equal(25, result.Profile.Progression.Experience);
        Assert.Equal(1, result.Profile.Progression.CompletedObjectives);
    }

    [Fact]
    public void Evaluate_ShouldRevealAndCompleteHiddenEventObjective()
    {
        DeterministicObjectiveSystem system = new(
            new[]
            {
                new ObjectiveDefinition(
                    "objective.mastery.first-year",
                    ObjectiveCategory.Mastery,
                    "Witness a full simulated year",
                    "Observe the first simulated new year.",
                    new[]
                    {
                        new ObjectiveRequirementDefinition(
                            "requirement.mastery.first-year",
                            ObjectiveRequirementType.WorldEvent,
                            targetCount: 1,
                            discoveryCategory: DiscoveryCategory.WorldEvents,
                            signalKey: "NewYearSimulationEvent"),
                    },
                    new ObjectiveRewardDefinition(30, System.Array.Empty<string>()),
                    ObjectiveStatus.Hidden),
            });
        PlayerProfile profile = CreateProfile();

        ObjectiveEvaluationResult result = system.Evaluate(
            profile,
            300,
            new[]
            {
                new ObjectiveSignal(DiscoveryCategory.WorldEvents, "NewYearSimulationEvent"),
            });

        ObjectiveEntry completed = Assert.Single(result.CompletedObjectives);
        Assert.Equal(ObjectiveStatus.Completed, completed.Status);
        Assert.Equal(30, result.Profile.Progression.Experience);
        Assert.Equal(1, result.Profile.Progression.CompletedObjectives);
    }

    private static PlayerProfile CreateProfile(PlayerKnowledge? knowledge = null)
    {
        return new PlayerProfile(
            new PlayerIdentity("player-001", "Oscar", "2026-06-30"),
            knowledge ?? new PlayerKnowledge(DiscoveryCollection.Empty, EncyclopediaCollection.Empty),
            ObjectiveCollection.Empty,
            new PlayerProgression(0, 0, 0, 0),
            AchievementCollection.Empty,
            new PlayerStatistics(0, 0));
    }
}

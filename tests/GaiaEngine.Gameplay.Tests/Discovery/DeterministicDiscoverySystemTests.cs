using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Encyclopedia;
using GaiaEngine.Gameplay.Objectives;
using GaiaEngine.Gameplay.Player;
using Xunit;

namespace GaiaEngine.Gameplay.Tests.Discovery;

public sealed class DeterministicDiscoverySystemTests
{
    [Fact]
    public void Evaluate_ShouldUnlockDiscoveryAndUpdateProfileProgress()
    {
        DeterministicDiscoverySystem system = new(CreateRuleSet());
        PlayerProfile profile = CreateProfile();
        WorldId worldId = WorldId.FromSequence(new EntitySequence(1));

        DiscoveryEvaluationResult result = system.Evaluate(
            profile,
            worldId,
            tick: 120,
            new[]
            {
                new DiscoverySignal(DiscoveryCategory.Species, DiscoverySignalSource.Observation, "species:herbivore-a"),
            });

        DiscoveryEntry unlocked = Assert.Single(result.UnlockedDiscoveries);
        Assert.Equal("species.herbivore.a", unlocked.DiscoveryId);
        Assert.Equal(1, result.Profile.Knowledge.Discoveries.Count);
        Assert.Equal(1, result.Profile.Knowledge.Encyclopedia.Count);
        Assert.Equal(10, result.Profile.Progression.Experience);
        Assert.Equal(1, result.Profile.Progression.Discoveries);
        Assert.Equal(1, result.Profile.Statistics.TotalDiscoveriesUnlocked);
    }

    [Fact]
    public void Evaluate_ShouldIgnoreDuplicateDiscoveriesAndUpdateStatistics()
    {
        DeterministicDiscoverySystem system = new(CreateRuleSet());
        PlayerProfile profile = CreateProfile(
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
            new PlayerProgression(10, 1, 0),
            new PlayerStatistics(1, 0));
        WorldId worldId = WorldId.FromSequence(new EntitySequence(2));

        DiscoveryEvaluationResult result = system.Evaluate(
            profile,
            worldId,
            tick: 220,
            new[]
            {
                new DiscoverySignal(DiscoveryCategory.Species, DiscoverySignalSource.Observation, "species:herbivore-a"),
                new DiscoverySignal(DiscoveryCategory.Species, DiscoverySignalSource.Observation, "species:herbivore-a"),
            });

        Assert.Empty(result.UnlockedDiscoveries);
        Assert.Equal(1, result.Profile.Knowledge.Discoveries.Count);
        Assert.Equal(1, result.Profile.Statistics.DuplicateDiscoveryObservations);
    }

    private static DiscoveryRuleSet CreateRuleSet()
    {
        return new DiscoveryRuleSet(
            new[]
            {
                new DiscoveryRuleDefinition(
                    "species.herbivore.a",
                    DiscoveryCategory.Species,
                    DiscoverySignalSource.Observation,
                    "species:herbivore-a",
                    "Herbivore A",
                    "Observed a new herbivore species.",
                    rewardExperience: 10),
            });
    }

    private static PlayerProfile CreateProfile(
        DiscoveryCollection? discoveries = null,
        PlayerProgression? progression = null,
        PlayerStatistics? statistics = null)
    {
        return new PlayerProfile(
            new PlayerIdentity("player-001", "Oscar", "2026-06-30"),
            new PlayerKnowledge(discoveries ?? DiscoveryCollection.Empty, EncyclopediaCollection.Empty),
            ObjectiveCollection.Empty,
            progression ?? new PlayerProgression(0, 0, 0),
            statistics ?? new PlayerStatistics(0, 0));
    }
}

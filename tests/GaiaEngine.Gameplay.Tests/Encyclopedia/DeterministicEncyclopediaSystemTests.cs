using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Encyclopedia;
using Xunit;

namespace GaiaEngine.Gameplay.Tests.Encyclopedia;

public sealed class DeterministicEncyclopediaSystemTests
{
    [Fact]
    public void Build_ShouldCreateDiscoveredEntriesFromDiscoveries()
    {
        DeterministicEncyclopediaSystem system = new();
        DiscoveryCollection discoveries = new(
            new[]
            {
                new DiscoveryEntry(
                    "species.alpha",
                    DiscoveryCategory.Species,
                    "Species Alpha",
                    "Observed a new species.",
                    unlockTick: 40,
                    WorldId.FromSequence(new EntitySequence(1)),
                    "player-001"),
            });

        EncyclopediaCollection result = system.Build(discoveries);

        EncyclopediaEntry entry = Assert.Single(result.GetAll());
        Assert.Equal("species.alpha", entry.EntryId);
        Assert.Equal(EncyclopediaCategory.Species, entry.Category);
        Assert.Equal(EncyclopediaUnlockState.Discovered, entry.UnlockState);
        Assert.Equal(2, entry.GetStatistics().Count);
    }
}

using System;
using System.Collections.Generic;
using GaiaEngine.Gameplay.Discovery;

namespace GaiaEngine.Gameplay.Encyclopedia;

/// <summary>
/// Builds encyclopedia entries deterministically from the player's unlocked discoveries.
/// </summary>
public sealed class DeterministicEncyclopediaSystem : IEncyclopediaSystem
{
    /// <summary>
    /// Builds one deterministic encyclopedia snapshot from the supplied discoveries.
    /// </summary>
    /// <param name="discoveries">The discoveries owned by the player profile.</param>
    /// <returns>The resulting encyclopedia snapshot.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="discoveries"/> is <see langword="null"/>.</exception>
    public EncyclopediaCollection Build(DiscoveryCollection discoveries)
    {
        ArgumentNullException.ThrowIfNull(discoveries);

        List<EncyclopediaEntry> entries = new(discoveries.Count);
        foreach (DiscoveryEntry discovery in discoveries.GetAll())
        {
            entries.Add(CreateEntry(discovery));
        }

        return new EncyclopediaCollection(entries.AsReadOnly());
    }

    private static EncyclopediaEntry CreateEntry(DiscoveryEntry discovery)
    {
        List<EncyclopediaStatistic> statistics =
        [
            new EncyclopediaStatistic("TimesObserved", 1),
            new EncyclopediaStatistic("WorldsFound", 1),
        ];

        return new EncyclopediaEntry(
            discovery.DiscoveryId,
            MapCategory(discovery.Category),
            discovery.Name,
            discovery.Description,
            EncyclopediaUnlockState.Discovered,
            discovery.UnlockTick.ToString(System.Globalization.CultureInfo.InvariantCulture),
            Array.Empty<string>(),
            statistics.AsReadOnly());
    }

    private static EncyclopediaCategory MapCategory(DiscoveryCategory category)
    {
        return category switch
        {
            DiscoveryCategory.Species => EncyclopediaCategory.Species,
            DiscoveryCategory.Traits => EncyclopediaCategory.Traits,
            DiscoveryCategory.Biomes => EncyclopediaCategory.Biomes,
            DiscoveryCategory.Resources => EncyclopediaCategory.Resources,
            DiscoveryCategory.Climate => EncyclopediaCategory.Climate,
            DiscoveryCategory.Behaviours => EncyclopediaCategory.Behaviours,
            DiscoveryCategory.WorldEvents => EncyclopediaCategory.WorldHistory,
            _ => throw new ArgumentOutOfRangeException(nameof(category), "The supplied discovery category is not supported by the encyclopedia."),
        };
    }
}

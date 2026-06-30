using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Gameplay.Encyclopedia;
using GaiaEngine.Gameplay.Player;

namespace GaiaEngine.Gameplay.Discovery;

/// <summary>
/// Unlocks permanent discoveries deterministically from configurable discovery rules.
/// </summary>
public sealed class DeterministicDiscoverySystem : IDiscoverySystem
{
    private readonly DiscoveryRuleSet ruleSet;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicDiscoverySystem"/> class.
    /// </summary>
    /// <param name="ruleSet">The configurable discovery rule set.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="ruleSet"/> is <see langword="null"/>.</exception>
    public DeterministicDiscoverySystem(DiscoveryRuleSet ruleSet)
        : this(ruleSet, new DeterministicEncyclopediaSystem())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicDiscoverySystem"/> class.
    /// </summary>
    /// <param name="ruleSet">The configurable discovery rule set.</param>
    /// <param name="encyclopediaSystem">The encyclopedia system used to rebuild player knowledge.</param>
    /// <exception cref="ArgumentNullException">Thrown when any dependency is <see langword="null"/>.</exception>
    public DeterministicDiscoverySystem(DiscoveryRuleSet ruleSet, IEncyclopediaSystem encyclopediaSystem)
    {
        this.ruleSet = ruleSet ?? throw new ArgumentNullException(nameof(ruleSet));
        this.encyclopediaSystem = encyclopediaSystem ?? throw new ArgumentNullException(nameof(encyclopediaSystem));
    }

    private readonly IEncyclopediaSystem encyclopediaSystem;

    /// <summary>
    /// Evaluates one discovery pass for the supplied player profile.
    /// </summary>
    /// <param name="profile">The current player profile.</param>
    /// <param name="worldId">The world associated with the evaluation pass.</param>
    /// <param name="tick">The current simulation tick.</param>
    /// <param name="signals">The observed discovery signals to evaluate.</param>
    /// <returns>The deterministic discovery evaluation result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="profile"/> or <paramref name="signals"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="tick"/> is negative.</exception>
    public DiscoveryEvaluationResult Evaluate(PlayerProfile profile, WorldId worldId, long tick, IReadOnlyList<DiscoverySignal> signals)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(signals);
        if (tick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tick), "The discovery evaluation tick must be zero or greater.");
        }

        List<DiscoverySignal> orderedSignals = new(signals.Count);
        foreach (DiscoverySignal signal in signals)
        {
            orderedSignals.Add(signal ?? throw new ArgumentNullException(nameof(signals), "The discovery signal list cannot contain null signals."));
        }

        orderedSignals.Sort(static (left, right) => CompareSignals(left, right));

        List<DiscoveryEntry> discoveries = new(profile.Knowledge.Discoveries.GetAll());
        List<DiscoveryEntry> unlocked = new();
        int experience = profile.Progression.Experience;
        int discoveryCount = profile.Progression.Discoveries;
        int totalUnlocked = profile.Statistics.TotalDiscoveriesUnlocked;
        int duplicateObservations = profile.Statistics.DuplicateDiscoveryObservations;
        HashSet<string> processedIds = new(StringComparer.Ordinal);

        foreach (DiscoverySignal signal in orderedSignals)
        {
            if (!ruleSet.TryResolve(signal, out DiscoveryRuleDefinition? rule) || rule is null)
            {
                continue;
            }

            string discoveryId = rule.RuleId;
            if (!processedIds.Add(discoveryId))
            {
                continue;
            }

            if (profile.Knowledge.Discoveries.Contains(discoveryId))
            {
                duplicateObservations++;
                continue;
            }

            DiscoveryEntry entry = new(
                discoveryId,
                rule.Category,
                rule.Name,
                rule.Description,
                tick,
                worldId,
                profile.Identity.PlayerId);
            discoveries.Add(entry);
            unlocked.Add(entry);
            experience += rule.RewardExperience;
            discoveryCount++;
            totalUnlocked++;
        }

        DiscoveryCollection updatedCollection = new(discoveries.AsReadOnly());
        EncyclopediaCollection updatedEncyclopedia = encyclopediaSystem.Build(updatedCollection);
        PlayerProfile updatedProfile = new(
            profile.Identity,
            new PlayerKnowledge(updatedCollection, updatedEncyclopedia),
            profile.Objectives,
            new PlayerProgression(experience, discoveryCount, profile.Progression.UnlockLevel, profile.Progression.CompletedObjectives),
            new PlayerStatistics(totalUnlocked, duplicateObservations));
        return new DiscoveryEvaluationResult(updatedProfile, unlocked.AsReadOnly());
    }

    private static int CompareSignals(DiscoverySignal left, DiscoverySignal right)
    {
        int sourceComparison = left.Source.CompareTo(right.Source);
        if (sourceComparison != 0)
        {
            return sourceComparison;
        }

        int categoryComparison = left.Category.CompareTo(right.Category);
        if (categoryComparison != 0)
        {
            return categoryComparison;
        }

        return string.CompareOrdinal(left.Key, right.Key);
    }
}

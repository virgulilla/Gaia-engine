using System;
using System.Collections.Generic;

namespace GaiaEngine.Gameplay.Discovery;

/// <summary>
/// Represents the immutable configurable discovery rule set used by the gameplay layer.
/// </summary>
public sealed class DiscoveryRuleSet
{
    private readonly Dictionary<string, DiscoveryRuleDefinition> rulesByIdentity;
    private readonly IReadOnlyList<DiscoveryRuleDefinition> orderedRules;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscoveryRuleSet"/> class.
    /// </summary>
    /// <param name="rules">The deterministic discovery rules.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="rules"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when duplicated rules are detected.</exception>
    public DiscoveryRuleSet(IReadOnlyList<DiscoveryRuleDefinition> rules)
    {
        ArgumentNullException.ThrowIfNull(rules);

        rulesByIdentity = new Dictionary<string, DiscoveryRuleDefinition>(rules.Count, StringComparer.Ordinal);
        List<DiscoveryRuleDefinition> ordered = new(rules.Count);
        foreach (DiscoveryRuleDefinition rule in rules)
        {
            ArgumentNullException.ThrowIfNull(rule);
            string identity = ComposeIdentity(rule.Source, rule.Category, rule.SignalKey);
            if (!rulesByIdentity.TryAdd(identity, rule))
            {
                throw new ArgumentException($"The discovery rule '{identity}' is duplicated.", nameof(rules));
            }

            ordered.Add(rule);
        }

        ordered.Sort(static (left, right) => string.CompareOrdinal(left.RuleId, right.RuleId));
        orderedRules = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty discovery rule set.
    /// </summary>
    public static DiscoveryRuleSet Empty { get; } = new(Array.Empty<DiscoveryRuleDefinition>());

    /// <summary>
    /// Returns the configured rules in deterministic order.
    /// </summary>
    /// <returns>The ordered rule set.</returns>
    public IReadOnlyList<DiscoveryRuleDefinition> GetAll()
    {
        return orderedRules;
    }

    /// <summary>
    /// Attempts to resolve one rule by signal identity.
    /// </summary>
    /// <param name="signal">The signal to resolve.</param>
    /// <param name="rule">The matching rule when found; otherwise <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when a matching rule exists; otherwise <see langword="false"/>.</returns>
    public bool TryResolve(DiscoverySignal signal, out DiscoveryRuleDefinition? rule)
    {
        ArgumentNullException.ThrowIfNull(signal);
        return rulesByIdentity.TryGetValue(ComposeIdentity(signal.Source, signal.Category, signal.Key), out rule);
    }

    private static string ComposeIdentity(DiscoverySignalSource source, DiscoveryCategory category, string signalKey)
    {
        return $"{(int)source}:{(int)category}:{signalKey}";
    }
}

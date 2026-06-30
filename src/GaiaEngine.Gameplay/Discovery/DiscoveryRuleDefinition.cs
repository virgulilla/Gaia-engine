using System;

namespace GaiaEngine.Gameplay.Discovery;

/// <summary>
/// Defines one configurable deterministic discovery rule.
/// </summary>
public sealed record DiscoveryRuleDefinition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DiscoveryRuleDefinition"/> class.
    /// </summary>
    /// <param name="ruleId">The stable unique rule identifier.</param>
    /// <param name="category">The discovery category unlocked by the rule.</param>
    /// <param name="source">The required signal source.</param>
    /// <param name="signalKey">The required subject key.</param>
    /// <param name="name">The player-facing discovery name.</param>
    /// <param name="description">The player-facing discovery description.</param>
    /// <param name="rewardExperience">The experience reward granted when the discovery is unlocked.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="ruleId"/>, <paramref name="signalKey"/>, <paramref name="name"/>, or <paramref name="description"/> is empty.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="rewardExperience"/> is negative.</exception>
    public DiscoveryRuleDefinition(
        string ruleId,
        DiscoveryCategory category,
        DiscoverySignalSource source,
        string signalKey,
        string name,
        string description,
        int rewardExperience)
    {
        if (string.IsNullOrWhiteSpace(ruleId))
        {
            throw new ArgumentException("The discovery rule identifier must contain a value.", nameof(ruleId));
        }

        if (string.IsNullOrWhiteSpace(signalKey))
        {
            throw new ArgumentException("The discovery signal key must contain a value.", nameof(signalKey));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The discovery name must contain a value.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("The discovery description must contain a value.", nameof(description));
        }

        if (rewardExperience < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rewardExperience), "The reward experience must be zero or greater.");
        }

        RuleId = ruleId;
        Category = category;
        Source = source;
        SignalKey = signalKey;
        Name = name;
        Description = description;
        RewardExperience = rewardExperience;
    }

    /// <summary>
    /// Gets the stable unique rule identifier.
    /// </summary>
    public string RuleId { get; }

    /// <summary>
    /// Gets the discovery category unlocked by the rule.
    /// </summary>
    public DiscoveryCategory Category { get; }

    /// <summary>
    /// Gets the required source that must produce the signal.
    /// </summary>
    public DiscoverySignalSource Source { get; }

    /// <summary>
    /// Gets the required signal key that activates the rule.
    /// </summary>
    public string SignalKey { get; }

    /// <summary>
    /// Gets the player-facing discovery name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the player-facing discovery description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the experience reward granted when the discovery is unlocked.
    /// </summary>
    public int RewardExperience { get; }
}

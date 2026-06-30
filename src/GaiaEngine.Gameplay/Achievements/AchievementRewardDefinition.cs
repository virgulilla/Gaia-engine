using System;

namespace GaiaEngine.Gameplay.Achievements;

/// <summary>
/// Represents the deterministic reward attached to one achievement.
/// </summary>
public sealed record AchievementRewardDefinition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AchievementRewardDefinition"/> class.
    /// </summary>
    /// <param name="rewardId">The stable reward identifier.</param>
    /// <param name="category">The reward category.</param>
    /// <param name="name">The player-facing reward name.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="rewardId"/> or <paramref name="name"/> is empty.</exception>
    public AchievementRewardDefinition(string rewardId, AchievementRewardCategory category, string name)
    {
        if (string.IsNullOrWhiteSpace(rewardId))
        {
            throw new ArgumentException("The achievement reward identifier must contain a value.", nameof(rewardId));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The achievement reward name must contain a value.", nameof(name));
        }

        RewardId = rewardId;
        Category = category;
        Name = name;
    }

    /// <summary>
    /// Gets the stable reward identifier.
    /// </summary>
    public string RewardId { get; }

    /// <summary>
    /// Gets the reward category.
    /// </summary>
    public AchievementRewardCategory Category { get; }

    /// <summary>
    /// Gets the player-facing reward name.
    /// </summary>
    public string Name { get; }
}

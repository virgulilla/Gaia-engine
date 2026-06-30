using System;
using GaiaEngine.Gameplay.Discovery;

namespace GaiaEngine.Gameplay.Achievements;

/// <summary>
/// Represents one configurable deterministic achievement definition.
/// </summary>
public sealed record AchievementDefinition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AchievementDefinition"/> class.
    /// </summary>
    /// <param name="achievementId">The stable achievement identifier.</param>
    /// <param name="category">The achievement category.</param>
    /// <param name="title">The player-facing achievement title.</param>
    /// <param name="description">The player-facing achievement description.</param>
    /// <param name="requirementType">The deterministic unlock condition type.</param>
    /// <param name="targetValue">The required target value.</param>
    /// <param name="reward">The deterministic reward definition.</param>
    /// <param name="hidden">Whether the achievement remains hidden until unlocked.</param>
    /// <param name="discoveryCategory">The optional discovery category used by category-specific conditions.</param>
    /// <exception cref="ArgumentException">Thrown when one required text argument is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="targetValue"/> is less than one.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="reward"/> is <see langword="null"/>.</exception>
    public AchievementDefinition(
        string achievementId,
        AchievementCategory category,
        string title,
        string description,
        AchievementRequirementType requirementType,
        int targetValue,
        AchievementRewardDefinition reward,
        bool hidden,
        DiscoveryCategory? discoveryCategory = null)
    {
        if (string.IsNullOrWhiteSpace(achievementId))
        {
            throw new ArgumentException("The achievement identifier must contain a value.", nameof(achievementId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("The achievement title must contain a value.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("The achievement description must contain a value.", nameof(description));
        }

        if (targetValue < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(targetValue), "The achievement target value must be greater than zero.");
        }

        AchievementId = achievementId;
        Category = category;
        Title = title;
        Description = description;
        RequirementType = requirementType;
        TargetValue = targetValue;
        Reward = reward ?? throw new ArgumentNullException(nameof(reward));
        Hidden = hidden;
        DiscoveryCategory = discoveryCategory;
    }

    /// <summary>
    /// Gets the stable achievement identifier.
    /// </summary>
    public string AchievementId { get; }

    /// <summary>
    /// Gets the achievement category.
    /// </summary>
    public AchievementCategory Category { get; }

    /// <summary>
    /// Gets the player-facing achievement title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the player-facing achievement description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the deterministic unlock condition type.
    /// </summary>
    public AchievementRequirementType RequirementType { get; }

    /// <summary>
    /// Gets the required target value.
    /// </summary>
    public int TargetValue { get; }

    /// <summary>
    /// Gets the deterministic reward definition.
    /// </summary>
    public AchievementRewardDefinition Reward { get; }

    /// <summary>
    /// Gets a value indicating whether the achievement remains hidden until unlocked.
    /// </summary>
    public bool Hidden { get; }

    /// <summary>
    /// Gets the optional discovery category used by category-specific conditions.
    /// </summary>
    public DiscoveryCategory? DiscoveryCategory { get; }
}

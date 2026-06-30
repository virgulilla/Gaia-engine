using System;

namespace GaiaEngine.Gameplay.Achievements;

/// <summary>
/// Represents one persistent achievement entry owned by a player profile.
/// </summary>
public sealed record AchievementEntry
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AchievementEntry"/> class.
    /// </summary>
    /// <param name="achievementId">The stable achievement identifier.</param>
    /// <param name="category">The achievement category.</param>
    /// <param name="title">The player-facing achievement title.</param>
    /// <param name="description">The player-facing achievement description.</param>
    /// <param name="targetValue">The required target value.</param>
    /// <param name="currentValue">The current progress value.</param>
    /// <param name="reward">The deterministic reward definition.</param>
    /// <param name="hidden">Whether the achievement remains hidden until unlocked.</param>
    /// <param name="unlockDate">The deterministic unlock date, if unlocked.</param>
    /// <exception cref="ArgumentException">Thrown when one required text argument is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="targetValue"/> is less than one or <paramref name="currentValue"/> is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="reward"/> is <see langword="null"/>.</exception>
    public AchievementEntry(
        string achievementId,
        AchievementCategory category,
        string title,
        string description,
        int targetValue,
        int currentValue,
        AchievementRewardDefinition reward,
        bool hidden,
        string? unlockDate)
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

        if (currentValue < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currentValue), "The achievement progress value must be zero or greater.");
        }

        AchievementId = achievementId;
        Category = category;
        Title = title;
        Description = description;
        TargetValue = targetValue;
        CurrentValue = currentValue;
        Reward = reward ?? throw new ArgumentNullException(nameof(reward));
        Hidden = hidden;
        UnlockDate = unlockDate;
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
    /// Gets the required target value.
    /// </summary>
    public int TargetValue { get; }

    /// <summary>
    /// Gets the current progress value.
    /// </summary>
    public int CurrentValue { get; }

    /// <summary>
    /// Gets the deterministic reward definition.
    /// </summary>
    public AchievementRewardDefinition Reward { get; }

    /// <summary>
    /// Gets a value indicating whether the achievement remains hidden until unlocked.
    /// </summary>
    public bool Hidden { get; }

    /// <summary>
    /// Gets the deterministic unlock date, if unlocked.
    /// </summary>
    public string? UnlockDate { get; }

    /// <summary>
    /// Gets a value indicating whether the achievement has been unlocked.
    /// </summary>
    public bool IsUnlocked => UnlockDate is not null;
}

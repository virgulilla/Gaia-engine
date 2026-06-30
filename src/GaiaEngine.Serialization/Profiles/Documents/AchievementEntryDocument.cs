namespace GaiaEngine.Serialization.Profiles.Documents;

/// <summary>
/// Represents the serialized achievement entry section.
/// </summary>
internal sealed class AchievementEntryDocument
{
    /// <summary>
    /// Gets or sets the achievement identifier.
    /// </summary>
    public string AchievementId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the achievement category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the achievement title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the achievement description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target value.
    /// </summary>
    public int TargetValue { get; set; }

    /// <summary>
    /// Gets or sets the current progress value.
    /// </summary>
    public int CurrentValue { get; set; }

    /// <summary>
    /// Gets or sets the serialized reward section.
    /// </summary>
    public AchievementRewardDocument? Reward { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the achievement is hidden.
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// Gets or sets the deterministic unlock date, if unlocked.
    /// </summary>
    public string? UnlockDate { get; set; }
}

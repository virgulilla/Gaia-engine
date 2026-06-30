namespace GaiaEngine.Serialization.Profiles.Documents;

/// <summary>
/// Represents the serialized achievement reward section.
/// </summary>
internal sealed class AchievementRewardDocument
{
    /// <summary>
    /// Gets or sets the reward identifier.
    /// </summary>
    public string RewardId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the reward category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the reward name.
    /// </summary>
    public string Name { get; set; } = string.Empty;
}

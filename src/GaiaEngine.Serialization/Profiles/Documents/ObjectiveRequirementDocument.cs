namespace GaiaEngine.Serialization.Profiles.Documents;

/// <summary>
/// Represents the serialized objective requirement definition.
/// </summary>
internal sealed class ObjectiveRequirementDocument
{
    /// <summary>
    /// Gets or sets the requirement identifier.
    /// </summary>
    public string RequirementId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the requirement type.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target count.
    /// </summary>
    public int TargetCount { get; set; }

    /// <summary>
    /// Gets or sets the optional discovery category.
    /// </summary>
    public string? DiscoveryCategory { get; set; }

    /// <summary>
    /// Gets or sets the optional signal key.
    /// </summary>
    public string? SignalKey { get; set; }
}

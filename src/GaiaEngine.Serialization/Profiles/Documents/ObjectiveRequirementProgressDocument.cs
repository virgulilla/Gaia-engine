namespace GaiaEngine.Serialization.Profiles.Documents;

/// <summary>
/// Represents the serialized objective requirement progress state.
/// </summary>
internal sealed class ObjectiveRequirementProgressDocument
{
    /// <summary>
    /// Gets or sets the requirement identifier.
    /// </summary>
    public string RequirementId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current progress value.
    /// </summary>
    public int CurrentValue { get; set; }
}

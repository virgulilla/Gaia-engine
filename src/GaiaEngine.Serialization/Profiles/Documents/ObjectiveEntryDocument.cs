using System.Collections.Generic;

namespace GaiaEngine.Serialization.Profiles.Documents;

/// <summary>
/// Represents the serialized objective entry section.
/// </summary>
internal sealed class ObjectiveEntryDocument
{
    /// <summary>
    /// Gets or sets the objective identifier.
    /// </summary>
    public string ObjectiveId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the objective category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the objective title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the objective description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the objective requirements.
    /// </summary>
    public List<ObjectiveRequirementDocument> Requirements { get; set; } = new();

    /// <summary>
    /// Gets or sets the objective requirement progress values.
    /// </summary>
    public List<ObjectiveRequirementProgressDocument> Progress { get; set; } = new();

    /// <summary>
    /// Gets or sets the objective reward.
    /// </summary>
    public ObjectiveRewardDocument? Reward { get; set; }

    /// <summary>
    /// Gets or sets the objective status.
    /// </summary>
    public string Status { get; set; } = string.Empty;
}

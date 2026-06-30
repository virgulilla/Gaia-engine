namespace GaiaEngine.Serialization.Profiles.Documents;

using System.Collections.Generic;

/// <summary>
/// Represents one serialized encyclopedia entry.
/// </summary>
internal sealed class EncyclopediaEntryDocument
{
    /// <summary>
    /// Gets or sets the entry identifier.
    /// </summary>
    public string EntryId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the entry category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unlock state.
    /// </summary>
    public string UnlockState { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the discovery date.
    /// </summary>
    public string DiscoveryDate { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the related entry identifiers.
    /// </summary>
    public List<string> RelatedEntries { get; set; } = new();

    /// <summary>
    /// Gets or sets the serialized statistics.
    /// </summary>
    public List<EncyclopediaStatisticDocument> Statistics { get; set; } = new();
}

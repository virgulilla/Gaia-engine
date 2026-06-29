using System.Collections.Generic;

namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized payload of one genome gene group.
/// </summary>
internal sealed class GenomeGeneGroupDocument
{
    /// <summary>
    /// Gets or sets the group category.
    /// </summary>
    public string GroupType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the serialized gene list.
    /// </summary>
    public List<GenomeGeneDocument> Genes { get; set; } = new();
}

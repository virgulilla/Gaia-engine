namespace GaiaEngine.Serialization.SaveGames.Documents;

using System.Collections.Generic;

/// <summary>
/// Represents the serialized save game document.
/// </summary>
internal sealed class WorldSaveGameDocument
{
    /// <summary>
    /// Gets or sets the serialized metadata section.
    /// </summary>
    public SaveMetadataDocument? Metadata { get; set; }

    /// <summary>
    /// Gets or sets the serialized world section.
    /// </summary>
    public WorldDocument? World { get; set; }

    /// <summary>
    /// Gets or sets the serialized configuration version.
    /// </summary>
    public string ConfigurationVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the serialized organism section.
    /// </summary>
    public List<OrganismDocument> Organisms { get; set; } = new();

    /// <summary>
    /// Gets or sets the serialized genome section.
    /// </summary>
    public List<GenomeDocument> Genomes { get; set; } = new();

    /// <summary>
    /// Gets or sets the serialized action request section.
    /// </summary>
    public List<ActionRequestDocument> ActionRequests { get; set; } = new();

    /// <summary>
    /// Gets or sets the serialized version section.
    /// </summary>
    public SaveVersionInfoDocument? Version { get; set; }
}

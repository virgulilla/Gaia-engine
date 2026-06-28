namespace GaiaEngine.Serialization.SaveGames.Documents;

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
    /// Gets or sets the serialized version section.
    /// </summary>
    public SaveVersionInfoDocument? Version { get; set; }
}

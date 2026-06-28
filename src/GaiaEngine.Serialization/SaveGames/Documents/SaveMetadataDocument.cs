namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized metadata section of a save game.
/// </summary>
internal sealed class SaveMetadataDocument
{
    public string SaveName { get; set; } = string.Empty;

    public string CreationDate { get; set; } = string.Empty;

    public string LastModified { get; set; } = string.Empty;

    public long WorldSeed { get; set; }

    public string EngineVersion { get; set; } = string.Empty;

    public string SaveVersion { get; set; } = string.Empty;
}

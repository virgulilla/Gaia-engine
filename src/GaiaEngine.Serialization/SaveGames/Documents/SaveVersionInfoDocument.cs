namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized version section of a save game.
/// </summary>
internal sealed class SaveVersionInfoDocument
{
    public string FormatVersion { get; set; } = string.Empty;

    public string EngineVersion { get; set; } = string.Empty;

    public string ContentVersion { get; set; } = string.Empty;
}

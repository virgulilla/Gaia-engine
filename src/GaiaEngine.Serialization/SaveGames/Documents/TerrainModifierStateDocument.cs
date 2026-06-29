namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents one serialized terrain modifier stored inside a terrain document.
/// </summary>
internal sealed class TerrainModifierStateDocument
{
    public string Type { get; set; } = string.Empty;

    public int Intensity { get; set; }
}

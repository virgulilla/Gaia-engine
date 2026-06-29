namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized optional river section inside a water document.
/// </summary>
internal sealed class RiverStateDocument
{
    public string RiverId { get; set; } = string.Empty;

    public int Width { get; set; }

    public int Depth { get; set; }

    public int FlowRate { get; set; }

    public int CurrentVelocity { get; set; }
}

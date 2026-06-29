namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized surface-water section inside a water document.
/// </summary>
internal sealed class SurfaceWaterStateDocument
{
    public int WaterLevel { get; set; }

    public int FlowSpeed { get; set; }

    public int FlowDirection { get; set; }

    public int WaterVolume { get; set; }
}

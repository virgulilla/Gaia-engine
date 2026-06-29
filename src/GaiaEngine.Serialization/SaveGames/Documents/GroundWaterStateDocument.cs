namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized groundwater section inside a water document.
/// </summary>
internal sealed class GroundWaterStateDocument
{
    public int WaterTable { get; set; }

    public int Saturation { get; set; }

    public int RechargeRate { get; set; }

    public int ExtractionRate { get; set; }
}

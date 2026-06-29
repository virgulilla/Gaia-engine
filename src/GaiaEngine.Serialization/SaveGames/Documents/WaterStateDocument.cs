namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized water section inside a chunk save document.
/// </summary>
internal sealed class WaterStateDocument
{
    public SurfaceWaterStateDocument SurfaceWater { get; set; } = new();

    public GroundWaterStateDocument GroundWater { get; set; } = new();

    public RiverStateDocument? River { get; set; }

    public LakeStateDocument? Lake { get; set; }

    public OceanStateDocument? Ocean { get; set; }
}

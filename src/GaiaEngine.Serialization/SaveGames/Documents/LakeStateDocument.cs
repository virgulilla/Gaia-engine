namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized optional lake section inside a water document.
/// </summary>
internal sealed class LakeStateDocument
{
    public int SurfaceArea { get; set; }

    public int MaximumDepth { get; set; }

    public int WaterVolume { get; set; }

    public int OverflowLevel { get; set; }
}

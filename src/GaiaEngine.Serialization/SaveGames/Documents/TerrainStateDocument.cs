using System.Collections.Generic;

namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized terrain section inside a chunk save document.
/// </summary>
internal sealed class TerrainStateDocument
{
    public int Height { get; set; }

    public int RelativeHeight { get; set; }

    public int SeaLevelOffset { get; set; }

    public int Gradient { get; set; }

    public int Aspect { get; set; }

    public int TraversalCost { get; set; }

    public string SoilType { get; set; } = string.Empty;

    public int Fertility { get; set; }

    public int Drainage { get; set; }

    public int MoistureCapacity { get; set; }

    public int OrganicMatter { get; set; }

    public string Surface { get; set; } = string.Empty;

    public string Geology { get; set; } = string.Empty;

    public List<TerrainModifierStateDocument> Modifiers { get; set; } = new();
}

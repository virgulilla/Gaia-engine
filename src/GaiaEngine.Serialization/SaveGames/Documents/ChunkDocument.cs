using System.Collections.Generic;

namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized chunk section inside a world save document.
/// </summary>
internal sealed class ChunkDocument
{
    public string ChunkId { get; set; } = string.Empty;

    public string WorldId { get; set; } = string.Empty;

    public int X { get; set; }

    public int Y { get; set; }

    public long Seed { get; set; }

    public int Size { get; set; }

    public string State { get; set; } = string.Empty;

    public ClimateStateDocument Climate { get; set; } = new();

    public List<ResourceStateDocument> Resources { get; set; } = new();

    public List<string> OrganismIds { get; set; } = new();
}

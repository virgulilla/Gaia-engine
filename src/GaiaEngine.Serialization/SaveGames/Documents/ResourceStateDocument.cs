namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents one serialized resource state stored inside a chunk document.
/// </summary>
internal sealed class ResourceStateDocument
{
    public string ResourceId { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public int CurrentAmount { get; set; }

    public int MaximumCapacity { get; set; }

    public int RegenerationRate { get; set; }

    public int Quality { get; set; }

    public int Availability { get; set; }
}

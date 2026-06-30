namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents one serialized memory entry.
/// </summary>
internal sealed class MemoryEntryDocument
{
    /// <summary>
    /// Gets or sets the remembered identifier.
    /// </summary>
    public ulong Identifier { get; set; }

    /// <summary>
    /// Gets or sets the memory category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the remembered chunk X coordinate.
    /// </summary>
    public int PositionX { get; set; }

    /// <summary>
    /// Gets or sets the remembered chunk Y coordinate.
    /// </summary>
    public int PositionY { get; set; }

    /// <summary>
    /// Gets or sets the remembered confidence.
    /// </summary>
    public int Confidence { get; set; }

    /// <summary>
    /// Gets or sets the creation tick.
    /// </summary>
    public long CreationTick { get; set; }

    /// <summary>
    /// Gets or sets the last update tick.
    /// </summary>
    public long LastUpdateTick { get; set; }

    /// <summary>
    /// Gets or sets the expiration tick.
    /// </summary>
    public long ExpirationTick { get; set; }

    /// <summary>
    /// Gets or sets the optional remembered availability.
    /// </summary>
    public int? EstimatedAvailability { get; set; }
}

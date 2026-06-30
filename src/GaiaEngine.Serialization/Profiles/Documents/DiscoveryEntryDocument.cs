namespace GaiaEngine.Serialization.Profiles.Documents;

/// <summary>
/// Represents one serialized player discovery entry.
/// </summary>
internal sealed class DiscoveryEntryDocument
{
    /// <summary>
    /// Gets or sets the discovery identifier.
    /// </summary>
    public string DiscoveryId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the discovery category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the discovery name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the discovery description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unlock tick.
    /// </summary>
    public long UnlockTick { get; set; }

    /// <summary>
    /// Gets or sets the world identifier.
    /// </summary>
    public string WorldId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the player identifier.
    /// </summary>
    public string PlayerId { get; set; } = string.Empty;
}

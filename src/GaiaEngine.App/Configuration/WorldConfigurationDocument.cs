namespace GaiaEngine.App.Configuration;

/// <summary>
/// Represents the raw JSON document used to deserialize world bootstrap configuration.
/// </summary>
internal sealed class WorldConfigurationDocument
{
    /// <summary>
    /// Gets or sets the deterministic world name.
    /// </summary>
    public string WorldName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the deterministic world seed.
    /// </summary>
    public long WorldSeed { get; set; }

    /// <summary>
    /// Gets or sets the number of chunk columns.
    /// </summary>
    public int ChunkColumns { get; set; }

    /// <summary>
    /// Gets or sets the number of chunk rows.
    /// </summary>
    public int ChunkRows { get; set; }

    /// <summary>
    /// Gets or sets the chunk size.
    /// </summary>
    public int ChunkSize { get; set; }

    /// <summary>
    /// Gets or sets the maximum elevation value.
    /// </summary>
    public int MaximumElevation { get; set; }

    /// <summary>
    /// Gets or sets the default initial climate zone.
    /// </summary>
    public string DefaultClimateZone { get; set; } = string.Empty;
}

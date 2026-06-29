namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized identity payload of one genome.
/// </summary>
internal sealed class GenomeIdentityDocument
{
    /// <summary>
    /// Gets or sets the serialized genome identifier.
    /// </summary>
    public string GenomeId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the deterministic genome payload version.
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// Gets or sets the first optional parent genome identifier.
    /// </summary>
    public string? ParentGenomeA { get; set; }

    /// <summary>
    /// Gets or sets the second optional parent genome identifier.
    /// </summary>
    public string? ParentGenomeB { get; set; }

    /// <summary>
    /// Gets or sets the accumulated mutation count.
    /// </summary>
    public int MutationCount { get; set; }

    /// <summary>
    /// Gets or sets the lineage generation number.
    /// </summary>
    public int Generation { get; set; }
}

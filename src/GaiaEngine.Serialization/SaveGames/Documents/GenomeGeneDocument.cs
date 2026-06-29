namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized payload of one genome gene.
/// </summary>
internal sealed class GenomeGeneDocument
{
    /// <summary>
    /// Gets or sets the gene identifier.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the normalized deterministic gene value scaled to [0, 1000].
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Gets or sets the gene dominance mode.
    /// </summary>
    public string Dominance { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the gene is active.
    /// </summary>
    public bool IsActive { get; set; }
}

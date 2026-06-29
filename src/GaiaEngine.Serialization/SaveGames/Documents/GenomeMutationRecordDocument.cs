namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized payload of one genome mutation record.
/// </summary>
internal sealed class GenomeMutationRecordDocument
{
    /// <summary>
    /// Gets or sets the deterministic mutation sequence.
    /// </summary>
    public int Sequence { get; set; }

    /// <summary>
    /// Gets or sets the affected gene group.
    /// </summary>
    public string GroupType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the affected gene key.
    /// </summary>
    public string GeneKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mutation category.
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the previous normalized value.
    /// </summary>
    public int PreviousValue { get; set; }

    /// <summary>
    /// Gets or sets the new normalized value.
    /// </summary>
    public int NewValue { get; set; }

    /// <summary>
    /// Gets or sets the previous dominance mode.
    /// </summary>
    public string PreviousDominance { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new dominance mode.
    /// </summary>
    public string NewDominance { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the previous activation state.
    /// </summary>
    public bool PreviousIsActive { get; set; }

    /// <summary>
    /// Gets or sets the new activation state.
    /// </summary>
    public bool NewIsActive { get; set; }
}

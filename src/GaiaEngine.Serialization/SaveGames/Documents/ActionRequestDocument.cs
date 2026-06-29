namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents one serialized common action request stored in a save game document.
/// </summary>
internal sealed class ActionRequestDocument
{
    /// <summary>
    /// Gets or sets the action identifier.
    /// </summary>
    public string ActionId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the owning organism identifier.
    /// </summary>
    public string OrganismId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the action type.
    /// </summary>
    public string ActionType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the target kind.
    /// </summary>
    public string TargetKind { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the serialized target identifier.
    /// </summary>
    public string TargetId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the start tick.
    /// </summary>
    public long StartTick { get; set; }

    /// <summary>
    /// Gets or sets the expected duration.
    /// </summary>
    public int ExpectedDuration { get; set; }

    /// <summary>
    /// Gets or sets the deterministic execution priority.
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// Gets or sets the current execution status.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the request can be interrupted.
    /// </summary>
    public bool Interruptible { get; set; }
}

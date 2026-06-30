namespace GaiaEngine.Serialization.Profiles.Documents;

/// <summary>
/// Represents the serialized player progression section.
/// </summary>
internal sealed class PlayerProgressionDocument
{
    /// <summary>
    /// Gets or sets the accumulated player experience.
    /// </summary>
    public int Experience { get; set; }

    /// <summary>
    /// Gets or sets the total number of unlocked discoveries.
    /// </summary>
    public int Discoveries { get; set; }

    /// <summary>
    /// Gets or sets the unlock level.
    /// </summary>
    public int UnlockLevel { get; set; }

    /// <summary>
    /// Gets or sets the completed objective count.
    /// </summary>
    public int CompletedObjectives { get; set; }
}

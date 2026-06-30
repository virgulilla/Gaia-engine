namespace GaiaEngine.Serialization.Profiles.Documents;

/// <summary>
/// Represents the serialized player identity section.
/// </summary>
internal sealed class PlayerIdentityDocument
{
    /// <summary>
    /// Gets or sets the player identifier.
    /// </summary>
    public string PlayerId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the profile name.
    /// </summary>
    public string ProfileName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public string CreationDate { get; set; } = string.Empty;
}

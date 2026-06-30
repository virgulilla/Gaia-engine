namespace GaiaEngine.Serialization.Profiles.Documents;

using System.Collections.Generic;

/// <summary>
/// Represents the serialized player profile document.
/// </summary>
internal sealed class PlayerProfileDocument
{
    /// <summary>
    /// Gets or sets the serialized identity section.
    /// </summary>
    public PlayerIdentityDocument? Identity { get; set; }

    /// <summary>
    /// Gets or sets the serialized discoveries section.
    /// </summary>
    public List<DiscoveryEntryDocument> Discoveries { get; set; } = new();

    /// <summary>
    /// Gets or sets the serialized encyclopedia section.
    /// </summary>
    public List<EncyclopediaEntryDocument> Encyclopedia { get; set; } = new();

    /// <summary>
    /// Gets or sets the serialized objectives section.
    /// </summary>
    public List<ObjectiveEntryDocument> Objectives { get; set; } = new();

    /// <summary>
    /// Gets or sets the serialized achievements section.
    /// </summary>
    public List<AchievementEntryDocument> Achievements { get; set; } = new();

    /// <summary>
    /// Gets or sets the serialized progression section.
    /// </summary>
    public PlayerProgressionDocument? Progression { get; set; }

    /// <summary>
    /// Gets or sets the serialized statistics section.
    /// </summary>
    public PlayerStatisticsDocument? Statistics { get; set; }
}

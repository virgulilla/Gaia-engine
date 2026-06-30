namespace GaiaEngine.Serialization.Profiles.Documents;

/// <summary>
/// Represents the serialized player statistics section.
/// </summary>
internal sealed class PlayerStatisticsDocument
{
    /// <summary>
    /// Gets or sets the total number of unlocked discoveries.
    /// </summary>
    public int TotalDiscoveriesUnlocked { get; set; }

    /// <summary>
    /// Gets or sets the duplicate discovery observation count.
    /// </summary>
    public int DuplicateDiscoveryObservations { get; set; }
}

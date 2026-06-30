namespace GaiaEngine.Gameplay.Progression;

/// <summary>
/// Defines the supported progression milestone requirement modes.
/// </summary>
public enum ProgressionMilestoneRequirementType
{
    /// <summary>
    /// Represents a milestone based on total experience.
    /// </summary>
    Experience = 0,

    /// <summary>
    /// Represents a milestone based on total discoveries.
    /// </summary>
    TotalDiscoveries = 1,

    /// <summary>
    /// Represents a milestone based on discoveries in one category.
    /// </summary>
    CategoryDiscoveries = 2,

    /// <summary>
    /// Represents a milestone based on completed objectives.
    /// </summary>
    CompletedObjectives = 3,

    /// <summary>
    /// Represents a milestone based on encyclopedia entries.
    /// </summary>
    EncyclopediaEntries = 4,
}

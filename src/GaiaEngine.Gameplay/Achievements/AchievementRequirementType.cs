namespace GaiaEngine.Gameplay.Achievements;

/// <summary>
/// Defines the supported achievement requirement modes.
/// </summary>
public enum AchievementRequirementType
{
    /// <summary>
    /// Represents a requirement based on total discoveries.
    /// </summary>
    TotalDiscoveries = 0,

    /// <summary>
    /// Represents a requirement based on discoveries in one category.
    /// </summary>
    CategoryDiscoveries = 1,

    /// <summary>
    /// Represents a requirement based on encyclopedia entries.
    /// </summary>
    EncyclopediaEntries = 2,

    /// <summary>
    /// Represents a requirement based on completed objectives.
    /// </summary>
    CompletedObjectives = 3,

    /// <summary>
    /// Represents a requirement based on progression level.
    /// </summary>
    UnlockLevel = 4,
}

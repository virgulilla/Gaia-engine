namespace GaiaEngine.Gameplay.Objectives;

/// <summary>
/// Defines the supported objective requirement modes.
/// </summary>
public enum ObjectiveRequirementType
{
    /// <summary>
    /// Represents a requirement satisfied by one matching event.
    /// </summary>
    SingleEvent = 0,

    /// <summary>
    /// Represents a requirement satisfied by a cumulative counter.
    /// </summary>
    Counter = 1,

    /// <summary>
    /// Represents a requirement satisfied by a specific observation.
    /// </summary>
    Observation = 2,

    /// <summary>
    /// Represents a requirement satisfied by building a collection.
    /// </summary>
    Collection = 3,

    /// <summary>
    /// Represents a requirement satisfied by a world event.
    /// </summary>
    WorldEvent = 4,
}

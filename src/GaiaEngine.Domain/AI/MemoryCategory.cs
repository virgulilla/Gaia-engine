namespace GaiaEngine.Domain.AI;

/// <summary>
/// Defines the supported deterministic organism memory categories.
/// </summary>
public enum MemoryCategory
{
    /// <summary>
    /// Identifies organism memories.
    /// </summary>
    Organism = 0,

    /// <summary>
    /// Identifies resource memories.
    /// </summary>
    Resource = 1,

    /// <summary>
    /// Identifies location memories.
    /// </summary>
    Location = 2,

    /// <summary>
    /// Identifies hazard memories.
    /// </summary>
    Hazard = 3,

    /// <summary>
    /// Identifies event memories.
    /// </summary>
    Event = 4,
}

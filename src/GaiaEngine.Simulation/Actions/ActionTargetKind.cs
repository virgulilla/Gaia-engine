namespace GaiaEngine.Simulation.Actions;

/// <summary>
/// Defines the supported target kinds referenced by action requests.
/// </summary>
public enum ActionTargetKind
{
    /// <summary>
    /// Identifies a chunk target.
    /// </summary>
    Chunk = 0,

    /// <summary>
    /// Identifies an organism target.
    /// </summary>
    Organism = 1,

    /// <summary>
    /// Identifies a resource target.
    /// </summary>
    Resource = 2,

    /// <summary>
    /// Identifies a world-position target.
    /// </summary>
    Position = 3,

    /// <summary>
    /// Identifies a structure target.
    /// </summary>
    Structure = 4,
}

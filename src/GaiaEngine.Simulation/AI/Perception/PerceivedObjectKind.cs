namespace GaiaEngine.Simulation.AI.Perception;

/// <summary>
/// Defines the supported deterministic perceived-object categories.
/// </summary>
public enum PerceivedObjectKind
{
    /// <summary>
    /// Identifies an observed organism.
    /// </summary>
    Organism = 0,

    /// <summary>
    /// Identifies an observed resource.
    /// </summary>
    Resource = 1,

    /// <summary>
    /// Identifies an observed local water source.
    /// </summary>
    Water = 2,
}

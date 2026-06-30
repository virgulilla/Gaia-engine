namespace GaiaEngine.Simulation.AI.Perception;

/// <summary>
/// Defines the deterministic sensor types supported by organism perception.
/// </summary>
public enum SensorType
{
    /// <summary>
    /// Identifies vision-based detection.
    /// </summary>
    Vision = 0,

    /// <summary>
    /// Identifies hearing-based detection.
    /// </summary>
    Hearing = 1,

    /// <summary>
    /// Identifies smell-based detection.
    /// </summary>
    Smell = 2,

    /// <summary>
    /// Identifies direct touch-based detection.
    /// </summary>
    Touch = 3,
}

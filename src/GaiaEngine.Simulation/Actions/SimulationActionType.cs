namespace GaiaEngine.Simulation.Actions;

/// <summary>
/// Defines the deterministic action types currently supported by simulation execution.
/// </summary>
public enum SimulationActionType
{
    /// <summary>
    /// Identifies a movement action.
    /// </summary>
    Move = 0,

    /// <summary>
    /// Identifies a feeding action.
    /// </summary>
    Eat = 1,

    /// <summary>
    /// Identifies a hydration action.
    /// </summary>
    Drink = 2,

    /// <summary>
    /// Identifies the default idle action.
    /// </summary>
    Idle = 3,
}

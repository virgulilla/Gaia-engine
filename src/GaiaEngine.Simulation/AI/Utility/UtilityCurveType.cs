namespace GaiaEngine.Simulation.AI.Utility;

/// <summary>
/// Defines the supported deterministic utility curve types.
/// </summary>
public enum UtilityCurveType
{
    /// <summary>
    /// Identifies a linear utility curve.
    /// </summary>
    Linear = 0,

    /// <summary>
    /// Identifies an exponential utility curve.
    /// </summary>
    Exponential = 1,

    /// <summary>
    /// Identifies a logistic utility curve.
    /// </summary>
    Logistic = 2,

    /// <summary>
    /// Identifies a custom point-based utility curve.
    /// </summary>
    Custom = 3,
}

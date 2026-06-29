namespace GaiaEngine.Domain.Organisms;

/// <summary>
/// Defines the deterministic lifecycle stage of one organism.
/// </summary>
public enum LifecycleStage
{
    /// <summary>
    /// The organism has not reached maturity yet.
    /// </summary>
    Juvenile = 0,

    /// <summary>
    /// The organism has reached maturity.
    /// </summary>
    Adult = 1,

    /// <summary>
    /// The organism has entered late life.
    /// </summary>
    Elder = 2,

    /// <summary>
    /// The organism is no longer alive.
    /// </summary>
    Dead = 3,
}

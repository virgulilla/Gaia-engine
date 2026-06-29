namespace GaiaEngine.Simulation.Actions;

/// <summary>
/// Defines the deterministic execution states of one action request.
/// </summary>
public enum ActionExecutionState
{
    /// <summary>
    /// The request is waiting to be executed.
    /// </summary>
    Waiting = 0,

    /// <summary>
    /// The request was accepted by the execution layer.
    /// </summary>
    Accepted = 1,

    /// <summary>
    /// The request is currently running.
    /// </summary>
    Running = 2,

    /// <summary>
    /// The request is temporarily suspended.
    /// </summary>
    Suspended = 3,

    /// <summary>
    /// The request completed successfully.
    /// </summary>
    Completed = 4,

    /// <summary>
    /// The request was cancelled.
    /// </summary>
    Cancelled = 5,

    /// <summary>
    /// The request failed.
    /// </summary>
    Failed = 6,
}

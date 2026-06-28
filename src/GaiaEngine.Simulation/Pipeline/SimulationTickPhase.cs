namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Defines the deterministic phases executed by every simulation tick.
/// </summary>
public enum SimulationTickPhase
{
    /// <summary>
    /// Receives validated external requests before simulation processing begins.
    /// </summary>
    InputCollection,

    /// <summary>
    /// Prepares temporary state and execution context for the current tick.
    /// </summary>
    PreUpdate,

    /// <summary>
    /// Executes world-level simulation updates.
    /// </summary>
    WorldUpdate,

    /// <summary>
    /// Executes organism-level simulation updates.
    /// </summary>
    OrganismUpdate,

    /// <summary>
    /// Processes simulation interactions.
    /// </summary>
    InteractionSystems,

    /// <summary>
    /// Executes environmental consequence updates.
    /// </summary>
    EnvironmentUpdate,

    /// <summary>
    /// Dispatches simulation events generated during the current tick.
    /// </summary>
    EventDispatch,

    /// <summary>
    /// Finalizes the current tick.
    /// </summary>
    PostUpdate,
}

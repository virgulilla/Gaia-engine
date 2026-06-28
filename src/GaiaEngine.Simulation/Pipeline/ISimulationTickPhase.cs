namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Defines a deterministic phase that participates in the simulation tick pipeline.
/// </summary>
public interface ISimulationTickPhase
{
    /// <summary>
    /// Gets the deterministic phase represented by the implementation.
    /// </summary>
    public SimulationTickPhase Phase { get; }

    /// <summary>
    /// Executes the phase against the supplied tick context.
    /// </summary>
    /// <param name="context">The mutable context shared by the current tick.</param>
    public void Execute(SimulationTickContext context);
}

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Executes the deterministic sequence of phases that forms one simulation tick.
/// </summary>
public interface ISimulationTickPipeline
{
    /// <summary>
    /// Executes one deterministic simulation tick starting from the supplied world time state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <returns>The deterministic result of the tick pipeline execution.</returns>
    public SimulationTickResult Execute(GaiaEngine.Domain.World.World world, ulong nextEventSequence);
}

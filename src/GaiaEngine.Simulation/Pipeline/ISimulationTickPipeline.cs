using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Executes the deterministic sequence of phases that forms one simulation tick.
/// </summary>
public interface ISimulationTickPipeline
{
    /// <summary>
    /// Executes one deterministic simulation tick starting from the supplied world time state.
    /// </summary>
    /// <param name="timeState">The current world time state.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <returns>The deterministic result of the tick pipeline execution.</returns>
    public SimulationTickResult Execute(WorldTimeState timeState, ulong nextEventSequence);
}

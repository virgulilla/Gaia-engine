using GaiaEngine.Simulation.Time;

namespace GaiaEngine.Simulation.Events;

/// <summary>
/// Publishes deterministic simulation events generated during one tick.
/// </summary>
public interface ISimulationEventPublisher
{
    /// <summary>
    /// Publishes the simulation events derived from a time advance result.
    /// </summary>
    /// <param name="result">The deterministic time advance result.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <returns>The deterministic publication result.</returns>
    public SimulationEventPublicationResult PublishTimeEvents(TimeAdvanceResult result, ulong nextEventSequence);
}

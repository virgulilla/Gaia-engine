using GaiaEngine.Simulation.Time;
using System.Collections.Generic;

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

    /// <summary>
    /// Publishes action-started events.
    /// </summary>
    /// <param name="actions">The actions that entered execution.</param>
    /// <param name="tick">The current simulation tick.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <returns>The deterministic publication result.</returns>
    public SimulationEventPublicationResult PublishActionStartedEvents(IReadOnlyList<ActionEventDescriptor> actions, long tick, ulong nextEventSequence);

    /// <summary>
    /// Publishes action-completed events.
    /// </summary>
    /// <param name="actions">The actions that completed successfully.</param>
    /// <param name="tick">The current simulation tick.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <returns>The deterministic publication result.</returns>
    public SimulationEventPublicationResult PublishActionCompletedEvents(IReadOnlyList<ActionEventDescriptor> actions, long tick, ulong nextEventSequence);

    /// <summary>
    /// Publishes action-failed events.
    /// </summary>
    /// <param name="actions">The actions that failed.</param>
    /// <param name="tick">The current simulation tick.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <returns>The deterministic publication result.</returns>
    public SimulationEventPublicationResult PublishActionFailedEvents(IReadOnlyList<ActionEventDescriptor> actions, long tick, ulong nextEventSequence);

    /// <summary>
    /// Publishes action-cancelled events.
    /// </summary>
    /// <param name="actions">The actions that were cancelled.</param>
    /// <param name="tick">The current simulation tick.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <returns>The deterministic publication result.</returns>
    public SimulationEventPublicationResult PublishActionCancelledEvents(IReadOnlyList<ActionEventDescriptor> actions, long tick, ulong nextEventSequence);
}

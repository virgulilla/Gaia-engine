using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Actions;

namespace GaiaEngine.Simulation.Events;

/// <summary>
/// Provides the common immutable metadata shared by deterministic simulation action events.
/// </summary>
public abstract record ActionEventBase : EventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionEventBase"/> class.
    /// </summary>
    /// <param name="eventId">The immutable event identifier.</param>
    /// <param name="sourceName">The logical simulation source that emitted the event.</param>
    /// <param name="priority">The deterministic event priority.</param>
    /// <param name="tick">The target simulation tick for dispatch.</param>
    /// <param name="timestamp">The deterministic timestamp associated with the event.</param>
    /// <param name="actionId">The immutable action identifier.</param>
    /// <param name="organismId">The organism associated with the action.</param>
    /// <param name="actionType">The deterministic action type.</param>
    /// <param name="target">The immutable target metadata.</param>
    protected ActionEventBase(
        EventId eventId,
        string sourceName,
        EventPriority priority,
        long tick,
        long timestamp,
        ActionId actionId,
        OrganismId organismId,
        SimulationActionType actionType,
        SimulationActionTarget target)
        : base(eventId, EventCategory.Simulation, new EventSource(sourceName), priority, tick, timestamp)
    {
        ActionId = actionId;
        OrganismId = organismId;
        ActionType = actionType;
        Target = target;
    }

    /// <summary>
    /// Gets the immutable action identifier.
    /// </summary>
    public ActionId ActionId { get; }

    /// <summary>
    /// Gets the organism associated with the action.
    /// </summary>
    public OrganismId OrganismId { get; }

    /// <summary>
    /// Gets the deterministic action type.
    /// </summary>
    public SimulationActionType ActionType { get; }

    /// <summary>
    /// Gets the immutable target metadata.
    /// </summary>
    public SimulationActionTarget Target { get; }
}

using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Actions;

namespace GaiaEngine.Simulation.Events;

/// <summary>
/// Represents one deterministic action-cancelled simulation event.
/// </summary>
public sealed record ActionCancelledSimulationEvent : ActionEventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionCancelledSimulationEvent"/> class.
    /// </summary>
    public ActionCancelledSimulationEvent(
        EventId eventId,
        long tick,
        long timestamp,
        ActionId actionId,
        OrganismId organismId,
        SimulationActionType actionType,
        SimulationActionTarget target)
        : base(eventId, "BehaviourExecution", EventPriority.High, tick, timestamp, actionId, organismId, actionType, target)
    {
    }
}

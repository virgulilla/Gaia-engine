using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Actions;

namespace GaiaEngine.Simulation.Events;

/// <summary>
/// Represents one deterministic action-started simulation event.
/// </summary>
public sealed record ActionStartedSimulationEvent : ActionEventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionStartedSimulationEvent"/> class.
    /// </summary>
    public ActionStartedSimulationEvent(
        EventId eventId,
        long tick,
        long timestamp,
        ActionId actionId,
        OrganismId organismId,
        SimulationActionType actionType,
        SimulationActionTarget target)
        : base(eventId, "BehaviourExecution", EventPriority.Normal, tick, timestamp, actionId, organismId, actionType, target)
    {
    }
}

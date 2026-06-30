using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Actions;

namespace GaiaEngine.Simulation.Events;

/// <summary>
/// Represents one deterministic action-failed simulation event.
/// </summary>
public sealed record ActionFailedSimulationEvent : ActionEventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionFailedSimulationEvent"/> class.
    /// </summary>
    public ActionFailedSimulationEvent(
        EventId eventId,
        long tick,
        long timestamp,
        ActionId actionId,
        OrganismId organismId,
        SimulationActionType actionType,
        SimulationActionTarget target)
        : base(eventId, "InteractionSystems", EventPriority.High, tick, timestamp, actionId, organismId, actionType, target)
    {
    }
}

using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Actions;

namespace GaiaEngine.Simulation.Events;

/// <summary>
/// Represents one deterministic action-completed simulation event.
/// </summary>
public sealed record ActionCompletedSimulationEvent : ActionEventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionCompletedSimulationEvent"/> class.
    /// </summary>
    public ActionCompletedSimulationEvent(
        EventId eventId,
        long tick,
        long timestamp,
        ActionId actionId,
        OrganismId organismId,
        SimulationActionType actionType,
        SimulationActionTarget target)
        : base(eventId, "InteractionSystems", EventPriority.Normal, tick, timestamp, actionId, organismId, actionType, target)
    {
    }
}

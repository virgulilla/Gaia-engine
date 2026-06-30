using System;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Simulation.Actions;

namespace GaiaEngine.Simulation.Events;

/// <summary>
/// Represents the immutable data required to publish one deterministic action lifecycle event.
/// </summary>
public sealed record ActionEventDescriptor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionEventDescriptor"/> class.
    /// </summary>
    /// <param name="actionId">The immutable action identifier.</param>
    /// <param name="organismId">The organism identifier associated with the action.</param>
    /// <param name="actionType">The deterministic action type.</param>
    /// <param name="target">The immutable action target metadata.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is <see langword="null"/>.</exception>
    public ActionEventDescriptor(
        ActionId actionId,
        OrganismId organismId,
        SimulationActionType actionType,
        SimulationActionTarget target)
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
    /// Gets the organism identifier associated with the action.
    /// </summary>
    public OrganismId OrganismId { get; }

    /// <summary>
    /// Gets the deterministic action type.
    /// </summary>
    public SimulationActionType ActionType { get; }

    /// <summary>
    /// Gets the immutable action target metadata.
    /// </summary>
    public SimulationActionTarget Target { get; }
}

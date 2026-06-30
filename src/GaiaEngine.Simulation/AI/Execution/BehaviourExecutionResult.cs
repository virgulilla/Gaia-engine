using System;
using System.Collections.Generic;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.Events;

namespace GaiaEngine.Simulation.AI.Execution;

/// <summary>
/// Represents the deterministic result produced by behaviour execution request translation.
/// </summary>
public sealed class BehaviourExecutionResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BehaviourExecutionResult"/> class.
    /// </summary>
    /// <param name="actionRequests">The updated common action request collection.</param>
    /// <param name="startedActions">The actions that were newly queued for execution.</param>
    /// <param name="cancelledActions">The actions that were cancelled before replacement.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="actionRequests"/>, <paramref name="startedActions"/>, or <paramref name="cancelledActions"/> is <see langword="null"/>.
    /// </exception>
    public BehaviourExecutionResult(
        SimulationActionRequestCollection actionRequests,
        IReadOnlyList<ActionEventDescriptor> startedActions,
        IReadOnlyList<ActionEventDescriptor> cancelledActions)
    {
        ActionRequests = actionRequests ?? throw new ArgumentNullException(nameof(actionRequests));
        StartedActions = startedActions ?? throw new ArgumentNullException(nameof(startedActions));
        CancelledActions = cancelledActions ?? throw new ArgumentNullException(nameof(cancelledActions));
    }

    /// <summary>
    /// Gets the updated common action request collection.
    /// </summary>
    public SimulationActionRequestCollection ActionRequests { get; }

    /// <summary>
    /// Gets the actions that were newly queued for execution.
    /// </summary>
    public IReadOnlyList<ActionEventDescriptor> StartedActions { get; }

    /// <summary>
    /// Gets the actions that were cancelled before replacement.
    /// </summary>
    public IReadOnlyList<ActionEventDescriptor> CancelledActions { get; }
}

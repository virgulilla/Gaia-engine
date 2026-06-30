using System;
using GaiaEngine.Simulation.Actions;

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
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="actionRequests"/> is <see langword="null"/>.</exception>
    public BehaviourExecutionResult(SimulationActionRequestCollection actionRequests)
    {
        ActionRequests = actionRequests ?? throw new ArgumentNullException(nameof(actionRequests));
    }

    /// <summary>
    /// Gets the updated common action request collection.
    /// </summary>
    public SimulationActionRequestCollection ActionRequests { get; }
}

using System;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Simulation.Actions;

/// <summary>
/// Represents one immutable deterministic action execution request.
/// </summary>
public sealed record SimulationActionRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationActionRequest"/> class.
    /// </summary>
    /// <param name="actionId">The immutable action identifier.</param>
    /// <param name="organismId">The organism that owns the request.</param>
    /// <param name="actionType">The action type.</param>
    /// <param name="target">The immutable target metadata.</param>
    /// <param name="startTick">The tick when the request becomes executable.</param>
    /// <param name="expectedDuration">The expected duration in ticks.</param>
    /// <param name="priority">The deterministic execution priority.</param>
    /// <param name="status">The current execution state.</param>
    /// <param name="interruptible">A value indicating whether the request can be interrupted.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="startTick"/>, <paramref name="expectedDuration"/>, or <paramref name="priority"/> is negative.
    /// </exception>
    public SimulationActionRequest(
        ActionId actionId,
        OrganismId organismId,
        SimulationActionType actionType,
        SimulationActionTarget target,
        long startTick,
        int expectedDuration,
        int priority,
        ActionExecutionState status,
        bool interruptible)
    {
        if (startTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startTick), "The action request start tick must be zero or greater.");
        }

        if (expectedDuration < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(expectedDuration), "The action request expected duration must be zero or greater.");
        }

        if (priority < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(priority), "The action request priority must be zero or greater.");
        }

        ActionId = actionId;
        OrganismId = organismId;
        ActionType = actionType;
        Target = target;
        StartTick = startTick;
        ExpectedDuration = expectedDuration;
        Priority = priority;
        Status = status;
        Interruptible = interruptible;
    }

    /// <summary>
    /// Gets the immutable action identifier.
    /// </summary>
    public ActionId ActionId { get; }

    /// <summary>
    /// Gets the organism that owns the request.
    /// </summary>
    public OrganismId OrganismId { get; }

    /// <summary>
    /// Gets the action type.
    /// </summary>
    public SimulationActionType ActionType { get; }

    /// <summary>
    /// Gets the immutable target metadata.
    /// </summary>
    public SimulationActionTarget Target { get; }

    /// <summary>
    /// Gets the tick when the request becomes executable.
    /// </summary>
    public long StartTick { get; }

    /// <summary>
    /// Gets the expected duration in ticks.
    /// </summary>
    public int ExpectedDuration { get; }

    /// <summary>
    /// Gets the deterministic execution priority.
    /// </summary>
    public int Priority { get; }

    /// <summary>
    /// Gets the current execution state.
    /// </summary>
    public ActionExecutionState Status { get; }

    /// <summary>
    /// Gets a value indicating whether the request can be interrupted.
    /// </summary>
    public bool Interruptible { get; }
}

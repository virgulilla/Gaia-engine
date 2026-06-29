using System;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Simulation.Interactions.Movement;

/// <summary>
/// Represents one immutable deterministic movement execution request.
/// </summary>
public sealed record MovementRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MovementRequest"/> class.
    /// </summary>
    /// <param name="organismId">The organism to move.</param>
    /// <param name="targetChunkId">The destination chunk identifier.</param>
    /// <param name="startTick">The tick when the request becomes executable.</param>
    /// <param name="expectedDuration">The expected movement duration in ticks.</param>
    /// <param name="priority">The deterministic execution priority.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="startTick"/>, <paramref name="expectedDuration"/>, or <paramref name="priority"/> is negative.
    /// </exception>
    public MovementRequest(
        OrganismId organismId,
        ChunkId targetChunkId,
        long startTick,
        int expectedDuration,
        int priority)
    {
        if (startTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startTick), "The movement request start tick must be zero or greater.");
        }

        if (expectedDuration < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(expectedDuration), "The movement request expected duration must be zero or greater.");
        }

        if (priority < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(priority), "The movement request priority must be zero or greater.");
        }

        OrganismId = organismId;
        TargetChunkId = targetChunkId;
        StartTick = startTick;
        ExpectedDuration = expectedDuration;
        Priority = priority;
    }

    /// <summary>
    /// Gets the organism to move.
    /// </summary>
    public OrganismId OrganismId { get; }

    /// <summary>
    /// Gets the destination chunk identifier.
    /// </summary>
    public ChunkId TargetChunkId { get; }

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
}

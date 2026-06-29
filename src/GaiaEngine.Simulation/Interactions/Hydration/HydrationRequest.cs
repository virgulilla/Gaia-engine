using System;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Simulation.Interactions.Hydration;

/// <summary>
/// Represents one immutable deterministic hydration execution request.
/// </summary>
public sealed record HydrationRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HydrationRequest"/> class.
    /// </summary>
    /// <param name="organismId">The organism that will drink.</param>
    /// <param name="targetChunkId">The chunk containing the targeted water resource.</param>
    /// <param name="startTick">The tick when the request becomes executable.</param>
    /// <param name="expectedDuration">The expected hydration duration in ticks.</param>
    /// <param name="priority">The deterministic execution priority.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="startTick"/>, <paramref name="expectedDuration"/>, or <paramref name="priority"/> is negative.
    /// </exception>
    public HydrationRequest(
        OrganismId organismId,
        ChunkId targetChunkId,
        long startTick,
        int expectedDuration,
        int priority)
    {
        if (startTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(startTick), "The hydration request start tick must be zero or greater.");
        }

        if (expectedDuration < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(expectedDuration), "The hydration request expected duration must be zero or greater.");
        }

        if (priority < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(priority), "The hydration request priority must be zero or greater.");
        }

        OrganismId = organismId;
        TargetChunkId = targetChunkId;
        StartTick = startTick;
        ExpectedDuration = expectedDuration;
        Priority = priority;
    }

    /// <summary>
    /// Gets the organism that will drink.
    /// </summary>
    public OrganismId OrganismId { get; }

    /// <summary>
    /// Gets the targeted chunk identifier.
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

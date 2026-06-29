using System;
using System.Collections.Generic;

namespace GaiaEngine.Simulation.Interactions.Movement;

/// <summary>
/// Represents a deterministic collection of immutable movement requests.
/// </summary>
public sealed class MovementRequestCollection
{
    private readonly IReadOnlyList<MovementRequest> orderedRequests;

    /// <summary>
    /// Initializes a new instance of the <see cref="MovementRequestCollection"/> class.
    /// </summary>
    /// <param name="requests">The requests stored by the collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="requests"/> is <see langword="null"/>.</exception>
    public MovementRequestCollection(IReadOnlyList<MovementRequest> requests)
    {
        ArgumentNullException.ThrowIfNull(requests);

        List<MovementRequest> ordered = new(requests.Count);
        foreach (MovementRequest request in requests)
        {
            ordered.Add(request ?? throw new ArgumentNullException(nameof(requests), "The movement request collection cannot contain null requests."));
        }

        ordered.Sort(CompareRequests);
        orderedRequests = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty movement request collection.
    /// </summary>
    public static MovementRequestCollection Empty { get; } = new(Array.Empty<MovementRequest>());

    /// <summary>
    /// Gets the number of requests stored in the collection.
    /// </summary>
    public int Count => orderedRequests.Count;

    /// <summary>
    /// Returns the stored requests in deterministic execution order.
    /// </summary>
    /// <returns>The stored requests in deterministic execution order.</returns>
    public IReadOnlyList<MovementRequest> GetAll()
    {
        return orderedRequests;
    }

    private static int CompareRequests(MovementRequest left, MovementRequest right)
    {
        int startTickComparison = left.StartTick.CompareTo(right.StartTick);
        if (startTickComparison != 0)
        {
            return startTickComparison;
        }

        int priorityComparison = left.Priority.CompareTo(right.Priority);
        if (priorityComparison != 0)
        {
            return priorityComparison;
        }

        int organismComparison = left.OrganismId.Value.CompareTo(right.OrganismId.Value);
        if (organismComparison != 0)
        {
            return organismComparison;
        }

        return left.TargetChunkId.Value.CompareTo(right.TargetChunkId.Value);
    }
}

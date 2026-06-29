using System;
using System.Collections.Generic;

namespace GaiaEngine.Simulation.Interactions.Feeding;

/// <summary>
/// Represents a deterministic collection of immutable feeding requests.
/// </summary>
public sealed class FeedingRequestCollection
{
    private readonly IReadOnlyList<FeedingRequest> orderedRequests;

    /// <summary>
    /// Initializes a new instance of the <see cref="FeedingRequestCollection"/> class.
    /// </summary>
    /// <param name="requests">The requests stored by the collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="requests"/> is <see langword="null"/>.</exception>
    public FeedingRequestCollection(IReadOnlyList<FeedingRequest> requests)
    {
        ArgumentNullException.ThrowIfNull(requests);

        List<FeedingRequest> ordered = new(requests.Count);
        foreach (FeedingRequest request in requests)
        {
            ordered.Add(request ?? throw new ArgumentNullException(nameof(requests), "The feeding request collection cannot contain null requests."));
        }

        ordered.Sort(CompareRequests);
        orderedRequests = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty feeding request collection.
    /// </summary>
    public static FeedingRequestCollection Empty { get; } = new(Array.Empty<FeedingRequest>());

    /// <summary>
    /// Gets the number of requests stored in the collection.
    /// </summary>
    public int Count => orderedRequests.Count;

    /// <summary>
    /// Returns the stored requests in deterministic execution order.
    /// </summary>
    /// <returns>The stored requests in deterministic execution order.</returns>
    public IReadOnlyList<FeedingRequest> GetAll()
    {
        return orderedRequests;
    }

    private static int CompareRequests(FeedingRequest left, FeedingRequest right)
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

using System;
using System.Collections.Generic;

namespace GaiaEngine.Simulation.Interactions.Hydration;

/// <summary>
/// Represents a deterministic collection of immutable hydration requests.
/// </summary>
public sealed class HydrationRequestCollection
{
    private readonly IReadOnlyList<HydrationRequest> orderedRequests;

    /// <summary>
    /// Initializes a new instance of the <see cref="HydrationRequestCollection"/> class.
    /// </summary>
    /// <param name="requests">The requests stored by the collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="requests"/> is <see langword="null"/>.</exception>
    public HydrationRequestCollection(IReadOnlyList<HydrationRequest> requests)
    {
        ArgumentNullException.ThrowIfNull(requests);

        List<HydrationRequest> ordered = new(requests.Count);
        foreach (HydrationRequest request in requests)
        {
            ordered.Add(request ?? throw new ArgumentNullException(nameof(requests), "The hydration request collection cannot contain null requests."));
        }

        ordered.Sort(CompareRequests);
        orderedRequests = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty hydration request collection.
    /// </summary>
    public static HydrationRequestCollection Empty { get; } = new(Array.Empty<HydrationRequest>());

    /// <summary>
    /// Gets the number of requests stored in the collection.
    /// </summary>
    public int Count => orderedRequests.Count;

    /// <summary>
    /// Returns the stored requests in deterministic execution order.
    /// </summary>
    /// <returns>The stored requests in deterministic execution order.</returns>
    public IReadOnlyList<HydrationRequest> GetAll()
    {
        return orderedRequests;
    }

    private static int CompareRequests(HydrationRequest left, HydrationRequest right)
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

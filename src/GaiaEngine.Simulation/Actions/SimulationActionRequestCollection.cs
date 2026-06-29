using System;
using System.Collections.Generic;

namespace GaiaEngine.Simulation.Actions;

/// <summary>
/// Represents a deterministic collection of immutable action execution requests.
/// </summary>
public sealed class SimulationActionRequestCollection
{
    private readonly IReadOnlyList<SimulationActionRequest> orderedRequests;

    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationActionRequestCollection"/> class.
    /// </summary>
    /// <param name="requests">The requests stored by the collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="requests"/> is <see langword="null"/>.</exception>
    public SimulationActionRequestCollection(IReadOnlyList<SimulationActionRequest> requests)
    {
        ArgumentNullException.ThrowIfNull(requests);

        List<SimulationActionRequest> ordered = new(requests.Count);
        foreach (SimulationActionRequest request in requests)
        {
            ordered.Add(request ?? throw new ArgumentNullException(nameof(requests), "The action request collection cannot contain null requests."));
        }

        ordered.Sort(CompareRequests);
        orderedRequests = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty action request collection.
    /// </summary>
    public static SimulationActionRequestCollection Empty { get; } = new(Array.Empty<SimulationActionRequest>());

    /// <summary>
    /// Gets the number of requests stored in the collection.
    /// </summary>
    public int Count => orderedRequests.Count;

    /// <summary>
    /// Returns the stored requests in deterministic execution order.
    /// </summary>
    /// <returns>The stored requests in deterministic execution order.</returns>
    public IReadOnlyList<SimulationActionRequest> GetAll()
    {
        return orderedRequests;
    }

    private static int CompareRequests(SimulationActionRequest left, SimulationActionRequest right)
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

        return left.ActionId.Value.CompareTo(right.ActionId.Value);
    }
}

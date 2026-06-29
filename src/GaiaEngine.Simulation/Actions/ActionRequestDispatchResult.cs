using System;
using GaiaEngine.Simulation.Interactions.Feeding;
using GaiaEngine.Simulation.Interactions.Hydration;
using GaiaEngine.Simulation.Interactions.Movement;

namespace GaiaEngine.Simulation.Actions;

/// <summary>
/// Represents the deterministic result produced by dispatching common action requests to specialized systems.
/// </summary>
public sealed record ActionRequestDispatchResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionRequestDispatchResult"/> class.
    /// </summary>
    public ActionRequestDispatchResult(
        SimulationActionRequestCollection deferredRequests,
        MovementRequestCollection movementRequests,
        FeedingRequestCollection feedingRequests,
        HydrationRequestCollection hydrationRequests)
    {
        DeferredRequests = deferredRequests ?? throw new ArgumentNullException(nameof(deferredRequests));
        MovementRequests = movementRequests ?? throw new ArgumentNullException(nameof(movementRequests));
        FeedingRequests = feedingRequests ?? throw new ArgumentNullException(nameof(feedingRequests));
        HydrationRequests = hydrationRequests ?? throw new ArgumentNullException(nameof(hydrationRequests));
    }

    /// <summary>
    /// Gets the requests that remain deferred in common form.
    /// </summary>
    public SimulationActionRequestCollection DeferredRequests { get; }

    /// <summary>
    /// Gets the dispatched movement requests.
    /// </summary>
    public MovementRequestCollection MovementRequests { get; }

    /// <summary>
    /// Gets the dispatched feeding requests.
    /// </summary>
    public FeedingRequestCollection FeedingRequests { get; }

    /// <summary>
    /// Gets the dispatched hydration requests.
    /// </summary>
    public HydrationRequestCollection HydrationRequests { get; }
}

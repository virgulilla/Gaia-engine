using GaiaEngine.Simulation.Interactions.Feeding;
using GaiaEngine.Simulation.Interactions.Hydration;
using GaiaEngine.Simulation.Interactions.Movement;

namespace GaiaEngine.Simulation.Actions;

/// <summary>
/// Dispatches common deterministic action requests to specialized simulation request collections.
/// </summary>
public interface IActionRequestDispatcher
{
    /// <summary>
    /// Dispatches common deterministic action requests to specialized system request collections.
    /// </summary>
    /// <param name="actionRequests">The current common action requests.</param>
    /// <returns>The deterministic dispatch result.</returns>
    public ActionRequestDispatchResult Dispatch(SimulationActionRequestCollection actionRequests);

    /// <summary>
    /// Rebuilds a common action request collection from specialized system request collections and deferred requests.
    /// </summary>
    /// <param name="deferredRequests">The common requests that stayed deferred.</param>
    /// <param name="movementRequests">The remaining movement requests.</param>
    /// <param name="feedingRequests">The remaining feeding requests.</param>
    /// <param name="hydrationRequests">The remaining hydration requests.</param>
    /// <returns>The rebuilt deterministic common request collection.</returns>
    public SimulationActionRequestCollection Rebuild(
        SimulationActionRequestCollection deferredRequests,
        MovementRequestCollection movementRequests,
        FeedingRequestCollection feedingRequests,
        HydrationRequestCollection hydrationRequests);
}

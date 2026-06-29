using GaiaEngine.Domain.Organisms;

namespace GaiaEngine.Simulation.Interactions.Feeding;

/// <summary>
/// Executes deterministic feeding requests during the interaction phase.
/// </summary>
public interface IFeedingSystem
{
    /// <summary>
    /// Executes deterministic feeding requests against the supplied world and organism state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="requests">The current feeding requests.</param>
    /// <returns>The deterministic feeding execution result.</returns>
    public FeedingSystemResult Execute(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        FeedingRequestCollection requests);
}

using GaiaEngine.Domain.Organisms;

namespace GaiaEngine.Simulation.Interactions.Hydration;

/// <summary>
/// Executes deterministic hydration requests during the interaction phase.
/// </summary>
public interface IHydrationSystem
{
    /// <summary>
    /// Executes deterministic hydration requests against the supplied world and organism state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="requests">The current hydration requests.</param>
    /// <returns>The deterministic hydration execution result.</returns>
    public HydrationSystemResult Execute(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        HydrationRequestCollection requests);
}

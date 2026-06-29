using GaiaEngine.Domain.Organisms;

namespace GaiaEngine.Simulation.Interactions.Movement;

/// <summary>
/// Executes deterministic movement requests during the interaction phase.
/// </summary>
public interface IMovementSystem
{
    /// <summary>
    /// Executes deterministic movement requests against the supplied world and organism state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="requests">The current movement requests.</param>
    /// <returns>The deterministic movement execution result.</returns>
    public MovementSystemResult Execute(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        MovementRequestCollection requests);
}

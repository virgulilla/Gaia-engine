using GaiaEngine.Domain.Organisms;
using GaiaEngine.Simulation.Actions;

namespace GaiaEngine.Simulation.AI.Execution;

/// <summary>
/// Coordinates deterministic organism AI evaluation and translates the selected results into action requests.
/// </summary>
public interface IAutonomousBehaviourSystem
{
    /// <summary>
    /// Updates the current action request collection from the current world and organism state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="actionRequests">The current action request collection.</param>
    /// <returns>The updated deterministic action request collection.</returns>
    public SimulationActionRequestCollection Update(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        SimulationActionRequestCollection actionRequests);
}

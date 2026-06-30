using GaiaEngine.Domain.AI;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.AI.Perception;

namespace GaiaEngine.Simulation.AI.Utility;

/// <summary>
/// Evaluates deterministic utility scores for the currently supported candidate actions.
/// </summary>
public interface IUtilityEvaluationSystem
{
    /// <summary>
    /// Evaluates candidate action utilities for one organism based on current perception and world state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="memories">The current memory state.</param>
    /// <param name="perception">The current perception output for the evaluated organism.</param>
    /// <param name="organismId">The evaluated organism identifier.</param>
    /// <returns>The deterministic utility evaluation result.</returns>
    public UtilityEvaluationResult Evaluate(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        MemoryCollection memories,
        PerceptionResult perception,
        OrganismId organismId);
}

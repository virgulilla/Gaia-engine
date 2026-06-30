using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.AI.Utility;

namespace GaiaEngine.Simulation.AI.Decision;

/// <summary>
/// Selects exactly one deterministic action from utility-evaluated candidates.
/// </summary>
public interface IDecisionMakingSystem
{
    /// <summary>
    /// Selects one deterministic decision for the supplied organism.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="utilityResult">The utility evaluation result to resolve.</param>
    /// <param name="organismId">The evaluated organism identifier.</param>
    /// <returns>The selected deterministic decision.</returns>
    public SelectedDecision Select(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        UtilityEvaluationResult utilityResult,
        OrganismId organismId);
}

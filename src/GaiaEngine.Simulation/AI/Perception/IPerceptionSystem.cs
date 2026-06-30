using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.AI.Perception;

/// <summary>
/// Generates deterministic observations for one organism without storing persistent state.
/// </summary>
public interface IPerceptionSystem
{
    /// <summary>
    /// Evaluates the current world state from the perspective of one organism.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="observerId">The observing organism identifier.</param>
    /// <returns>The generated deterministic observations.</returns>
    public PerceptionResult Evaluate(GaiaEngine.Domain.World.World world, OrganismCollection organisms, OrganismId observerId);
}

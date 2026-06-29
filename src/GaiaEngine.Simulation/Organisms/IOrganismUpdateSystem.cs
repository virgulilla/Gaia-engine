using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.Organisms;

/// <summary>
/// Updates deterministic organism state during the organism phase of the simulation pipeline.
/// </summary>
public interface IOrganismUpdateSystem
{
    /// <summary>
    /// Updates the supplied organism state deterministically.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <returns>The updated organism state.</returns>
    public OrganismCollection Update(GaiaEngine.Domain.World.World world, OrganismCollection organisms);
}

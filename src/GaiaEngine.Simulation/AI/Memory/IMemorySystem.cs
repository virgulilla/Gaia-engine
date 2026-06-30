using GaiaEngine.Domain.AI;
using GaiaEngine.Domain.Organisms;

namespace GaiaEngine.Simulation.AI.Memory;

/// <summary>
/// Updates deterministic organism memories from perceived observations.
/// </summary>
public interface IMemorySystem
{
    /// <summary>
    /// Updates the current memory collection from the current world and organism state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="memories">The current organism memory state.</param>
    /// <returns>The updated deterministic memory collection.</returns>
    public MemoryCollection Update(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        MemoryCollection memories);
}

using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Organisms;

namespace GaiaEngine.Simulation.Genetics;

/// <summary>
/// Updates deterministic species lifecycle state using the current organism population.
/// </summary>
public interface ISpeciesLifecycleSystem
{
    /// <summary>
    /// Updates the supplied species state deterministically.
    /// </summary>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="species">The current species state.</param>
    /// <param name="currentTick">The current deterministic simulation tick.</param>
    /// <returns>The updated species state.</returns>
    public SpeciesCollection Update(OrganismCollection organisms, SpeciesCollection species, long currentTick);
}

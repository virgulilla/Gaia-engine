using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.Genetics;

/// <summary>
/// Recognizes new deterministic species classifications from the current population state.
/// </summary>
public interface ISpeciesRecognitionSystem
{
    /// <summary>
    /// Evaluates the supplied population and produces updated organism classifications and species lineage state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="genomes">The current genome state.</param>
    /// <param name="species">The current species state.</param>
    /// <returns>The updated recognition result.</returns>
    public SpeciesRecognitionResult Update(GaiaEngine.Domain.World.World world, OrganismCollection organisms, GenomeCollection genomes, SpeciesCollection species);
}

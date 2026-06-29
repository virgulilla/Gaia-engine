using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Simulation.Genetics;

/// <summary>
/// Combines two parent genomes into one deterministic offspring genome.
/// </summary>
public interface IGenomeInheritanceSystem
{
    /// <summary>
    /// Creates one deterministic offspring genome from two parent genomes.
    /// </summary>
    /// <param name="parentA">The first parent genome candidate.</param>
    /// <param name="parentB">The second parent genome candidate.</param>
    /// <param name="offspringGenomeId">The new genome identifier to assign to the offspring.</param>
    /// <returns>The combined offspring genome.</returns>
    public Genome Inherit(Genome parentA, Genome parentB, GenomeId offspringGenomeId);
}

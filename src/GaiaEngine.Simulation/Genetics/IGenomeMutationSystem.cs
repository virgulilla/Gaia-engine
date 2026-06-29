using GaiaEngine.Domain.Genetics;
using GaiaEngine.Foundation.Determinism;

namespace GaiaEngine.Simulation.Genetics;

/// <summary>
/// Applies deterministic mutations to a newly inherited genome.
/// </summary>
public interface IGenomeMutationSystem
{
    /// <summary>
    /// Applies deterministic mutations to a newly inherited genome.
    /// </summary>
    /// <param name="genome">The genome produced by inheritance.</param>
    /// <param name="worldSeed">The world seed that drives deterministic mutation decisions.</param>
    /// <returns>The mutated genome.</returns>
    public Genome Mutate(Genome genome, WorldSeed worldSeed);
}

using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Determinism;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Creates deterministic bootstrap genomes for the initial population.
/// </summary>
public interface IGenomeBootstrapFactory
{
    /// <summary>
    /// Creates one deterministic genome for the supplied bootstrap context.
    /// </summary>
    /// <param name="worldSeed">The owning world seed.</param>
    /// <param name="chunk">The organism spawn chunk.</param>
    /// <param name="index">The deterministic bootstrap organism index.</param>
    /// <returns>The created immutable genome.</returns>
    public Genome CreateGenome(WorldSeed worldSeed, Chunk chunk, int index);
}

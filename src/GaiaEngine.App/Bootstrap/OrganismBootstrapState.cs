using System;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Organisms;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Represents the deterministic bootstrap state produced for the organism module.
/// </summary>
public sealed record OrganismBootstrapState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrganismBootstrapState"/> class.
    /// </summary>
    /// <param name="world">The world updated with organism references.</param>
    /// <param name="organisms">The initial organism collection.</param>
    /// <param name="genomes">The initial genome collection.</param>
    /// <param name="species">The initial species collection.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/>, <paramref name="organisms"/>, <paramref name="genomes"/>, or <paramref name="species"/> is <see langword="null"/>.
    /// </exception>
    public OrganismBootstrapState(GaiaEngine.Domain.World.World world, OrganismCollection organisms, GenomeCollection genomes, SpeciesCollection species)
    {
        World = world ?? throw new ArgumentNullException(nameof(world));
        Organisms = organisms ?? throw new ArgumentNullException(nameof(organisms));
        Genomes = genomes ?? throw new ArgumentNullException(nameof(genomes));
        Species = species ?? throw new ArgumentNullException(nameof(species));
    }

    /// <summary>
    /// Gets the world updated with organism references.
    /// </summary>
    public GaiaEngine.Domain.World.World World { get; }

    /// <summary>
    /// Gets the initial organism collection.
    /// </summary>
    public OrganismCollection Organisms { get; }

    /// <summary>
    /// Gets the initial genome collection.
    /// </summary>
    public GenomeCollection Genomes { get; }

    /// <summary>
    /// Gets the initial species collection.
    /// </summary>
    public SpeciesCollection Species { get; }
}

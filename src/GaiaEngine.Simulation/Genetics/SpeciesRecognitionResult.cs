using System;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Organisms;

namespace GaiaEngine.Simulation.Genetics;

/// <summary>
/// Represents the deterministic result produced by one species recognition evaluation.
/// </summary>
public sealed record SpeciesRecognitionResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpeciesRecognitionResult"/> class.
    /// </summary>
    /// <param name="organisms">The updated organism classification state.</param>
    /// <param name="species">The updated species lineage state.</param>
    public SpeciesRecognitionResult(OrganismCollection organisms, SpeciesCollection species)
    {
        Organisms = organisms ?? throw new ArgumentNullException(nameof(organisms));
        Species = species ?? throw new ArgumentNullException(nameof(species));
    }

    /// <summary>
    /// Gets the updated organism classification state.
    /// </summary>
    public OrganismCollection Organisms { get; }

    /// <summary>
    /// Gets the updated species lineage state.
    /// </summary>
    public SpeciesCollection Species { get; }
}

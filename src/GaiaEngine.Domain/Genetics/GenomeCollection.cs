using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Stores the immutable deterministic set of genomes available in the current runtime slice.
/// </summary>
public sealed class GenomeCollection
{
    private readonly IReadOnlyList<Genome> orderedGenomes;
    private readonly Dictionary<GenomeId, Genome> genomesById;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenomeCollection"/> class.
    /// </summary>
    /// <param name="genomes">The genomes to store.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="genomes"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when duplicated genome identifiers are detected.</exception>
    public GenomeCollection(IReadOnlyList<Genome> genomes)
    {
        ArgumentNullException.ThrowIfNull(genomes);

        List<Genome> ordered = new(genomes.Count);
        genomesById = new Dictionary<GenomeId, Genome>(genomes.Count);
        foreach (Genome genome in genomes)
        {
            ArgumentNullException.ThrowIfNull(genome);
            if (!genomesById.TryAdd(genome.Id, genome))
            {
                throw new ArgumentException($"The genome '{genome.Id}' is duplicated.", nameof(genomes));
            }

            ordered.Add(genome);
        }

        ordered.Sort(static (left, right) => left.Id.Value.CompareTo(right.Id.Value));
        orderedGenomes = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty genome collection instance.
    /// </summary>
    public static GenomeCollection Empty { get; } = new(Array.Empty<Genome>());

    /// <summary>
    /// Gets the number of stored genomes.
    /// </summary>
    public int Count => orderedGenomes.Count;

    /// <summary>
    /// Gets all stored genomes in deterministic order.
    /// </summary>
    /// <returns>The ordered genome set.</returns>
    public IReadOnlyList<Genome> GetAll()
    {
        return orderedGenomes;
    }

    /// <summary>
    /// Attempts to resolve one genome by identifier.
    /// </summary>
    /// <param name="genomeId">The genome identifier to resolve.</param>
    /// <param name="genome">The resolved genome when found; otherwise <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when the genome exists; otherwise <see langword="false"/>.</returns>
    public bool TryGet(GenomeId genomeId, out Genome? genome)
    {
        return genomesById.TryGetValue(genomeId, out genome);
    }
}

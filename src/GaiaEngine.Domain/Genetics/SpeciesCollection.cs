using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Stores the immutable deterministic set of species available in the current runtime slice.
/// </summary>
public sealed class SpeciesCollection
{
    private readonly IReadOnlyList<Species> orderedSpecies;
    private readonly Dictionary<SpeciesId, Species> speciesById;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpeciesCollection"/> class.
    /// </summary>
    /// <param name="species">The species records to store.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="species"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when duplicated species identifiers are detected.</exception>
    public SpeciesCollection(IReadOnlyList<Species> species)
    {
        ArgumentNullException.ThrowIfNull(species);

        List<Species> ordered = new(species.Count);
        speciesById = new Dictionary<SpeciesId, Species>(species.Count);
        foreach (Species entry in species)
        {
            ArgumentNullException.ThrowIfNull(entry);
            if (!speciesById.TryAdd(entry.Id, entry))
            {
                throw new ArgumentException($"The species '{entry.Id}' is duplicated.", nameof(species));
            }

            ordered.Add(entry);
        }

        ordered.Sort(static (left, right) => left.Id.Value.CompareTo(right.Id.Value));
        orderedSpecies = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty species collection instance.
    /// </summary>
    public static SpeciesCollection Empty { get; } = new(Array.Empty<Species>());

    /// <summary>
    /// Gets the number of stored species records.
    /// </summary>
    public int Count => orderedSpecies.Count;

    /// <summary>
    /// Gets all stored species in deterministic order.
    /// </summary>
    /// <returns>The ordered species set.</returns>
    public IReadOnlyList<Species> GetAll()
    {
        return orderedSpecies;
    }

    /// <summary>
    /// Attempts to resolve one species by identifier.
    /// </summary>
    /// <param name="speciesId">The species identifier to resolve.</param>
    /// <param name="species">The resolved species when found; otherwise <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when the species exists; otherwise <see langword="false"/>.</returns>
    public bool TryGet(SpeciesId speciesId, out Species? species)
    {
        return speciesById.TryGetValue(speciesId, out species);
    }
}

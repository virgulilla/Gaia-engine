using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Domain.Organisms;

/// <summary>
/// Represents a deterministic collection of organism aggregates.
/// </summary>
public sealed class OrganismCollection
{
    private readonly Dictionary<OrganismId, Organism> organismsById;
    private readonly IReadOnlyList<Organism> orderedOrganisms;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganismCollection"/> class.
    /// </summary>
    /// <param name="organisms">The organisms contained in the collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="organisms"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when duplicate organism identifiers are supplied.</exception>
    public OrganismCollection(IReadOnlyList<Organism> organisms)
    {
        ArgumentNullException.ThrowIfNull(organisms);

        organismsById = new Dictionary<OrganismId, Organism>(organisms.Count);
        List<Organism> ordered = new(organisms.Count);
        foreach (Organism organism in organisms)
        {
            ArgumentNullException.ThrowIfNull(organism);
            if (!organismsById.TryAdd(organism.Id, organism))
            {
                throw new ArgumentException("Organism identifiers must be unique.", nameof(organisms));
            }

            ordered.Add(organism);
        }

        ordered.Sort(static (left, right) => left.Id.Value.CompareTo(right.Id.Value));
        orderedOrganisms = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty organism collection instance.
    /// </summary>
    public static OrganismCollection Empty { get; } = new(Array.Empty<Organism>());

    /// <summary>
    /// Gets the number of organisms stored in the collection.
    /// </summary>
    public int Count => organismsById.Count;

    /// <summary>
    /// Tries to resolve one organism by identifier.
    /// </summary>
    /// <param name="organismId">The organism identifier.</param>
    /// <param name="organism">The resolved organism when present.</param>
    /// <returns><see langword="true"/> when the organism exists; otherwise <see langword="false"/>.</returns>
    public bool TryGet(OrganismId organismId, out Organism? organism)
    {
        return organismsById.TryGetValue(organismId, out organism);
    }

    /// <summary>
    /// Gets one organism by identifier.
    /// </summary>
    /// <param name="organismId">The organism identifier.</param>
    /// <returns>The resolved organism.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the organism does not exist.</exception>
    public Organism Get(OrganismId organismId)
    {
        if (!TryGet(organismId, out Organism? organism))
        {
            throw new KeyNotFoundException("The requested organism does not exist in the collection.");
        }

        return organism!;
    }

    /// <summary>
    /// Returns every stored organism in deterministic identifier order.
    /// </summary>
    /// <returns>The stored organisms in deterministic identifier order.</returns>
    public IReadOnlyList<Organism> GetAll()
    {
        return orderedOrganisms;
    }
}

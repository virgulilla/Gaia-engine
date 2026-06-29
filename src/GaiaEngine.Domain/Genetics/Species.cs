using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Represents one deterministic species classification record.
/// </summary>
public sealed class Species
{
    private readonly IReadOnlyList<OrganismId> founderPopulation;

    /// <summary>
    /// Initializes a new instance of the <see cref="Species"/> class.
    /// </summary>
    /// <param name="speciesId">The immutable species identifier.</param>
    /// <param name="parentSpeciesId">The parent species identifier when the species emerged from another lineage.</param>
    /// <param name="originTick">The deterministic tick when the species was recognized.</param>
    /// <param name="extinctionTick">The deterministic tick when the species became extinct, or <see langword="null"/> when still living.</param>
    /// <param name="founderPopulation">The founder organism identifiers recorded for lineage tracking.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="founderPopulation"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when the founder population contains duplicates.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when tick values are invalid.</exception>
    public Species(
        SpeciesId speciesId,
        SpeciesId? parentSpeciesId,
        long originTick,
        long? extinctionTick,
        IReadOnlyList<OrganismId> founderPopulation)
    {
        if (originTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(originTick), "The origin tick must be zero or greater.");
        }

        if (extinctionTick.HasValue && extinctionTick.Value < originTick)
        {
            throw new ArgumentOutOfRangeException(nameof(extinctionTick), "The extinction tick cannot be lower than the origin tick.");
        }

        ArgumentNullException.ThrowIfNull(founderPopulation);

        HashSet<OrganismId> founderIds = new(founderPopulation.Count);
        List<OrganismId> orderedFounders = new(founderPopulation.Count);
        foreach (OrganismId founderId in founderPopulation)
        {
            if (!founderIds.Add(founderId))
            {
                throw new ArgumentException($"The founder organism '{founderId}' is duplicated.", nameof(founderPopulation));
            }

            orderedFounders.Add(founderId);
        }

        orderedFounders.Sort(static (left, right) => left.Value.CompareTo(right.Value));

        Id = speciesId;
        ParentSpeciesId = parentSpeciesId;
        OriginTick = originTick;
        ExtinctionTick = extinctionTick;
        this.founderPopulation = orderedFounders.AsReadOnly();
    }

    /// <summary>
    /// Gets the immutable species identifier.
    /// </summary>
    public SpeciesId Id { get; }

    /// <summary>
    /// Gets the parent species identifier when lineage is known.
    /// </summary>
    public SpeciesId? ParentSpeciesId { get; }

    /// <summary>
    /// Gets the deterministic tick when the species was recognized.
    /// </summary>
    public long OriginTick { get; }

    /// <summary>
    /// Gets the deterministic extinction tick when present.
    /// </summary>
    public long? ExtinctionTick { get; }

    /// <summary>
    /// Gets a value indicating whether the species is extinct.
    /// </summary>
    public bool IsExtinct => ExtinctionTick.HasValue;

    /// <summary>
    /// Gets the founder organism identifiers in deterministic order.
    /// </summary>
    /// <returns>The founder organism identifiers.</returns>
    public IReadOnlyList<OrganismId> GetFounderPopulation()
    {
        return founderPopulation;
    }
}

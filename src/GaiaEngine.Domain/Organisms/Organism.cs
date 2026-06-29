using System;
using GaiaEngine.Domain.Entities;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Domain.Organisms;

/// <summary>
/// Represents one deterministic organism aggregate.
/// </summary>
public sealed class Organism : IEntity<OrganismId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Organism"/> class.
    /// </summary>
    /// <param name="organismId">The immutable organism identifier.</param>
    /// <param name="speciesId">The current species identifier.</param>
    /// <param name="genomeId">The immutable genome identifier.</param>
    /// <param name="currentChunkId">The current chunk identifier.</param>
    /// <param name="physiology">The stored physiology component.</param>
    /// <param name="needs">The stored needs component.</param>
    /// <param name="lifecycle">The stored lifecycle component.</param>
    /// <param name="health">The stored health component.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any supplied component is <see langword="null"/>.
    /// </exception>
    public Organism(
        OrganismId organismId,
        SpeciesId speciesId,
        GenomeId genomeId,
        ChunkId currentChunkId,
        PhysiologyComponent physiology,
        NeedsComponent needs,
        LifecycleComponent lifecycle,
        HealthComponent health)
    {
        Id = organismId;
        SpeciesId = speciesId;
        GenomeId = genomeId;
        CurrentChunkId = currentChunkId;
        Physiology = physiology ?? throw new ArgumentNullException(nameof(physiology));
        Needs = needs ?? throw new ArgumentNullException(nameof(needs));
        Lifecycle = lifecycle ?? throw new ArgumentNullException(nameof(lifecycle));
        Health = health ?? throw new ArgumentNullException(nameof(health));
    }

    /// <summary>
    /// Gets the immutable organism identifier.
    /// </summary>
    public OrganismId Id { get; }

    /// <summary>
    /// Gets the current species identifier.
    /// </summary>
    public SpeciesId SpeciesId { get; }

    /// <summary>
    /// Gets the immutable genome identifier.
    /// </summary>
    public GenomeId GenomeId { get; }

    /// <summary>
    /// Gets the current chunk identifier.
    /// </summary>
    public ChunkId CurrentChunkId { get; }

    /// <summary>
    /// Gets the stored physiology component.
    /// </summary>
    public PhysiologyComponent Physiology { get; }

    /// <summary>
    /// Gets the stored needs component.
    /// </summary>
    public NeedsComponent Needs { get; }

    /// <summary>
    /// Gets the stored lifecycle component.
    /// </summary>
    public LifecycleComponent Lifecycle { get; }

    /// <summary>
    /// Gets the stored health component.
    /// </summary>
    public HealthComponent Health { get; }
}

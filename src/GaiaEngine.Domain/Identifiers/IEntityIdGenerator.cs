namespace GaiaEngine.Domain.Identifiers;

/// <summary>
/// Generates deterministic persistent identifiers for engine entities.
/// </summary>
public interface IEntityIdGenerator
{
    /// <summary>
    /// Creates a world identifier.
    /// </summary>
    public WorldId CreateWorldId(IdentifierGenerationContext context);

    /// <summary>
    /// Creates a chunk identifier.
    /// </summary>
    public ChunkId CreateChunkId(IdentifierGenerationContext context);

    /// <summary>
    /// Creates an organism identifier.
    /// </summary>
    public OrganismId CreateOrganismId(IdentifierGenerationContext context);

    /// <summary>
    /// Creates a species identifier.
    /// </summary>
    public SpeciesId CreateSpeciesId(IdentifierGenerationContext context);

    /// <summary>
    /// Creates a genome identifier.
    /// </summary>
    public GenomeId CreateGenomeId(IdentifierGenerationContext context);

    /// <summary>
    /// Creates a resource identifier.
    /// </summary>
    public ResourceId CreateResourceId(IdentifierGenerationContext context);

    /// <summary>
    /// Creates a biome identifier.
    /// </summary>
    public BiomeId CreateBiomeId(IdentifierGenerationContext context);

    /// <summary>
    /// Creates an event identifier.
    /// </summary>
    public EventId CreateEventId(IdentifierGenerationContext context);
}

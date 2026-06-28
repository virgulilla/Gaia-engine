namespace GaiaEngine.Engine.Identifiers;

/// <summary>
/// Creates deterministic typed identifiers without relying on hidden mutable state.
/// </summary>
public sealed class DeterministicEntityIdGenerator : IEntityIdGenerator
{
    /// <summary>
    /// Creates a world identifier.
    /// </summary>
    public WorldId CreateWorldId(IdentifierGenerationContext context)
    {
        return WorldId.FromSequence(context.Sequence);
    }

    /// <summary>
    /// Creates a chunk identifier.
    /// </summary>
    public ChunkId CreateChunkId(IdentifierGenerationContext context)
    {
        return ChunkId.FromSequence(context.Sequence);
    }

    /// <summary>
    /// Creates an organism identifier.
    /// </summary>
    public OrganismId CreateOrganismId(IdentifierGenerationContext context)
    {
        return OrganismId.FromSequence(context.Sequence);
    }

    /// <summary>
    /// Creates a species identifier.
    /// </summary>
    public SpeciesId CreateSpeciesId(IdentifierGenerationContext context)
    {
        return SpeciesId.FromSequence(context.Sequence);
    }

    /// <summary>
    /// Creates a genome identifier.
    /// </summary>
    public GenomeId CreateGenomeId(IdentifierGenerationContext context)
    {
        return GenomeId.FromSequence(context.Sequence);
    }

    /// <summary>
    /// Creates a resource identifier.
    /// </summary>
    public ResourceId CreateResourceId(IdentifierGenerationContext context)
    {
        return ResourceId.FromSequence(context.Sequence);
    }

    /// <summary>
    /// Creates a biome identifier.
    /// </summary>
    public BiomeId CreateBiomeId(IdentifierGenerationContext context)
    {
        return BiomeId.FromSequence(context.Sequence);
    }

    /// <summary>
    /// Creates an event identifier.
    /// </summary>
    public EventId CreateEventId(IdentifierGenerationContext context)
    {
        return EventId.FromSequence(context.Sequence);
    }
}

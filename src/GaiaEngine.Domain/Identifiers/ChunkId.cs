namespace GaiaEngine.Domain.Identifiers;

/// <summary>
/// Represents the immutable identifier of a chunk entity.
/// </summary>
public readonly record struct ChunkId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChunkId"/> struct.
    /// </summary>
    /// <param name="value">The raw identifier value.</param>
    public ChunkId(ulong value)
    {
        IdentifierValue.ValidateCategory(value, IdentifierCategory.Chunk);
        Value = value;
    }

    /// <summary>
    /// Gets the raw identifier value.
    /// </summary>
    public ulong Value { get; }

    /// <summary>
    /// Creates a chunk identifier from a deterministic sequence.
    /// </summary>
    /// <param name="sequence">The deterministic sequence value.</param>
    /// <returns>The generated chunk identifier.</returns>
    public static ChunkId FromSequence(EntitySequence sequence)
    {
        return new ChunkId(IdentifierValue.Create(IdentifierCategory.Chunk, sequence));
    }

    /// <summary>
    /// Parses a serialized chunk identifier.
    /// </summary>
    /// <param name="value">The serialized identifier value.</param>
    /// <returns>The parsed identifier.</returns>
    public static ChunkId Parse(string value)
    {
        return new ChunkId(IdentifierValue.Parse(value, IdentifierCategory.Chunk));
    }

    /// <summary>
    /// Gets the deterministic sequence used to create the identifier.
    /// </summary>
    public EntitySequence Sequence => IdentifierValue.ExtractSequence(Value);

    /// <summary>
    /// Returns the serialized identifier value.
    /// </summary>
    /// <returns>The serialized identifier value.</returns>
    public override string ToString()
    {
        return IdentifierValue.Format(Value);
    }
}

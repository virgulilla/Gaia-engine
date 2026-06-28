namespace GaiaEngine.Domain.Identifiers;

/// <summary>
/// Represents the immutable identifier of a biome entity.
/// </summary>
public readonly record struct BiomeId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BiomeId"/> struct.
    /// </summary>
    /// <param name="value">The raw identifier value.</param>
    public BiomeId(ulong value)
    {
        IdentifierValue.ValidateCategory(value, IdentifierCategory.Biome);
        Value = value;
    }

    /// <summary>
    /// Gets the raw identifier value.
    /// </summary>
    public ulong Value { get; }

    /// <summary>
    /// Creates a biome identifier from a deterministic sequence.
    /// </summary>
    public static BiomeId FromSequence(EntitySequence sequence)
    {
        return new BiomeId(IdentifierValue.Create(IdentifierCategory.Biome, sequence));
    }

    /// <summary>
    /// Parses a serialized biome identifier.
    /// </summary>
    public static BiomeId Parse(string value)
    {
        return new BiomeId(IdentifierValue.Parse(value, IdentifierCategory.Biome));
    }

    /// <summary>
    /// Gets the deterministic sequence used to create the identifier.
    /// </summary>
    public EntitySequence Sequence => IdentifierValue.ExtractSequence(Value);

    /// <summary>
    /// Returns the serialized identifier value.
    /// </summary>
    public override string ToString()
    {
        return IdentifierValue.Format(Value);
    }
}

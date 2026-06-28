namespace GaiaEngine.Domain.Identifiers;

/// <summary>
/// Represents the immutable identifier of a genome entity.
/// </summary>
public readonly record struct GenomeId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenomeId"/> struct.
    /// </summary>
    /// <param name="value">The raw identifier value.</param>
    public GenomeId(ulong value)
    {
        IdentifierValue.ValidateCategory(value, IdentifierCategory.Genome);
        Value = value;
    }

    /// <summary>
    /// Gets the raw identifier value.
    /// </summary>
    public ulong Value { get; }

    /// <summary>
    /// Creates a genome identifier from a deterministic sequence.
    /// </summary>
    public static GenomeId FromSequence(EntitySequence sequence)
    {
        return new GenomeId(IdentifierValue.Create(IdentifierCategory.Genome, sequence));
    }

    /// <summary>
    /// Parses a serialized genome identifier.
    /// </summary>
    public static GenomeId Parse(string value)
    {
        return new GenomeId(IdentifierValue.Parse(value, IdentifierCategory.Genome));
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

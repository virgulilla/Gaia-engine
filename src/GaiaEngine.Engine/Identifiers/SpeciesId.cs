namespace GaiaEngine.Engine.Identifiers;

/// <summary>
/// Represents the immutable identifier of a species entity.
/// </summary>
public readonly record struct SpeciesId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpeciesId"/> struct.
    /// </summary>
    /// <param name="value">The raw identifier value.</param>
    public SpeciesId(ulong value)
    {
        IdentifierValue.ValidateCategory(value, IdentifierCategory.Species);
        Value = value;
    }

    /// <summary>
    /// Gets the raw identifier value.
    /// </summary>
    public ulong Value { get; }

    /// <summary>
    /// Creates a species identifier from a deterministic sequence.
    /// </summary>
    public static SpeciesId FromSequence(EntitySequence sequence)
    {
        return new SpeciesId(IdentifierValue.Create(IdentifierCategory.Species, sequence));
    }

    /// <summary>
    /// Parses a serialized species identifier.
    /// </summary>
    public static SpeciesId Parse(string value)
    {
        return new SpeciesId(IdentifierValue.Parse(value, IdentifierCategory.Species));
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

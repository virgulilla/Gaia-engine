namespace GaiaEngine.Engine.Identifiers;

/// <summary>
/// Represents the immutable identifier of an organism entity.
/// </summary>
public readonly record struct OrganismId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrganismId"/> struct.
    /// </summary>
    /// <param name="value">The raw identifier value.</param>
    public OrganismId(ulong value)
    {
        IdentifierValue.ValidateCategory(value, IdentifierCategory.Organism);
        Value = value;
    }

    /// <summary>
    /// Gets the raw identifier value.
    /// </summary>
    public ulong Value { get; }

    /// <summary>
    /// Creates an organism identifier from a deterministic sequence.
    /// </summary>
    /// <param name="sequence">The deterministic sequence value.</param>
    /// <returns>The generated organism identifier.</returns>
    public static OrganismId FromSequence(EntitySequence sequence)
    {
        return new OrganismId(IdentifierValue.Create(IdentifierCategory.Organism, sequence));
    }

    /// <summary>
    /// Parses a serialized organism identifier.
    /// </summary>
    /// <param name="value">The serialized identifier value.</param>
    /// <returns>The parsed identifier.</returns>
    public static OrganismId Parse(string value)
    {
        return new OrganismId(IdentifierValue.Parse(value, IdentifierCategory.Organism));
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

namespace GaiaEngine.Engine.Identifiers;

/// <summary>
/// Represents the immutable identifier of a resource entity.
/// </summary>
public readonly record struct ResourceId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceId"/> struct.
    /// </summary>
    /// <param name="value">The raw identifier value.</param>
    public ResourceId(ulong value)
    {
        IdentifierValue.ValidateCategory(value, IdentifierCategory.Resource);
        Value = value;
    }

    /// <summary>
    /// Gets the raw identifier value.
    /// </summary>
    public ulong Value { get; }

    /// <summary>
    /// Creates a resource identifier from a deterministic sequence.
    /// </summary>
    public static ResourceId FromSequence(EntitySequence sequence)
    {
        return new ResourceId(IdentifierValue.Create(IdentifierCategory.Resource, sequence));
    }

    /// <summary>
    /// Parses a serialized resource identifier.
    /// </summary>
    public static ResourceId Parse(string value)
    {
        return new ResourceId(IdentifierValue.Parse(value, IdentifierCategory.Resource));
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

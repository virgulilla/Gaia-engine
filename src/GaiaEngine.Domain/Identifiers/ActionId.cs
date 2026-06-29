namespace GaiaEngine.Domain.Identifiers;

/// <summary>
/// Represents the immutable identifier of an action request entity.
/// </summary>
public readonly record struct ActionId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ActionId"/> struct.
    /// </summary>
    /// <param name="value">The raw identifier value.</param>
    public ActionId(ulong value)
    {
        IdentifierValue.ValidateCategory(value, IdentifierCategory.Action);
        Value = value;
    }

    /// <summary>
    /// Gets the raw identifier value.
    /// </summary>
    public ulong Value { get; }

    /// <summary>
    /// Creates an action identifier from a deterministic sequence.
    /// </summary>
    public static ActionId FromSequence(EntitySequence sequence)
    {
        return new ActionId(IdentifierValue.Create(IdentifierCategory.Action, sequence));
    }

    /// <summary>
    /// Parses a serialized action identifier.
    /// </summary>
    public static ActionId Parse(string value)
    {
        return new ActionId(IdentifierValue.Parse(value, IdentifierCategory.Action));
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

namespace GaiaEngine.Engine.Identifiers;

/// <summary>
/// Represents the immutable identifier of a runtime event.
/// </summary>
public readonly record struct EventId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventId"/> struct.
    /// </summary>
    /// <param name="value">The raw identifier value.</param>
    public EventId(ulong value)
    {
        IdentifierValue.ValidateCategory(value, IdentifierCategory.Event);
        Value = value;
    }

    /// <summary>
    /// Gets the raw identifier value.
    /// </summary>
    public ulong Value { get; }

    /// <summary>
    /// Creates an event identifier from a deterministic sequence.
    /// </summary>
    public static EventId FromSequence(EntitySequence sequence)
    {
        return new EventId(IdentifierValue.Create(IdentifierCategory.Event, sequence));
    }

    /// <summary>
    /// Parses a serialized event identifier.
    /// </summary>
    public static EventId Parse(string value)
    {
        return new EventId(IdentifierValue.Parse(value, IdentifierCategory.Event));
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

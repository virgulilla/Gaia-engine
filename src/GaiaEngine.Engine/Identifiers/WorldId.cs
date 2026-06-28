using System;

namespace GaiaEngine.Engine.Identifiers;

/// <summary>
/// Represents the immutable identifier of a world entity.
/// </summary>
public readonly record struct WorldId
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorldId"/> struct.
    /// </summary>
    /// <param name="value">The raw identifier value.</param>
    public WorldId(ulong value)
    {
        IdentifierValue.ValidateCategory(value, IdentifierCategory.World);
        Value = value;
    }

    /// <summary>
    /// Gets the raw identifier value.
    /// </summary>
    public ulong Value { get; }

    /// <summary>
    /// Creates a world identifier from a deterministic sequence.
    /// </summary>
    /// <param name="sequence">The deterministic sequence value.</param>
    /// <returns>The generated world identifier.</returns>
    public static WorldId FromSequence(EntitySequence sequence)
    {
        return new WorldId(IdentifierValue.Create(IdentifierCategory.World, sequence));
    }

    /// <summary>
    /// Parses a serialized world identifier.
    /// </summary>
    /// <param name="value">The serialized identifier value.</param>
    /// <returns>The parsed identifier.</returns>
    public static WorldId Parse(string value)
    {
        return new WorldId(IdentifierValue.Parse(value, IdentifierCategory.World));
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

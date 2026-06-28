using System;

namespace GaiaEngine.Engine.Identifiers;

/// <summary>
/// Represents a deterministic sequence value used to generate persistent identifiers.
/// </summary>
public readonly record struct EntitySequence
{
    /// <summary>
    /// Defines the maximum supported deterministic sequence value.
    /// </summary>
    public const ulong MAX_VALUE = 0x00FF_FFFF_FFFF_FFFF;

    /// <summary>
    /// Initializes a new instance of the <see cref="EntitySequence"/> struct.
    /// </summary>
    /// <param name="value">The deterministic sequence value.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="value"/> exceeds the supported range.</exception>
    public EntitySequence(ulong value)
    {
        if (value > MAX_VALUE)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "The entity sequence must fit within 56 bits.");
        }

        Value = value;
    }

    /// <summary>
    /// Gets the deterministic sequence value.
    /// </summary>
    public ulong Value { get; }
}

using System;

namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Stores one deterministic normalized gene value scaled to the inclusive range [0, 1000].
/// </summary>
public readonly record struct NormalizedGeneValue
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NormalizedGeneValue"/> struct.
    /// </summary>
    /// <param name="scaledValue">
    /// The normalized deterministic value scaled to the inclusive range [0, 1000],
    /// which represents the specification interval [0.0, 1.0].
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="scaledValue"/> is outside the supported range.</exception>
    public NormalizedGeneValue(int scaledValue)
    {
        if (scaledValue < 0 || scaledValue > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(scaledValue), "The normalized gene value must be between 0 and 1000.");
        }

        ScaledValue = scaledValue;
    }

    /// <summary>
    /// Gets the normalized deterministic value scaled to the inclusive range [0, 1000].
    /// </summary>
    public int ScaledValue { get; }
}

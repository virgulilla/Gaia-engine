using System;

namespace GaiaEngine.Audio.SoundEffects;

/// <summary>
/// Represents one deterministic pitch range used by a sound effect.
/// </summary>
public sealed record PitchRange
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PitchRange"/> class.
    /// </summary>
    /// <param name="minimum">The minimum pitch multiplier.</param>
    /// <param name="maximum">The maximum pitch multiplier.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one bound is less than or equal to zero.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="minimum"/> is greater than <paramref name="maximum"/>.</exception>
    public PitchRange(float minimum, float maximum)
    {
        if (minimum <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minimum), "The minimum pitch must be greater than zero.");
        }

        if (maximum <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maximum), "The maximum pitch must be greater than zero.");
        }

        if (minimum > maximum)
        {
            throw new ArgumentException("The minimum pitch cannot be greater than the maximum pitch.", nameof(minimum));
        }

        Minimum = minimum;
        Maximum = maximum;
    }

    /// <summary>
    /// Gets the minimum pitch multiplier.
    /// </summary>
    public float Minimum { get; }

    /// <summary>
    /// Gets the maximum pitch multiplier.
    /// </summary>
    public float Maximum { get; }

    /// <summary>
    /// Gets the shared neutral pitch range.
    /// </summary>
    public static PitchRange Neutral { get; } = new(1f, 1f);
}

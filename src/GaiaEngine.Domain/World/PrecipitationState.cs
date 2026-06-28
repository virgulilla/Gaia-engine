using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the passive precipitation values stored for one chunk climate state.
/// </summary>
public sealed record PrecipitationState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PrecipitationState"/> class.
    /// </summary>
    /// <param name="type">The deterministic precipitation type.</param>
    /// <param name="intensity">The deterministic precipitation intensity.</param>
    /// <param name="duration">The deterministic precipitation duration in ticks.</param>
    /// <param name="coverage">The deterministic precipitation coverage percentage.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="intensity"/> or <paramref name="duration"/> is negative,
    /// or when <paramref name="coverage"/> is outside the inclusive range [0, 100].
    /// </exception>
    public PrecipitationState(PrecipitationType type, int intensity, int duration, int coverage)
    {
        if (intensity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(intensity), "The precipitation intensity must be zero or greater.");
        }

        if (duration < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(duration), "The precipitation duration must be zero or greater.");
        }

        if (coverage < 0 || coverage > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(coverage), "The precipitation coverage must be between 0 and 100.");
        }

        Type = type;
        Intensity = intensity;
        Duration = duration;
        Coverage = coverage;
    }

    /// <summary>
    /// Gets the deterministic precipitation type.
    /// </summary>
    public PrecipitationType Type { get; }

    /// <summary>
    /// Gets the deterministic precipitation intensity.
    /// </summary>
    public int Intensity { get; }

    /// <summary>
    /// Gets the deterministic precipitation duration in ticks.
    /// </summary>
    public int Duration { get; }

    /// <summary>
    /// Gets the deterministic precipitation coverage percentage.
    /// </summary>
    public int Coverage { get; }
}

using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents immutable deterministic slope data for one chunk terrain slice.
/// </summary>
public sealed record SlopeState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SlopeState"/> class.
    /// </summary>
    /// <param name="gradient">The terrain gradient in degrees.</param>
    /// <param name="aspect">The terrain aspect in degrees.</param>
    /// <param name="traversalCost">The deterministic traversal cost multiplier scaled by 100.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="gradient"/> is outside [0, 90], when <paramref name="aspect"/> is outside [0, 359],
    /// or when <paramref name="traversalCost"/> is less than 1.
    /// </exception>
    public SlopeState(int gradient, int aspect, int traversalCost)
    {
        if (gradient < 0 || gradient > 90)
        {
            throw new ArgumentOutOfRangeException(nameof(gradient), "The terrain gradient must be between 0 and 90.");
        }

        if (aspect < 0 || aspect > 359)
        {
            throw new ArgumentOutOfRangeException(nameof(aspect), "The terrain aspect must be between 0 and 359.");
        }

        if (traversalCost < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(traversalCost), "The traversal cost must be greater than zero.");
        }

        Gradient = gradient;
        Aspect = aspect;
        TraversalCost = traversalCost;
    }

    /// <summary>
    /// Gets the terrain gradient in degrees.
    /// </summary>
    public int Gradient { get; }

    /// <summary>
    /// Gets the terrain aspect in degrees.
    /// </summary>
    public int Aspect { get; }

    /// <summary>
    /// Gets the deterministic traversal cost multiplier scaled by 100.
    /// </summary>
    public int TraversalCost { get; }
}

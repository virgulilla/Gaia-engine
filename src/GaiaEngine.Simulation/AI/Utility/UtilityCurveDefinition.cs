using System;
using System.Collections.Generic;

namespace GaiaEngine.Simulation.AI.Utility;

/// <summary>
/// Describes one configurable deterministic utility curve definition.
/// </summary>
public sealed class UtilityCurveDefinition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UtilityCurveDefinition"/> class.
    /// </summary>
    /// <param name="curveType">The utility curve type.</param>
    /// <param name="strength">
    /// The curve strength in the inclusive range [1, 8].
    /// For exponential curves it acts as the exponent.
    /// For logistic curves it controls the sigmoid steepness.
    /// </param>
    /// <param name="midpoint">
    /// The normalized logistic midpoint scaled to the inclusive range [0, 1000].
    /// This value is used only by logistic curves.
    /// </param>
    /// <param name="customPoints">The custom interpolation points used by custom curves.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="customPoints"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="strength"/> is outside [1, 8]
    /// or when <paramref name="midpoint"/> is outside [0, 1000].
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when custom points are invalid for a custom curve.
    /// </exception>
    public UtilityCurveDefinition(
        UtilityCurveType curveType,
        int strength,
        int midpoint,
        IReadOnlyList<UtilityCurvePoint> customPoints)
    {
        ArgumentNullException.ThrowIfNull(customPoints);

        if (strength < 1 || strength > 8)
        {
            throw new ArgumentOutOfRangeException(nameof(strength), "The utility curve strength must be between 1 and 8.");
        }

        if (midpoint < 0 || midpoint > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(midpoint), "The utility curve midpoint must be between 0 and 1000.");
        }

        CurveType = curveType;
        Strength = strength;
        Midpoint = midpoint;
        CustomPoints = customPoints;

        if (curveType == UtilityCurveType.Custom)
        {
            ValidateCustomPoints(customPoints);
        }
    }

    /// <summary>
    /// Gets the utility curve type.
    /// </summary>
    public UtilityCurveType CurveType { get; }

    /// <summary>
    /// Gets the configurable curve strength.
    /// </summary>
    public int Strength { get; }

    /// <summary>
    /// Gets the configurable logistic midpoint.
    /// </summary>
    public int Midpoint { get; }

    /// <summary>
    /// Gets the custom interpolation points.
    /// </summary>
    public IReadOnlyList<UtilityCurvePoint> CustomPoints { get; }

    /// <summary>
    /// Creates a linear utility curve definition.
    /// </summary>
    /// <returns>The configured utility curve definition.</returns>
    public static UtilityCurveDefinition Linear()
    {
        return new UtilityCurveDefinition(UtilityCurveType.Linear, strength: 1, midpoint: 500, customPoints: Array.Empty<UtilityCurvePoint>());
    }

    /// <summary>
    /// Creates an exponential utility curve definition.
    /// </summary>
    /// <param name="strength">The exponent strength in the inclusive range [1, 8].</param>
    /// <returns>The configured utility curve definition.</returns>
    public static UtilityCurveDefinition Exponential(int strength)
    {
        return new UtilityCurveDefinition(UtilityCurveType.Exponential, strength, midpoint: 500, customPoints: Array.Empty<UtilityCurvePoint>());
    }

    /// <summary>
    /// Creates a logistic utility curve definition.
    /// </summary>
    /// <param name="strength">The logistic steepness strength in the inclusive range [1, 8].</param>
    /// <param name="midpoint">The normalized logistic midpoint in the inclusive range [0, 1000].</param>
    /// <returns>The configured utility curve definition.</returns>
    public static UtilityCurveDefinition Logistic(int strength, int midpoint)
    {
        return new UtilityCurveDefinition(UtilityCurveType.Logistic, strength, midpoint, Array.Empty<UtilityCurvePoint>());
    }

    /// <summary>
    /// Creates a custom utility curve definition.
    /// </summary>
    /// <param name="customPoints">The custom interpolation points.</param>
    /// <returns>The configured utility curve definition.</returns>
    public static UtilityCurveDefinition Custom(IReadOnlyList<UtilityCurvePoint> customPoints)
    {
        return new UtilityCurveDefinition(UtilityCurveType.Custom, strength: 1, midpoint: 500, customPoints);
    }

    private static void ValidateCustomPoints(IReadOnlyList<UtilityCurvePoint> customPoints)
    {
        if (customPoints.Count < 2)
        {
            throw new ArgumentException("A custom utility curve must contain at least two points.", nameof(customPoints));
        }

        if (customPoints[0].Input != 0)
        {
            throw new ArgumentException("A custom utility curve must start at input 0.", nameof(customPoints));
        }

        if (customPoints[^1].Input != 1000)
        {
            throw new ArgumentException("A custom utility curve must end at input 1000.", nameof(customPoints));
        }

        for (int index = 1; index < customPoints.Count; index++)
        {
            if (customPoints[index].Input <= customPoints[index - 1].Input)
            {
                throw new ArgumentException("Custom utility curve points must be ordered by strictly increasing input.", nameof(customPoints));
            }
        }
    }
}

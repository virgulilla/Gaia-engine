using System;

namespace GaiaEngine.Simulation.AI.Utility;

/// <summary>
/// Represents one deterministic point inside a custom utility curve.
/// </summary>
public readonly record struct UtilityCurvePoint
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UtilityCurvePoint"/> struct.
    /// </summary>
    /// <param name="input">
    /// The normalized input scaled to the inclusive range [0, 1000],
    /// which represents the specification range [0.0, 1.0].
    /// </param>
    /// <param name="output">
    /// The normalized output scaled to the inclusive range [0, 1000],
    /// which represents the specification range [0.0, 1.0].
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="input"/> or <paramref name="output"/> is outside [0, 1000].
    /// </exception>
    public UtilityCurvePoint(int input, int output)
    {
        if (input < 0 || input > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(input), "The utility curve point input must be between 0 and 1000.");
        }

        if (output < 0 || output > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(output), "The utility curve point output must be between 0 and 1000.");
        }

        Input = input;
        Output = output;
    }

    /// <summary>
    /// Gets the normalized input value scaled to the inclusive range [0, 1000].
    /// </summary>
    public int Input { get; }

    /// <summary>
    /// Gets the normalized output value scaled to the inclusive range [0, 1000].
    /// </summary>
    public int Output { get; }
}

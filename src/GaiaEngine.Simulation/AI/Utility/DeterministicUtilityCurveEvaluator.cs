using System;

namespace GaiaEngine.Simulation.AI.Utility;

/// <summary>
/// Evaluates supported utility curves in a deterministic and renderer-independent way.
/// </summary>
public sealed class DeterministicUtilityCurveEvaluator : IUtilityCurveEvaluator
{
    /// <summary>
    /// Evaluates one normalized utility input against the supplied curve definition.
    /// </summary>
    /// <param name="input">
    /// The normalized input scaled to the inclusive range [0, 1000],
    /// which represents the specification range [0.0, 1.0].
    /// </param>
    /// <param name="definition">The curve definition to evaluate.</param>
    /// <returns>
    /// The normalized output scaled to the inclusive range [0, 1000],
    /// which represents the specification range [0.0, 1.0].
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="definition"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="input"/> is outside [0, 1000].</exception>
    public int Evaluate(int input, UtilityCurveDefinition definition)
    {
        if (input < 0 || input > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(input), "The utility input must be between 0 and 1000.");
        }

        ArgumentNullException.ThrowIfNull(definition);

        return definition.CurveType switch
        {
            UtilityCurveType.Linear => input,
            UtilityCurveType.Exponential => EvaluateExponential(input, definition.Strength),
            UtilityCurveType.Logistic => EvaluateLogistic(input, definition.Strength, definition.Midpoint),
            UtilityCurveType.Custom => EvaluateCustom(input, definition),
            _ => throw new InvalidOperationException("The supplied utility curve type is not supported."),
        };
    }

    private static int EvaluateExponential(int input, int strength)
    {
        double normalized = input / 1000d;
        double output = Math.Pow(normalized, strength);
        return ClampNormalized(output);
    }

    private static int EvaluateLogistic(int input, int strength, int midpoint)
    {
        double normalized = input / 1000d;
        double normalizedMidpoint = midpoint / 1000d;
        double steepness = 2d + (strength * 1.5d);
        double output = 1d / (1d + Math.Exp(-steepness * (normalized - normalizedMidpoint)));
        return ClampNormalized(output);
    }

    private static int EvaluateCustom(int input, UtilityCurveDefinition definition)
    {
        if (input <= definition.CustomPoints[0].Input)
        {
            return definition.CustomPoints[0].Output;
        }

        for (int index = 1; index < definition.CustomPoints.Count; index++)
        {
            UtilityCurvePoint previous = definition.CustomPoints[index - 1];
            UtilityCurvePoint current = definition.CustomPoints[index];
            if (input > current.Input)
            {
                continue;
            }

            int inputDelta = current.Input - previous.Input;
            int outputDelta = current.Output - previous.Output;
            int offset = input - previous.Input;
            int interpolated = previous.Output + ((outputDelta * offset) / inputDelta);
            return interpolated;
        }

        return definition.CustomPoints[^1].Output;
    }

    private static int ClampNormalized(double value)
    {
        int scaled = (int)Math.Round(value * 1000d, MidpointRounding.AwayFromZero);
        return Math.Clamp(scaled, 0, 1000);
    }
}

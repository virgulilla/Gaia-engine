using System;

namespace GaiaEngine.Simulation.AI.Utility;

/// <summary>
/// Defines the configurable deterministic rules used by utility evaluation.
/// </summary>
public sealed class UtilityEvaluationSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UtilityEvaluationSettings"/> class.
    /// </summary>
    /// <param name="eatCurve">The curve applied to hunger urgency.</param>
    /// <param name="drinkCurve">The curve applied to hydration urgency.</param>
    /// <param name="moveCurve">The curve applied to movement urgency.</param>
    /// <param name="minimumResourceAmount">The minimum resource amount required for direct eating or drinking.</param>
    /// <param name="minimumPerceptionConfidence">The minimum accepted perception confidence.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="eatCurve"/>, <paramref name="drinkCurve"/>, or <paramref name="moveCurve"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="minimumResourceAmount"/> is not positive
    /// or when <paramref name="minimumPerceptionConfidence"/> is outside [0, 1000].
    /// </exception>
    public UtilityEvaluationSettings(
        UtilityCurveDefinition eatCurve,
        UtilityCurveDefinition drinkCurve,
        UtilityCurveDefinition moveCurve,
        int minimumResourceAmount,
        int minimumPerceptionConfidence)
    {
        EatCurve = eatCurve ?? throw new ArgumentNullException(nameof(eatCurve));
        DrinkCurve = drinkCurve ?? throw new ArgumentNullException(nameof(drinkCurve));
        MoveCurve = moveCurve ?? throw new ArgumentNullException(nameof(moveCurve));

        if (minimumResourceAmount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumResourceAmount), "The minimum resource amount must be greater than zero.");
        }

        if (minimumPerceptionConfidence < 0 || minimumPerceptionConfidence > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumPerceptionConfidence), "The minimum perception confidence must be between 0 and 1000.");
        }

        MinimumResourceAmount = minimumResourceAmount;
        MinimumPerceptionConfidence = minimumPerceptionConfidence;
    }

    /// <summary>
    /// Gets a shared default utility evaluation configuration.
    /// </summary>
    public static UtilityEvaluationSettings Default { get; } = new(
        UtilityCurveDefinition.Logistic(strength: 5, midpoint: 450),
        UtilityCurveDefinition.Logistic(strength: 5, midpoint: 450),
        UtilityCurveDefinition.Exponential(strength: 2),
        minimumResourceAmount: 10,
        minimumPerceptionConfidence: 250);

    /// <summary>
    /// Gets the curve applied to hunger urgency.
    /// </summary>
    public UtilityCurveDefinition EatCurve { get; }

    /// <summary>
    /// Gets the curve applied to hydration urgency.
    /// </summary>
    public UtilityCurveDefinition DrinkCurve { get; }

    /// <summary>
    /// Gets the curve applied to movement urgency.
    /// </summary>
    public UtilityCurveDefinition MoveCurve { get; }

    /// <summary>
    /// Gets the minimum resource amount required for direct consumption.
    /// </summary>
    public int MinimumResourceAmount { get; }

    /// <summary>
    /// Gets the minimum accepted perception confidence.
    /// </summary>
    public int MinimumPerceptionConfidence { get; }
}

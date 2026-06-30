using GaiaEngine.Simulation.AI.Utility;
using Xunit;

namespace GaiaEngine.Simulation.Tests.AI.Utility;

public sealed class DeterministicUtilityCurveEvaluatorTests
{
    [Fact]
    public void Evaluate_ShouldReturnIdentityForLinearCurve()
    {
        DeterministicUtilityCurveEvaluator evaluator = new();

        int result = evaluator.Evaluate(650, UtilityCurveDefinition.Linear());

        Assert.Equal(650, result);
    }

    [Fact]
    public void Evaluate_ShouldEmphasizeHighInputsForExponentialCurve()
    {
        DeterministicUtilityCurveEvaluator evaluator = new();

        int midResult = evaluator.Evaluate(500, UtilityCurveDefinition.Exponential(strength: 2));
        int highResult = evaluator.Evaluate(900, UtilityCurveDefinition.Exponential(strength: 2));

        Assert.Equal(250, midResult);
        Assert.True(highResult > 800);
    }

    [Fact]
    public void Evaluate_ShouldPivotAroundConfiguredMidpointForLogisticCurve()
    {
        DeterministicUtilityCurveEvaluator evaluator = new();

        int lowResult = evaluator.Evaluate(300, UtilityCurveDefinition.Logistic(strength: 4, midpoint: 500));
        int midpointResult = evaluator.Evaluate(500, UtilityCurveDefinition.Logistic(strength: 4, midpoint: 500));
        int highResult = evaluator.Evaluate(700, UtilityCurveDefinition.Logistic(strength: 4, midpoint: 500));

        Assert.True(lowResult < midpointResult);
        Assert.Equal(500, midpointResult);
        Assert.True(highResult > midpointResult);
    }

    [Fact]
    public void Evaluate_ShouldInterpolateCustomCurvePoints()
    {
        DeterministicUtilityCurveEvaluator evaluator = new();
        UtilityCurveDefinition curve = UtilityCurveDefinition.Custom(
            new[]
            {
                new UtilityCurvePoint(0, 0),
                new UtilityCurvePoint(400, 200),
                new UtilityCurvePoint(1000, 1000),
            });

        int result = evaluator.Evaluate(700, curve);

        Assert.Equal(600, result);
    }

    [Fact]
    public void Evaluate_ShouldRemainDeterministicForRepeatedInputs()
    {
        DeterministicUtilityCurveEvaluator evaluator = new();
        UtilityCurveDefinition curve = UtilityCurveDefinition.Logistic(strength: 5, midpoint: 450);

        int first = evaluator.Evaluate(640, curve);
        int second = evaluator.Evaluate(640, curve);

        Assert.Equal(first, second);
    }
}

namespace GaiaEngine.Simulation.AI.Utility;

/// <summary>
/// Evaluates configurable deterministic utility curves.
/// </summary>
public interface IUtilityCurveEvaluator
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
    public int Evaluate(int input, UtilityCurveDefinition definition);
}

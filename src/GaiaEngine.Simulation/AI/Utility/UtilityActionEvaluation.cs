using System;
using GaiaEngine.Simulation.Actions;

namespace GaiaEngine.Simulation.AI.Utility;

/// <summary>
/// Represents one deterministic utility score assigned to one candidate action.
/// </summary>
public sealed record UtilityActionEvaluation
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UtilityActionEvaluation"/> class.
    /// </summary>
    /// <param name="actionType">The candidate action type.</param>
    /// <param name="target">The candidate target metadata.</param>
    /// <param name="utilityScore">
    /// The normalized utility score scaled to the inclusive range [0, 1000],
    /// which represents the specification range [0.0, 1.0].
    /// </param>
    /// <param name="estimatedCost">The deterministic estimated energy or traversal cost.</param>
    /// <param name="expectedDuration">The deterministic expected duration in ticks.</param>
    /// <param name="isValid">A value indicating whether the candidate passed current validation.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="utilityScore"/>, <paramref name="estimatedCost"/>, or <paramref name="expectedDuration"/> is negative.
    /// </exception>
    public UtilityActionEvaluation(
        SimulationActionType actionType,
        SimulationActionTarget target,
        int utilityScore,
        int estimatedCost,
        int expectedDuration,
        bool isValid)
    {
        if (utilityScore < 0 || utilityScore > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(utilityScore), "The utility score must be between 0 and 1000.");
        }

        if (estimatedCost < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(estimatedCost), "The estimated cost must be zero or greater.");
        }

        if (expectedDuration < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(expectedDuration), "The expected duration must be zero or greater.");
        }

        ActionType = actionType;
        Target = target;
        UtilityScore = utilityScore;
        EstimatedCost = estimatedCost;
        ExpectedDuration = expectedDuration;
        IsValid = isValid;
    }

    /// <summary>
    /// Gets the candidate action type.
    /// </summary>
    public SimulationActionType ActionType { get; }

    /// <summary>
    /// Gets the candidate target metadata.
    /// </summary>
    public SimulationActionTarget Target { get; }

    /// <summary>
    /// Gets the normalized utility score scaled to the inclusive range [0, 1000].
    /// </summary>
    public int UtilityScore { get; }

    /// <summary>
    /// Gets the deterministic estimated cost.
    /// </summary>
    public int EstimatedCost { get; }

    /// <summary>
    /// Gets the deterministic expected duration in ticks.
    /// </summary>
    public int ExpectedDuration { get; }

    /// <summary>
    /// Gets a value indicating whether the candidate passed current validation.
    /// </summary>
    public bool IsValid { get; }
}

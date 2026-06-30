using System;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Simulation.Actions;

namespace GaiaEngine.Simulation.AI.Decision;

/// <summary>
/// Represents one deterministic selected action ready to be handed to behaviour execution.
/// </summary>
public sealed class SelectedDecision
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SelectedDecision"/> class.
    /// </summary>
    /// <param name="organismId">The organism that owns the decision.</param>
    /// <param name="actionId">The deterministic selected action identifier.</param>
    /// <param name="actionType">The selected action type.</param>
    /// <param name="target">The selected target metadata.</param>
    /// <param name="utilityScore">
    /// The normalized utility score scaled to the inclusive range [0, 1000],
    /// which represents the specification range [0.0, 1.0].
    /// </param>
    /// <param name="estimatedCost">The deterministic estimated cost.</param>
    /// <param name="expectedDuration">The deterministic expected duration in ticks.</param>
    /// <param name="decisionTick">The simulation tick used during selection.</param>
    /// <param name="isIdleFallback">A value indicating whether this decision was produced by the default idle fallback.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="target"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when one or more numeric values are negative or when <paramref name="utilityScore"/> is outside [0, 1000].
    /// </exception>
    public SelectedDecision(
        OrganismId organismId,
        ActionId actionId,
        SimulationActionType actionType,
        SimulationActionTarget target,
        int utilityScore,
        int estimatedCost,
        int expectedDuration,
        long decisionTick,
        bool isIdleFallback)
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

        if (decisionTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(decisionTick), "The decision tick must be zero or greater.");
        }

        OrganismId = organismId;
        ActionId = actionId;
        ActionType = actionType;
        Target = target;
        UtilityScore = utilityScore;
        EstimatedCost = estimatedCost;
        ExpectedDuration = expectedDuration;
        DecisionTick = decisionTick;
        IsIdleFallback = isIdleFallback;
    }

    /// <summary>
    /// Gets the organism that owns the decision.
    /// </summary>
    public OrganismId OrganismId { get; }

    /// <summary>
    /// Gets the selected action identifier.
    /// </summary>
    public ActionId ActionId { get; }

    /// <summary>
    /// Gets the selected action type.
    /// </summary>
    public SimulationActionType ActionType { get; }

    /// <summary>
    /// Gets the selected target metadata.
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
    /// Gets the simulation tick used during selection.
    /// </summary>
    public long DecisionTick { get; }

    /// <summary>
    /// Gets a value indicating whether this decision was produced by the idle fallback.
    /// </summary>
    public bool IsIdleFallback { get; }
}

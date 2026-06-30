using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Simulation.AI.Utility;

/// <summary>
/// Represents the deterministic utility evaluation output for one organism.
/// </summary>
public sealed class UtilityEvaluationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UtilityEvaluationResult"/> class.
    /// </summary>
    /// <param name="organismId">The evaluated organism identifier.</param>
    /// <param name="evaluationTick">The simulation tick used during evaluation.</param>
    /// <param name="candidates">The candidate action evaluations in deterministic priority order.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="candidates"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="evaluationTick"/> is negative.</exception>
    public UtilityEvaluationResult(OrganismId organismId, long evaluationTick, IReadOnlyList<UtilityActionEvaluation> candidates)
    {
        if (evaluationTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(evaluationTick), "The evaluation tick must be zero or greater.");
        }

        OrganismId = organismId;
        EvaluationTick = evaluationTick;
        Candidates = candidates ?? throw new ArgumentNullException(nameof(candidates));
    }

    /// <summary>
    /// Gets the evaluated organism identifier.
    /// </summary>
    public OrganismId OrganismId { get; }

    /// <summary>
    /// Gets the simulation tick used during evaluation.
    /// </summary>
    public long EvaluationTick { get; }

    /// <summary>
    /// Gets the candidate action evaluations in deterministic priority order.
    /// </summary>
    public IReadOnlyList<UtilityActionEvaluation> Candidates { get; }
}

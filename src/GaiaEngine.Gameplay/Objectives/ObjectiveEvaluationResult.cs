using System;
using System.Collections.Generic;
using GaiaEngine.Gameplay.Player;

namespace GaiaEngine.Gameplay.Objectives;

/// <summary>
/// Represents the deterministic outcome of one objective evaluation pass.
/// </summary>
public sealed record ObjectiveEvaluationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectiveEvaluationResult"/> class.
    /// </summary>
    /// <param name="profile">The updated player profile.</param>
    /// <param name="completedObjectives">The objectives completed during the evaluation pass.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="profile"/> or <paramref name="completedObjectives"/> is <see langword="null"/>.</exception>
    public ObjectiveEvaluationResult(PlayerProfile profile, IReadOnlyList<ObjectiveEntry> completedObjectives)
    {
        Profile = profile ?? throw new ArgumentNullException(nameof(profile));
        CompletedObjectives = completedObjectives ?? throw new ArgumentNullException(nameof(completedObjectives));
    }

    /// <summary>
    /// Gets the updated player profile.
    /// </summary>
    public PlayerProfile Profile { get; }

    /// <summary>
    /// Gets the objectives completed during the evaluation pass.
    /// </summary>
    public IReadOnlyList<ObjectiveEntry> CompletedObjectives { get; }
}

using System;
using System.Collections.Generic;
using GaiaEngine.Gameplay.Player;

namespace GaiaEngine.Gameplay.Progression;

/// <summary>
/// Represents the deterministic outcome of one progression evaluation pass.
/// </summary>
public sealed record ProgressionEvaluationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProgressionEvaluationResult"/> class.
    /// </summary>
    /// <param name="profile">The updated player profile.</param>
    /// <param name="newUnlockIds">The newly granted unlock identifiers.</param>
    /// <param name="newMilestoneIds">The newly completed milestone identifiers.</param>
    /// <exception cref="ArgumentNullException">Thrown when one required argument is <see langword="null"/>.</exception>
    public ProgressionEvaluationResult(
        PlayerProfile profile,
        IReadOnlyList<string> newUnlockIds,
        IReadOnlyList<string> newMilestoneIds)
    {
        Profile = profile ?? throw new ArgumentNullException(nameof(profile));
        NewUnlockIds = newUnlockIds ?? throw new ArgumentNullException(nameof(newUnlockIds));
        NewMilestoneIds = newMilestoneIds ?? throw new ArgumentNullException(nameof(newMilestoneIds));
    }

    /// <summary>
    /// Gets the updated player profile.
    /// </summary>
    public PlayerProfile Profile { get; }

    /// <summary>
    /// Gets the newly granted unlock identifiers.
    /// </summary>
    public IReadOnlyList<string> NewUnlockIds { get; }

    /// <summary>
    /// Gets the newly completed milestone identifiers.
    /// </summary>
    public IReadOnlyList<string> NewMilestoneIds { get; }
}

using System;
using System.Collections.Generic;
using GaiaEngine.Gameplay.Player;

namespace GaiaEngine.Gameplay.Achievements;

/// <summary>
/// Represents the deterministic outcome of one achievement evaluation pass.
/// </summary>
public sealed record AchievementEvaluationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AchievementEvaluationResult"/> class.
    /// </summary>
    /// <param name="profile">The updated player profile.</param>
    /// <param name="unlockedAchievements">The achievements unlocked during the evaluation pass.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="profile"/> or <paramref name="unlockedAchievements"/> is <see langword="null"/>.</exception>
    public AchievementEvaluationResult(PlayerProfile profile, IReadOnlyList<AchievementEntry> unlockedAchievements)
    {
        Profile = profile ?? throw new ArgumentNullException(nameof(profile));
        UnlockedAchievements = unlockedAchievements ?? throw new ArgumentNullException(nameof(unlockedAchievements));
    }

    /// <summary>
    /// Gets the updated player profile.
    /// </summary>
    public PlayerProfile Profile { get; }

    /// <summary>
    /// Gets the achievements unlocked during the evaluation pass.
    /// </summary>
    public IReadOnlyList<AchievementEntry> UnlockedAchievements { get; }
}

using System;
using GaiaEngine.Gameplay.Achievements;
using GaiaEngine.Gameplay.Objectives;

namespace GaiaEngine.Gameplay.Player;

/// <summary>
/// Represents the persistent player profile consumed by gameplay systems.
/// </summary>
public sealed record PlayerProfile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerProfile"/> class.
    /// </summary>
    /// <param name="identity">The immutable player identity.</param>
    /// <param name="knowledge">The accumulated permanent player knowledge.</param>
    /// <param name="objectives">The persistent objective state owned by the player profile.</param>
    /// <param name="progression">The persistent player progression state.</param>
    /// <param name="achievements">The persistent achievement state owned by the player profile.</param>
    /// <param name="statistics">The deterministic player activity statistics.</param>
    /// <exception cref="ArgumentNullException">Thrown when any argument is <see langword="null"/>.</exception>
    public PlayerProfile(
        PlayerIdentity identity,
        PlayerKnowledge knowledge,
        ObjectiveCollection objectives,
        PlayerProgression progression,
        AchievementCollection achievements,
        PlayerStatistics statistics)
    {
        Identity = identity ?? throw new ArgumentNullException(nameof(identity));
        Knowledge = knowledge ?? throw new ArgumentNullException(nameof(knowledge));
        Objectives = objectives ?? throw new ArgumentNullException(nameof(objectives));
        Progression = progression ?? throw new ArgumentNullException(nameof(progression));
        Achievements = achievements ?? throw new ArgumentNullException(nameof(achievements));
        Statistics = statistics ?? throw new ArgumentNullException(nameof(statistics));
    }

    /// <summary>
    /// Gets the immutable player identity.
    /// </summary>
    public PlayerIdentity Identity { get; }

    /// <summary>
    /// Gets the accumulated permanent player knowledge.
    /// </summary>
    public PlayerKnowledge Knowledge { get; }

    /// <summary>
    /// Gets the persistent objective state owned by the player profile.
    /// </summary>
    public ObjectiveCollection Objectives { get; }

    /// <summary>
    /// Gets the persistent player progression state.
    /// </summary>
    public PlayerProgression Progression { get; }

    /// <summary>
    /// Gets the persistent achievement state owned by the player profile.
    /// </summary>
    public AchievementCollection Achievements { get; }

    /// <summary>
    /// Gets the deterministic player activity statistics.
    /// </summary>
    public PlayerStatistics Statistics { get; }
}

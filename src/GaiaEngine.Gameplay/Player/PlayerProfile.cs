using System;

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
    /// <param name="progression">The persistent player progression state.</param>
    /// <param name="statistics">The deterministic player activity statistics.</param>
    /// <exception cref="ArgumentNullException">Thrown when any argument is <see langword="null"/>.</exception>
    public PlayerProfile(
        PlayerIdentity identity,
        PlayerKnowledge knowledge,
        PlayerProgression progression,
        PlayerStatistics statistics)
    {
        Identity = identity ?? throw new ArgumentNullException(nameof(identity));
        Knowledge = knowledge ?? throw new ArgumentNullException(nameof(knowledge));
        Progression = progression ?? throw new ArgumentNullException(nameof(progression));
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
    /// Gets the persistent player progression state.
    /// </summary>
    public PlayerProgression Progression { get; }

    /// <summary>
    /// Gets the deterministic player activity statistics.
    /// </summary>
    public PlayerStatistics Statistics { get; }
}

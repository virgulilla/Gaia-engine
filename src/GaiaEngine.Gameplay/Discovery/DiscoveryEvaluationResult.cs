using System;
using System.Collections.Generic;
using GaiaEngine.Gameplay.Player;

namespace GaiaEngine.Gameplay.Discovery;

/// <summary>
/// Represents the deterministic output produced by one discovery evaluation pass.
/// </summary>
public sealed class DiscoveryEvaluationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DiscoveryEvaluationResult"/> class.
    /// </summary>
    /// <param name="profile">The updated player profile.</param>
    /// <param name="unlockedDiscoveries">The discoveries unlocked during the current evaluation pass.</param>
    /// <exception cref="ArgumentNullException">Thrown when any argument is <see langword="null"/>.</exception>
    public DiscoveryEvaluationResult(PlayerProfile profile, IReadOnlyList<DiscoveryEntry> unlockedDiscoveries)
    {
        Profile = profile ?? throw new ArgumentNullException(nameof(profile));
        UnlockedDiscoveries = unlockedDiscoveries ?? throw new ArgumentNullException(nameof(unlockedDiscoveries));
    }

    /// <summary>
    /// Gets the updated player profile.
    /// </summary>
    public PlayerProfile Profile { get; }

    /// <summary>
    /// Gets the discoveries unlocked during the current evaluation pass.
    /// </summary>
    public IReadOnlyList<DiscoveryEntry> UnlockedDiscoveries { get; }
}

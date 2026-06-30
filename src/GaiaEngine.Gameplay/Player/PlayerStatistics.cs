using System;

namespace GaiaEngine.Gameplay.Player;

/// <summary>
/// Represents the deterministic activity statistics accumulated by one player profile.
/// </summary>
public sealed record PlayerStatistics
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerStatistics"/> class.
    /// </summary>
    /// <param name="totalDiscoveriesUnlocked">The total number of unlocked discoveries.</param>
    /// <param name="duplicateDiscoveryObservations">The total number of duplicate discovery observations.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="totalDiscoveriesUnlocked"/> or <paramref name="duplicateDiscoveryObservations"/> is negative.
    /// </exception>
    public PlayerStatistics(int totalDiscoveriesUnlocked, int duplicateDiscoveryObservations)
    {
        if (totalDiscoveriesUnlocked < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(totalDiscoveriesUnlocked), "The unlocked discovery count must be zero or greater.");
        }

        if (duplicateDiscoveryObservations < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(duplicateDiscoveryObservations), "The duplicate discovery observation count must be zero or greater.");
        }

        TotalDiscoveriesUnlocked = totalDiscoveriesUnlocked;
        DuplicateDiscoveryObservations = duplicateDiscoveryObservations;
    }

    /// <summary>
    /// Gets the total number of unlocked discoveries.
    /// </summary>
    public int TotalDiscoveriesUnlocked { get; }

    /// <summary>
    /// Gets the total number of duplicate discovery observations.
    /// </summary>
    public int DuplicateDiscoveryObservations { get; }
}

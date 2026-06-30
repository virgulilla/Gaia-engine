using System;
using GaiaEngine.Gameplay.Discovery;

namespace GaiaEngine.Gameplay.Player;

/// <summary>
/// Represents the permanent player knowledge accumulated through gameplay discoveries.
/// </summary>
public sealed record PlayerKnowledge
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerKnowledge"/> class.
    /// </summary>
    /// <param name="discoveries">The unlocked discoveries owned by the player profile.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="discoveries"/> is <see langword="null"/>.</exception>
    public PlayerKnowledge(DiscoveryCollection discoveries)
    {
        Discoveries = discoveries ?? throw new ArgumentNullException(nameof(discoveries));
    }

    /// <summary>
    /// Gets the unlocked discoveries owned by the player profile.
    /// </summary>
    public DiscoveryCollection Discoveries { get; }
}

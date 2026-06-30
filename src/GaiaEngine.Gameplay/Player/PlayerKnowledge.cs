using System;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Encyclopedia;

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
    /// <param name="encyclopedia">The encyclopedia entries owned by the player profile.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="discoveries"/> or <paramref name="encyclopedia"/> is <see langword="null"/>.
    /// </exception>
    public PlayerKnowledge(DiscoveryCollection discoveries, EncyclopediaCollection encyclopedia)
    {
        Discoveries = discoveries ?? throw new ArgumentNullException(nameof(discoveries));
        Encyclopedia = encyclopedia ?? throw new ArgumentNullException(nameof(encyclopedia));
    }

    /// <summary>
    /// Gets the unlocked discoveries owned by the player profile.
    /// </summary>
    public DiscoveryCollection Discoveries { get; }

    /// <summary>
    /// Gets the encyclopedia entries owned by the player profile.
    /// </summary>
    public EncyclopediaCollection Encyclopedia { get; }
}

using System;

namespace GaiaEngine.Gameplay.Player;

/// <summary>
/// Represents the immutable identity section of one player profile.
/// </summary>
public sealed record PlayerIdentity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerIdentity"/> class.
    /// </summary>
    /// <param name="playerId">The stable player identifier.</param>
    /// <param name="profileName">The player-facing profile name.</param>
    /// <param name="creationDate">The immutable creation date string.</param>
    /// <exception cref="ArgumentException">Thrown when any argument is empty.</exception>
    public PlayerIdentity(string playerId, string profileName, string creationDate)
    {
        if (string.IsNullOrWhiteSpace(playerId))
        {
            throw new ArgumentException("The player identifier must contain a value.", nameof(playerId));
        }

        if (string.IsNullOrWhiteSpace(profileName))
        {
            throw new ArgumentException("The player profile name must contain a value.", nameof(profileName));
        }

        if (string.IsNullOrWhiteSpace(creationDate))
        {
            throw new ArgumentException("The player creation date must contain a value.", nameof(creationDate));
        }

        PlayerId = playerId;
        ProfileName = profileName;
        CreationDate = creationDate;
    }

    /// <summary>
    /// Gets the stable player identifier.
    /// </summary>
    public string PlayerId { get; }

    /// <summary>
    /// Gets the player-facing profile name.
    /// </summary>
    public string ProfileName { get; }

    /// <summary>
    /// Gets the immutable creation date string.
    /// </summary>
    public string CreationDate { get; }
}

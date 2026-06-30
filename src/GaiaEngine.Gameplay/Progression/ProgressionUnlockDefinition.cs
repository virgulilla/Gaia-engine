using System;

namespace GaiaEngine.Gameplay.Progression;

/// <summary>
/// Represents one configurable progression unlock definition.
/// </summary>
public sealed record ProgressionUnlockDefinition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProgressionUnlockDefinition"/> class.
    /// </summary>
    /// <param name="unlockId">The stable unlock identifier.</param>
    /// <param name="category">The unlock category.</param>
    /// <param name="name">The player-facing unlock name.</param>
    /// <param name="description">The player-facing unlock description.</param>
    /// <exception cref="ArgumentException">Thrown when one required text argument is empty.</exception>
    public ProgressionUnlockDefinition(string unlockId, ProgressionUnlockCategory category, string name, string description)
    {
        if (string.IsNullOrWhiteSpace(unlockId))
        {
            throw new ArgumentException("The progression unlock identifier must contain a value.", nameof(unlockId));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The progression unlock name must contain a value.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("The progression unlock description must contain a value.", nameof(description));
        }

        UnlockId = unlockId;
        Category = category;
        Name = name;
        Description = description;
    }

    /// <summary>
    /// Gets the stable unlock identifier.
    /// </summary>
    public string UnlockId { get; }

    /// <summary>
    /// Gets the unlock category.
    /// </summary>
    public ProgressionUnlockCategory Category { get; }

    /// <summary>
    /// Gets the player-facing unlock name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the player-facing unlock description.
    /// </summary>
    public string Description { get; }
}

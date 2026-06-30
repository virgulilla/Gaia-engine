using System;

namespace GaiaEngine.Gameplay.Player;

/// <summary>
/// Represents the persistent progression values owned by a player profile.
/// </summary>
public sealed record PlayerProgression
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerProgression"/> class.
    /// </summary>
    /// <param name="experience">The accumulated player experience.</param>
    /// <param name="discoveries">The total number of unlocked discoveries.</param>
    /// <param name="unlockLevel">The current unlock level.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="experience"/>, <paramref name="discoveries"/>, or <paramref name="unlockLevel"/> is negative.
    /// </exception>
    public PlayerProgression(int experience, int discoveries, int unlockLevel)
    {
        if (experience < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(experience), "The player experience must be zero or greater.");
        }

        if (discoveries < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(discoveries), "The discovery count must be zero or greater.");
        }

        if (unlockLevel < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unlockLevel), "The unlock level must be zero or greater.");
        }

        Experience = experience;
        Discoveries = discoveries;
        UnlockLevel = unlockLevel;
    }

    /// <summary>
    /// Gets the accumulated player experience.
    /// </summary>
    public int Experience { get; }

    /// <summary>
    /// Gets the total number of unlocked discoveries.
    /// </summary>
    public int Discoveries { get; }

    /// <summary>
    /// Gets the current unlock level.
    /// </summary>
    public int UnlockLevel { get; }
}

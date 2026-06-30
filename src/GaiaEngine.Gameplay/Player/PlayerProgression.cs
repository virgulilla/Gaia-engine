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
    /// <param name="completedObjectives">The total number of completed objectives.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="experience"/>, <paramref name="discoveries"/>, <paramref name="unlockLevel"/>,
    /// or <paramref name="completedObjectives"/> is negative.
    /// </exception>
    public PlayerProgression(int experience, int discoveries, int unlockLevel, int completedObjectives)
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

        if (completedObjectives < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(completedObjectives), "The completed objective count must be zero or greater.");
        }

        Experience = experience;
        Discoveries = discoveries;
        UnlockLevel = unlockLevel;
        CompletedObjectives = completedObjectives;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerProgression"/> class with no completed objectives.
    /// </summary>
    /// <param name="experience">The accumulated player experience.</param>
    /// <param name="discoveries">The total number of unlocked discoveries.</param>
    /// <param name="unlockLevel">The current unlock level.</param>
    public PlayerProgression(int experience, int discoveries, int unlockLevel)
        : this(experience, discoveries, unlockLevel, 0)
    {
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

    /// <summary>
    /// Gets the total number of completed objectives.
    /// </summary>
    public int CompletedObjectives { get; }
}

using System;
using GaiaEngine.Gameplay.Progression;

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
    /// <param name="unlocks">The persistent progression unlock identifiers.</param>
    /// <param name="completedMilestones">The persistent completed milestone identifiers.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="experience"/>, <paramref name="discoveries"/>, <paramref name="unlockLevel"/>,
    /// or <paramref name="completedObjectives"/> is negative.
    /// </exception>
    public PlayerProgression(
        int experience,
        int discoveries,
        int unlockLevel,
        int completedObjectives,
        ProgressionUnlockCollection unlocks,
        ProgressionMilestoneCollection completedMilestones)
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
        Unlocks = unlocks ?? throw new ArgumentNullException(nameof(unlocks));
        CompletedMilestones = completedMilestones ?? throw new ArgumentNullException(nameof(completedMilestones));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerProgression"/> class with no completed objectives.
    /// </summary>
    /// <param name="experience">The accumulated player experience.</param>
    /// <param name="discoveries">The total number of unlocked discoveries.</param>
    /// <param name="unlockLevel">The current unlock level.</param>
    public PlayerProgression(int experience, int discoveries, int unlockLevel)
        : this(
            experience,
            discoveries,
            unlockLevel,
            0,
            ProgressionUnlockCollection.Empty,
            ProgressionMilestoneCollection.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PlayerProgression"/> class with no stored unlocks or milestones.
    /// </summary>
    /// <param name="experience">The accumulated player experience.</param>
    /// <param name="discoveries">The total number of unlocked discoveries.</param>
    /// <param name="unlockLevel">The current unlock level.</param>
    /// <param name="completedObjectives">The total number of completed objectives.</param>
    public PlayerProgression(int experience, int discoveries, int unlockLevel, int completedObjectives)
        : this(
            experience,
            discoveries,
            unlockLevel,
            completedObjectives,
            ProgressionUnlockCollection.Empty,
            ProgressionMilestoneCollection.Empty)
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

    /// <summary>
    /// Gets the persistent progression unlock identifiers.
    /// </summary>
    public ProgressionUnlockCollection Unlocks { get; }

    /// <summary>
    /// Gets the persistent completed milestone identifiers.
    /// </summary>
    public ProgressionMilestoneCollection CompletedMilestones { get; }
}

using System;
using System.Collections.Generic;

namespace GaiaEngine.Gameplay.Progression;

/// <summary>
/// Represents one configurable progression level definition.
/// </summary>
public sealed record ProgressionLevelDefinition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProgressionLevelDefinition"/> class.
    /// </summary>
    /// <param name="level">The deterministic progression level.</param>
    /// <param name="requiredExperience">The required experience threshold.</param>
    /// <param name="unlocks">The unlocks granted by the level.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="level"/> or <paramref name="requiredExperience"/> is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="unlocks"/> is <see langword="null"/>.</exception>
    public ProgressionLevelDefinition(int level, int requiredExperience, IReadOnlyList<ProgressionUnlockDefinition> unlocks)
    {
        if (level < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(level), "The progression level must be zero or greater.");
        }

        if (requiredExperience < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(requiredExperience), "The required experience must be zero or greater.");
        }

        Level = level;
        RequiredExperience = requiredExperience;
        Unlocks = unlocks ?? throw new ArgumentNullException(nameof(unlocks));
    }

    /// <summary>
    /// Gets the deterministic progression level.
    /// </summary>
    public int Level { get; }

    /// <summary>
    /// Gets the required experience threshold.
    /// </summary>
    public int RequiredExperience { get; }

    /// <summary>
    /// Gets the unlocks granted by the level.
    /// </summary>
    public IReadOnlyList<ProgressionUnlockDefinition> Unlocks { get; }
}

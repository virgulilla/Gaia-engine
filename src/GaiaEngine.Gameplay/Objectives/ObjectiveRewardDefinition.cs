using System;
using System.Collections.Generic;

namespace GaiaEngine.Gameplay.Objectives;

/// <summary>
/// Represents the deterministic reward granted by one objective.
/// </summary>
public sealed record ObjectiveRewardDefinition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectiveRewardDefinition"/> class.
    /// </summary>
    /// <param name="experience">The experience awarded on completion.</param>
    /// <param name="unlocks">The optional unlock identifiers granted on completion.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="experience"/> is negative.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="unlocks"/> is <see langword="null"/>.</exception>
    public ObjectiveRewardDefinition(int experience, IReadOnlyList<string> unlocks)
    {
        if (experience < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(experience), "The objective reward experience must be zero or greater.");
        }

        Experience = experience;
        Unlocks = unlocks ?? throw new ArgumentNullException(nameof(unlocks));
    }

    /// <summary>
    /// Gets the experience awarded on completion.
    /// </summary>
    public int Experience { get; }

    /// <summary>
    /// Gets the optional unlock identifiers granted on completion.
    /// </summary>
    public IReadOnlyList<string> Unlocks { get; }

    /// <summary>
    /// Gets the shared empty reward definition.
    /// </summary>
    public static ObjectiveRewardDefinition Empty { get; } = new(0, Array.Empty<string>());
}

using System;
using System.Collections.Generic;

namespace GaiaEngine.Gameplay.Progression;

/// <summary>
/// Represents the configurable progression catalog used by the gameplay layer.
/// </summary>
public sealed class ProgressionCatalog
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProgressionCatalog"/> class.
    /// </summary>
    /// <param name="levels">The ordered progression level definitions.</param>
    /// <param name="milestones">The ordered progression milestone definitions.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="levels"/> or <paramref name="milestones"/> is <see langword="null"/>.</exception>
    public ProgressionCatalog(IReadOnlyList<ProgressionLevelDefinition> levels, IReadOnlyList<ProgressionMilestoneDefinition> milestones)
    {
        Levels = levels ?? throw new ArgumentNullException(nameof(levels));
        Milestones = milestones ?? throw new ArgumentNullException(nameof(milestones));
    }

    /// <summary>
    /// Gets the ordered progression level definitions.
    /// </summary>
    public IReadOnlyList<ProgressionLevelDefinition> Levels { get; }

    /// <summary>
    /// Gets the ordered progression milestone definitions.
    /// </summary>
    public IReadOnlyList<ProgressionMilestoneDefinition> Milestones { get; }
}

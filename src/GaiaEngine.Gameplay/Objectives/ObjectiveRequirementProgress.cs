using System;

namespace GaiaEngine.Gameplay.Objectives;

/// <summary>
/// Represents the persistent progress value of one objective requirement.
/// </summary>
public sealed record ObjectiveRequirementProgress
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectiveRequirementProgress"/> class.
    /// </summary>
    /// <param name="requirementId">The stable requirement identifier.</param>
    /// <param name="currentValue">The current progress value.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="requirementId"/> is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="currentValue"/> is negative.</exception>
    public ObjectiveRequirementProgress(string requirementId, int currentValue)
    {
        if (string.IsNullOrWhiteSpace(requirementId))
        {
            throw new ArgumentException("The objective requirement progress identifier must contain a value.", nameof(requirementId));
        }

        if (currentValue < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currentValue), "The objective requirement progress value must be zero or greater.");
        }

        RequirementId = requirementId;
        CurrentValue = currentValue;
    }

    /// <summary>
    /// Gets the stable requirement identifier.
    /// </summary>
    public string RequirementId { get; }

    /// <summary>
    /// Gets the current progress value.
    /// </summary>
    public int CurrentValue { get; }
}

using System;
using GaiaEngine.Gameplay.Discovery;

namespace GaiaEngine.Gameplay.Progression;

/// <summary>
/// Represents one configurable progression milestone definition.
/// </summary>
public sealed record ProgressionMilestoneDefinition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProgressionMilestoneDefinition"/> class.
    /// </summary>
    /// <param name="milestoneId">The stable milestone identifier.</param>
    /// <param name="title">The player-facing milestone title.</param>
    /// <param name="description">The player-facing milestone description.</param>
    /// <param name="requirementType">The deterministic milestone requirement type.</param>
    /// <param name="targetValue">The required target value.</param>
    /// <param name="discoveryCategory">The optional discovery category used by category-specific milestones.</param>
    /// <exception cref="ArgumentException">Thrown when one required text argument is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="targetValue"/> is less than one.</exception>
    public ProgressionMilestoneDefinition(
        string milestoneId,
        string title,
        string description,
        ProgressionMilestoneRequirementType requirementType,
        int targetValue,
        DiscoveryCategory? discoveryCategory = null)
    {
        if (string.IsNullOrWhiteSpace(milestoneId))
        {
            throw new ArgumentException("The progression milestone identifier must contain a value.", nameof(milestoneId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("The progression milestone title must contain a value.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("The progression milestone description must contain a value.", nameof(description));
        }

        if (targetValue < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(targetValue), "The milestone target value must be greater than zero.");
        }

        MilestoneId = milestoneId;
        Title = title;
        Description = description;
        RequirementType = requirementType;
        TargetValue = targetValue;
        DiscoveryCategory = discoveryCategory;
    }

    /// <summary>
    /// Gets the stable milestone identifier.
    /// </summary>
    public string MilestoneId { get; }

    /// <summary>
    /// Gets the player-facing milestone title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the player-facing milestone description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the deterministic milestone requirement type.
    /// </summary>
    public ProgressionMilestoneRequirementType RequirementType { get; }

    /// <summary>
    /// Gets the required target value.
    /// </summary>
    public int TargetValue { get; }

    /// <summary>
    /// Gets the optional discovery category used by category-specific milestones.
    /// </summary>
    public DiscoveryCategory? DiscoveryCategory { get; }
}

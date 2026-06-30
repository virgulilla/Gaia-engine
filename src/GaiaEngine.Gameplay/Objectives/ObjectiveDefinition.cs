using System;
using System.Collections.Generic;

namespace GaiaEngine.Gameplay.Objectives;

/// <summary>
/// Represents one deterministic objective definition.
/// </summary>
public sealed record ObjectiveDefinition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectiveDefinition"/> class.
    /// </summary>
    /// <param name="objectiveId">The stable objective identifier.</param>
    /// <param name="category">The objective category.</param>
    /// <param name="title">The player-facing objective title.</param>
    /// <param name="description">The player-facing objective description.</param>
    /// <param name="requirements">The ordered objective requirements.</param>
    /// <param name="reward">The deterministic reward definition.</param>
    /// <param name="initialStatus">The initial lifecycle status.</param>
    /// <exception cref="ArgumentException">Thrown when one required text argument is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="requirements"/> or <paramref name="reward"/> is <see langword="null"/>.</exception>
    public ObjectiveDefinition(
        string objectiveId,
        ObjectiveCategory category,
        string title,
        string description,
        IReadOnlyList<ObjectiveRequirementDefinition> requirements,
        ObjectiveRewardDefinition reward,
        ObjectiveStatus initialStatus)
    {
        if (string.IsNullOrWhiteSpace(objectiveId))
        {
            throw new ArgumentException("The objective identifier must contain a value.", nameof(objectiveId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("The objective title must contain a value.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("The objective description must contain a value.", nameof(description));
        }

        ObjectiveId = objectiveId;
        Category = category;
        Title = title;
        Description = description;
        Requirements = requirements ?? throw new ArgumentNullException(nameof(requirements));
        Reward = reward ?? throw new ArgumentNullException(nameof(reward));
        InitialStatus = initialStatus;
    }

    /// <summary>
    /// Gets the stable objective identifier.
    /// </summary>
    public string ObjectiveId { get; }

    /// <summary>
    /// Gets the objective category.
    /// </summary>
    public ObjectiveCategory Category { get; }

    /// <summary>
    /// Gets the player-facing objective title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the player-facing objective description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the ordered objective requirements.
    /// </summary>
    public IReadOnlyList<ObjectiveRequirementDefinition> Requirements { get; }

    /// <summary>
    /// Gets the deterministic reward definition.
    /// </summary>
    public ObjectiveRewardDefinition Reward { get; }

    /// <summary>
    /// Gets the initial lifecycle status.
    /// </summary>
    public ObjectiveStatus InitialStatus { get; }
}

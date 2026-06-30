using System;
using System.Collections.Generic;

namespace GaiaEngine.Gameplay.Objectives;

/// <summary>
/// Represents one persistent objective entry owned by a player profile.
/// </summary>
public sealed record ObjectiveEntry
{
    private readonly IReadOnlyList<ObjectiveRequirementProgress> progress;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectiveEntry"/> class.
    /// </summary>
    /// <param name="objectiveId">The stable objective identifier.</param>
    /// <param name="category">The objective category.</param>
    /// <param name="title">The player-facing title.</param>
    /// <param name="description">The player-facing description.</param>
    /// <param name="requirements">The ordered objective requirements.</param>
    /// <param name="progress">The ordered requirement progress values.</param>
    /// <param name="reward">The deterministic reward definition.</param>
    /// <param name="status">The current lifecycle status.</param>
    /// <exception cref="ArgumentException">Thrown when one required text argument is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when one reference argument is <see langword="null"/>.</exception>
    public ObjectiveEntry(
        string objectiveId,
        ObjectiveCategory category,
        string title,
        string description,
        IReadOnlyList<ObjectiveRequirementDefinition> requirements,
        IReadOnlyList<ObjectiveRequirementProgress> progress,
        ObjectiveRewardDefinition reward,
        ObjectiveStatus status)
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
        this.progress = progress ?? throw new ArgumentNullException(nameof(progress));
        Reward = reward ?? throw new ArgumentNullException(nameof(reward));
        Status = status;
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
    /// Gets the player-facing title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the player-facing description.
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
    /// Gets the current lifecycle status.
    /// </summary>
    public ObjectiveStatus Status { get; }

    /// <summary>
    /// Returns the ordered requirement progress values.
    /// </summary>
    /// <returns>The ordered requirement progress values.</returns>
    public IReadOnlyList<ObjectiveRequirementProgress> GetProgress()
    {
        return progress;
    }
}

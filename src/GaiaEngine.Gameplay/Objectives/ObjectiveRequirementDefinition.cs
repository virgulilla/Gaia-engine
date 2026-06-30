using System;
using GaiaEngine.Gameplay.Discovery;

namespace GaiaEngine.Gameplay.Objectives;

/// <summary>
/// Represents one data-driven requirement attached to an objective definition.
/// </summary>
public sealed record ObjectiveRequirementDefinition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectiveRequirementDefinition"/> class.
    /// </summary>
    /// <param name="requirementId">The stable requirement identifier.</param>
    /// <param name="type">The requirement evaluation mode.</param>
    /// <param name="targetCount">The required target count.</param>
    /// <param name="discoveryCategory">The optional discovery category used for count-based requirements.</param>
    /// <param name="signalKey">The optional signal key used for direct signal matching.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="requirementId"/> is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="targetCount"/> is less than one.</exception>
    public ObjectiveRequirementDefinition(
        string requirementId,
        ObjectiveRequirementType type,
        int targetCount,
        DiscoveryCategory? discoveryCategory = null,
        string? signalKey = null)
    {
        if (string.IsNullOrWhiteSpace(requirementId))
        {
            throw new ArgumentException("The objective requirement identifier must contain a value.", nameof(requirementId));
        }

        if (targetCount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(targetCount), "The objective target count must be greater than zero.");
        }

        RequirementId = requirementId;
        Type = type;
        TargetCount = targetCount;
        DiscoveryCategory = discoveryCategory;
        SignalKey = signalKey;
    }

    /// <summary>
    /// Gets the stable requirement identifier.
    /// </summary>
    public string RequirementId { get; }

    /// <summary>
    /// Gets the requirement evaluation mode.
    /// </summary>
    public ObjectiveRequirementType Type { get; }

    /// <summary>
    /// Gets the required target count.
    /// </summary>
    public int TargetCount { get; }

    /// <summary>
    /// Gets the optional discovery category associated with the requirement.
    /// </summary>
    public DiscoveryCategory? DiscoveryCategory { get; }

    /// <summary>
    /// Gets the optional signal key associated with the requirement.
    /// </summary>
    public string? SignalKey { get; }
}

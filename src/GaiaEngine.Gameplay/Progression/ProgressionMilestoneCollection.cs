using System;
using System.Collections.Generic;

namespace GaiaEngine.Gameplay.Progression;

/// <summary>
/// Represents the deterministic set of completed milestone identifiers owned by one player profile.
/// </summary>
public sealed class ProgressionMilestoneCollection
{
    private readonly HashSet<string> milestonesById;
    private readonly IReadOnlyList<string> orderedMilestones;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProgressionMilestoneCollection"/> class.
    /// </summary>
    /// <param name="milestoneIds">The stored completed milestone identifiers.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="milestoneIds"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when a duplicate milestone identifier is detected.</exception>
    public ProgressionMilestoneCollection(IReadOnlyList<string> milestoneIds)
    {
        ArgumentNullException.ThrowIfNull(milestoneIds);

        milestonesById = new HashSet<string>(milestoneIds.Count, StringComparer.Ordinal);
        List<string> ordered = new(milestoneIds.Count);
        foreach (string milestoneId in milestoneIds)
        {
            if (string.IsNullOrWhiteSpace(milestoneId))
            {
                throw new ArgumentException("One progression milestone identifier is empty.", nameof(milestoneIds));
            }

            if (!milestonesById.Add(milestoneId))
            {
                throw new ArgumentException($"The progression milestone '{milestoneId}' is duplicated.", nameof(milestoneIds));
            }

            ordered.Add(milestoneId);
        }

        ordered.Sort(StringComparer.Ordinal);
        orderedMilestones = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty milestone collection.
    /// </summary>
    public static ProgressionMilestoneCollection Empty { get; } = new(Array.Empty<string>());

    /// <summary>
    /// Gets the number of stored milestone identifiers.
    /// </summary>
    public int Count => orderedMilestones.Count;

    /// <summary>
    /// Returns the completed milestone identifiers in deterministic order.
    /// </summary>
    /// <returns>The ordered completed milestone identifiers.</returns>
    public IReadOnlyList<string> GetAll()
    {
        return orderedMilestones;
    }

    /// <summary>
    /// Determines whether one milestone identifier exists.
    /// </summary>
    /// <param name="milestoneId">The milestone identifier to test.</param>
    /// <returns><see langword="true"/> when the identifier exists; otherwise <see langword="false"/>.</returns>
    public bool Contains(string milestoneId)
    {
        return !string.IsNullOrWhiteSpace(milestoneId) && milestonesById.Contains(milestoneId);
    }
}

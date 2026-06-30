using System;
using System.Collections.Generic;

namespace GaiaEngine.Gameplay.Encyclopedia;

/// <summary>
/// Represents one permanent encyclopedia entry stored in the player profile.
/// </summary>
public sealed class EncyclopediaEntry
{
    private readonly IReadOnlyList<string> relatedEntries;
    private readonly IReadOnlyList<EncyclopediaStatistic> statistics;

    /// <summary>
    /// Initializes a new instance of the <see cref="EncyclopediaEntry"/> class.
    /// </summary>
    /// <param name="entryId">The stable encyclopedia entry identifier.</param>
    /// <param name="category">The encyclopedia category.</param>
    /// <param name="title">The player-facing title.</param>
    /// <param name="description">The player-facing description.</param>
    /// <param name="unlockState">The current unlock state.</param>
    /// <param name="discoveryDate">The discovery date string.</param>
    /// <param name="relatedEntries">The related entry identifiers.</param>
    /// <param name="statistics">The entry statistics.</param>
    /// <exception cref="ArgumentException">Thrown when required textual values are empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when collections are <see langword="null"/>.</exception>
    public EncyclopediaEntry(
        string entryId,
        EncyclopediaCategory category,
        string title,
        string description,
        EncyclopediaUnlockState unlockState,
        string discoveryDate,
        IReadOnlyList<string> relatedEntries,
        IReadOnlyList<EncyclopediaStatistic> statistics)
    {
        if (string.IsNullOrWhiteSpace(entryId))
        {
            throw new ArgumentException("The encyclopedia entry identifier must contain a value.", nameof(entryId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("The encyclopedia entry title must contain a value.", nameof(title));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("The encyclopedia entry description must contain a value.", nameof(description));
        }

        if (string.IsNullOrWhiteSpace(discoveryDate))
        {
            throw new ArgumentException("The encyclopedia entry discovery date must contain a value.", nameof(discoveryDate));
        }

        ArgumentNullException.ThrowIfNull(relatedEntries);
        ArgumentNullException.ThrowIfNull(statistics);

        EntryId = entryId;
        Category = category;
        Title = title;
        Description = description;
        UnlockState = unlockState;
        DiscoveryDate = discoveryDate;

        List<string> orderedRelatedEntries = new(relatedEntries.Count);
        foreach (string entry in relatedEntries)
        {
            if (string.IsNullOrWhiteSpace(entry))
            {
                throw new ArgumentException("The encyclopedia related entry list cannot contain empty values.", nameof(relatedEntries));
            }

            orderedRelatedEntries.Add(entry);
        }

        orderedRelatedEntries.Sort(StringComparer.Ordinal);
        this.relatedEntries = orderedRelatedEntries.AsReadOnly();

        List<EncyclopediaStatistic> orderedStatistics = new(statistics.Count);
        foreach (EncyclopediaStatistic statistic in statistics)
        {
            orderedStatistics.Add(statistic ?? throw new ArgumentNullException(nameof(statistics), "The encyclopedia statistics list cannot contain null values."));
        }

        orderedStatistics.Sort(static (left, right) => string.CompareOrdinal(left.Key, right.Key));
        this.statistics = orderedStatistics.AsReadOnly();
    }

    /// <summary>
    /// Gets the stable encyclopedia entry identifier.
    /// </summary>
    public string EntryId { get; }

    /// <summary>
    /// Gets the encyclopedia category.
    /// </summary>
    public EncyclopediaCategory Category { get; }

    /// <summary>
    /// Gets the player-facing title.
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Gets the player-facing description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the current unlock state.
    /// </summary>
    public EncyclopediaUnlockState UnlockState { get; }

    /// <summary>
    /// Gets the discovery date string.
    /// </summary>
    public string DiscoveryDate { get; }

    /// <summary>
    /// Returns the related entry identifiers in deterministic order.
    /// </summary>
    /// <returns>The related entry identifiers.</returns>
    public IReadOnlyList<string> GetRelatedEntries()
    {
        return relatedEntries;
    }

    /// <summary>
    /// Returns the entry statistics in deterministic order.
    /// </summary>
    /// <returns>The entry statistics.</returns>
    public IReadOnlyList<EncyclopediaStatistic> GetStatistics()
    {
        return statistics;
    }
}

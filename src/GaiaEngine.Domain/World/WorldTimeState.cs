using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the stored temporal state of a world.
/// </summary>
public sealed record WorldTimeState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorldTimeState"/> class.
    /// </summary>
    /// <param name="currentTick">The current simulation tick.</param>
    /// <param name="currentDay">The current world day.</param>
    /// <param name="currentSeason">The current season name.</param>
    /// <param name="currentYear">The current world year.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when numeric values are negative.</exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="currentSeason"/> is empty.</exception>
    public WorldTimeState(long currentTick, int currentDay, string currentSeason, int currentYear)
    {
        if (currentTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currentTick), "The current tick must be zero or greater.");
        }

        if (currentDay < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currentDay), "The current day must be zero or greater.");
        }

        if (string.IsNullOrWhiteSpace(currentSeason))
        {
            throw new ArgumentException("The current season must contain a value.", nameof(currentSeason));
        }

        if (currentYear < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currentYear), "The current year must be zero or greater.");
        }

        CurrentTick = currentTick;
        CurrentDay = currentDay;
        CurrentSeason = currentSeason;
        CurrentYear = currentYear;
    }

    /// <summary>
    /// Gets the current simulation tick.
    /// </summary>
    public long CurrentTick { get; }

    /// <summary>
    /// Gets the current world day.
    /// </summary>
    public int CurrentDay { get; }

    /// <summary>
    /// Gets the current season name.
    /// </summary>
    public string CurrentSeason { get; }

    /// <summary>
    /// Gets the current world year.
    /// </summary>
    public int CurrentYear { get; }
}

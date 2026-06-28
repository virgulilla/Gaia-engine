using System;
using System.Collections.Generic;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.Time;

/// <summary>
/// Implements the deterministic simulation Time System defined by the simulation specifications.
/// </summary>
public sealed class DeterministicTimeSystem : ITimeSystem
{
    private const int SeasonsPerYear = 4;
    private readonly SimulationCalendar calendar;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicTimeSystem"/> class.
    /// </summary>
    /// <param name="calendar">The deterministic calendar configuration.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="calendar"/> is <see langword="null"/>.</exception>
    public DeterministicTimeSystem(SimulationCalendar calendar)
    {
        this.calendar = calendar ?? throw new ArgumentNullException(nameof(calendar));
    }

    /// <summary>
    /// Advances the supplied world time state by one deterministic simulation tick.
    /// </summary>
    /// <param name="timeState">The current world time state.</param>
    /// <returns>The deterministic advance result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="timeState"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when the current season is not supported.</exception>
    public TimeAdvanceResult Advance(WorldTimeState timeState)
    {
        ArgumentNullException.ThrowIfNull(timeState);

        int currentSeasonIndex = GetSeasonIndex(timeState.CurrentSeason);
        long nextTick = checked(timeState.CurrentTick + 1);
        int currentDay = timeState.CurrentDay;
        int currentYear = timeState.CurrentYear;
        int nextSeasonIndex = currentSeasonIndex;

        List<TemporalTransition> transitions = new(3);

        if ((nextTick % calendar.TicksPerDay) == 0)
        {
            currentDay++;

            WorldTimeState dayState = new(nextTick, currentDay, GetSeasonName(nextSeasonIndex), currentYear);
            transitions.Add(new TemporalTransition(TemporalTransitionKind.NewDay, dayState));

            if (currentDay >= calendar.DaysPerSeason)
            {
                currentDay = 0;
                nextSeasonIndex = (currentSeasonIndex + 1) % SeasonsPerYear;

                WorldTimeState seasonState = new(nextTick, currentDay, GetSeasonName(nextSeasonIndex), currentYear);
                transitions.Add(new TemporalTransition(TemporalTransitionKind.NewSeason, seasonState));

                if (nextSeasonIndex == 0)
                {
                    currentYear++;

                    WorldTimeState yearState = new(nextTick, currentDay, GetSeasonName(nextSeasonIndex), currentYear);
                    transitions.Add(new TemporalTransition(TemporalTransitionKind.NewYear, yearState));
                }
            }
        }

        WorldTimeState nextState = new(nextTick, currentDay, GetSeasonName(nextSeasonIndex), currentYear);
        return new TimeAdvanceResult(nextState, transitions.AsReadOnly());
    }

    private static int GetSeasonIndex(string season)
    {
        if (!Enum.TryParse(season, ignoreCase: false, out SimulationSeason parsedSeason))
        {
            throw new ArgumentException("The supplied world time state contains an unsupported season.", nameof(season));
        }

        return parsedSeason switch
        {
            SimulationSeason.Spring => 0,
            SimulationSeason.Summer => 1,
            SimulationSeason.Autumn => 2,
            SimulationSeason.Winter => 3,
            _ => throw new ArgumentOutOfRangeException(nameof(season), "The supplied world time state contains an unsupported season."),
        };
    }

    private static string GetSeasonName(int seasonIndex)
    {
        return seasonIndex switch
        {
            0 => SimulationSeason.Spring.ToString(),
            1 => SimulationSeason.Summer.ToString(),
            2 => SimulationSeason.Autumn.ToString(),
            3 => SimulationSeason.Winter.ToString(),
            _ => throw new ArgumentOutOfRangeException(nameof(seasonIndex), "The season index must resolve to a supported season."),
        };
    }
}

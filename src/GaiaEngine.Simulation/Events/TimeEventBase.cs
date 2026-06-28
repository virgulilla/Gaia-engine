using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Engine.Events;

namespace GaiaEngine.Simulation.Events;

/// <summary>
/// Provides the common immutable metadata shared by deterministic simulation time events.
/// </summary>
public abstract record TimeEventBase : EventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeEventBase"/> class.
    /// </summary>
    /// <param name="eventId">The immutable event identifier.</param>
    /// <param name="priority">The deterministic event priority.</param>
    /// <param name="tick">The target simulation tick for dispatch.</param>
    /// <param name="timestamp">The deterministic timestamp associated with the event.</param>
    /// <param name="currentDay">The current world day after the transition.</param>
    /// <param name="currentSeason">The current world season after the transition.</param>
    /// <param name="currentYear">The current world year after the transition.</param>
    protected TimeEventBase(
        EventId eventId,
        EventPriority priority,
        long tick,
        long timestamp,
        int currentDay,
        string currentSeason,
        int currentYear)
        : base(eventId, EventCategory.Simulation, new EventSource("TimeSystem"), priority, tick, timestamp)
    {
        CurrentDay = currentDay;
        CurrentSeason = currentSeason;
        CurrentYear = currentYear;
    }

    /// <summary>
    /// Gets the current world day after the transition.
    /// </summary>
    public int CurrentDay { get; }

    /// <summary>
    /// Gets the current world season after the transition.
    /// </summary>
    public string CurrentSeason { get; }

    /// <summary>
    /// Gets the current world year after the transition.
    /// </summary>
    public int CurrentYear { get; }
}

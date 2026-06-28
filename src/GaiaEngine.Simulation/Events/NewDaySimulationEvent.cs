using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Engine.Events;

namespace GaiaEngine.Simulation.Events;

/// <summary>
/// Represents the deterministic simulation event emitted when a new world day begins.
/// </summary>
public sealed record NewDaySimulationEvent : TimeEventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NewDaySimulationEvent"/> class.
    /// </summary>
    /// <param name="eventId">The immutable event identifier.</param>
    /// <param name="tick">The target simulation tick for dispatch.</param>
    /// <param name="timestamp">The deterministic timestamp associated with the event.</param>
    /// <param name="currentDay">The current world day after the transition.</param>
    /// <param name="currentSeason">The current world season after the transition.</param>
    /// <param name="currentYear">The current world year after the transition.</param>
    public NewDaySimulationEvent(
        EventId eventId,
        long tick,
        long timestamp,
        int currentDay,
        string currentSeason,
        int currentYear)
        : base(eventId, EventPriority.Normal, tick, timestamp, currentDay, currentSeason, currentYear)
    {
    }
}

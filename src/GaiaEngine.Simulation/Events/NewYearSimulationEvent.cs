using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Engine.Events;

namespace GaiaEngine.Simulation.Events;

/// <summary>
/// Represents the deterministic simulation event emitted when a new world year begins.
/// </summary>
public sealed record NewYearSimulationEvent : TimeEventBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NewYearSimulationEvent"/> class.
    /// </summary>
    /// <param name="eventId">The immutable event identifier.</param>
    /// <param name="tick">The target simulation tick for dispatch.</param>
    /// <param name="timestamp">The deterministic timestamp associated with the event.</param>
    /// <param name="currentDay">The current world day after the transition.</param>
    /// <param name="currentSeason">The current world season after the transition.</param>
    /// <param name="currentYear">The current world year after the transition.</param>
    public NewYearSimulationEvent(
        EventId eventId,
        long tick,
        long timestamp,
        int currentDay,
        string currentSeason,
        int currentYear)
        : base(eventId, EventPriority.Critical, tick, timestamp, currentDay, currentSeason, currentYear)
    {
    }
}

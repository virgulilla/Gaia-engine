using System.Collections.Generic;
using GaiaEngine.Engine.Events;
using GaiaEngine.Gameplay.Discovery;

namespace GaiaEngine.Gameplay.Objectives;

/// <summary>
/// Creates deterministic objective signals from unlocked discoveries and simulation events.
/// </summary>
public static class ObjectiveSignalFactory
{
    /// <summary>
    /// Creates objective signals from the supplied discoveries and simulation events.
    /// </summary>
    /// <param name="unlockedDiscoveries">The discoveries unlocked during the current pass.</param>
    /// <param name="events">The simulation events published during the current tick.</param>
    /// <returns>The deterministic objective signals.</returns>
    public static IReadOnlyList<ObjectiveSignal> CreateSignals(
        IReadOnlyList<DiscoveryEntry> unlockedDiscoveries,
        IReadOnlyList<IEvent> events)
    {
        List<ObjectiveSignal> signals = new(unlockedDiscoveries.Count + events.Count);

        foreach (DiscoveryEntry discovery in unlockedDiscoveries)
        {
            signals.Add(new ObjectiveSignal(discovery.Category, discovery.DiscoveryId));
        }

        foreach (IEvent eventInstance in events)
        {
            signals.Add(new ObjectiveSignal(DiscoveryCategory.WorldEvents, eventInstance.EventType));
        }

        return signals.AsReadOnly();
    }
}

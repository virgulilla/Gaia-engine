using System;
using System.Collections.Generic;
using GaiaEngine.Engine.Events;

namespace GaiaEngine.Simulation.Events;

/// <summary>
/// Represents the deterministic result of publishing simulation events during one tick.
/// </summary>
public sealed record SimulationEventPublicationResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationEventPublicationResult"/> class.
    /// </summary>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <param name="publishedEvents">The ordered immutable events published during the tick.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="publishedEvents"/> is <see langword="null"/>.</exception>
    public SimulationEventPublicationResult(ulong nextEventSequence, IReadOnlyList<IEvent> publishedEvents)
    {
        NextEventSequence = nextEventSequence;
        PublishedEvents = publishedEvents ?? throw new ArgumentNullException(nameof(publishedEvents));
    }

    /// <summary>
    /// Gets the next deterministic event sequence value to use.
    /// </summary>
    public ulong NextEventSequence { get; }

    /// <summary>
    /// Gets the ordered immutable events published during the tick.
    /// </summary>
    public IReadOnlyList<IEvent> PublishedEvents { get; }
}

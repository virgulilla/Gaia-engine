using System;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Engine.Events;

/// <summary>
/// Provides a base immutable event implementation with standard metadata.
/// </summary>
public abstract record EventBase : IEvent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventBase"/> class.
    /// </summary>
    /// <param name="eventId">The immutable event identifier.</param>
    /// <param name="category">The event category.</param>
    /// <param name="source">The logical event source.</param>
    /// <param name="priority">The deterministic event priority.</param>
    /// <param name="tick">The target simulation tick for dispatch.</param>
    /// <param name="timestamp">The deterministic timestamp associated with the event.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="tick"/> or <paramref name="timestamp"/> is negative.
    /// </exception>
    protected EventBase(
        EventId eventId,
        EventCategory category,
        EventSource source,
        EventPriority priority,
        long tick,
        long timestamp)
    {
        if (tick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tick), "The event tick must be zero or greater.");
        }

        if (timestamp < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(timestamp), "The event timestamp must be zero or greater.");
        }

        EventId = eventId;
        Category = category;
        Source = source;
        Priority = priority;
        Tick = tick;
        Timestamp = timestamp;
    }

    /// <summary>
    /// Gets the immutable event identifier.
    /// </summary>
    public EventId EventId { get; }

    /// <summary>
    /// Gets the event category.
    /// </summary>
    public EventCategory Category { get; }

    /// <summary>
    /// Gets the logical event type name.
    /// </summary>
    public string EventType => GetType().Name;

    /// <summary>
    /// Gets the logical event source.
    /// </summary>
    public EventSource Source { get; }

    /// <summary>
    /// Gets the deterministic event priority.
    /// </summary>
    public EventPriority Priority { get; }

    /// <summary>
    /// Gets the target simulation tick for dispatch.
    /// </summary>
    public long Tick { get; }

    /// <summary>
    /// Gets the deterministic timestamp associated with the event.
    /// </summary>
    public long Timestamp { get; }
}

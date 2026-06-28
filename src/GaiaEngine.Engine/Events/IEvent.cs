using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Engine.Events;

/// <summary>
/// Defines the immutable metadata required by every runtime event.
/// </summary>
public interface IEvent
{
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
    public string EventType { get; }

    /// <summary>
    /// Gets the logical event source.
    /// </summary>
    public EventSource Source { get; }

    /// <summary>
    /// Gets the deterministic event priority.
    /// </summary>
    public EventPriority Priority { get; }

    /// <summary>
    /// Gets the target simulation tick for event dispatch.
    /// </summary>
    public long Tick { get; }

    /// <summary>
    /// Gets the deterministic timestamp associated with the event.
    /// </summary>
    public long Timestamp { get; }
}

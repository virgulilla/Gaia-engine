using System;

namespace GaiaEngine.Engine.Events;

/// <summary>
/// Represents a subscriber failure that occurred during event dispatch.
/// </summary>
public sealed record EventDispatchFailure
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventDispatchFailure"/> class.
    /// </summary>
    /// <param name="eventType">The logical event type.</param>
    /// <param name="subscriberOrder">The deterministic subscriber order.</param>
    /// <param name="exception">The captured subscriber exception.</param>
    public EventDispatchFailure(string eventType, int subscriberOrder, Exception exception)
    {
        EventType = eventType;
        SubscriberOrder = subscriberOrder;
        Exception = exception;
    }

    /// <summary>
    /// Gets the logical event type.
    /// </summary>
    public string EventType { get; }

    /// <summary>
    /// Gets the deterministic subscriber order.
    /// </summary>
    public int SubscriberOrder { get; }

    /// <summary>
    /// Gets the captured exception.
    /// </summary>
    public Exception Exception { get; }
}

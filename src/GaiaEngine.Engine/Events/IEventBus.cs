namespace GaiaEngine.Engine.Events;

/// <summary>
/// Provides deterministic publication, subscription, and dispatch of runtime events.
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publishes an immutable event into the deterministic event queue.
    /// </summary>
    /// <param name="eventInstance">The event instance to enqueue.</param>
    public void Publish(IEvent eventInstance);

    /// <summary>
    /// Subscribes a handler to a specific event type.
    /// </summary>
    /// <typeparam name="TEvent">The event type.</typeparam>
    /// <param name="handler">The event handler.</param>
    /// <returns>A disposable subscription handle.</returns>
    public EventSubscription Subscribe<TEvent>(EventHandler<TEvent> handler)
        where TEvent : class, IEvent;

    /// <summary>
    /// Dispatches all events scheduled up to and including the specified tick.
    /// </summary>
    /// <param name="tick">The current simulation tick.</param>
    /// <returns>The dispatch result.</returns>
    public EventDispatchResult Dispatch(long tick);

    /// <summary>
    /// Gets the number of queued events awaiting dispatch.
    /// </summary>
    public int PendingEventCount { get; }
}

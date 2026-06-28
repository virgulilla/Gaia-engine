namespace GaiaEngine.Engine.Events;

/// <summary>
/// Represents a deterministic handler for a strongly typed event.
/// </summary>
/// <typeparam name="TEvent">The event type.</typeparam>
/// <param name="eventInstance">The published event instance.</param>
public delegate void EventHandler<in TEvent>(TEvent eventInstance)
    where TEvent : class, IEvent;

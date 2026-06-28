using System;
using System.Collections.Generic;

namespace GaiaEngine.Engine.Events;

/// <summary>
/// Implements a deterministic in-memory event bus for runtime event delivery.
/// </summary>
public sealed class EventBus : IEventBus
{
    private readonly object syncRoot = new();
    private readonly List<QueuedEvent> queuedEvents = new();
    private readonly Dictionary<Type, List<SubscriberRegistration>> subscribers = new();
    private ulong nextPublicationOrder;
    private int nextSubscriberOrder;

    /// <summary>
    /// Gets the number of queued events awaiting dispatch.
    /// </summary>
    public int PendingEventCount
    {
        get
        {
            lock (syncRoot)
            {
                return queuedEvents.Count;
            }
        }
    }

    /// <summary>
    /// Publishes an immutable event into the deterministic event queue.
    /// </summary>
    /// <param name="eventInstance">The event instance to enqueue.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="eventInstance"/> is <see langword="null"/>.</exception>
    public void Publish(IEvent eventInstance)
    {
        ArgumentNullException.ThrowIfNull(eventInstance);

        lock (syncRoot)
        {
            queuedEvents.Add(new QueuedEvent(eventInstance, nextPublicationOrder));
            nextPublicationOrder++;
        }
    }

    /// <summary>
    /// Subscribes a handler to a specific event type.
    /// </summary>
    /// <typeparam name="TEvent">The event type.</typeparam>
    /// <param name="handler">The event handler.</param>
    /// <returns>A disposable subscription handle.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="handler"/> is <see langword="null"/>.</exception>
    public EventSubscription Subscribe<TEvent>(EventHandler<TEvent> handler)
        where TEvent : class, IEvent
    {
        ArgumentNullException.ThrowIfNull(handler);

        int subscriberOrder;
        lock (syncRoot)
        {
            Type eventType = typeof(TEvent);
            if (!subscribers.TryGetValue(eventType, out List<SubscriberRegistration>? registrations))
            {
                registrations = new List<SubscriberRegistration>();
                subscribers.Add(eventType, registrations);
            }

            subscriberOrder = nextSubscriberOrder;
            nextSubscriberOrder++;
            registrations.Add(new SubscriberRegistration(subscriberOrder, evt => handler((TEvent)evt)));
        }

        return new EventSubscription(() => Unsubscribe<TEvent>(subscriberOrder));
    }

    /// <summary>
    /// Dispatches all events scheduled up to and including the specified tick.
    /// </summary>
    /// <param name="tick">The current simulation tick.</param>
    /// <returns>The dispatch result.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="tick"/> is negative.</exception>
    public EventDispatchResult Dispatch(long tick)
    {
        if (tick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tick), "The dispatch tick must be zero or greater.");
        }

        List<QueuedEvent> dueEvents = new();
        Dictionary<Type, SubscriberRegistration[]> subscriberSnapshot = new();

        lock (syncRoot)
        {
            for (int index = queuedEvents.Count - 1; index >= 0; index--)
            {
                QueuedEvent queuedEvent = queuedEvents[index];
                if (queuedEvent.Event.Tick <= tick)
                {
                    dueEvents.Add(queuedEvent);
                    queuedEvents.RemoveAt(index);
                }
            }

            dueEvents.Sort(QueuedEventComparer.Instance);

            foreach (KeyValuePair<Type, List<SubscriberRegistration>> pair in subscribers)
            {
                subscriberSnapshot.Add(pair.Key, pair.Value.ToArray());
            }
        }

        List<EventDispatchFailure> failures = new();
        foreach (QueuedEvent queuedEvent in dueEvents)
        {
            Type eventType = queuedEvent.Event.GetType();
            if (!subscriberSnapshot.TryGetValue(eventType, out SubscriberRegistration[]? registrations))
            {
                continue;
            }

            foreach (SubscriberRegistration registration in registrations)
            {
                try
                {
                    registration.Handler(queuedEvent.Event);
                }
                catch (Exception exception)
                {
                    failures.Add(new EventDispatchFailure(queuedEvent.Event.EventType, registration.Order, exception));
                }
            }
        }

        return new EventDispatchResult(dueEvents.Count, failures.Count, failures.AsReadOnly());
    }

    private void Unsubscribe<TEvent>(int subscriberOrder)
        where TEvent : class, IEvent
    {
        lock (syncRoot)
        {
            if (!subscribers.TryGetValue(typeof(TEvent), out List<SubscriberRegistration>? registrations))
            {
                return;
            }

            for (int index = 0; index < registrations.Count; index++)
            {
                if (registrations[index].Order == subscriberOrder)
                {
                    registrations.RemoveAt(index);
                    break;
                }
            }

            if (registrations.Count == 0)
            {
                subscribers.Remove(typeof(TEvent));
            }
        }
    }

    private readonly record struct QueuedEvent(IEvent Event, ulong PublicationOrder);

    private sealed class QueuedEventComparer : IComparer<QueuedEvent>
    {
        public static readonly QueuedEventComparer Instance = new();

        public int Compare(QueuedEvent x, QueuedEvent y)
        {
            int tickComparison = x.Event.Tick.CompareTo(y.Event.Tick);
            if (tickComparison != 0)
            {
                return tickComparison;
            }

            int priorityComparison = y.Event.Priority.CompareTo(x.Event.Priority);
            if (priorityComparison != 0)
            {
                return priorityComparison;
            }

            return x.PublicationOrder.CompareTo(y.PublicationOrder);
        }
    }

    private readonly record struct SubscriberRegistration(int Order, Action<IEvent> Handler);
}

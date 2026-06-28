using System;
using System.Collections.Generic;
using GaiaEngine.Engine.Events;
using GaiaEngine.Engine.Identifiers;
using Xunit;

namespace GaiaEngine.Engine.Tests.Events;

public sealed class EventBusTests
{
    [Fact]
    public void Dispatch_ShouldProcessEventsByTickThenPriorityThenPublicationOrder()
    {
        EventBus eventBus = new();
        List<int> processedOrders = new();

        eventBus.Subscribe<TestEvent>(evt => processedOrders.Add(evt.Order));

        eventBus.Publish(new TestEvent(EventId.FromSequence(new EntitySequence(1)), EventPriority.Low, 2, 2));
        eventBus.Publish(new TestEvent(EventId.FromSequence(new EntitySequence(2)), EventPriority.High, 1, 0));
        eventBus.Publish(new TestEvent(EventId.FromSequence(new EntitySequence(3)), EventPriority.Normal, 1, 1));

        EventDispatchResult result = eventBus.Dispatch(2);

        Assert.Equal(3, result.ProcessedEventCount);
        Assert.Equal(new[] { 0, 1, 2 }, processedOrders);
    }

    [Fact]
    public void Dispatch_ShouldLeaveFutureEventsQueued()
    {
        EventBus eventBus = new();
        eventBus.Publish(new TestEvent(EventId.FromSequence(new EntitySequence(1)), EventPriority.Normal, 5, 0));

        EventDispatchResult result = eventBus.Dispatch(4);

        Assert.Equal(0, result.ProcessedEventCount);
        Assert.Equal(1, eventBus.PendingEventCount);
    }

    [Fact]
    public void Dispatch_ShouldInvokeSubscribersInSubscriptionOrder()
    {
        EventBus eventBus = new();
        List<int> subscriberOrder = new();

        eventBus.Subscribe<TestEvent>(_ => subscriberOrder.Add(0));
        eventBus.Subscribe<TestEvent>(_ => subscriberOrder.Add(1));

        eventBus.Publish(new TestEvent(EventId.FromSequence(new EntitySequence(1)), EventPriority.Normal, 0, 0));
        eventBus.Dispatch(0);

        Assert.Equal(new[] { 0, 1 }, subscriberOrder);
    }

    [Fact]
    public void Dispatch_ShouldContinueWhenASubscriberFails()
    {
        EventBus eventBus = new();
        List<int> subscriberOrder = new();

        eventBus.Subscribe<TestEvent>(_ => throw new InvalidOperationException("Failure"));
        eventBus.Subscribe<TestEvent>(_ => subscriberOrder.Add(1));

        eventBus.Publish(new TestEvent(EventId.FromSequence(new EntitySequence(1)), EventPriority.Normal, 0, 0));
        EventDispatchResult result = eventBus.Dispatch(0);

        Assert.Equal(1, result.ProcessedEventCount);
        Assert.Equal(1, result.FailureCount);
        Assert.Single(subscriberOrder);
    }

    [Fact]
    public void SubscriptionDispose_ShouldRemoveTheHandler()
    {
        EventBus eventBus = new();
        int callCount = 0;

        EventSubscription subscription = eventBus.Subscribe<TestEvent>(_ => callCount++);
        subscription.Dispose();

        eventBus.Publish(new TestEvent(EventId.FromSequence(new EntitySequence(1)), EventPriority.Normal, 0, 0));
        eventBus.Dispatch(0);

        Assert.Equal(0, callCount);
    }

    private sealed record TestEvent : EventBase
    {
        public TestEvent(EventId eventId, EventPriority priority, long tick, int order)
            : base(eventId, EventCategory.System, new EventSource("Tests"), priority, tick, tick)
        {
            Order = order;
        }

        public int Order { get; }
    }
}

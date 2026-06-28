using System;

namespace GaiaEngine.Engine.Events;

/// <summary>
/// Represents a deterministic event subscription.
/// </summary>
public sealed class EventSubscription : IDisposable
{
    private readonly Action unsubscribe;
    private bool disposed;

    internal EventSubscription(Action unsubscribe)
    {
        this.unsubscribe = unsubscribe;
    }

    /// <summary>
    /// Unsubscribes the registered handler.
    /// </summary>
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;
        unsubscribe();
    }
}

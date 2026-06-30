using System;

namespace GaiaEngine.Gameplay.Discovery;

/// <summary>
/// Represents one deterministic discovery input signal derived from observation or simulation events.
/// </summary>
public sealed record DiscoverySignal
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DiscoverySignal"/> class.
    /// </summary>
    /// <param name="category">The discovery category associated with the signal.</param>
    /// <param name="source">The deterministic signal source.</param>
    /// <param name="key">The stable subject key associated with the signal.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is empty.</exception>
    public DiscoverySignal(DiscoveryCategory category, DiscoverySignalSource source, string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("The discovery signal key must contain a value.", nameof(key));
        }

        Category = category;
        Source = source;
        Key = key;
    }

    /// <summary>
    /// Gets the discovery category associated with the signal.
    /// </summary>
    public DiscoveryCategory Category { get; }

    /// <summary>
    /// Gets the deterministic source that produced the signal.
    /// </summary>
    public DiscoverySignalSource Source { get; }

    /// <summary>
    /// Gets the stable subject key associated with the signal.
    /// </summary>
    public string Key { get; }
}

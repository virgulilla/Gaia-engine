using System;
using GaiaEngine.Gameplay.Discovery;

namespace GaiaEngine.Gameplay.Objectives;

/// <summary>
/// Represents one deterministic gameplay signal consumed by the objective system.
/// </summary>
public sealed record ObjectiveSignal
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectiveSignal"/> class.
    /// </summary>
    /// <param name="category">The discovery category associated with the signal.</param>
    /// <param name="key">The stable signal key.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is empty.</exception>
    public ObjectiveSignal(DiscoveryCategory category, string key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("The objective signal key must contain a value.", nameof(key));
        }

        Category = category;
        Key = key;
    }

    /// <summary>
    /// Gets the discovery category associated with the signal.
    /// </summary>
    public DiscoveryCategory Category { get; }

    /// <summary>
    /// Gets the stable signal key.
    /// </summary>
    public string Key { get; }
}

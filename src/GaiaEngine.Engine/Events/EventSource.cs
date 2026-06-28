using System;

namespace GaiaEngine.Engine.Events;

/// <summary>
/// Represents the immutable source that published an event.
/// </summary>
public readonly record struct EventSource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventSource"/> struct.
    /// </summary>
    /// <param name="value">The logical source name.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is empty.</exception>
    public EventSource(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("The event source must contain a value.", nameof(value));
        }

        Value = value;
    }

    /// <summary>
    /// Gets the logical source name.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Returns the event source value.
    /// </summary>
    /// <returns>The event source value.</returns>
    public override string ToString()
    {
        return Value;
    }
}

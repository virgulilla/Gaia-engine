using System;

namespace GaiaEngine.Domain.Organisms;

/// <summary>
/// Stores deterministic health values for one organism.
/// </summary>
public sealed record HealthComponent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HealthComponent"/> class.
    /// </summary>
    /// <param name="currentValue">The current health value.</param>
    /// <param name="maximumValue">The maximum health value.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the supplied values are outside the supported deterministic range.
    /// </exception>
    public HealthComponent(int currentValue, int maximumValue)
    {
        if (maximumValue <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumValue), "The maximum health value must be greater than zero.");
        }

        if (currentValue < 0 || currentValue > maximumValue)
        {
            throw new ArgumentOutOfRangeException(nameof(currentValue), "The current health value must be between 0 and the maximum health value.");
        }

        CurrentValue = currentValue;
        MaximumValue = maximumValue;
    }

    /// <summary>
    /// Gets the current health value.
    /// </summary>
    public int CurrentValue { get; }

    /// <summary>
    /// Gets the maximum health value.
    /// </summary>
    public int MaximumValue { get; }
}

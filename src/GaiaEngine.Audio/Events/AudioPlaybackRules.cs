using System;

namespace GaiaEngine.Audio.Events;

/// <summary>
/// Represents the deterministic playback rules attached to one audio event.
/// </summary>
public sealed record AudioPlaybackRules
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioPlaybackRules"/> class.
    /// </summary>
    /// <param name="cooldownTicks">The cooldown measured in ticks.</param>
    /// <param name="maximumInstances">The maximum number of simultaneous instances.</param>
    /// <param name="looping">Whether the sound loops.</param>
    /// <param name="variationKey">The optional variation group key.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="cooldownTicks"/> or <paramref name="maximumInstances"/> is negative.
    /// </exception>
    public AudioPlaybackRules(int cooldownTicks, int maximumInstances, bool looping, string? variationKey)
    {
        if (cooldownTicks < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cooldownTicks), "The audio cooldown must be zero or greater.");
        }

        if (maximumInstances < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumInstances), "The maximum instance count must be zero or greater.");
        }

        CooldownTicks = cooldownTicks;
        MaximumInstances = maximumInstances;
        Looping = looping;
        VariationKey = variationKey;
    }

    /// <summary>
    /// Gets the cooldown measured in ticks.
    /// </summary>
    public int CooldownTicks { get; }

    /// <summary>
    /// Gets the maximum number of simultaneous instances.
    /// </summary>
    public int MaximumInstances { get; }

    /// <summary>
    /// Gets a value indicating whether the sound loops.
    /// </summary>
    public bool Looping { get; }

    /// <summary>
    /// Gets the optional variation group key.
    /// </summary>
    public string? VariationKey { get; }

    /// <summary>
    /// Gets the shared default playback rules.
    /// </summary>
    public static AudioPlaybackRules Default { get; } = new(0, 1, false, null);
}

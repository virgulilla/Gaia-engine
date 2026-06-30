using System;
using System.Collections.Generic;
using GaiaEngine.Audio.Events;

namespace GaiaEngine.Audio.SoundEffects;

/// <summary>
/// Represents one configurable sound effect definition.
/// </summary>
public sealed record SoundEffectDefinition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoundEffectDefinition"/> class.
    /// </summary>
    /// <param name="soundId">The stable sound effect identifier.</param>
    /// <param name="category">The sound effect category.</param>
    /// <param name="variants">The available clip variants.</param>
    /// <param name="priority">The playback priority.</param>
    /// <param name="volume">The normalized playback volume.</param>
    /// <param name="pitchRange">The pitch range.</param>
    /// <param name="maximumInstances">The maximum simultaneous instance count.</param>
    /// <param name="cooldownTicks">The cooldown measured in ticks.</param>
    /// <param name="interruptible">Whether the sound may be interrupted by a higher priority sound.</param>
    /// <param name="mixerGroup">The assigned mixer group.</param>
    /// <param name="spatialSettings">The optional spatial settings.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="soundId"/> is empty or when no variants are supplied.</exception>
    /// <exception cref="ArgumentNullException">Thrown when one reference argument is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="volume"/>, <paramref name="maximumInstances"/>, or <paramref name="cooldownTicks"/> is invalid.
    /// </exception>
    public SoundEffectDefinition(
        string soundId,
        SoundEffectCategory category,
        IReadOnlyList<SoundEffectVariant> variants,
        AudioEventPriority priority,
        float volume,
        PitchRange pitchRange,
        int maximumInstances,
        int cooldownTicks,
        bool interruptible,
        SoundMixerGroup mixerGroup,
        SoundSpatialSettings? spatialSettings)
    {
        if (string.IsNullOrWhiteSpace(soundId))
        {
            throw new ArgumentException("The sound effect identifier must contain a value.", nameof(soundId));
        }

        if (volume < 0 || volume > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(volume), "The sound effect volume must remain between zero and one.");
        }

        if (maximumInstances < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumInstances), "The maximum instance count must be zero or greater.");
        }

        if (cooldownTicks < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(cooldownTicks), "The sound cooldown must be zero or greater.");
        }

        SoundId = soundId;
        Category = category;
        Variants = variants ?? throw new ArgumentNullException(nameof(variants));
        if (Variants.Count == 0)
        {
            throw new ArgumentException("One sound effect definition requires at least one variant.", nameof(variants));
        }

        Priority = priority;
        Volume = volume;
        PitchRange = pitchRange ?? throw new ArgumentNullException(nameof(pitchRange));
        MaximumInstances = maximumInstances;
        CooldownTicks = cooldownTicks;
        Interruptible = interruptible;
        MixerGroup = mixerGroup;
        SpatialSettings = spatialSettings;
    }

    /// <summary>
    /// Gets the stable sound effect identifier.
    /// </summary>
    public string SoundId { get; }

    /// <summary>
    /// Gets the sound effect category.
    /// </summary>
    public SoundEffectCategory Category { get; }

    /// <summary>
    /// Gets the available clip variants.
    /// </summary>
    public IReadOnlyList<SoundEffectVariant> Variants { get; }

    /// <summary>
    /// Gets the playback priority.
    /// </summary>
    public AudioEventPriority Priority { get; }

    /// <summary>
    /// Gets the normalized playback volume.
    /// </summary>
    public float Volume { get; }

    /// <summary>
    /// Gets the pitch range.
    /// </summary>
    public PitchRange PitchRange { get; }

    /// <summary>
    /// Gets the maximum simultaneous instance count.
    /// </summary>
    public int MaximumInstances { get; }

    /// <summary>
    /// Gets the cooldown measured in ticks.
    /// </summary>
    public int CooldownTicks { get; }

    /// <summary>
    /// Gets a value indicating whether the sound may be interrupted.
    /// </summary>
    public bool Interruptible { get; }

    /// <summary>
    /// Gets the assigned mixer group.
    /// </summary>
    public SoundMixerGroup MixerGroup { get; }

    /// <summary>
    /// Gets the optional spatial settings.
    /// </summary>
    public SoundSpatialSettings? SpatialSettings { get; }
}

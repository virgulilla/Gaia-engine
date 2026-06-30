using System;
using GaiaEngine.Audio.Events;

namespace GaiaEngine.Audio.SoundEffects;

/// <summary>
/// Represents one deterministic sound effect selection result.
/// </summary>
public sealed record SoundEffectSelection
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoundEffectSelection"/> class.
    /// </summary>
    /// <param name="soundEffect">The selected sound effect definition.</param>
    /// <param name="variant">The selected sound effect variant.</param>
    /// <param name="resolvedPitch">The resolved cosmetic pitch multiplier.</param>
    /// <param name="audioEvent">The source audio event.</param>
    /// <exception cref="ArgumentNullException">Thrown when one reference argument is <see langword="null"/>.</exception>
    public SoundEffectSelection(
        SoundEffectDefinition soundEffect,
        SoundEffectVariant variant,
        float resolvedPitch,
        AudioEvent audioEvent)
    {
        SoundEffect = soundEffect ?? throw new ArgumentNullException(nameof(soundEffect));
        Variant = variant ?? throw new ArgumentNullException(nameof(variant));
        AudioEvent = audioEvent ?? throw new ArgumentNullException(nameof(audioEvent));
        ResolvedPitch = resolvedPitch;
    }

    /// <summary>
    /// Gets the selected sound effect definition.
    /// </summary>
    public SoundEffectDefinition SoundEffect { get; }

    /// <summary>
    /// Gets the selected sound effect variant.
    /// </summary>
    public SoundEffectVariant Variant { get; }

    /// <summary>
    /// Gets the resolved cosmetic pitch multiplier.
    /// </summary>
    public float ResolvedPitch { get; }

    /// <summary>
    /// Gets the source audio event.
    /// </summary>
    public AudioEvent AudioEvent { get; }
}

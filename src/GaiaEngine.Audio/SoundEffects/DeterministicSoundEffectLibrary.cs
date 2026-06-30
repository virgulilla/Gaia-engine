using System;
using GaiaEngine.Audio.Events;

namespace GaiaEngine.Audio.SoundEffects;

/// <summary>
/// Selects deterministic sound effect variants from runtime audio events.
/// </summary>
public sealed class DeterministicSoundEffectLibrary : ISoundEffectLibrary
{
    private readonly SoundEffectCatalog catalog;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicSoundEffectLibrary"/> class.
    /// </summary>
    /// <param name="catalog">The configurable sound effect catalog.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="catalog"/> is <see langword="null"/>.</exception>
    public DeterministicSoundEffectLibrary(SoundEffectCatalog catalog)
    {
        this.catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
    }

    /// <inheritdoc />
    public bool TrySelect(AudioEvent audioEvent, out SoundEffectSelection? selection)
    {
        ArgumentNullException.ThrowIfNull(audioEvent);

        if (!catalog.TryResolveByClipId(audioEvent.AudioClipId, out SoundEffectDefinition? definition) || definition is null)
        {
            selection = null;
            return false;
        }

        int variantIndex = (int)(audioEvent.EventId.Value % (ulong)definition.Variants.Count);
        SoundEffectVariant variant = definition.Variants[variantIndex];
        float resolvedPitch = ResolvePitch(definition.PitchRange, audioEvent.EventId.Value);
        selection = new SoundEffectSelection(definition, variant, resolvedPitch, audioEvent);
        return true;
    }

    private static float ResolvePitch(PitchRange pitchRange, ulong eventValue)
    {
        if (pitchRange.Minimum == pitchRange.Maximum)
        {
            return pitchRange.Minimum;
        }

        float normalized = (eventValue % 1000UL) / 999f;
        return pitchRange.Minimum + ((pitchRange.Maximum - pitchRange.Minimum) * normalized);
    }
}

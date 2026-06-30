using System;
using System.Collections.Generic;
using GaiaEngine.Audio.Ambient;
using GaiaEngine.Audio.Events;
using GaiaEngine.Audio.Music;
using GaiaEngine.Audio.SoundEffects;

namespace GaiaEngine.Audio.Mixing;

/// <summary>
/// Resolves deterministic mixer bus targets from music, ambience, and sound-effect selections.
/// </summary>
public sealed class DeterministicAudioMixerSystem : IAudioMixerSystem
{
    private readonly AudioMixerSettings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicAudioMixerSystem"/> class.
    /// </summary>
    /// <param name="settings">The deterministic mixer settings.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="settings"/> is <see langword="null"/>.</exception>
    public DeterministicAudioMixerSystem(AudioMixerSettings settings)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    /// <inheritdoc />
    public AudioMixSnapshot Mix(
        MusicSelectionSnapshot? musicSelection,
        AmbientMixSnapshot? ambientMix,
        IReadOnlyList<SoundEffectSelection> soundEffects)
    {
        ArgumentNullException.ThrowIfNull(soundEffects);

        IReadOnlyList<AmbientLayerState> ambientLayers = ambientMix?.Layers ?? Array.Empty<AmbientLayerState>();
        float musicTarget = ResolveMusicTarget(musicSelection);
        float ambienceTarget = ResolveAmbienceTarget(ambientLayers);
        float uiTarget = ResolveSoundGroupTarget(soundEffects, SoundMixerGroup.UI, settings.UiVolume);
        float creaturesTarget = ResolveSoundGroupTarget(soundEffects, SoundMixerGroup.Creatures, settings.CreaturesVolume);
        float environmentTarget = ResolveSoundGroupTarget(soundEffects, SoundMixerGroup.Environment, settings.EnvironmentVolume);

        if (uiTarget > 0f)
        {
            musicTarget *= settings.MusicUiDuckMultiplier;
            ambienceTarget *= settings.AmbienceUiDuckMultiplier;
        }

        if (HasPriorityEffects(soundEffects))
        {
            musicTarget *= settings.MusicPriorityDuckMultiplier;
        }

        musicTarget = Clamp01(musicTarget);
        ambienceTarget = Clamp01(ambienceTarget);
        uiTarget = Clamp01(uiTarget);
        creaturesTarget = Clamp01(creaturesTarget);
        environmentTarget = Clamp01(environmentTarget);

        float masterTarget = ResolveMasterTarget(musicTarget, ambienceTarget, uiTarget, creaturesTarget, environmentTarget);
        List<AudioBusMixState> busStates =
        [
            new AudioBusMixState(SoundMixerGroup.Master, masterTarget, GetActiveBusCount(masterTarget)),
            new AudioBusMixState(SoundMixerGroup.Music, musicTarget, GetActiveBusCount(musicTarget)),
            new AudioBusMixState(SoundMixerGroup.Ambience, ambienceTarget, ambientLayers.Count),
            new AudioBusMixState(SoundMixerGroup.UI, uiTarget, CountSoundEffects(soundEffects, SoundMixerGroup.UI)),
            new AudioBusMixState(SoundMixerGroup.Creatures, creaturesTarget, CountSoundEffects(soundEffects, SoundMixerGroup.Creatures)),
            new AudioBusMixState(SoundMixerGroup.Environment, environmentTarget, CountSoundEffects(soundEffects, SoundMixerGroup.Environment)),
        ];

        return new AudioMixSnapshot(musicSelection, ambientLayers, soundEffects, busStates.AsReadOnly());
    }

    private float ResolveMusicTarget(MusicSelectionSnapshot? musicSelection)
    {
        if (musicSelection is null || musicSelection.TrackId == "music.silence.ambient")
        {
            return 0f;
        }

        return settings.MusicVolume;
    }

    private float ResolveAmbienceTarget(IReadOnlyList<AmbientLayerState> ambientLayers)
    {
        if (ambientLayers.Count == 0)
        {
            return 0f;
        }

        float total = 0f;
        foreach (AmbientLayerState layer in ambientLayers)
        {
            total += layer.Volume;
        }

        return settings.AmbienceVolume * (total / ambientLayers.Count);
    }

    private static float ResolveSoundGroupTarget(IReadOnlyList<SoundEffectSelection> soundEffects, SoundMixerGroup group, float baseVolume)
    {
        float target = 0f;
        foreach (SoundEffectSelection selection in soundEffects)
        {
            if (selection.SoundEffect.MixerGroup != group)
            {
                continue;
            }

            target = Math.Max(target, selection.SoundEffect.Volume * baseVolume);
        }

        return target;
    }

    private static bool HasPriorityEffects(IReadOnlyList<SoundEffectSelection> soundEffects)
    {
        foreach (SoundEffectSelection selection in soundEffects)
        {
            if (selection.AudioEvent.Priority is AudioEventPriority.Important or AudioEventPriority.Critical)
            {
                return true;
            }
        }

        return false;
    }

    private float ResolveMasterTarget(float musicTarget, float ambienceTarget, float uiTarget, float creaturesTarget, float environmentTarget)
    {
        float highest = Math.Max(
            Math.Max(musicTarget, ambienceTarget),
            Math.Max(uiTarget, Math.Max(creaturesTarget, environmentTarget)));
        return highest <= 0f ? 0f : Clamp01(settings.MasterVolume);
    }

    private static int CountSoundEffects(IReadOnlyList<SoundEffectSelection> soundEffects, SoundMixerGroup group)
    {
        int count = 0;
        foreach (SoundEffectSelection selection in soundEffects)
        {
            if (selection.SoundEffect.MixerGroup == group)
            {
                count++;
            }
        }

        return count;
    }

    private static int GetActiveBusCount(float targetVolume)
    {
        return targetVolume > 0f ? 1 : 0;
    }

    private static float Clamp01(float value)
    {
        if (value < 0f)
        {
            return 0f;
        }

        return value > 1f ? 1f : value;
    }
}

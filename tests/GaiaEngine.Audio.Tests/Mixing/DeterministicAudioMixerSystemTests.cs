using System;
using GaiaEngine.Audio.Ambient;
using GaiaEngine.Audio.Events;
using GaiaEngine.Audio.Mixing;
using GaiaEngine.Audio.Music;
using GaiaEngine.Audio.SoundEffects;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;
using Xunit;

namespace GaiaEngine.Audio.Tests.Mixing;

public sealed class DeterministicAudioMixerSystemTests
{
    [Fact]
    public void Mix_ShouldBuildBusLevelsFromMusicAmbienceAndSoundEffects()
    {
        DeterministicAudioMixerSystem system = new(CreateSettings());

        AudioMixSnapshot result = system.Mix(
            CreateMusicSelection("music.exploration"),
            new AmbientMixSnapshot(
                tick: 10,
                ChunkId.FromSequence(new EntitySequence(1)),
                new[]
                {
                    new AmbientLayerState("global", AmbientLayerKind.Global, AmbientSpatialScope.Global, 4, "ambient.global.temperate", 0.4f, 10, null),
                    new AmbientLayerState("biome", AmbientLayerKind.Biome, AmbientSpatialScope.Regional, 5, "ambient.biome.forest", 0.6f, 10, null),
                }),
            new[]
            {
                CreateSoundSelection("sfx.discovery", SoundMixerGroup.UI, 0.9f, AudioEventPriority.Important),
                CreateSoundSelection("sfx.footstep", SoundMixerGroup.Creatures, 0.7f, AudioEventPriority.Normal),
                CreateSoundSelection("sfx.season", SoundMixerGroup.Environment, 0.5f, AudioEventPriority.Normal),
            });

        Assert.Equal(6, result.BusStates.Count);
        Assert.Equal(0.36f, FindBus(result, SoundMixerGroup.Music).TargetVolume, 3);
        Assert.Equal(0.1375f, FindBus(result, SoundMixerGroup.Ambience).TargetVolume, 3);
        Assert.Equal(0.9f, FindBus(result, SoundMixerGroup.UI).TargetVolume, 3);
        Assert.Equal(0.7f, FindBus(result, SoundMixerGroup.Creatures).TargetVolume, 3);
        Assert.Equal(0.5f, FindBus(result, SoundMixerGroup.Environment).TargetVolume, 3);
        Assert.Equal(1f, FindBus(result, SoundMixerGroup.Master).TargetVolume, 3);
    }

    [Fact]
    public void Mix_ShouldSilenceInactiveBuses()
    {
        DeterministicAudioMixerSystem system = new(CreateSettings());

        AudioMixSnapshot result = system.Mix(
            musicSelection: null,
            ambientMix: null,
            Array.Empty<SoundEffectSelection>());

        Assert.All(result.BusStates, bus => Assert.Equal(0f, bus.TargetVolume));
    }

    [Fact]
    public void Mix_ShouldMuteMusicWhenSilentThemeIsSelected()
    {
        DeterministicAudioMixerSystem system = new(CreateSettings());

        AudioMixSnapshot result = system.Mix(
            CreateMusicSelection("music.silence.ambient"),
            ambientMix: null,
            Array.Empty<SoundEffectSelection>());

        Assert.Equal(0f, FindBus(result, SoundMixerGroup.Music).TargetVolume);
        Assert.Equal(0f, FindBus(result, SoundMixerGroup.Master).TargetVolume);
    }

    private static AudioMixerSettings CreateSettings()
    {
        return new AudioMixerSettings(
            masterVolume: 1f,
            musicVolume: 0.8f,
            ambienceVolume: 0.55f,
            uiVolume: 1f,
            creaturesVolume: 1f,
            environmentVolume: 1f,
            musicUiDuckMultiplier: 0.6f,
            ambienceUiDuckMultiplier: 0.5f,
            musicPriorityDuckMultiplier: 0.75f);
    }

    private static MusicSelectionSnapshot CreateMusicSelection(string trackId)
    {
        return new MusicSelectionSnapshot(
            timestamp: 10,
            MusicPresentationContext.InGame,
            new MusicThemeDefinition(
                "music.exploration",
                MusicThemeKind.Exploration,
                MusicPrimaryState.Exploration,
                priority: 4,
                "music.exploration",
                new MusicPlaybackRules(looping: true, canInterruptLowerPriority: false, MusicTransitionKind.LayerBlend, transitionTicks: 12)),
            trackId,
            "test");
    }

    private static SoundEffectSelection CreateSoundSelection(string soundId, SoundMixerGroup group, float volume, AudioEventPriority priority)
    {
        SoundEffectDefinition definition = new(
            soundId,
            SoundEffectCategory.Gameplay,
            new[] { new SoundEffectVariant($"{soundId}.a", $"{soundId}.clip") },
            priority,
            volume,
            new PitchRange(1f, 1f),
            maximumInstances: 1,
            cooldownTicks: 0,
            interruptible: true,
            group,
            spatialSettings: null);
        AudioEvent audioEvent = new(
            EventId.FromSequence(new EntitySequence((ulong)(10 + (int)group))),
            AudioEventCategory.Gameplay,
            priority,
            spatialProfile: null,
            timestamp: 10,
            $"{soundId}.clip",
            AudioPlaybackRules.Default);
        return new SoundEffectSelection(definition, definition.Variants[0], 1f, audioEvent);
    }

    private static AudioBusMixState FindBus(AudioMixSnapshot snapshot, SoundMixerGroup group)
    {
        foreach (AudioBusMixState bus in snapshot.BusStates)
        {
            if (bus.Group == group)
            {
                return bus;
            }
        }

        throw new InvalidOperationException("The expected mixer bus was not found.");
    }
}

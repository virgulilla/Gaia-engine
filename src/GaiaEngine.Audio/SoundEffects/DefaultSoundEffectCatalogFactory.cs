using System.Collections.Generic;
using GaiaEngine.Audio.Events;

namespace GaiaEngine.Audio.SoundEffects;

/// <summary>
/// Creates the default deterministic sound effect catalog for the current audio slice.
/// </summary>
public static class DefaultSoundEffectCatalogFactory
{
    /// <summary>
    /// Creates the ordered default sound effect catalog.
    /// </summary>
    /// <returns>The ordered default sound effect catalog.</returns>
    public static SoundEffectCatalog Create()
    {
        return new SoundEffectCatalog(
            new List<SoundEffectDefinition>
            {
                new(
                    "sfx.organism.footstep",
                    SoundEffectCategory.Organisms,
                    new[]
                    {
                        new SoundEffectVariant("footstep.a", "organism.footstep"),
                        new SoundEffectVariant("footstep.b", "organism.footstep.variant-b"),
                        new SoundEffectVariant("footstep.c", "organism.footstep.variant-c"),
                        new SoundEffectVariant("footstep.d", "organism.footstep.variant-d"),
                    },
                    AudioEventPriority.Normal,
                    volume: 0.75f,
                    new PitchRange(0.96f, 1.04f),
                    maximumInstances: 8,
                    cooldownTicks: 1,
                    interruptible: true,
                    SoundMixerGroup.Creatures,
                    new SoundSpatialSettings(radius: 24, attenuation: 1, stereoSpread: 0.15f)),
                new(
                    "sfx.organism.eat",
                    SoundEffectCategory.Organisms,
                    new[]
                    {
                        new SoundEffectVariant("eat.a", "organism.eat"),
                        new SoundEffectVariant("eat.b", "organism.eat.variant-b"),
                    },
                    AudioEventPriority.Normal,
                    volume: 0.8f,
                    new PitchRange(0.98f, 1.02f),
                    maximumInstances: 4,
                    cooldownTicks: 2,
                    interruptible: true,
                    SoundMixerGroup.Creatures,
                    new SoundSpatialSettings(radius: 20, attenuation: 1, stereoSpread: 0.1f)),
                new(
                    "sfx.organism.drink",
                    SoundEffectCategory.Organisms,
                    new[]
                    {
                        new SoundEffectVariant("drink.a", "organism.drink"),
                        new SoundEffectVariant("drink.b", "organism.drink.variant-b"),
                    },
                    AudioEventPriority.Normal,
                    volume: 0.78f,
                    new PitchRange(0.98f, 1.02f),
                    maximumInstances: 4,
                    cooldownTicks: 2,
                    interruptible: true,
                    SoundMixerGroup.Creatures,
                    new SoundSpatialSettings(radius: 20, attenuation: 1, stereoSpread: 0.1f)),
                new(
                    "sfx.gameplay.discovery",
                    SoundEffectCategory.Gameplay,
                    new[]
                    {
                        new SoundEffectVariant("discovery.a", "gameplay.discovery"),
                    },
                    AudioEventPriority.Important,
                    volume: 1f,
                    PitchRange.Neutral,
                    maximumInstances: 2,
                    cooldownTicks: 0,
                    interruptible: false,
                    SoundMixerGroup.UI,
                    spatialSettings: null),
                new(
                    "sfx.gameplay.achievement",
                    SoundEffectCategory.Gameplay,
                    new[]
                    {
                        new SoundEffectVariant("achievement.a", "gameplay.achievement.unlocked"),
                    },
                    AudioEventPriority.Critical,
                    volume: 1f,
                    PitchRange.Neutral,
                    maximumInstances: 1,
                    cooldownTicks: 0,
                    interruptible: false,
                    SoundMixerGroup.UI,
                    spatialSettings: null),
                new(
                    "sfx.environment.season-change",
                    SoundEffectCategory.Environment,
                    new[]
                    {
                        new SoundEffectVariant("season.a", "environment.season.change"),
                    },
                    AudioEventPriority.Important,
                    volume: 0.9f,
                    PitchRange.Neutral,
                    maximumInstances: 1,
                    cooldownTicks: 0,
                    interruptible: false,
                    SoundMixerGroup.Environment,
                    spatialSettings: null),
                new(
                    "sfx.system.new-year",
                    SoundEffectCategory.System,
                    new[]
                    {
                        new SoundEffectVariant("newyear.a", "system.new.year"),
                    },
                    AudioEventPriority.Critical,
                    volume: 1f,
                    PitchRange.Neutral,
                    maximumInstances: 1,
                    cooldownTicks: 0,
                    interruptible: false,
                    SoundMixerGroup.Master,
                    spatialSettings: null),
            }.AsReadOnly());
    }
}

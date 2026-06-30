using GaiaEngine.Audio.Events;
using GaiaEngine.Audio.SoundEffects;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;
using Xunit;

namespace GaiaEngine.Audio.Tests.SoundEffects;

public sealed class DeterministicSoundEffectLibraryTests
{
    [Fact]
    public void TrySelect_ShouldResolveOneDeterministicVariantAndPitch()
    {
        DeterministicSoundEffectLibrary library = new(DefaultSoundEffectCatalogFactory.Create());
        AudioEvent audioEvent = new(
            EventId.FromSequence(new EntitySequence(7)),
            AudioEventCategory.Organisms,
            AudioEventPriority.Normal,
            new AudioSpatialProfile(new AudioPosition(0, 0, 0), 24, 1),
            timestamp: 20,
            "organism.footstep",
            new AudioPlaybackRules(1, 8, looping: false, variationKey: "footstep"));

        bool found = library.TrySelect(audioEvent, out SoundEffectSelection? selection);

        Assert.True(found);
        Assert.NotNull(selection);
        Assert.Equal("sfx.organism.footstep", selection!.SoundEffect.SoundId);
        Assert.InRange(selection.ResolvedPitch, 0.96f, 1.04f);
    }

    [Fact]
    public void TrySelect_ShouldReturnFalseWhenNoSoundEffectMatchesTheClip()
    {
        DeterministicSoundEffectLibrary library = new(DefaultSoundEffectCatalogFactory.Create());
        AudioEvent audioEvent = new(
            EventId.FromSequence(new EntitySequence(3)),
            AudioEventCategory.Gameplay,
            AudioEventPriority.Important,
            spatialProfile: null,
            timestamp: 10,
            "gameplay.unknown",
            AudioPlaybackRules.Default);

        bool found = library.TrySelect(audioEvent, out SoundEffectSelection? selection);

        Assert.False(found);
        Assert.Null(selection);
    }
}

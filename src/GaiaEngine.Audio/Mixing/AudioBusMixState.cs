using System;
using GaiaEngine.Audio.SoundEffects;

namespace GaiaEngine.Audio.Mixing;

/// <summary>
/// Represents one deterministic mixer bus target state.
/// </summary>
public sealed record AudioBusMixState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioBusMixState"/> class.
    /// </summary>
    /// <param name="group">The resolved mixer group.</param>
    /// <param name="targetVolume">The normalized target volume in the inclusive range [0, 1].</param>
    /// <param name="activeVoiceCount">The active voice count assigned to the bus.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="targetVolume"/> is out of range or when <paramref name="activeVoiceCount"/> is negative.</exception>
    public AudioBusMixState(SoundMixerGroup group, float targetVolume, int activeVoiceCount)
    {
        if (targetVolume < 0f || targetVolume > 1f)
        {
            throw new ArgumentOutOfRangeException(nameof(targetVolume), "The target mixer volume must remain between zero and one.");
        }

        if (activeVoiceCount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(activeVoiceCount), "The active voice count must be zero or greater.");
        }

        Group = group;
        TargetVolume = targetVolume;
        ActiveVoiceCount = activeVoiceCount;
    }

    /// <summary>
    /// Gets the resolved mixer group.
    /// </summary>
    public SoundMixerGroup Group { get; }

    /// <summary>
    /// Gets the normalized target volume in the inclusive range [0, 1].
    /// </summary>
    public float TargetVolume { get; }

    /// <summary>
    /// Gets the active voice count assigned to the bus.
    /// </summary>
    public int ActiveVoiceCount { get; }
}

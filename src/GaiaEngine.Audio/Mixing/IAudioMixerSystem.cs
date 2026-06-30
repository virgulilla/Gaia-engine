using System.Collections.Generic;
using GaiaEngine.Audio.Ambient;
using GaiaEngine.Audio.Music;
using GaiaEngine.Audio.SoundEffects;

namespace GaiaEngine.Audio.Mixing;

/// <summary>
/// Defines a deterministic service that resolves audio bus levels from already selected music, ambience, and sound effects.
/// </summary>
public interface IAudioMixerSystem
{
    /// <summary>
    /// Builds one deterministic mix snapshot for the supplied audio selections.
    /// </summary>
    /// <param name="musicSelection">The optional active music selection.</param>
    /// <param name="ambientMix">The optional active ambient mix.</param>
    /// <param name="soundEffects">The currently selected sound effects.</param>
    /// <returns>The deterministic audio mix snapshot.</returns>
    public AudioMixSnapshot Mix(
        MusicSelectionSnapshot? musicSelection,
        AmbientMixSnapshot? ambientMix,
        IReadOnlyList<SoundEffectSelection> soundEffects);
}

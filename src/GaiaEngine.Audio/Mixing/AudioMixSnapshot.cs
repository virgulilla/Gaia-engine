using System;
using System.Collections.Generic;
using GaiaEngine.Audio.Ambient;
using GaiaEngine.Audio.Music;
using GaiaEngine.Audio.SoundEffects;

namespace GaiaEngine.Audio.Mixing;

/// <summary>
/// Represents one deterministic audio mix snapshot for the current frame or tick.
/// </summary>
public sealed record AudioMixSnapshot
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioMixSnapshot"/> class.
    /// </summary>
    /// <param name="musicSelection">The optional active music selection.</param>
    /// <param name="ambientLayers">The active ambient layers assigned to the ambience bus.</param>
    /// <param name="soundEffects">The selected sound effects assigned to their buses.</param>
    /// <param name="busStates">The resolved mixer bus states.</param>
    /// <exception cref="ArgumentNullException">Thrown when one collection argument is <see langword="null"/>.</exception>
    public AudioMixSnapshot(
        MusicSelectionSnapshot? musicSelection,
        IReadOnlyList<AmbientLayerState> ambientLayers,
        IReadOnlyList<SoundEffectSelection> soundEffects,
        IReadOnlyList<AudioBusMixState> busStates)
    {
        MusicSelection = musicSelection;
        AmbientLayers = ambientLayers ?? throw new ArgumentNullException(nameof(ambientLayers));
        SoundEffects = soundEffects ?? throw new ArgumentNullException(nameof(soundEffects));
        BusStates = busStates ?? throw new ArgumentNullException(nameof(busStates));
    }

    /// <summary>
    /// Gets the optional active music selection.
    /// </summary>
    public MusicSelectionSnapshot? MusicSelection { get; }

    /// <summary>
    /// Gets the active ambient layers assigned to the ambience bus.
    /// </summary>
    public IReadOnlyList<AmbientLayerState> AmbientLayers { get; }

    /// <summary>
    /// Gets the selected sound effects assigned to their buses.
    /// </summary>
    public IReadOnlyList<SoundEffectSelection> SoundEffects { get; }

    /// <summary>
    /// Gets the resolved mixer bus states.
    /// </summary>
    public IReadOnlyList<AudioBusMixState> BusStates { get; }
}

using System;

namespace GaiaEngine.Audio.Mixing;

/// <summary>
/// Represents deterministic settings used by the audio mixer.
/// </summary>
public sealed record AudioMixerSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioMixerSettings"/> class.
    /// </summary>
    /// <param name="masterVolume">The base master volume.</param>
    /// <param name="musicVolume">The base music bus volume.</param>
    /// <param name="ambienceVolume">The base ambience bus volume.</param>
    /// <param name="uiVolume">The base user-interface bus volume.</param>
    /// <param name="creaturesVolume">The base creatures bus volume.</param>
    /// <param name="environmentVolume">The base environment bus volume.</param>
    /// <param name="musicUiDuckMultiplier">The music ducking multiplier applied while user-interface sounds are active.</param>
    /// <param name="ambienceUiDuckMultiplier">The ambience ducking multiplier applied while user-interface sounds are active.</param>
    /// <param name="musicPriorityDuckMultiplier">The music ducking multiplier applied while important or critical sound effects are active.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one value is outside the inclusive range [0, 1].</exception>
    public AudioMixerSettings(
        float masterVolume,
        float musicVolume,
        float ambienceVolume,
        float uiVolume,
        float creaturesVolume,
        float environmentVolume,
        float musicUiDuckMultiplier,
        float ambienceUiDuckMultiplier,
        float musicPriorityDuckMultiplier)
    {
        ValidateNormalized(masterVolume, nameof(masterVolume));
        ValidateNormalized(musicVolume, nameof(musicVolume));
        ValidateNormalized(ambienceVolume, nameof(ambienceVolume));
        ValidateNormalized(uiVolume, nameof(uiVolume));
        ValidateNormalized(creaturesVolume, nameof(creaturesVolume));
        ValidateNormalized(environmentVolume, nameof(environmentVolume));
        ValidateNormalized(musicUiDuckMultiplier, nameof(musicUiDuckMultiplier));
        ValidateNormalized(ambienceUiDuckMultiplier, nameof(ambienceUiDuckMultiplier));
        ValidateNormalized(musicPriorityDuckMultiplier, nameof(musicPriorityDuckMultiplier));

        MasterVolume = masterVolume;
        MusicVolume = musicVolume;
        AmbienceVolume = ambienceVolume;
        UiVolume = uiVolume;
        CreaturesVolume = creaturesVolume;
        EnvironmentVolume = environmentVolume;
        MusicUiDuckMultiplier = musicUiDuckMultiplier;
        AmbienceUiDuckMultiplier = ambienceUiDuckMultiplier;
        MusicPriorityDuckMultiplier = musicPriorityDuckMultiplier;
    }

    /// <summary>
    /// Gets the base master volume.
    /// </summary>
    public float MasterVolume { get; }

    /// <summary>
    /// Gets the base music bus volume.
    /// </summary>
    public float MusicVolume { get; }

    /// <summary>
    /// Gets the base ambience bus volume.
    /// </summary>
    public float AmbienceVolume { get; }

    /// <summary>
    /// Gets the base user-interface bus volume.
    /// </summary>
    public float UiVolume { get; }

    /// <summary>
    /// Gets the base creatures bus volume.
    /// </summary>
    public float CreaturesVolume { get; }

    /// <summary>
    /// Gets the base environment bus volume.
    /// </summary>
    public float EnvironmentVolume { get; }

    /// <summary>
    /// Gets the music ducking multiplier applied while user-interface sounds are active.
    /// </summary>
    public float MusicUiDuckMultiplier { get; }

    /// <summary>
    /// Gets the ambience ducking multiplier applied while user-interface sounds are active.
    /// </summary>
    public float AmbienceUiDuckMultiplier { get; }

    /// <summary>
    /// Gets the music ducking multiplier applied while important or critical sound effects are active.
    /// </summary>
    public float MusicPriorityDuckMultiplier { get; }

    private static void ValidateNormalized(float value, string parameterName)
    {
        if (value < 0f || value > 1f)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Mixer values must remain between zero and one.");
        }
    }
}

using System;

namespace GaiaEngine.Audio.SoundEffects;

/// <summary>
/// Represents one deterministic spatial sound profile.
/// </summary>
public sealed record SoundSpatialSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoundSpatialSettings"/> class.
    /// </summary>
    /// <param name="radius">The maximum hearing radius.</param>
    /// <param name="attenuation">The distance attenuation multiplier.</param>
    /// <param name="stereoSpread">The stereo spread value.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one argument is negative.</exception>
    public SoundSpatialSettings(float radius, float attenuation, float stereoSpread)
    {
        if (radius < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(radius), "The radius must be zero or greater.");
        }

        if (attenuation < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(attenuation), "The attenuation must be zero or greater.");
        }

        if (stereoSpread < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(stereoSpread), "The stereo spread must be zero or greater.");
        }

        Radius = radius;
        Attenuation = attenuation;
        StereoSpread = stereoSpread;
    }

    /// <summary>
    /// Gets the maximum hearing radius.
    /// </summary>
    public float Radius { get; }

    /// <summary>
    /// Gets the distance attenuation multiplier.
    /// </summary>
    public float Attenuation { get; }

    /// <summary>
    /// Gets the stereo spread value.
    /// </summary>
    public float StereoSpread { get; }
}

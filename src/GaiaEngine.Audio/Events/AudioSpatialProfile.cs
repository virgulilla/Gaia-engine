using System;

namespace GaiaEngine.Audio.Events;

/// <summary>
/// Represents one deterministic spatial playback profile.
/// </summary>
public sealed record AudioSpatialProfile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioSpatialProfile"/> class.
    /// </summary>
    /// <param name="position">The world position associated with the event.</param>
    /// <param name="maximumDistance">The maximum playback distance.</param>
    /// <param name="volumeFalloff">The volume falloff multiplier.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="position"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="maximumDistance"/> or <paramref name="volumeFalloff"/> is negative.
    /// </exception>
    public AudioSpatialProfile(AudioPosition position, float maximumDistance, float volumeFalloff)
    {
        if (maximumDistance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumDistance), "The maximum distance must be zero or greater.");
        }

        if (volumeFalloff < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(volumeFalloff), "The volume falloff must be zero or greater.");
        }

        Position = position ?? throw new ArgumentNullException(nameof(position));
        MaximumDistance = maximumDistance;
        VolumeFalloff = volumeFalloff;
    }

    /// <summary>
    /// Gets the world position associated with the event.
    /// </summary>
    public AudioPosition Position { get; }

    /// <summary>
    /// Gets the maximum playback distance.
    /// </summary>
    public float MaximumDistance { get; }

    /// <summary>
    /// Gets the volume falloff multiplier.
    /// </summary>
    public float VolumeFalloff { get; }
}

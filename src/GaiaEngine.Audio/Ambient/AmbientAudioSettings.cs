using System;

namespace GaiaEngine.Audio.Ambient;

/// <summary>
/// Represents deterministic settings used by the ambient audio system.
/// </summary>
public sealed record AmbientAudioSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AmbientAudioSettings"/> class.
    /// </summary>
    /// <param name="ticksPerDay">The number of simulation ticks contained in one day.</param>
    /// <param name="transitionTicks">The preferred transition duration measured in ticks.</param>
    /// <param name="nearbyChunkRange">The nearby chunk range used for local ambience queries.</param>
    /// <param name="localSpatialRadius">The maximum local ambience radius.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="ticksPerDay"/> is not positive or when one of the other numeric values is negative.
    /// </exception>
    public AmbientAudioSettings(int ticksPerDay, int transitionTicks, int nearbyChunkRange, float localSpatialRadius)
    {
        if (ticksPerDay <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(ticksPerDay), "The ticks-per-day value must be greater than zero.");
        }

        if (transitionTicks < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(transitionTicks), "The transition duration must be zero or greater.");
        }

        if (nearbyChunkRange < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(nearbyChunkRange), "The nearby chunk range must be zero or greater.");
        }

        if (localSpatialRadius < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(localSpatialRadius), "The local spatial radius must be zero or greater.");
        }

        TicksPerDay = ticksPerDay;
        TransitionTicks = transitionTicks;
        NearbyChunkRange = nearbyChunkRange;
        LocalSpatialRadius = localSpatialRadius;
    }

    /// <summary>
    /// Gets the number of simulation ticks contained in one day.
    /// </summary>
    public int TicksPerDay { get; }

    /// <summary>
    /// Gets the preferred transition duration measured in ticks.
    /// </summary>
    public int TransitionTicks { get; }

    /// <summary>
    /// Gets the nearby chunk range used for local ambience queries.
    /// </summary>
    public int NearbyChunkRange { get; }

    /// <summary>
    /// Gets the maximum local ambience radius.
    /// </summary>
    public float LocalSpatialRadius { get; }
}

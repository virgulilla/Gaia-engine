using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents immutable deterministic elevation data for one chunk terrain slice.
/// </summary>
public sealed record ElevationState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ElevationState"/> class.
    /// </summary>
    /// <param name="height">The absolute terrain height.</param>
    /// <param name="relativeHeight">The relative terrain height within the local world profile.</param>
    /// <param name="seaLevelOffset">The signed offset relative to sea level.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="height"/> is negative.</exception>
    public ElevationState(int height, int relativeHeight, int seaLevelOffset)
    {
        if (height < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height), "The terrain height must be zero or greater.");
        }

        Height = height;
        RelativeHeight = relativeHeight;
        SeaLevelOffset = seaLevelOffset;
    }

    /// <summary>
    /// Gets the absolute terrain height.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the relative terrain height.
    /// </summary>
    public int RelativeHeight { get; }

    /// <summary>
    /// Gets the signed terrain offset relative to sea level.
    /// </summary>
    public int SeaLevelOffset { get; }
}

using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the optional deterministic lake state stored by one chunk.
/// </summary>
public sealed record LakeState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LakeState"/> class.
    /// </summary>
    /// <param name="surfaceArea">The lake surface area.</param>
    /// <param name="maximumDepth">The lake maximum depth.</param>
    /// <param name="waterVolume">The lake water volume.</param>
    /// <param name="overflowLevel">The lake overflow level.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more numeric values are negative.</exception>
    public LakeState(int surfaceArea, int maximumDepth, int waterVolume, int overflowLevel)
    {
        if (surfaceArea < 0 || maximumDepth < 0 || waterVolume < 0 || overflowLevel < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(surfaceArea), "The lake values must be zero or greater.");
        }

        SurfaceArea = surfaceArea;
        MaximumDepth = maximumDepth;
        WaterVolume = waterVolume;
        OverflowLevel = overflowLevel;
    }

    /// <summary>
    /// Gets the lake surface area.
    /// </summary>
    public int SurfaceArea { get; }

    /// <summary>
    /// Gets the lake maximum depth.
    /// </summary>
    public int MaximumDepth { get; }

    /// <summary>
    /// Gets the lake water volume.
    /// </summary>
    public int WaterVolume { get; }

    /// <summary>
    /// Gets the lake overflow level.
    /// </summary>
    public int OverflowLevel { get; }
}

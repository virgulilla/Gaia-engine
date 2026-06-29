using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the local deterministic surface-water state stored by one chunk.
/// </summary>
public sealed record SurfaceWaterState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SurfaceWaterState"/> class.
    /// </summary>
    /// <param name="waterLevel">The local surface-water level in the inclusive range [0, 1000].</param>
    /// <param name="flowSpeed">The local flow speed.</param>
    /// <param name="flowDirection">The local flow direction in the inclusive range [0, 359].</param>
    /// <param name="waterVolume">The local surface-water volume.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when one or more values are outside the supported deterministic range.
    /// </exception>
    public SurfaceWaterState(int waterLevel, int flowSpeed, int flowDirection, int waterVolume)
    {
        if (waterLevel < 0 || waterLevel > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(waterLevel), "The surface-water level must be between 0 and 1000.");
        }

        if (flowSpeed < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(flowSpeed), "The surface-water flow speed must be zero or greater.");
        }

        if (flowDirection < 0 || flowDirection > 359)
        {
            throw new ArgumentOutOfRangeException(nameof(flowDirection), "The surface-water flow direction must be between 0 and 359.");
        }

        if (waterVolume < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(waterVolume), "The surface-water volume must be zero or greater.");
        }

        WaterLevel = waterLevel;
        FlowSpeed = flowSpeed;
        FlowDirection = flowDirection;
        WaterVolume = waterVolume;
    }

    /// <summary>
    /// Gets the local surface-water level.
    /// </summary>
    public int WaterLevel { get; }

    /// <summary>
    /// Gets the local flow speed.
    /// </summary>
    public int FlowSpeed { get; }

    /// <summary>
    /// Gets the local flow direction.
    /// </summary>
    public int FlowDirection { get; }

    /// <summary>
    /// Gets the local surface-water volume.
    /// </summary>
    public int WaterVolume { get; }
}

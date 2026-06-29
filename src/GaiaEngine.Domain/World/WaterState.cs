using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the passive deterministic water state stored by one chunk.
/// </summary>
public sealed record WaterState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WaterState"/> class.
    /// </summary>
    /// <param name="surfaceWater">The local surface-water state.</param>
    /// <param name="groundWater">The local groundwater state.</param>
    /// <param name="river">The optional local river state.</param>
    /// <param name="lake">The optional local lake state.</param>
    /// <param name="ocean">The optional local ocean state.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="surfaceWater"/> or <paramref name="groundWater"/> is <see langword="null"/>.</exception>
    public WaterState(
        SurfaceWaterState surfaceWater,
        GroundWaterState groundWater,
        RiverState? river,
        LakeState? lake,
        OceanState? ocean)
    {
        SurfaceWater = surfaceWater ?? throw new ArgumentNullException(nameof(surfaceWater));
        GroundWater = groundWater ?? throw new ArgumentNullException(nameof(groundWater));
        River = river;
        Lake = lake;
        Ocean = ocean;
    }

    /// <summary>
    /// Gets the local surface-water state.
    /// </summary>
    public SurfaceWaterState SurfaceWater { get; }

    /// <summary>
    /// Gets the local groundwater state.
    /// </summary>
    public GroundWaterState GroundWater { get; }

    /// <summary>
    /// Gets the optional local river state.
    /// </summary>
    public RiverState? River { get; }

    /// <summary>
    /// Gets the optional local lake state.
    /// </summary>
    public LakeState? Lake { get; }

    /// <summary>
    /// Gets the optional local ocean state.
    /// </summary>
    public OceanState? Ocean { get; }
}

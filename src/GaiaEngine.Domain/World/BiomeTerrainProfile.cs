using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the typical deterministic terrain profile of a biome.
/// </summary>
public sealed record BiomeTerrainProfile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BiomeTerrainProfile"/> class.
    /// </summary>
    /// <param name="minimumElevation">The minimum typical elevation.</param>
    /// <param name="maximumElevation">The maximum typical elevation.</param>
    /// <param name="dominantSoil">The dominant soil type.</param>
    /// <param name="surface">The dominant surface type.</param>
    /// <param name="drainage">The typical drainage value in the inclusive range [0, 100].</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="maximumElevation"/> is less than <paramref name="minimumElevation"/>
    /// or when <paramref name="drainage"/> is outside [0, 100].
    /// </exception>
    public BiomeTerrainProfile(int minimumElevation, int maximumElevation, SoilType dominantSoil, SurfaceType surface, int drainage)
    {
        if (maximumElevation < minimumElevation)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumElevation), "The maximum elevation must be greater than or equal to the minimum elevation.");
        }

        if (drainage < 0 || drainage > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(drainage), "The biome drainage must be between 0 and 100.");
        }

        MinimumElevation = minimumElevation;
        MaximumElevation = maximumElevation;
        DominantSoil = dominantSoil;
        Surface = surface;
        Drainage = drainage;
    }

    /// <summary>
    /// Gets the minimum typical elevation.
    /// </summary>
    public int MinimumElevation { get; }

    /// <summary>
    /// Gets the maximum typical elevation.
    /// </summary>
    public int MaximumElevation { get; }

    /// <summary>
    /// Gets the dominant soil type.
    /// </summary>
    public SoilType DominantSoil { get; }

    /// <summary>
    /// Gets the dominant surface type.
    /// </summary>
    public SurfaceType Surface { get; }

    /// <summary>
    /// Gets the typical drainage value.
    /// </summary>
    public int Drainage { get; }
}

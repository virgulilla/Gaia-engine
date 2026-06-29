using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the dominant vegetation profile of a biome.
/// </summary>
public sealed record BiomeVegetationProfile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BiomeVegetationProfile"/> class.
    /// </summary>
    /// <param name="dominantVegetation">The dominant vegetation type.</param>
    /// <param name="density">The vegetation density in the inclusive range [0, 100].</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="density"/> is outside [0, 100].</exception>
    public BiomeVegetationProfile(VegetationType dominantVegetation, int density)
    {
        if (density < 0 || density > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(density), "The biome vegetation density must be between 0 and 100.");
        }

        DominantVegetation = dominantVegetation;
        Density = density;
    }

    /// <summary>
    /// Gets the dominant vegetation type.
    /// </summary>
    public VegetationType DominantVegetation { get; }

    /// <summary>
    /// Gets the vegetation density.
    /// </summary>
    public int Density { get; }
}

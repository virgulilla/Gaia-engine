using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the typical deterministic resource abundance profile of a biome.
/// </summary>
public sealed record BiomeResourceProfile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BiomeResourceProfile"/> class.
    /// </summary>
    /// <param name="water">The typical water abundance in the inclusive range [0, 1000].</param>
    /// <param name="food">The typical food abundance in the inclusive range [0, 1000].</param>
    /// <param name="minerals">The typical mineral abundance in the inclusive range [0, 1000].</param>
    /// <param name="biomass">The typical biomass abundance in the inclusive range [0, 1000].</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more values are outside [0, 1000].</exception>
    public BiomeResourceProfile(int water, int food, int minerals, int biomass)
    {
        Validate(water, nameof(water));
        Validate(food, nameof(food));
        Validate(minerals, nameof(minerals));
        Validate(biomass, nameof(biomass));

        Water = water;
        Food = food;
        Minerals = minerals;
        Biomass = biomass;
    }

    /// <summary>
    /// Gets the typical water abundance.
    /// </summary>
    public int Water { get; }

    /// <summary>
    /// Gets the typical food abundance.
    /// </summary>
    public int Food { get; }

    /// <summary>
    /// Gets the typical mineral abundance.
    /// </summary>
    public int Minerals { get; }

    /// <summary>
    /// Gets the typical biomass abundance.
    /// </summary>
    public int Biomass { get; }

    private static void Validate(int value, string parameterName)
    {
        if (value < 0 || value > 1000)
        {
            throw new ArgumentOutOfRangeException(parameterName, "The biome resource profile value must be between 0 and 1000.");
        }
    }
}

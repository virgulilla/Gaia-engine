using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the deterministic ecological suitability profile of a biome.
/// </summary>
public sealed record BiomeSpeciesAffinityProfile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BiomeSpeciesAffinityProfile"/> class.
    /// </summary>
    /// <param name="herbivoreAffinity">The herbivore affinity in the inclusive range [0, 100].</param>
    /// <param name="carnivoreAffinity">The carnivore affinity in the inclusive range [0, 100].</param>
    /// <param name="plantDiversity">The plant diversity in the inclusive range [0, 100].</param>
    /// <param name="aquaticSuitability">The aquatic suitability in the inclusive range [0, 100].</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more values are outside [0, 100].</exception>
    public BiomeSpeciesAffinityProfile(int herbivoreAffinity, int carnivoreAffinity, int plantDiversity, int aquaticSuitability)
    {
        Validate(herbivoreAffinity, nameof(herbivoreAffinity));
        Validate(carnivoreAffinity, nameof(carnivoreAffinity));
        Validate(plantDiversity, nameof(plantDiversity));
        Validate(aquaticSuitability, nameof(aquaticSuitability));

        HerbivoreAffinity = herbivoreAffinity;
        CarnivoreAffinity = carnivoreAffinity;
        PlantDiversity = plantDiversity;
        AquaticSuitability = aquaticSuitability;
    }

    /// <summary>
    /// Gets the herbivore affinity.
    /// </summary>
    public int HerbivoreAffinity { get; }

    /// <summary>
    /// Gets the carnivore affinity.
    /// </summary>
    public int CarnivoreAffinity { get; }

    /// <summary>
    /// Gets the plant diversity.
    /// </summary>
    public int PlantDiversity { get; }

    /// <summary>
    /// Gets the aquatic suitability.
    /// </summary>
    public int AquaticSuitability { get; }

    private static void Validate(int value, string parameterName)
    {
        if (value < 0 || value > 100)
        {
            throw new ArgumentOutOfRangeException(parameterName, "The biome affinity value must be between 0 and 100.");
        }
    }
}

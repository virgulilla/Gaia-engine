using System;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the passive deterministic biome classification stored by one chunk.
/// </summary>
public sealed record BiomeState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BiomeState"/> class.
    /// </summary>
    /// <param name="biomeId">The biome identifier.</param>
    /// <param name="name">The biome name.</param>
    /// <param name="category">The biome category.</param>
    /// <param name="description">The biome description.</param>
    /// <param name="climateProfile">The climate profile.</param>
    /// <param name="terrainProfile">The terrain profile.</param>
    /// <param name="resourceProfile">The resource profile.</param>
    /// <param name="vegetationProfile">The vegetation profile.</param>
    /// <param name="speciesAffinity">The species affinity profile.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> or <paramref name="description"/> is empty.</exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when one or more profile arguments are <see langword="null"/>.
    /// </exception>
    public BiomeState(
        BiomeId biomeId,
        string name,
        BiomeCategory category,
        string description,
        BiomeClimateProfile climateProfile,
        BiomeTerrainProfile terrainProfile,
        BiomeResourceProfile resourceProfile,
        BiomeVegetationProfile vegetationProfile,
        BiomeSpeciesAffinityProfile speciesAffinity)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("The biome name must contain a value.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("The biome description must contain a value.", nameof(description));
        }

        BiomeId = biomeId;
        Name = name;
        Category = category;
        Description = description;
        ClimateProfile = climateProfile ?? throw new ArgumentNullException(nameof(climateProfile));
        TerrainProfile = terrainProfile ?? throw new ArgumentNullException(nameof(terrainProfile));
        ResourceProfile = resourceProfile ?? throw new ArgumentNullException(nameof(resourceProfile));
        VegetationProfile = vegetationProfile ?? throw new ArgumentNullException(nameof(vegetationProfile));
        SpeciesAffinity = speciesAffinity ?? throw new ArgumentNullException(nameof(speciesAffinity));
    }

    /// <summary>
    /// Gets the biome identifier.
    /// </summary>
    public BiomeId BiomeId { get; }

    /// <summary>
    /// Gets the biome name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the biome category.
    /// </summary>
    public BiomeCategory Category { get; }

    /// <summary>
    /// Gets the biome description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the climate profile.
    /// </summary>
    public BiomeClimateProfile ClimateProfile { get; }

    /// <summary>
    /// Gets the terrain profile.
    /// </summary>
    public BiomeTerrainProfile TerrainProfile { get; }

    /// <summary>
    /// Gets the resource profile.
    /// </summary>
    public BiomeResourceProfile ResourceProfile { get; }

    /// <summary>
    /// Gets the vegetation profile.
    /// </summary>
    public BiomeVegetationProfile VegetationProfile { get; }

    /// <summary>
    /// Gets the species affinity profile.
    /// </summary>
    public BiomeSpeciesAffinityProfile SpeciesAffinity { get; }
}

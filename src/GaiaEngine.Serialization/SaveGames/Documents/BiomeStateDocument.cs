namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized biome section inside a chunk save document.
/// </summary>
internal sealed class BiomeStateDocument
{
    public string BiomeId { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int AverageTemperature { get; set; }

    public int AverageRainfall { get; set; }

    public int Humidity { get; set; }

    public int WindIntensity { get; set; }

    public int SeasonalVariation { get; set; }

    public int MinimumElevation { get; set; }

    public int MaximumElevation { get; set; }

    public string DominantSoil { get; set; } = string.Empty;

    public string Surface { get; set; } = string.Empty;

    public int Drainage { get; set; }

    public int Water { get; set; }

    public int Food { get; set; }

    public int Minerals { get; set; }

    public int Biomass { get; set; }

    public string DominantVegetation { get; set; } = string.Empty;

    public int VegetationDensity { get; set; }

    public int HerbivoreAffinity { get; set; }

    public int CarnivoreAffinity { get; set; }

    public int PlantDiversity { get; set; }

    public int AquaticSuitability { get; set; }
}

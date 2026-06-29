using System;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using Xunit;

namespace GaiaEngine.Domain.Tests.WorldModel;

public sealed class BiomeStateTests
{
    [Fact]
    public void Constructor_ShouldStoreProfiles()
    {
        BiomeState biome = new(
            BiomeId.FromSequence(new EntitySequence(1)),
            "Forest",
            BiomeCategory.Forest,
            "Dense wooded biome.",
            new BiomeClimateProfile(17, 5, 68, 6, 10),
            new BiomeTerrainProfile(20, 80, SoilType.Loam, SurfaceType.Grass, 62),
            new BiomeResourceProfile(780, 820, 520, 900),
            new BiomeVegetationProfile(VegetationType.Forest, 84),
            new BiomeSpeciesAffinityProfile(80, 58, 78, 24));

        Assert.Equal("Forest", biome.Name);
        Assert.Equal(BiomeCategory.Forest, biome.Category);
        Assert.Equal(84, biome.VegetationProfile.Density);
        Assert.Equal(900, biome.ResourceProfile.Biomass);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyName()
    {
        Assert.Throws<ArgumentException>(
            () => new BiomeState(
                BiomeId.FromSequence(new EntitySequence(1)),
                string.Empty,
                BiomeCategory.Forest,
                "Dense wooded biome.",
                new BiomeClimateProfile(17, 5, 68, 6, 10),
                new BiomeTerrainProfile(20, 80, SoilType.Loam, SurfaceType.Grass, 62),
                new BiomeResourceProfile(780, 820, 520, 900),
                new BiomeVegetationProfile(VegetationType.Forest, 84),
                new BiomeSpeciesAffinityProfile(80, 58, 78, 24)));
    }
}

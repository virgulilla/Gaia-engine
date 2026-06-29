using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Serialization.SaveGames;
using Xunit;

namespace GaiaEngine.Serialization.Tests.SaveGames;

public sealed class JsonWorldSaveGameSerializerTests
{
    [Fact]
    public void SerializeAndDeserialize_ShouldRoundTripTheWorldSaveGame()
    {
        JsonWorldSaveGameSerializer serializer = new();
        WorldSaveGame saveGame = CreateSaveGame();

        string payload = serializer.Serialize(saveGame);
        WorldSaveGame restored = serializer.Deserialize(payload);

        Assert.Equal(saveGame.Metadata.SaveName, restored.Metadata.SaveName);
        Assert.Equal(saveGame.Metadata.WorldSeed, restored.Metadata.WorldSeed);
        Assert.Equal(saveGame.World.Metadata.WorldName, restored.World.Metadata.WorldName);
        Assert.Equal(saveGame.World.TimeState.CurrentTick, restored.World.TimeState.CurrentTick);
        Assert.Equal(saveGame.World.ChunkCount, restored.World.ChunkCount);
        Assert.Equal(saveGame.World.GetChunks()[0].Metadata.Coordinates, restored.World.GetChunks()[0].Metadata.Coordinates);
        Assert.Equal(saveGame.World.GetChunks()[0].Terrain.Elevation.Height, restored.World.GetChunks()[0].Terrain.Elevation.Height);
        Assert.Equal(saveGame.World.GetChunks()[0].Terrain.Soil.Fertility, restored.World.GetChunks()[0].Terrain.Soil.Fertility);
        Assert.Equal(saveGame.World.GetChunks()[0].Biome.Name, restored.World.GetChunks()[0].Biome.Name);
        Assert.Equal(saveGame.World.GetChunks()[0].Biome.ResourceProfile.Biomass, restored.World.GetChunks()[0].Biome.ResourceProfile.Biomass);
        Assert.Equal(saveGame.World.GetChunks()[0].Climate.Zone, restored.World.GetChunks()[0].Climate.Zone);
        Assert.Equal(saveGame.World.GetChunks()[0].Climate.Temperature.CurrentTemperature, restored.World.GetChunks()[0].Climate.Temperature.CurrentTemperature);
        Assert.Equal(saveGame.World.GetChunks()[0].Resources.GetAll()[0].ResourceId, restored.World.GetChunks()[0].Resources.GetAll()[0].ResourceId);
        Assert.Equal(saveGame.World.GetChunks()[0].Resources.GetAll()[0].CurrentAmount, restored.World.GetChunks()[0].Resources.GetAll()[0].CurrentAmount);
    }

    [Fact]
    public void Deserialize_ShouldRejectPayloadWithoutMetadata()
    {
        JsonWorldSaveGameSerializer serializer = new();
        string payload =
            """
            {"world":{"worldId":"72057594037927937","worldName":"Gaia","seed":42,"creationDate":"2026-06-28","engineVersion":"1.0.0","configurationVersion":"2026.06.28","width":10,"height":10,"chunkSize":16,"chunkCount":0,"maximumElevation":1,"currentTick":0,"currentDay":0,"currentSeason":"Spring","currentYear":0,"chunks":[]},"configurationVersion":"2026.06.28","version":{"formatVersion":"1.0.0","engineVersion":"1.0.0","contentVersion":"1.0.0"}}
            """;

        Assert.Throws<InvalidOperationException>(() => serializer.Deserialize(payload));
    }

    [Fact]
    public void Deserialize_ShouldRejectChunkWithAnotherWorldId()
    {
        JsonWorldSaveGameSerializer serializer = new();
        string payload =
            """
            {"metadata":{"saveName":"Gaia","creationDate":"2026-06-28","lastModified":"2026-06-28","worldSeed":42,"engineVersion":"1.0.0","saveVersion":"1.0.0"},"world":{"worldId":"72057594037927937","worldName":"Gaia","seed":42,"creationDate":"2026-06-28","engineVersion":"1.0.0","configurationVersion":"2026.06.28","width":10,"height":10,"chunkSize":16,"chunkCount":1,"maximumElevation":1,"currentTick":0,"currentDay":0,"currentSeason":"Spring","currentYear":0,"chunks":[{"chunkId":"144115188075855873","worldId":"72057594037927938","x":0,"y":0,"seed":1,"size":16,"state":"Active","terrain":{"height":10,"relativeHeight":0,"seaLevelOffset":0,"gradient":4,"aspect":90,"traversalCost":120,"soilType":"Loam","fertility":70,"drainage":60,"moistureCapacity":70,"organicMatter":65,"surface":"Grass","geology":"Granite","modifiers":[]},"biome":{"biomeId":"504403158265495658","name":"Grassland","category":"Plains","description":"Open plains biome.","averageTemperature":18,"averageRainfall":2,"humidity":55,"windIntensity":4,"seasonalVariation":8,"minimumElevation":0,"maximumElevation":20,"dominantSoil":"Loam","surface":"Grass","drainage":60,"water":750,"food":800,"minerals":500,"biomass":800,"dominantVegetation":"Grassland","vegetationDensity":62,"herbivoreAffinity":72,"carnivoreAffinity":46,"plantDiversity":60,"aquaticSuitability":20},"climate":{"zone":"Temperate","weatherState":"Clear","currentTemperature":18,"dailyAverageTemperature":18,"seasonalAverageTemperature":18,"dailyTemperatureVariation":0,"relativeHumidity":55,"evaporationRate":3,"condensationRate":2,"windDirection":90,"windSpeed":4,"windGustStrength":6,"precipitationType":"None","precipitationIntensity":0,"precipitationDuration":0,"precipitationCoverage":0,"pressure":1012},"resources":[],"organismIds":[]}]},"configurationVersion":"2026.06.28","version":{"formatVersion":"1.0.0","engineVersion":"1.0.0","contentVersion":"1.0.0"}}
            """;

        Assert.Throws<ArgumentException>(() => serializer.Deserialize(payload));
    }

    private static WorldSaveGame CreateSaveGame()
    {
        WorldId worldId = WorldId.FromSequence(new EntitySequence(1));
        World world = new(
            new WorldMetadata(
                worldId,
                "Gaia",
                new WorldSeed(42),
                "2026-06-28",
                new EngineVersion(1, 0, 0),
                new ConfigurationVersion("2026.06.28")),
            new WorldDimensions(100, 100, 16, 1, 200),
            new WorldTimeState(20, 2, "Spring", 0),
            new List<Chunk>
            {
                new(
                    new ChunkMetadata(
                        ChunkId.FromSequence(new EntitySequence(10)),
                        worldId,
                        new ChunkCoordinates(0, 0),
                        new WorldSeed(100),
                        16),
                    ChunkState.Active,
                    CreateTerrain(10),
                    CreateBiome(10),
                    new ClimateState(
                        ClimateZone.Temperate,
                        WeatherState.Clear,
                        new TemperatureState(18, 18, 18, 0),
                        new HumidityState(55, 3, 2),
                        new WindState(90, 4, 6),
                        new PrecipitationState(PrecipitationType.None, 0, 0, 0),
                        new PressureState(1012)),
                    CreateResources(10),
                    Array.Empty<OrganismId>()),
            });

        SaveMetadata metadata = new(
            "Gaia",
            "2026-06-28",
            "2026-06-28",
            new WorldSeed(42),
            new EngineVersion(1, 0, 0),
            "1.0.0");

        SaveVersionInfo version = new("1.0.0", new EngineVersion(1, 0, 0), "1.0.0");
        return new WorldSaveGame(metadata, world, new ConfigurationVersion("2026.06.28"), version);
    }

    private static TerrainState CreateTerrain(ulong sequence)
    {
        return new TerrainState(
            new ElevationState(60 + (int)sequence, (int)sequence, (int)sequence),
            new SlopeState(10, (int)((sequence * 31) % 360), 120),
            new SoilState(SoilType.Loam, 70, 60, 70, 65),
            SurfaceType.Grass,
            GeologyType.Granite,
            Array.Empty<TerrainModifierState>());
    }

    private static BiomeState CreateBiome(ulong sequence)
    {
        return new BiomeState(
            BiomeId.FromSequence(new EntitySequence((sequence * 10) + 4)),
            "Grassland",
            BiomeCategory.Plains,
            "Open plains with moderate fertility and dominant grass vegetation.",
            new BiomeClimateProfile(18, 2, 55, 4, 8),
            new BiomeTerrainProfile(50, 80, SoilType.Loam, SurfaceType.Grass, 60),
            new BiomeResourceProfile(750, 800, 500, 800),
            new BiomeVegetationProfile(VegetationType.Grassland, 62),
            new BiomeSpeciesAffinityProfile(72, 46, 60, 20));
    }

    private static ChunkResources CreateResources(ulong sequence)
    {
        return new ChunkResources(
            new ResourceState[]
            {
                new(
                    ResourceId.FromSequence(new EntitySequence((sequence * 10) + 1)),
                    ResourceType.Vegetation,
                    ResourceCategory.Renewable,
                    400,
                    500,
                    4,
                    70,
                    800),
                new(
                    ResourceId.FromSequence(new EntitySequence((sequence * 10) + 2)),
                    ResourceType.FreshWater,
                    ResourceCategory.Renewable,
                    300,
                    400,
                    3,
                    80,
                    750),
                new(
                    ResourceId.FromSequence(new EntitySequence((sequence * 10) + 3)),
                    ResourceType.Minerals,
                    ResourceCategory.NonRenewable,
                    250,
                    250,
                    0,
                    65,
                    500),
            });
    }
}

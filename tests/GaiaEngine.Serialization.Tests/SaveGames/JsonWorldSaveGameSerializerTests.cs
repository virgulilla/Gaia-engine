using System;
using System.Collections.Generic;
using GaiaEngine.Domain.AI;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Player;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Serialization.Profiles;
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
        Assert.Equal(saveGame.World.GetChunks()[0].Water.SurfaceWater.WaterLevel, restored.World.GetChunks()[0].Water.SurfaceWater.WaterLevel);
        Assert.Equal(saveGame.World.GetChunks()[0].Water.GroundWater.Saturation, restored.World.GetChunks()[0].Water.GroundWater.Saturation);
        Assert.Equal(saveGame.World.GetChunks()[0].Resources.GetAll()[0].ResourceId, restored.World.GetChunks()[0].Resources.GetAll()[0].ResourceId);
        Assert.Equal(saveGame.World.GetChunks()[0].Resources.GetAll()[0].CurrentAmount, restored.World.GetChunks()[0].Resources.GetAll()[0].CurrentAmount);
        Assert.Single(restored.Organisms.GetAll());
        Assert.Equal(saveGame.Organisms.GetAll()[0].Id, restored.Organisms.GetAll()[0].Id);
        Assert.Equal(saveGame.Organisms.GetAll()[0].CurrentChunkId, restored.Organisms.GetAll()[0].CurrentChunkId);
        Assert.Equal(saveGame.Organisms.GetAll()[0].Needs.Hunger, restored.Organisms.GetAll()[0].Needs.Hunger);
        Assert.Single(restored.Genomes.GetAll());
        Assert.Equal(saveGame.Genomes.GetAll()[0].Id, restored.Genomes.GetAll()[0].Id);
        Assert.Equal(saveGame.Genomes.GetAll()[0].Physiology.GetGenes()[0].Key, restored.Genomes.GetAll()[0].Physiology.GetGenes()[0].Key);
        Assert.Equal(saveGame.Genomes.GetAll()[0].MutationVersion, restored.Genomes.GetAll()[0].MutationVersion);
        Assert.Single(restored.Genomes.GetAll()[0].MutationHistory.GetAll());
        Assert.Single(restored.Species.GetAll());
        Assert.Equal(saveGame.Species.GetAll()[0].Id, restored.Species.GetAll()[0].Id);
        Assert.Equal(saveGame.Species.GetAll()[0].GetFounderPopulation()[0], restored.Species.GetAll()[0].GetFounderPopulation()[0]);
        Assert.Single(restored.Memories.GetAll());
        Assert.Equal(saveGame.Memories.GetAll()[0].OrganismId, restored.Memories.GetAll()[0].OrganismId);
        Assert.Equal(saveGame.Memories.GetAll()[0].GetAll()[0].Identifier, restored.Memories.GetAll()[0].GetAll()[0].Identifier);
        Assert.Equal(saveGame.Memories.GetAll()[0].GetAll()[0].Confidence, restored.Memories.GetAll()[0].GetAll()[0].Confidence);
        Assert.Single(restored.ActionRequests.GetAll());
        Assert.Equal(saveGame.ActionRequests.GetAll()[0].ActionId, restored.ActionRequests.GetAll()[0].ActionId);
        Assert.Equal(saveGame.ActionRequests.GetAll()[0].ActionType, restored.ActionRequests.GetAll()[0].ActionType);
    }

    [Fact]
    public void PlayerProfileSerializer_ShouldRoundTripThePlayerProfile()
    {
        JsonPlayerProfileSerializer serializer = new();
        PlayerProfile profile = new(
            new PlayerIdentity("player-001", "Oscar", "2026-06-30"),
            new PlayerKnowledge(
                new DiscoveryCollection(
                    new[]
                    {
                        new DiscoveryEntry(
                            "species.herbivore.a",
                            DiscoveryCategory.Species,
                            "Herbivore A",
                            "Observed a new herbivore species.",
                            unlockTick: 40,
                            WorldId.FromSequence(new EntitySequence(1)),
                            "player-001"),
                    })),
            new PlayerProgression(10, 1, 0),
            new PlayerStatistics(1, 0));

        string payload = serializer.Serialize(profile);
        PlayerProfile restored = serializer.Deserialize(payload);

        Assert.Equal(profile.Identity.PlayerId, restored.Identity.PlayerId);
        Assert.Equal(profile.Identity.ProfileName, restored.Identity.ProfileName);
        Assert.Equal(profile.Knowledge.Discoveries.GetAll()[0].DiscoveryId, restored.Knowledge.Discoveries.GetAll()[0].DiscoveryId);
        Assert.Equal(profile.Knowledge.Discoveries.GetAll()[0].WorldId, restored.Knowledge.Discoveries.GetAll()[0].WorldId);
        Assert.Equal(profile.Progression.Experience, restored.Progression.Experience);
        Assert.Equal(profile.Statistics.TotalDiscoveriesUnlocked, restored.Statistics.TotalDiscoveriesUnlocked);
    }

    [Fact]
    public void Deserialize_ShouldRejectPayloadWithoutMetadata()
    {
        JsonWorldSaveGameSerializer serializer = new();
        string payload =
            """
            {"world":{"worldId":"72057594037927937","worldName":"Gaia","seed":42,"creationDate":"2026-06-28","engineVersion":"1.0.0","configurationVersion":"2026.06.28","width":10,"height":10,"chunkSize":16,"chunkCount":0,"maximumElevation":1,"currentTick":0,"currentDay":0,"currentSeason":"Spring","currentYear":0,"chunks":[]},"configurationVersion":"2026.06.28","organisms":[],"genomes":[],"actionRequests":[],"version":{"formatVersion":"1.0.0","engineVersion":"1.0.0","contentVersion":"1.0.0"}}
            """;

        Assert.Throws<InvalidOperationException>(() => serializer.Deserialize(payload));
    }

    [Fact]
    public void Deserialize_ShouldRejectChunkWithAnotherWorldId()
    {
        JsonWorldSaveGameSerializer serializer = new();
        string payload =
            """
            {"metadata":{"saveName":"Gaia","creationDate":"2026-06-28","lastModified":"2026-06-28","worldSeed":42,"engineVersion":"1.0.0","saveVersion":"1.0.0"},"world":{"worldId":"72057594037927937","worldName":"Gaia","seed":42,"creationDate":"2026-06-28","engineVersion":"1.0.0","configurationVersion":"2026.06.28","width":10,"height":10,"chunkSize":16,"chunkCount":1,"maximumElevation":1,"currentTick":0,"currentDay":0,"currentSeason":"Spring","currentYear":0,"chunks":[{"chunkId":"144115188075855873","worldId":"72057594037927938","x":0,"y":0,"seed":1,"size":16,"state":"Active","terrain":{"height":10,"relativeHeight":0,"seaLevelOffset":0,"gradient":4,"aspect":90,"traversalCost":120,"soilType":"Loam","fertility":70,"drainage":60,"moistureCapacity":70,"organicMatter":65,"surface":"Grass","geology":"Granite","modifiers":[]},"biome":{"biomeId":"504403158265495658","name":"Grassland","category":"Plains","description":"Open plains biome.","averageTemperature":18,"averageRainfall":2,"humidity":55,"windIntensity":4,"seasonalVariation":8,"minimumElevation":0,"maximumElevation":20,"dominantSoil":"Loam","surface":"Grass","drainage":60,"water":750,"food":800,"minerals":500,"biomass":800,"dominantVegetation":"Grassland","vegetationDensity":62,"herbivoreAffinity":72,"carnivoreAffinity":46,"plantDiversity":60,"aquaticSuitability":20},"climate":{"zone":"Temperate","weatherState":"Clear","currentTemperature":18,"dailyAverageTemperature":18,"seasonalAverageTemperature":18,"dailyTemperatureVariation":0,"relativeHumidity":55,"evaporationRate":3,"condensationRate":2,"windDirection":90,"windSpeed":4,"windGustStrength":6,"precipitationType":"None","precipitationIntensity":0,"precipitationDuration":0,"precipitationCoverage":0,"pressure":1012},"water":{"surfaceWater":{"waterLevel":220,"flowSpeed":3,"flowDirection":90,"waterVolume":400},"groundWater":{"waterTable":42,"saturation":58,"rechargeRate":6,"extractionRate":0},"river":null,"lake":null,"ocean":null},"resources":[],"organismIds":[]}]},"configurationVersion":"2026.06.28","organisms":[],"genomes":[],"actionRequests":[],"version":{"formatVersion":"1.0.0","engineVersion":"1.0.0","contentVersion":"1.0.0"}}
            """;

        Assert.Throws<ArgumentException>(() => serializer.Deserialize(payload));
    }

    [Fact]
    public void Deserialize_ShouldRejectOrganismMissingFromOwningChunk()
    {
        JsonWorldSaveGameSerializer serializer = new();
        string payload =
            """
            {"metadata":{"saveName":"Gaia","creationDate":"2026-06-28","lastModified":"2026-06-28","worldSeed":42,"engineVersion":"1.0.0","saveVersion":"1.0.0"},"world":{"worldId":"72057594037927937","worldName":"Gaia","seed":42,"creationDate":"2026-06-28","engineVersion":"1.0.0","configurationVersion":"2026.06.28","width":10,"height":10,"chunkSize":16,"chunkCount":1,"maximumElevation":1,"currentTick":0,"currentDay":0,"currentSeason":"Spring","currentYear":0,"chunks":[{"chunkId":"144115188075855874","worldId":"72057594037927937","x":0,"y":0,"seed":1,"size":16,"state":"Active","terrain":{"height":10,"relativeHeight":0,"seaLevelOffset":0,"gradient":4,"aspect":90,"traversalCost":120,"soilType":"Loam","fertility":70,"drainage":60,"moistureCapacity":70,"organicMatter":65,"surface":"Grass","geology":"Granite","modifiers":[]},"biome":{"biomeId":"504403158265495658","name":"Grassland","category":"Plains","description":"Open plains biome.","averageTemperature":18,"averageRainfall":2,"humidity":55,"windIntensity":4,"seasonalVariation":8,"minimumElevation":0,"maximumElevation":20,"dominantSoil":"Loam","surface":"Grass","drainage":60,"water":750,"food":800,"minerals":500,"biomass":800,"dominantVegetation":"Grassland","vegetationDensity":62,"herbivoreAffinity":72,"carnivoreAffinity":46,"plantDiversity":60,"aquaticSuitability":20},"climate":{"zone":"Temperate","weatherState":"Clear","currentTemperature":18,"dailyAverageTemperature":18,"seasonalAverageTemperature":18,"dailyTemperatureVariation":0,"relativeHumidity":55,"evaporationRate":3,"condensationRate":2,"windDirection":90,"windSpeed":4,"windGustStrength":6,"precipitationType":"None","precipitationIntensity":0,"precipitationDuration":0,"precipitationCoverage":0,"pressure":1012},"water":{"surfaceWater":{"waterLevel":220,"flowSpeed":3,"flowDirection":90,"waterVolume":400},"groundWater":{"waterTable":42,"saturation":58,"rechargeRate":6,"extractionRate":0},"river":null,"lake":null,"ocean":null},"resources":[],"organismIds":[]}]},"configurationVersion":"2026.06.28","organisms":[{"organismId":"216172782113783908","speciesId":"288230376151711745","genomeId":"360287970189639880","currentChunkId":"144115188075855874","metabolismRate":3,"growthRate":2,"lifespanTicks":500,"waterEfficiency":60,"digestionEfficiency":55,"bodyTemperature":18,"hunger":100,"hydration":100,"rest":100,"reproductionUrge":0,"birthTick":0,"ageTicks":0,"maturityAgeTicks":100,"stage":"Juvenile","isAlive":true,"currentHealth":100,"maximumHealth":100}],"genomes":[],"actionRequests":[],"version":{"formatVersion":"1.0.0","engineVersion":"1.0.0","contentVersion":"1.0.0"}}
            """;

        Assert.Throws<InvalidOperationException>(() => serializer.Deserialize(payload));
    }

    private static WorldSaveGame CreateSaveGame()
    {
        WorldId worldId = WorldId.FromSequence(new EntitySequence(1));
        Organism organism = new(
            OrganismId.FromSequence(new EntitySequence(100)),
            SpeciesId.FromSequence(new EntitySequence(1)),
            GenomeId.FromSequence(new EntitySequence(200)),
            ChunkId.FromSequence(new EntitySequence(10)),
            new PhysiologyComponent(3, 2, 500, 60, 55, 18),
            new NeedsComponent(120, 110, 90, 0),
            new LifecycleComponent(0, 12, 100, LifecycleStage.Juvenile, true),
            new HealthComponent(95, 100));
        SimulationActionRequest actionRequest = new(
            ActionId.FromSequence(new EntitySequence(500)),
            organism.Id,
            SimulationActionType.Drink,
            new SimulationActionTarget(ActionTargetKind.Chunk, ChunkId.FromSequence(new EntitySequence(10)).ToString()),
            20,
            1,
            0,
            ActionExecutionState.Waiting,
            true);
        Genome genome = new(
            new GenomeIdentity(organism.GenomeId, 1, null, null, 0, 0),
            new GenomeGeneGroup(GenomeGroupType.Morphology, new[] { new GenomeGene(GenomeGeneKey.BodySize, new NormalizedGeneValue(520), GeneDominance.Dominant, true) }),
            new GenomeGeneGroup(GenomeGroupType.Physiology, new[] { new GenomeGene(GenomeGeneKey.Metabolism, new NormalizedGeneValue(410), GeneDominance.Recessive, true) }),
            new GenomeGeneGroup(GenomeGroupType.Reproduction, new[] { new GenomeGene(GenomeGeneKey.Fertility, new NormalizedGeneValue(330), GeneDominance.Blended, true) }),
            new GenomeGeneGroup(GenomeGroupType.Senses, new[] { new GenomeGene(GenomeGeneKey.VisionRange, new NormalizedGeneValue(470), GeneDominance.CoDominant, true) }),
            new GenomeGeneGroup(GenomeGroupType.Adaptation, new[] { new GenomeGene(GenomeGeneKey.DesertAdaptation, new NormalizedGeneValue(180), GeneDominance.Dominant, true) }),
            new GenomeGeneGroup(GenomeGroupType.Appearance, new[] { new GenomeGene(GenomeGeneKey.PrimaryColor, new NormalizedGeneValue(650), GeneDominance.Dominant, true) }),
            new GenomeGeneGroup(GenomeGroupType.BehaviourBias, new[] { new GenomeGene(GenomeGeneKey.Curiosity, new NormalizedGeneValue(510), GeneDominance.Blended, true) }),
            mutationVersion: 1,
            new GenomeMutationHistory(
                new[]
                {
                    new GenomeMutationRecord(
                        sequence: 0,
                        GenomeGroupType.Morphology,
                        GenomeGeneKey.BodySize,
                        MutationCategory.Parameter,
                        previousValue: 500,
                        newValue: 520,
                        previousDominance: GeneDominance.Dominant,
                        newDominance: GeneDominance.Dominant,
                        previousIsActive: true,
                        newIsActive: true),
                }));
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
                    CreateWater(10),
                    CreateResources(10),
                    new[] { organism.Id }),
            });

        SaveMetadata metadata = new(
            "Gaia",
            "2026-06-28",
            "2026-06-28",
            new WorldSeed(42),
            new EngineVersion(1, 0, 0),
            "1.0.0");

        SaveVersionInfo version = new("1.0.0", new EngineVersion(1, 0, 0), "1.0.0");
        Species species = new(
            organism.SpeciesId,
            parentSpeciesId: null,
            originTick: 0,
            extinctionTick: null,
            new[] { organism.Id });
        MemoryCollection memories = new(
            new[]
            {
                new OrganismMemory(
                    organism.Id,
                    new[]
                    {
                        new MemoryEntry(
                            identifier: 1234,
                            MemoryCategory.Resource,
                            new ChunkCoordinates(0, 0),
                            confidence: 780,
                            creationTick: 10,
                            lastUpdateTick: 15,
                            expirationTick: 120,
                            estimatedAvailability: 640),
                    }),
            });
        return new WorldSaveGame(
            metadata,
            world,
            new OrganismCollection(new[] { organism }),
            new GenomeCollection(new[] { genome }),
            new SpeciesCollection(new[] { species }),
            memories,
            new SimulationActionRequestCollection(new[] { actionRequest }),
            new ConfigurationVersion("2026.06.28"),
            version);
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

    private static WaterState CreateWater(ulong sequence)
    {
        return new WaterState(
            new SurfaceWaterState(220 + (int)sequence, 3, 90, 400 + ((int)sequence * 10)),
            new GroundWaterState(42, 58, 6, 0),
            null,
            null,
            null);
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

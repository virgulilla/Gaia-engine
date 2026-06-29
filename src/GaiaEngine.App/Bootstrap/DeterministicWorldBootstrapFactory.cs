using System;
using System.Collections.Generic;
using GaiaEngine.App.Configuration;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Creates the initial deterministic world instance used by the current host bootstrap slice.
/// </summary>
public sealed class DeterministicWorldBootstrapFactory
{
    private readonly WorldConfiguration worldConfiguration;
    private readonly EngineConfiguration engineConfiguration;
    private readonly SimulationConfiguration simulationConfiguration;
    private readonly IEntityIdGenerator idGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicWorldBootstrapFactory"/> class.
    /// </summary>
    /// <param name="worldConfiguration">The bootstrap world configuration.</param>
    /// <param name="engineConfiguration">The engine configuration.</param>
    /// <param name="simulationConfiguration">The simulation configuration.</param>
    /// <param name="idGenerator">The deterministic identifier generator.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any required dependency is <see langword="null"/>.
    /// </exception>
    public DeterministicWorldBootstrapFactory(
        WorldConfiguration worldConfiguration,
        EngineConfiguration engineConfiguration,
        SimulationConfiguration simulationConfiguration,
        IEntityIdGenerator idGenerator)
    {
        this.worldConfiguration = worldConfiguration ?? throw new ArgumentNullException(nameof(worldConfiguration));
        this.engineConfiguration = engineConfiguration ?? throw new ArgumentNullException(nameof(engineConfiguration));
        this.simulationConfiguration = simulationConfiguration ?? throw new ArgumentNullException(nameof(simulationConfiguration));
        this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    }

    /// <summary>
    /// Creates the initial deterministic world instance.
    /// </summary>
    /// <returns>The created world instance.</returns>
    public GaiaEngine.Domain.World.World CreateWorld()
    {
        WorldSeed worldSeed = new(worldConfiguration.WorldSeed);
        WorldId worldId = idGenerator.CreateWorldId(new IdentifierGenerationContext(worldSeed, 0, new EntitySequence(1)));
        List<Chunk> chunks = new(worldConfiguration.ChunkColumns * worldConfiguration.ChunkRows);

        ulong sequence = 2;
        for (int row = 0; row < worldConfiguration.ChunkRows; row++)
        {
            for (int column = 0; column < worldConfiguration.ChunkColumns; column++)
            {
                chunks.Add(CreateChunk(worldId, worldSeed, column, row, sequence));
                sequence++;
            }
        }

        return new GaiaEngine.Domain.World.World(
            new WorldMetadata(
                worldId,
                worldConfiguration.WorldName,
                worldSeed,
                "2026-06-28",
                engineConfiguration.EngineVersion,
                engineConfiguration.ConfigurationVersion),
            new WorldDimensions(
                worldConfiguration.ChunkColumns * worldConfiguration.ChunkSize,
                worldConfiguration.ChunkRows * worldConfiguration.ChunkSize,
                worldConfiguration.ChunkSize,
                worldConfiguration.ChunkColumns * worldConfiguration.ChunkRows,
                worldConfiguration.MaximumElevation),
            new WorldTimeState(
                currentTick: 0,
                currentDay: simulationConfiguration.StartingDay,
                currentSeason: simulationConfiguration.StartingSeason,
                currentYear: simulationConfiguration.StartingYear),
            chunks.AsReadOnly());
    }

    private Chunk CreateChunk(WorldId worldId, WorldSeed worldSeed, int column, int row, ulong sequence)
    {
        ChunkId chunkId = idGenerator.CreateChunkId(new IdentifierGenerationContext(worldSeed, 0, new EntitySequence(sequence)));
        long chunkSeedValue = worldSeed.Value + (column * 101L) + (row * 211L) + (long)sequence;
        ClimateState climate = new(
            worldConfiguration.DefaultClimateZone,
            WeatherState.Clear,
            new TemperatureState(18, 18, 18, 0),
            new HumidityState(55, 3, 2),
            new WindState(90, 4, 6),
            new PrecipitationState(PrecipitationType.None, 0, 0, 0),
            new PressureState(1012));
        TerrainState terrain = CreateTerrainState(column, row, chunkSeedValue);
        WaterState water = CreateWaterState(column, row, chunkSeedValue, terrain, climate);
        ChunkResources resources = CreateDefaultResources(sequence);
        BiomeState biome = CreateBiomeState(worldSeed, sequence, terrain, climate, resources);

        return new Chunk(
            new ChunkMetadata(
                chunkId,
                worldId,
                new ChunkCoordinates(column, row),
                new WorldSeed(chunkSeedValue),
                worldConfiguration.ChunkSize),
            ChunkState.Active,
            terrain,
            biome,
            climate,
            water,
            resources,
            Array.Empty<OrganismId>());
    }

    private TerrainState CreateTerrainState(int column, int row, long chunkSeedValue)
    {
        int normalizedSeed = (int)Math.Abs(chunkSeedValue % 997);
        int seaLevel = worldConfiguration.MaximumElevation / 3;
        int height = Math.Min(worldConfiguration.MaximumElevation, seaLevel + ((normalizedSeed + (column * 13) - (row * 7)) % Math.Max(1, worldConfiguration.MaximumElevation - seaLevel + 1)));
        int relativeHeight = height - seaLevel;
        int gradient = normalizedSeed % 36;
        int aspect = (normalizedSeed * 17) % 360;
        int traversalCost = 100 + (gradient * 2);
        SoilType soilType = ResolveSoilType(normalizedSeed);
        SoilState soil = new(
            soilType,
            fertility: ResolveFertility(soilType, relativeHeight),
            drainage: ResolveDrainage(soilType, gradient),
            moistureCapacity: ResolveMoistureCapacity(soilType),
            organicMatter: ResolveOrganicMatter(soilType));
        SurfaceType surface = ResolveSurfaceType(soilType, relativeHeight, gradient);
        GeologyType geology = ResolveGeologyType(normalizedSeed);

        return new TerrainState(
            new ElevationState(height, relativeHeight, relativeHeight),
            new SlopeState(gradient, aspect, traversalCost),
            soil,
            surface,
            geology,
            Array.Empty<TerrainModifierState>());
    }

    private static ChunkResources CreateDefaultResources(ulong chunkSequence)
    {
        return new ChunkResources(
            new ResourceState[]
            {
                new(
                    ResourceId.FromSequence(new EntitySequence((chunkSequence * 10) + 1)),
                    ResourceType.Vegetation,
                    ResourceCategory.Renewable,
                    400,
                    500,
                    4,
                    70,
                    800),
                new(
                    ResourceId.FromSequence(new EntitySequence((chunkSequence * 10) + 2)),
                    ResourceType.FreshWater,
                    ResourceCategory.Renewable,
                    300,
                    400,
                    3,
                    80,
                    750),
                new(
                    ResourceId.FromSequence(new EntitySequence((chunkSequence * 10) + 3)),
                    ResourceType.Minerals,
                    ResourceCategory.NonRenewable,
                    250,
                    250,
                    0,
                    65,
                    500),
            });
    }

    private static SoilType ResolveSoilType(int normalizedSeed)
    {
        return (normalizedSeed % 6) switch
        {
            0 => SoilType.Loam,
            1 => SoilType.Clay,
            2 => SoilType.Sand,
            3 => SoilType.Silt,
            4 => SoilType.Peat,
            _ => SoilType.Rock,
        };
    }

    private static GeologyType ResolveGeologyType(int normalizedSeed)
    {
        return (normalizedSeed % 4) switch
        {
            0 => GeologyType.Granite,
            1 => GeologyType.Limestone,
            2 => GeologyType.Clay,
            _ => GeologyType.VolcanicRock,
        };
    }

    private static int ResolveFertility(SoilType soilType, int relativeHeight)
    {
        int baseFertility = soilType switch
        {
            SoilType.Loam => 80,
            SoilType.Silt => 74,
            SoilType.Peat => 68,
            SoilType.Clay => 60,
            SoilType.Sand => 34,
            SoilType.Rock => 18,
            _ => 50,
        };

        return Math.Clamp(baseFertility - Math.Max(0, relativeHeight / 4), 0, 100);
    }

    private static int ResolveDrainage(SoilType soilType, int gradient)
    {
        int baseDrainage = soilType switch
        {
            SoilType.Sand => 82,
            SoilType.Loam => 66,
            SoilType.Silt => 58,
            SoilType.Peat => 45,
            SoilType.Clay => 38,
            SoilType.Rock => 70,
            _ => 50,
        };

        return Math.Clamp(baseDrainage + (gradient / 3), 0, 100);
    }

    private static int ResolveMoistureCapacity(SoilType soilType)
    {
        return soilType switch
        {
            SoilType.Clay => 82,
            SoilType.Peat => 88,
            SoilType.Loam => 70,
            SoilType.Silt => 64,
            SoilType.Sand => 30,
            SoilType.Rock => 18,
            _ => 50,
        };
    }

    private static int ResolveOrganicMatter(SoilType soilType)
    {
        return soilType switch
        {
            SoilType.Peat => 92,
            SoilType.Loam => 72,
            SoilType.Silt => 58,
            SoilType.Clay => 46,
            SoilType.Sand => 16,
            SoilType.Rock => 6,
            _ => 40,
        };
    }

    private static SurfaceType ResolveSurfaceType(SoilType soilType, int relativeHeight, int gradient)
    {
        if (relativeHeight < -10)
        {
            return SurfaceType.Mud;
        }

        if (relativeHeight > 45)
        {
            return SurfaceType.Snow;
        }

        if (gradient > 24 || soilType == SoilType.Rock)
        {
            return SurfaceType.Rock;
        }

        return soilType switch
        {
            SoilType.Sand => SurfaceType.Sand,
            SoilType.Clay => SurfaceType.Mud,
            _ => SurfaceType.Grass,
        };
    }

    private WaterState CreateWaterState(int column, int row, long chunkSeedValue, TerrainState terrain, ClimateState climate)
    {
        int normalizedSeed = (int)Math.Abs(chunkSeedValue % 997);
        int seaLevel = worldConfiguration.MaximumElevation / 3;
        int drainageResistance = Math.Max(0, 100 - terrain.Soil.Drainage);
        int baseSurfaceLevel = Math.Clamp((climate.Humidity.RelativeHumidity * 6) + drainageResistance - Math.Max(0, terrain.Elevation.RelativeHeight * 4), 0, 1000);
        int flowSpeed = Math.Max(0, (terrain.Slope.Gradient / 2) + (baseSurfaceLevel / 100));
        int flowDirection = (int)(((column * 37L) + (row * 53L) + normalizedSeed) % 360);
        int surfaceVolume = Math.Max(0, (baseSurfaceLevel * worldConfiguration.ChunkSize) / 10);
        int groundWaterTable = Math.Clamp(seaLevel + (terrain.Soil.MoistureCapacity / 2) - Math.Max(0, terrain.Elevation.RelativeHeight / 2), 0, worldConfiguration.MaximumElevation);
        int groundWaterSaturation = Math.Clamp((terrain.Soil.MoistureCapacity + climate.Humidity.RelativeHumidity) / 2, 0, 100);
        int rechargeRate = Math.Max(0, (climate.Humidity.CondensationRate + terrain.Soil.Drainage) / 4);

        RiverState? river = null;
        if (terrain.Slope.Gradient >= 10 && baseSurfaceLevel >= 360)
        {
            river = new RiverState(
                $"river-{column}-{row}",
                width: Math.Max(1, terrain.Slope.Gradient / 4),
                depth: Math.Max(1, baseSurfaceLevel / 180),
                flowRate: Math.Max(1, flowSpeed * 3),
                currentVelocity: Math.Max(1, flowSpeed));
        }

        LakeState? lake = null;
        if (terrain.Soil.Drainage <= 48 && baseSurfaceLevel >= 420)
        {
            lake = new LakeState(
                surfaceArea: Math.Max(1, (baseSurfaceLevel * worldConfiguration.ChunkSize) / 20),
                maximumDepth: Math.Max(1, baseSurfaceLevel / 140),
                waterVolume: Math.Max(1, surfaceVolume / 2),
                overflowLevel: Math.Clamp(baseSurfaceLevel + 120, 0, 1000));
        }

        OceanState? ocean = null;
        if (terrain.Elevation.Height <= seaLevel)
        {
            ocean = new OceanState(seaLevel, salinity: 350, temperature: climate.Temperature.CurrentTemperature);
        }

        return new WaterState(
            new SurfaceWaterState(baseSurfaceLevel, flowSpeed, flowDirection, surfaceVolume),
            new GroundWaterState(groundWaterTable, groundWaterSaturation, rechargeRate, extractionRate: 0),
            river,
            lake,
            ocean);
    }

    private BiomeState CreateBiomeState(WorldSeed worldSeed, ulong chunkSequence, TerrainState terrain, ClimateState climate, ChunkResources resources)
    {
        BiomeDefinition definition = ResolveBiomeDefinition(terrain, climate, resources);
        BiomeId biomeId = idGenerator.CreateBiomeId(new IdentifierGenerationContext(worldSeed, 0, new EntitySequence((chunkSequence * 10) + 4)));

        return new BiomeState(
            biomeId,
            definition.Name,
            definition.Category,
            definition.Description,
            definition.ClimateProfile,
            definition.TerrainProfile,
            definition.ResourceProfile,
            definition.VegetationProfile,
            definition.SpeciesAffinity);
    }

    private static BiomeDefinition ResolveBiomeDefinition(TerrainState terrain, ClimateState climate, ChunkResources resources)
    {
        int waterAvailability = GetAvailability(resources, ResourceType.FreshWater);
        int vegetationAvailability = GetAvailability(resources, ResourceType.Vegetation);
        int mineralAvailability = GetAvailability(resources, ResourceType.Minerals);

        if (terrain.Geology == GeologyType.VolcanicRock)
        {
            return CreateVolcanicDefinition(terrain, climate, waterAvailability, vegetationAvailability, mineralAvailability);
        }

        if (terrain.Elevation.RelativeHeight > 35 || terrain.Slope.Gradient >= 24)
        {
            return CreateMountainDefinition(terrain, climate, waterAvailability, vegetationAvailability, mineralAvailability);
        }

        if (climate.Zone == ClimateZone.Arid || vegetationAvailability < 350)
        {
            return CreateDesertDefinition(terrain, climate, waterAvailability, vegetationAvailability, mineralAvailability);
        }

        if (terrain.Surface == SurfaceType.Snow || climate.Zone == ClimateZone.Polar)
        {
            return CreateTundraDefinition(terrain, climate, waterAvailability, vegetationAvailability, mineralAvailability);
        }

        if (terrain.Surface == SurfaceType.Mud && waterAvailability >= 700)
        {
            return CreateSwampDefinition(terrain, climate, waterAvailability, vegetationAvailability, mineralAvailability);
        }

        if (climate.Zone == ClimateZone.Tropical && vegetationAvailability >= 750)
        {
            return CreateRainforestDefinition(terrain, climate, waterAvailability, vegetationAvailability, mineralAvailability);
        }

        if (vegetationAvailability >= 720)
        {
            return CreateForestDefinition(terrain, climate, waterAvailability, vegetationAvailability, mineralAvailability);
        }

        return CreateGrasslandDefinition(terrain, climate, waterAvailability, vegetationAvailability, mineralAvailability);
    }

    private static int GetAvailability(ChunkResources resources, ResourceType resourceType)
    {
        if (!resources.TryGet(resourceType, out ResourceState? resource))
        {
            throw new InvalidOperationException($"The chunk resource '{resourceType}' is required to classify a biome.");
        }

        return resource!.Availability;
    }

    private static BiomeDefinition CreateGrasslandDefinition(TerrainState terrain, ClimateState climate, int water, int vegetation, int minerals)
    {
        return new BiomeDefinition(
            "Grassland",
            BiomeCategory.Plains,
            "Open plains with moderate fertility and dominant grass vegetation.",
            new BiomeClimateProfile(18, climate.Precipitation.Intensity, climate.Humidity.RelativeHumidity, climate.Wind.Speed, 8),
            new BiomeTerrainProfile(terrain.Elevation.Height - 10, terrain.Elevation.Height + 10, terrain.Soil.SoilType, terrain.Surface, terrain.Soil.Drainage),
            new BiomeResourceProfile(water, vegetation, minerals, vegetation),
            new BiomeVegetationProfile(VegetationType.Grassland, 62),
            new BiomeSpeciesAffinityProfile(72, 46, 60, 20));
    }

    private static BiomeDefinition CreateForestDefinition(TerrainState terrain, ClimateState climate, int water, int vegetation, int minerals)
    {
        return new BiomeDefinition(
            "Forest",
            BiomeCategory.Forest,
            "Dense wooded biome with strong biomass and shelter availability.",
            new BiomeClimateProfile(17, climate.Precipitation.Intensity + 2, Math.Max(climate.Humidity.RelativeHumidity, 60), climate.Wind.Speed, 10),
            new BiomeTerrainProfile(terrain.Elevation.Height - 12, terrain.Elevation.Height + 12, terrain.Soil.SoilType, SurfaceType.Grass, terrain.Soil.Drainage),
            new BiomeResourceProfile(water, vegetation, minerals, Math.Min(1000, vegetation + 120)),
            new BiomeVegetationProfile(VegetationType.Forest, 84),
            new BiomeSpeciesAffinityProfile(80, 58, 78, 24));
    }

    private static BiomeDefinition CreateRainforestDefinition(TerrainState terrain, ClimateState climate, int water, int vegetation, int minerals)
    {
        return new BiomeDefinition(
            "Rainforest",
            BiomeCategory.Forest,
            "Humid tropical biome with abundant biomass and dense vegetation.",
            new BiomeClimateProfile(26, Math.Max(8, climate.Precipitation.Intensity + 4), Math.Max(climate.Humidity.RelativeHumidity, 78), climate.Wind.Speed, 6),
            new BiomeTerrainProfile(terrain.Elevation.Height - 8, terrain.Elevation.Height + 8, terrain.Soil.SoilType, SurfaceType.Grass, terrain.Soil.Drainage),
            new BiomeResourceProfile(Math.Min(1000, water + 120), Math.Min(1000, vegetation + 120), minerals, Math.Min(1000, vegetation + 180)),
            new BiomeVegetationProfile(VegetationType.Forest, 94),
            new BiomeSpeciesAffinityProfile(86, 62, 92, 38));
    }

    private static BiomeDefinition CreateSwampDefinition(TerrainState terrain, ClimateState climate, int water, int vegetation, int minerals)
    {
        return new BiomeDefinition(
            "Swamp",
            BiomeCategory.Wetland,
            "Waterlogged biome with rich organic matter and high aquatic suitability.",
            new BiomeClimateProfile(20, Math.Max(6, climate.Precipitation.Intensity), Math.Max(climate.Humidity.RelativeHumidity, 72), climate.Wind.Speed, 7),
            new BiomeTerrainProfile(terrain.Elevation.Height - 6, terrain.Elevation.Height + 4, terrain.Soil.SoilType, SurfaceType.Mud, terrain.Soil.Drainage),
            new BiomeResourceProfile(Math.Min(1000, water + 140), vegetation, minerals, Math.Min(1000, vegetation + 80)),
            new BiomeVegetationProfile(VegetationType.Shrubs, 70),
            new BiomeSpeciesAffinityProfile(68, 40, 74, 88));
    }

    private static BiomeDefinition CreateDesertDefinition(TerrainState terrain, ClimateState climate, int water, int vegetation, int minerals)
    {
        return new BiomeDefinition(
            "Desert",
            BiomeCategory.Arid,
            "Dry biome with scarce vegetation and exposed mineral surfaces.",
            new BiomeClimateProfile(28, climate.Precipitation.Intensity, Math.Min(climate.Humidity.RelativeHumidity, 30), climate.Wind.Speed + 2, 14),
            new BiomeTerrainProfile(terrain.Elevation.Height - 14, terrain.Elevation.Height + 14, SoilType.Sand, SurfaceType.Sand, Math.Max(terrain.Soil.Drainage, 70)),
            new BiomeResourceProfile(Math.Min(water, 320), Math.Min(vegetation, 260), Math.Max(minerals, 550), Math.Min(vegetation, 180)),
            new BiomeVegetationProfile(VegetationType.None, 12),
            new BiomeSpeciesAffinityProfile(26, 24, 10, 4));
    }

    private static BiomeDefinition CreateTundraDefinition(TerrainState terrain, ClimateState climate, int water, int vegetation, int minerals)
    {
        return new BiomeDefinition(
            "Tundra",
            BiomeCategory.Cold,
            "Cold open biome with low vegetation density and short growing seasons.",
            new BiomeClimateProfile(-4, climate.Precipitation.Intensity, Math.Max(climate.Humidity.RelativeHumidity, 42), climate.Wind.Speed, 16),
            new BiomeTerrainProfile(terrain.Elevation.Height - 10, terrain.Elevation.Height + 10, terrain.Soil.SoilType, terrain.Surface, terrain.Soil.Drainage),
            new BiomeResourceProfile(water, Math.Min(vegetation, 300), minerals, Math.Min(vegetation, 220)),
            new BiomeVegetationProfile(VegetationType.Moss, 28),
            new BiomeSpeciesAffinityProfile(34, 30, 22, 18));
    }

    private static BiomeDefinition CreateMountainDefinition(TerrainState terrain, ClimateState climate, int water, int vegetation, int minerals)
    {
        return new BiomeDefinition(
            "Mountain",
            BiomeCategory.Mountain,
            "High-relief biome with steep slopes and strong mineral exposure.",
            new BiomeClimateProfile(8, climate.Precipitation.Intensity, climate.Humidity.RelativeHumidity, climate.Wind.Speed + 3, 18),
            new BiomeTerrainProfile(terrain.Elevation.Height - 6, terrain.Elevation.Height + 20, terrain.Soil.SoilType, terrain.Surface, terrain.Soil.Drainage),
            new BiomeResourceProfile(water, Math.Min(vegetation, 420), Math.Max(minerals, 650), Math.Min(vegetation, 260)),
            new BiomeVegetationProfile(VegetationType.Shrubs, 24),
            new BiomeSpeciesAffinityProfile(30, 42, 18, 12));
    }

    private static BiomeDefinition CreateVolcanicDefinition(TerrainState terrain, ClimateState climate, int water, int vegetation, int minerals)
    {
        return new BiomeDefinition(
            "Volcanic",
            BiomeCategory.Volcanic,
            "Geologically active biome with exposed volcanic rock and high mineral availability.",
            new BiomeClimateProfile(22, climate.Precipitation.Intensity, climate.Humidity.RelativeHumidity, climate.Wind.Speed + 1, 12),
            new BiomeTerrainProfile(terrain.Elevation.Height - 8, terrain.Elevation.Height + 16, terrain.Soil.SoilType, SurfaceType.Rock, terrain.Soil.Drainage),
            new BiomeResourceProfile(water, Math.Min(vegetation, 320), Math.Max(minerals, 820), Math.Min(vegetation, 220)),
            new BiomeVegetationProfile(VegetationType.None, 8),
            new BiomeSpeciesAffinityProfile(14, 18, 6, 2));
    }

    private sealed record BiomeDefinition(
        string Name,
        BiomeCategory Category,
        string Description,
        BiomeClimateProfile ClimateProfile,
        BiomeTerrainProfile TerrainProfile,
        BiomeResourceProfile ResourceProfile,
        BiomeVegetationProfile VegetationProfile,
        BiomeSpeciesAffinityProfile SpeciesAffinity);
}

using System;
using System.Collections.Generic;
using System.Text.Json;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Serialization.SaveGames.Documents;

namespace GaiaEngine.Serialization.SaveGames;

/// <summary>
/// Serializes world save documents using deterministic JSON payloads.
/// </summary>
public sealed class JsonWorldSaveGameSerializer : IWorldSaveGameSerializer
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
    };

    /// <summary>
    /// Serializes a world save game into a deterministic payload.
    /// </summary>
    /// <param name="saveGame">The save game to serialize.</param>
    /// <returns>The serialized payload.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="saveGame"/> is <see langword="null"/>.</exception>
    public string Serialize(WorldSaveGame saveGame)
    {
        ArgumentNullException.ThrowIfNull(saveGame);

        WorldSaveGameDocument document = CreateDocument(saveGame);
        return JsonSerializer.Serialize(document, SerializerOptions);
    }

    /// <summary>
    /// Deserializes a world save game from a payload.
    /// </summary>
    /// <param name="payload">The serialized payload.</param>
    /// <returns>The deserialized save game.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="payload"/> is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the payload is invalid or incomplete.</exception>
    public WorldSaveGame Deserialize(string payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
        {
            throw new ArgumentException("The payload must contain a value.", nameof(payload));
        }

        WorldSaveGameDocument? document = JsonSerializer.Deserialize<WorldSaveGameDocument>(payload, SerializerOptions);
        if (document is null)
        {
            throw new InvalidOperationException("The save document could not be deserialized.");
        }

        return CreateSaveGame(document);
    }

    private static WorldSaveGameDocument CreateDocument(WorldSaveGame saveGame)
    {
        List<ChunkDocument> chunkDocuments = new();
        List<OrganismDocument> organismDocuments = new();
        List<ActionRequestDocument> actionRequestDocuments = new();
        foreach (Chunk chunk in saveGame.World.GetChunks())
        {
            List<string> organismIds = new(chunk.OrganismIds.Count);
            foreach (OrganismId organismId in chunk.OrganismIds)
            {
                organismIds.Add(organismId.ToString());
            }

            List<TerrainModifierStateDocument> terrainModifierDocuments = new(chunk.Terrain.Modifiers.Count);
            foreach (TerrainModifierState modifier in chunk.Terrain.Modifiers)
            {
                terrainModifierDocuments.Add(
                    new TerrainModifierStateDocument
                    {
                        Type = modifier.Type.ToString(),
                        Intensity = modifier.Intensity,
                    });
            }

            List<ResourceStateDocument> resourceDocuments = new(chunk.Resources.Count);
            foreach (ResourceState resource in chunk.Resources.GetAll())
            {
                resourceDocuments.Add(
                    new ResourceStateDocument
                    {
                        ResourceId = resource.ResourceId.ToString(),
                        Type = resource.Type.ToString(),
                        Category = resource.Category.ToString(),
                        CurrentAmount = resource.CurrentAmount,
                        MaximumCapacity = resource.MaximumCapacity,
                        RegenerationRate = resource.RegenerationRate,
                        Quality = resource.Quality,
                        Availability = resource.Availability,
                    });
            }

            chunkDocuments.Add(
                new ChunkDocument
                {
                    ChunkId = chunk.Id.ToString(),
                    WorldId = chunk.Metadata.WorldId.ToString(),
                    X = chunk.Metadata.Coordinates.X,
                    Y = chunk.Metadata.Coordinates.Y,
                    Seed = chunk.Metadata.Seed.Value,
                    Size = chunk.Metadata.Size,
                    State = chunk.State.ToString(),
                    Terrain = new TerrainStateDocument
                    {
                        Height = chunk.Terrain.Elevation.Height,
                        RelativeHeight = chunk.Terrain.Elevation.RelativeHeight,
                        SeaLevelOffset = chunk.Terrain.Elevation.SeaLevelOffset,
                        Gradient = chunk.Terrain.Slope.Gradient,
                        Aspect = chunk.Terrain.Slope.Aspect,
                        TraversalCost = chunk.Terrain.Slope.TraversalCost,
                        SoilType = chunk.Terrain.Soil.SoilType.ToString(),
                        Fertility = chunk.Terrain.Soil.Fertility,
                        Drainage = chunk.Terrain.Soil.Drainage,
                        MoistureCapacity = chunk.Terrain.Soil.MoistureCapacity,
                        OrganicMatter = chunk.Terrain.Soil.OrganicMatter,
                        Surface = chunk.Terrain.Surface.ToString(),
                        Geology = chunk.Terrain.Geology.ToString(),
                        Modifiers = terrainModifierDocuments,
                    },
                    Biome = new BiomeStateDocument
                    {
                        BiomeId = chunk.Biome.BiomeId.ToString(),
                        Name = chunk.Biome.Name,
                        Category = chunk.Biome.Category.ToString(),
                        Description = chunk.Biome.Description,
                        AverageTemperature = chunk.Biome.ClimateProfile.AverageTemperature,
                        AverageRainfall = chunk.Biome.ClimateProfile.AverageRainfall,
                        Humidity = chunk.Biome.ClimateProfile.Humidity,
                        WindIntensity = chunk.Biome.ClimateProfile.WindIntensity,
                        SeasonalVariation = chunk.Biome.ClimateProfile.SeasonalVariation,
                        MinimumElevation = chunk.Biome.TerrainProfile.MinimumElevation,
                        MaximumElevation = chunk.Biome.TerrainProfile.MaximumElevation,
                        DominantSoil = chunk.Biome.TerrainProfile.DominantSoil.ToString(),
                        Surface = chunk.Biome.TerrainProfile.Surface.ToString(),
                        Drainage = chunk.Biome.TerrainProfile.Drainage,
                        Water = chunk.Biome.ResourceProfile.Water,
                        Food = chunk.Biome.ResourceProfile.Food,
                        Minerals = chunk.Biome.ResourceProfile.Minerals,
                        Biomass = chunk.Biome.ResourceProfile.Biomass,
                        DominantVegetation = chunk.Biome.VegetationProfile.DominantVegetation.ToString(),
                        VegetationDensity = chunk.Biome.VegetationProfile.Density,
                        HerbivoreAffinity = chunk.Biome.SpeciesAffinity.HerbivoreAffinity,
                        CarnivoreAffinity = chunk.Biome.SpeciesAffinity.CarnivoreAffinity,
                        PlantDiversity = chunk.Biome.SpeciesAffinity.PlantDiversity,
                        AquaticSuitability = chunk.Biome.SpeciesAffinity.AquaticSuitability,
                    },
                    Climate = new ClimateStateDocument
                    {
                        Zone = chunk.Climate.Zone.ToString(),
                        WeatherState = chunk.Climate.WeatherState.ToString(),
                        CurrentTemperature = chunk.Climate.Temperature.CurrentTemperature,
                        DailyAverageTemperature = chunk.Climate.Temperature.DailyAverage,
                        SeasonalAverageTemperature = chunk.Climate.Temperature.SeasonalAverage,
                        DailyTemperatureVariation = chunk.Climate.Temperature.DailyVariation,
                        RelativeHumidity = chunk.Climate.Humidity.RelativeHumidity,
                        EvaporationRate = chunk.Climate.Humidity.EvaporationRate,
                        CondensationRate = chunk.Climate.Humidity.CondensationRate,
                        WindDirection = chunk.Climate.Wind.Direction,
                        WindSpeed = chunk.Climate.Wind.Speed,
                        WindGustStrength = chunk.Climate.Wind.GustStrength,
                        PrecipitationType = chunk.Climate.Precipitation.Type.ToString(),
                        PrecipitationIntensity = chunk.Climate.Precipitation.Intensity,
                        PrecipitationDuration = chunk.Climate.Precipitation.Duration,
                        PrecipitationCoverage = chunk.Climate.Precipitation.Coverage,
                        Pressure = chunk.Climate.Pressure.CurrentPressure,
                    },
                    Water = new WaterStateDocument
                    {
                        SurfaceWater = new SurfaceWaterStateDocument
                        {
                            WaterLevel = chunk.Water.SurfaceWater.WaterLevel,
                            FlowSpeed = chunk.Water.SurfaceWater.FlowSpeed,
                            FlowDirection = chunk.Water.SurfaceWater.FlowDirection,
                            WaterVolume = chunk.Water.SurfaceWater.WaterVolume,
                        },
                        GroundWater = new GroundWaterStateDocument
                        {
                            WaterTable = chunk.Water.GroundWater.WaterTable,
                            Saturation = chunk.Water.GroundWater.Saturation,
                            RechargeRate = chunk.Water.GroundWater.RechargeRate,
                            ExtractionRate = chunk.Water.GroundWater.ExtractionRate,
                        },
                        River = chunk.Water.River is null
                            ? null
                            : new RiverStateDocument
                            {
                                RiverId = chunk.Water.River.RiverId,
                                Width = chunk.Water.River.Width,
                                Depth = chunk.Water.River.Depth,
                                FlowRate = chunk.Water.River.FlowRate,
                                CurrentVelocity = chunk.Water.River.CurrentVelocity,
                            },
                        Lake = chunk.Water.Lake is null
                            ? null
                            : new LakeStateDocument
                            {
                                SurfaceArea = chunk.Water.Lake.SurfaceArea,
                                MaximumDepth = chunk.Water.Lake.MaximumDepth,
                                WaterVolume = chunk.Water.Lake.WaterVolume,
                                OverflowLevel = chunk.Water.Lake.OverflowLevel,
                            },
                        Ocean = chunk.Water.Ocean is null
                            ? null
                            : new OceanStateDocument
                            {
                                SeaLevel = chunk.Water.Ocean.SeaLevel,
                                Salinity = chunk.Water.Ocean.Salinity,
                                Temperature = chunk.Water.Ocean.Temperature,
                            },
                    },
                    Resources = resourceDocuments,
                    OrganismIds = organismIds,
                });
        }

        foreach (Organism organism in saveGame.Organisms.GetAll())
        {
            organismDocuments.Add(
                new OrganismDocument
                {
                    OrganismId = organism.Id.ToString(),
                    SpeciesId = organism.SpeciesId.ToString(),
                    GenomeId = organism.GenomeId.ToString(),
                    CurrentChunkId = organism.CurrentChunkId.ToString(),
                    MetabolismRate = organism.Physiology.MetabolismRate,
                    GrowthRate = organism.Physiology.GrowthRate,
                    LifespanTicks = organism.Physiology.LifespanTicks,
                    WaterEfficiency = organism.Physiology.WaterEfficiency,
                    DigestionEfficiency = organism.Physiology.DigestionEfficiency,
                    BodyTemperature = organism.Physiology.BodyTemperature,
                    Hunger = organism.Needs.Hunger,
                    Hydration = organism.Needs.Hydration,
                    Rest = organism.Needs.Rest,
                    ReproductionUrge = organism.Needs.ReproductionUrge,
                    BirthTick = organism.Lifecycle.BirthTick,
                    AgeTicks = organism.Lifecycle.AgeTicks,
                    MaturityAgeTicks = organism.Lifecycle.MaturityAgeTicks,
                    Stage = organism.Lifecycle.Stage.ToString(),
                    IsAlive = organism.Lifecycle.IsAlive,
                    CurrentHealth = organism.Health.CurrentValue,
                    MaximumHealth = organism.Health.MaximumValue,
                });
        }

        foreach (SimulationActionRequest actionRequest in saveGame.ActionRequests.GetAll())
        {
            actionRequestDocuments.Add(
                new ActionRequestDocument
                {
                    ActionId = actionRequest.ActionId.ToString(),
                    OrganismId = actionRequest.OrganismId.ToString(),
                    ActionType = actionRequest.ActionType.ToString(),
                    TargetKind = actionRequest.Target.Kind.ToString(),
                    TargetId = actionRequest.Target.TargetId,
                    StartTick = actionRequest.StartTick,
                    ExpectedDuration = actionRequest.ExpectedDuration,
                    Priority = actionRequest.Priority,
                    Status = actionRequest.Status.ToString(),
                    Interruptible = actionRequest.Interruptible,
                });
        }

        return new WorldSaveGameDocument
        {
            Metadata = new SaveMetadataDocument
            {
                SaveName = saveGame.Metadata.SaveName,
                CreationDate = saveGame.Metadata.CreationDate,
                LastModified = saveGame.Metadata.LastModified,
                WorldSeed = saveGame.Metadata.WorldSeed.Value,
                EngineVersion = saveGame.Metadata.EngineVersion.ToString(),
                SaveVersion = saveGame.Metadata.SaveVersion,
            },
            World = new WorldDocument
            {
                WorldId = saveGame.World.Id.ToString(),
                WorldName = saveGame.World.Metadata.WorldName,
                Seed = saveGame.World.Metadata.Seed.Value,
                CreationDate = saveGame.World.Metadata.CreationDate,
                EngineVersion = saveGame.World.Metadata.EngineVersion.ToString(),
                ConfigurationVersion = saveGame.World.Metadata.ConfigurationVersion.ToString(),
                Width = saveGame.World.Dimensions.Width,
                Height = saveGame.World.Dimensions.Height,
                ChunkSize = saveGame.World.Dimensions.ChunkSize,
                ChunkCount = saveGame.World.Dimensions.ChunkCount,
                MaximumElevation = saveGame.World.Dimensions.MaximumElevation,
                CurrentTick = saveGame.World.TimeState.CurrentTick,
                CurrentDay = saveGame.World.TimeState.CurrentDay,
                CurrentSeason = saveGame.World.TimeState.CurrentSeason,
                CurrentYear = saveGame.World.TimeState.CurrentYear,
                Chunks = chunkDocuments,
            },
            ConfigurationVersion = saveGame.ConfigurationVersion.ToString(),
            Organisms = organismDocuments,
            ActionRequests = actionRequestDocuments,
            Version = new SaveVersionInfoDocument
            {
                FormatVersion = saveGame.Version.FormatVersion,
                EngineVersion = saveGame.Version.EngineVersion.ToString(),
                ContentVersion = saveGame.Version.ContentVersion,
            },
        };
    }

    private static WorldSaveGame CreateSaveGame(WorldSaveGameDocument document)
    {
        if (document.Metadata is null)
        {
            throw new InvalidOperationException("The save metadata section is required.");
        }

        if (document.World is null)
        {
            throw new InvalidOperationException("The world section is required.");
        }

        if (document.Version is null)
        {
            throw new InvalidOperationException("The version section is required.");
        }

        WorldId worldId = WorldId.Parse(document.World.WorldId);
        List<Chunk> chunks = new(document.World.Chunks.Count);
        foreach (ChunkDocument chunkDocument in document.World.Chunks)
        {
            List<OrganismId> organismIds = new(chunkDocument.OrganismIds.Count);
            foreach (string organismId in chunkDocument.OrganismIds)
            {
                organismIds.Add(OrganismId.Parse(organismId));
            }

            List<TerrainModifierState> terrainModifiers = new(chunkDocument.Terrain.Modifiers.Count);
            foreach (TerrainModifierStateDocument modifierDocument in chunkDocument.Terrain.Modifiers)
            {
                terrainModifiers.Add(
                    new TerrainModifierState(
                        Enum.Parse<TerrainModifierType>(modifierDocument.Type, ignoreCase: false),
                        modifierDocument.Intensity));
            }

            List<ResourceState> resourceStates = new(chunkDocument.Resources.Count);
            foreach (ResourceStateDocument resourceDocument in chunkDocument.Resources)
            {
                resourceStates.Add(
                    new ResourceState(
                        ResourceId.Parse(resourceDocument.ResourceId),
                        Enum.Parse<ResourceType>(resourceDocument.Type, ignoreCase: false),
                        Enum.Parse<ResourceCategory>(resourceDocument.Category, ignoreCase: false),
                        resourceDocument.CurrentAmount,
                        resourceDocument.MaximumCapacity,
                        resourceDocument.RegenerationRate,
                        resourceDocument.Quality,
                        resourceDocument.Availability));
            }

            ChunkState parsedState = Enum.Parse<ChunkState>(chunkDocument.State, ignoreCase: false);
            chunks.Add(
                new Chunk(
                    new ChunkMetadata(
                        ChunkId.Parse(chunkDocument.ChunkId),
                        WorldId.Parse(chunkDocument.WorldId),
                        new ChunkCoordinates(chunkDocument.X, chunkDocument.Y),
                        new WorldSeed(chunkDocument.Seed),
                        chunkDocument.Size),
                    parsedState,
                    new TerrainState(
                        new ElevationState(
                            chunkDocument.Terrain.Height,
                            chunkDocument.Terrain.RelativeHeight,
                            chunkDocument.Terrain.SeaLevelOffset),
                        new SlopeState(
                            chunkDocument.Terrain.Gradient,
                            chunkDocument.Terrain.Aspect,
                            chunkDocument.Terrain.TraversalCost),
                        new SoilState(
                            Enum.Parse<SoilType>(chunkDocument.Terrain.SoilType, ignoreCase: false),
                            chunkDocument.Terrain.Fertility,
                            chunkDocument.Terrain.Drainage,
                            chunkDocument.Terrain.MoistureCapacity,
                            chunkDocument.Terrain.OrganicMatter),
                        Enum.Parse<SurfaceType>(chunkDocument.Terrain.Surface, ignoreCase: false),
                        Enum.Parse<GeologyType>(chunkDocument.Terrain.Geology, ignoreCase: false),
                        terrainModifiers.AsReadOnly()),
                    new BiomeState(
                        BiomeId.Parse(chunkDocument.Biome.BiomeId),
                        chunkDocument.Biome.Name,
                        Enum.Parse<BiomeCategory>(chunkDocument.Biome.Category, ignoreCase: false),
                        chunkDocument.Biome.Description,
                        new BiomeClimateProfile(
                            chunkDocument.Biome.AverageTemperature,
                            chunkDocument.Biome.AverageRainfall,
                            chunkDocument.Biome.Humidity,
                            chunkDocument.Biome.WindIntensity,
                            chunkDocument.Biome.SeasonalVariation),
                        new BiomeTerrainProfile(
                            chunkDocument.Biome.MinimumElevation,
                            chunkDocument.Biome.MaximumElevation,
                            Enum.Parse<SoilType>(chunkDocument.Biome.DominantSoil, ignoreCase: false),
                            Enum.Parse<SurfaceType>(chunkDocument.Biome.Surface, ignoreCase: false),
                            chunkDocument.Biome.Drainage),
                        new BiomeResourceProfile(
                            chunkDocument.Biome.Water,
                            chunkDocument.Biome.Food,
                            chunkDocument.Biome.Minerals,
                            chunkDocument.Biome.Biomass),
                        new BiomeVegetationProfile(
                            Enum.Parse<VegetationType>(chunkDocument.Biome.DominantVegetation, ignoreCase: false),
                            chunkDocument.Biome.VegetationDensity),
                        new BiomeSpeciesAffinityProfile(
                            chunkDocument.Biome.HerbivoreAffinity,
                            chunkDocument.Biome.CarnivoreAffinity,
                            chunkDocument.Biome.PlantDiversity,
                            chunkDocument.Biome.AquaticSuitability)),
                    new ClimateState(
                        Enum.Parse<ClimateZone>(chunkDocument.Climate.Zone, ignoreCase: false),
                        Enum.Parse<WeatherState>(chunkDocument.Climate.WeatherState, ignoreCase: false),
                        new TemperatureState(
                            chunkDocument.Climate.CurrentTemperature,
                            chunkDocument.Climate.DailyAverageTemperature,
                            chunkDocument.Climate.SeasonalAverageTemperature,
                            chunkDocument.Climate.DailyTemperatureVariation),
                        new HumidityState(
                            chunkDocument.Climate.RelativeHumidity,
                            chunkDocument.Climate.EvaporationRate,
                            chunkDocument.Climate.CondensationRate),
                        new WindState(
                            chunkDocument.Climate.WindDirection,
                            chunkDocument.Climate.WindSpeed,
                            chunkDocument.Climate.WindGustStrength),
                        new PrecipitationState(
                            Enum.Parse<PrecipitationType>(chunkDocument.Climate.PrecipitationType, ignoreCase: false),
                            chunkDocument.Climate.PrecipitationIntensity,
                            chunkDocument.Climate.PrecipitationDuration,
                            chunkDocument.Climate.PrecipitationCoverage),
                        new PressureState(chunkDocument.Climate.Pressure)),
                    new WaterState(
                        new SurfaceWaterState(
                            chunkDocument.Water.SurfaceWater.WaterLevel,
                            chunkDocument.Water.SurfaceWater.FlowSpeed,
                            chunkDocument.Water.SurfaceWater.FlowDirection,
                            chunkDocument.Water.SurfaceWater.WaterVolume),
                        new GroundWaterState(
                            chunkDocument.Water.GroundWater.WaterTable,
                            chunkDocument.Water.GroundWater.Saturation,
                            chunkDocument.Water.GroundWater.RechargeRate,
                            chunkDocument.Water.GroundWater.ExtractionRate),
                        chunkDocument.Water.River is null
                            ? null
                            : new RiverState(
                                chunkDocument.Water.River.RiverId,
                                chunkDocument.Water.River.Width,
                                chunkDocument.Water.River.Depth,
                                chunkDocument.Water.River.FlowRate,
                                chunkDocument.Water.River.CurrentVelocity),
                        chunkDocument.Water.Lake is null
                            ? null
                            : new LakeState(
                                chunkDocument.Water.Lake.SurfaceArea,
                                chunkDocument.Water.Lake.MaximumDepth,
                                chunkDocument.Water.Lake.WaterVolume,
                                chunkDocument.Water.Lake.OverflowLevel),
                        chunkDocument.Water.Ocean is null
                            ? null
                            : new OceanState(
                                chunkDocument.Water.Ocean.SeaLevel,
                                chunkDocument.Water.Ocean.Salinity,
                                chunkDocument.Water.Ocean.Temperature)),
                    new ChunkResources(resourceStates.AsReadOnly()),
                    organismIds.AsReadOnly()));
        }

        World world = new(
            new WorldMetadata(
                worldId,
                document.World.WorldName,
                new WorldSeed(document.World.Seed),
                document.World.CreationDate,
                EngineVersion.Parse(document.World.EngineVersion),
                new ConfigurationVersion(document.World.ConfigurationVersion)),
            new WorldDimensions(
                document.World.Width,
                document.World.Height,
                document.World.ChunkSize,
                document.World.ChunkCount,
                document.World.MaximumElevation),
            new WorldTimeState(
                document.World.CurrentTick,
                document.World.CurrentDay,
                document.World.CurrentSeason,
                document.World.CurrentYear),
            chunks.AsReadOnly());

        List<Organism> organisms = new(document.Organisms.Count);
        foreach (OrganismDocument organismDocument in document.Organisms)
        {
            organisms.Add(
                new Organism(
                    OrganismId.Parse(organismDocument.OrganismId),
                    SpeciesId.Parse(organismDocument.SpeciesId),
                    GenomeId.Parse(organismDocument.GenomeId),
                    ChunkId.Parse(organismDocument.CurrentChunkId),
                    new PhysiologyComponent(
                        organismDocument.MetabolismRate,
                        organismDocument.GrowthRate,
                        organismDocument.LifespanTicks,
                        organismDocument.WaterEfficiency,
                        organismDocument.DigestionEfficiency,
                        organismDocument.BodyTemperature),
                    new NeedsComponent(
                        organismDocument.Hunger,
                        organismDocument.Hydration,
                        organismDocument.Rest,
                        organismDocument.ReproductionUrge),
                    new LifecycleComponent(
                        organismDocument.BirthTick,
                        organismDocument.AgeTicks,
                        organismDocument.MaturityAgeTicks,
                        Enum.Parse<LifecycleStage>(organismDocument.Stage, ignoreCase: false),
                        organismDocument.IsAlive),
                    new HealthComponent(
                        organismDocument.CurrentHealth,
                        organismDocument.MaximumHealth)));
        }

        OrganismCollection organismCollection = new(organisms.AsReadOnly());
        ValidateOrganismReferences(world, organismCollection);

        List<SimulationActionRequest> actionRequests = new(document.ActionRequests.Count);
        foreach (ActionRequestDocument actionRequestDocument in document.ActionRequests)
        {
            actionRequests.Add(
                new SimulationActionRequest(
                    ActionId.Parse(actionRequestDocument.ActionId),
                    OrganismId.Parse(actionRequestDocument.OrganismId),
                    Enum.Parse<SimulationActionType>(actionRequestDocument.ActionType, ignoreCase: false),
                    new SimulationActionTarget(
                        Enum.Parse<ActionTargetKind>(actionRequestDocument.TargetKind, ignoreCase: false),
                        actionRequestDocument.TargetId),
                    actionRequestDocument.StartTick,
                    actionRequestDocument.ExpectedDuration,
                    actionRequestDocument.Priority,
                    Enum.Parse<ActionExecutionState>(actionRequestDocument.Status, ignoreCase: false),
                    actionRequestDocument.Interruptible));
        }

        SaveMetadata metadata = new(
            document.Metadata.SaveName,
            document.Metadata.CreationDate,
            document.Metadata.LastModified,
            new WorldSeed(document.Metadata.WorldSeed),
            EngineVersion.Parse(document.Metadata.EngineVersion),
            document.Metadata.SaveVersion);

        SaveVersionInfo version = new(
            document.Version.FormatVersion,
            EngineVersion.Parse(document.Version.EngineVersion),
            document.Version.ContentVersion);

        return new WorldSaveGame(
            metadata,
            world,
            organismCollection,
            new SimulationActionRequestCollection(actionRequests.AsReadOnly()),
            new ConfigurationVersion(document.ConfigurationVersion),
            version);
    }

    private static void ValidateOrganismReferences(GaiaEngine.Domain.World.World world, OrganismCollection organisms)
    {
        Dictionary<ChunkId, HashSet<OrganismId>> chunkOrganismIds = new();
        foreach (Chunk chunk in world.GetChunks())
        {
            HashSet<OrganismId> chunkIds = new();
            foreach (OrganismId organismId in chunk.OrganismIds)
            {
                chunkIds.Add(organismId);
            }

            chunkOrganismIds.Add(chunk.Id, chunkIds);
        }

        foreach (Organism organism in organisms.GetAll())
        {
            if (!chunkOrganismIds.TryGetValue(organism.CurrentChunkId, out HashSet<OrganismId>? chunkIds))
            {
                throw new InvalidOperationException($"The organism '{organism.Id}' references a missing chunk.");
            }

            if (!chunkIds.Contains(organism.Id))
            {
                throw new InvalidOperationException($"The organism '{organism.Id}' is not referenced by its owning chunk.");
            }
        }
    }
}

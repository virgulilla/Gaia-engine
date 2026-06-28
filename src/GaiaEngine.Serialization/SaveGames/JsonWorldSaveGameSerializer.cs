using System;
using System.Collections.Generic;
using System.Text.Json;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
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
        foreach (Chunk chunk in saveGame.World.GetChunks())
        {
            List<string> organismIds = new(chunk.OrganismIds.Count);
            foreach (OrganismId organismId in chunk.OrganismIds)
            {
                organismIds.Add(organismId.ToString());
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
                    OrganismIds = organismIds,
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
            new ConfigurationVersion(document.ConfigurationVersion),
            version);
    }
}

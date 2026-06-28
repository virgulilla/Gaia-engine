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
        Assert.Equal(saveGame.World.GetChunks()[0].Climate.Zone, restored.World.GetChunks()[0].Climate.Zone);
        Assert.Equal(saveGame.World.GetChunks()[0].Climate.Temperature.CurrentTemperature, restored.World.GetChunks()[0].Climate.Temperature.CurrentTemperature);
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
            {"metadata":{"saveName":"Gaia","creationDate":"2026-06-28","lastModified":"2026-06-28","worldSeed":42,"engineVersion":"1.0.0","saveVersion":"1.0.0"},"world":{"worldId":"72057594037927937","worldName":"Gaia","seed":42,"creationDate":"2026-06-28","engineVersion":"1.0.0","configurationVersion":"2026.06.28","width":10,"height":10,"chunkSize":16,"chunkCount":1,"maximumElevation":1,"currentTick":0,"currentDay":0,"currentSeason":"Spring","currentYear":0,"chunks":[{"chunkId":"144115188075855873","worldId":"72057594037927938","x":0,"y":0,"seed":1,"size":16,"state":"Active","organismIds":[]}]},"configurationVersion":"2026.06.28","version":{"formatVersion":"1.0.0","engineVersion":"1.0.0","contentVersion":"1.0.0"}}
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
                    new ClimateState(
                        ClimateZone.Temperate,
                        WeatherState.Clear,
                        new TemperatureState(18, 18, 18, 0),
                        new HumidityState(55, 3, 2),
                        new WindState(90, 4, 6),
                        new PrecipitationState(PrecipitationType.None, 0, 0, 0),
                        new PressureState(1012)),
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
}

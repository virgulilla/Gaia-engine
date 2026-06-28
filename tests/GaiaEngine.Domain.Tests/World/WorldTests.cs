using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using Xunit;

namespace GaiaEngine.Domain.Tests.WorldModel;

public sealed class WorldTests
{
    [Fact]
    public void Constructor_ShouldOwnChunksAndExposeDeterministicOrder()
    {
        WorldId worldId = WorldId.FromSequence(new EntitySequence(1));
        GaiaEngine.Domain.World.World world = new(
            new WorldMetadata(
                worldId,
                "Gaia",
                new WorldSeed(42),
                "2026-06-28",
                new EngineVersion(1, 0, 0),
                new ConfigurationVersion("2026.06.28")),
            new WorldDimensions(100, 100, 16, 2, 200),
            new WorldTimeState(10, 1, "Spring", 0),
            new List<Chunk>
            {
                CreateChunk(worldId, 2, new ChunkCoordinates(5, 1)),
                CreateChunk(worldId, 1, new ChunkCoordinates(1, 0)),
            });

        IReadOnlyList<Chunk> chunks = world.GetChunks();

        Assert.Equal(2, world.ChunkCount);
        Assert.Equal(new ChunkCoordinates(1, 0), chunks[0].Metadata.Coordinates);
        Assert.Equal(new ChunkCoordinates(5, 1), chunks[1].Metadata.Coordinates);
    }

    [Fact]
    public void Constructor_ShouldRejectChunksFromAnotherWorld()
    {
        WorldId worldId = WorldId.FromSequence(new EntitySequence(1));
        WorldId otherWorldId = WorldId.FromSequence(new EntitySequence(2));

        Assert.Throws<ArgumentException>(() =>
            new GaiaEngine.Domain.World.World(
                new WorldMetadata(
                    worldId,
                    "Gaia",
                    new WorldSeed(42),
                    "2026-06-28",
                    new EngineVersion(1, 0, 0),
                    new ConfigurationVersion("2026.06.28")),
                new WorldDimensions(100, 100, 16, 1, 200),
                new WorldTimeState(10, 1, "Spring", 0),
                new List<Chunk>
                {
                    CreateChunk(otherWorldId, 1, new ChunkCoordinates(0, 0)),
                }));
    }

    [Fact]
    public void GetChunk_ShouldReturnTheRequestedChunk()
    {
        WorldId worldId = WorldId.FromSequence(new EntitySequence(1));
        Chunk expectedChunk = CreateChunk(worldId, 1, new ChunkCoordinates(2, 3));
        GaiaEngine.Domain.World.World world = new(
            new WorldMetadata(
                worldId,
                "Gaia",
                new WorldSeed(42),
                "2026-06-28",
                new EngineVersion(1, 0, 0),
                new ConfigurationVersion("2026.06.28")),
            new WorldDimensions(100, 100, 16, 1, 200),
            new WorldTimeState(10, 1, "Spring", 0),
            new List<Chunk> { expectedChunk });

        Chunk resolvedChunk = world.GetChunk(new ChunkCoordinates(2, 3));

        Assert.Equal(expectedChunk, resolvedChunk);
    }

    private static Chunk CreateChunk(WorldId worldId, ulong sequence, ChunkCoordinates coordinates)
    {
        return new Chunk(
            new ChunkMetadata(
                ChunkId.FromSequence(new EntitySequence(sequence)),
                worldId,
                coordinates,
                new WorldSeed((long)sequence),
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
            Array.Empty<OrganismId>());
    }
}

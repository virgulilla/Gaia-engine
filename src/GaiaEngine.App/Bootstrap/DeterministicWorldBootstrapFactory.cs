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

        return new Chunk(
            new ChunkMetadata(
                chunkId,
                worldId,
                new ChunkCoordinates(column, row),
                new WorldSeed(chunkSeedValue),
                worldConfiguration.ChunkSize),
            ChunkState.Active,
            new ClimateState(
                worldConfiguration.DefaultClimateZone,
                WeatherState.Clear,
                new TemperatureState(18, 18, 18, 0),
                new HumidityState(55, 3, 2),
                new WindState(90, 4, 6),
                new PrecipitationState(PrecipitationType.None, 0, 0, 0),
                new PressureState(1012)),
            Array.Empty<OrganismId>());
    }
}

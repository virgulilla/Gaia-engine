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
            CreateTerrainState(column, row, chunkSeedValue),
            new ClimateState(
                worldConfiguration.DefaultClimateZone,
                WeatherState.Clear,
                new TemperatureState(18, 18, 18, 0),
                new HumidityState(55, 3, 2),
                new WindState(90, 4, 6),
                new PrecipitationState(PrecipitationType.None, 0, 0, 0),
                new PressureState(1012)),
            CreateDefaultResources(sequence),
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
}

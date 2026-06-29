using System;
using System.Collections.Generic;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.World.Water;

/// <summary>
/// Updates deterministic per-chunk water state from climate, terrain, and neighbouring chunks.
/// </summary>
public sealed class DeterministicWaterSystem : IWaterSystem
{
    private readonly WaterSystemSettings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicWaterSystem"/> class.
    /// </summary>
    /// <param name="settings">The explicit deterministic water settings.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="settings"/> is <see langword="null"/>.</exception>
    public DeterministicWaterSystem(WaterSystemSettings settings)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    /// <summary>
    /// Updates the water state of every chunk in the supplied world.
    /// </summary>
    /// <param name="world">The world to update.</param>
    /// <returns>A new world instance containing the updated water state.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="world"/> is <see langword="null"/>.</exception>
    public GaiaEngine.Domain.World.World UpdateWorld(GaiaEngine.Domain.World.World world)
    {
        ArgumentNullException.ThrowIfNull(world);

        IReadOnlyList<Chunk> existingChunks = world.GetChunks();
        List<Chunk> updatedChunks = new(existingChunks.Count);
        foreach (Chunk chunk in existingChunks)
        {
            updatedChunks.Add(
                new Chunk(
                    chunk.Metadata,
                    chunk.State,
                    chunk.Terrain,
                    chunk.Biome,
                    chunk.Climate,
                    UpdateWater(world, chunk),
                    chunk.Resources,
                    chunk.OrganismIds));
        }

        return new GaiaEngine.Domain.World.World(world.Metadata, world.Dimensions, world.TimeState, updatedChunks.AsReadOnly());
    }

    private WaterState UpdateWater(GaiaEngine.Domain.World.World world, Chunk chunk)
    {
        int neighbourRunoff = CalculateNeighbourRunoff(world, chunk);
        int precipitationGain = chunk.Climate.Precipitation.Intensity * settings.PrecipitationMultiplier;
        int evaporationLoss = chunk.Climate.Humidity.EvaporationRate / settings.EvaporationDivider;
        int drainageLoss = chunk.Terrain.Soil.Drainage / 5;
        int newWaterLevel = Math.Clamp(
            chunk.Water.SurfaceWater.WaterLevel + precipitationGain + neighbourRunoff - evaporationLoss - drainageLoss,
            0,
            1000);

        int newWaterVolume = Math.Max(0, chunk.Water.SurfaceWater.WaterVolume + (newWaterLevel - chunk.Water.SurfaceWater.WaterLevel) * Math.Max(1, chunk.Metadata.Size / 2));
        int newFlowSpeed = Math.Max(0, (chunk.Terrain.Slope.Gradient / 2) + (newWaterLevel / 120));
        int newFlowDirection = ResolveFlowDirection(world, chunk);
        int rechargeRate = Math.Max(0, (precipitationGain + chunk.Terrain.Soil.MoistureCapacity) / settings.InfiltrationDivider);
        int saturation = Math.Clamp(chunk.Water.GroundWater.Saturation + (rechargeRate / 4) - Math.Max(0, evaporationLoss / 2), 0, 100);
        int waterTable = Math.Max(0, chunk.Water.GroundWater.WaterTable + (rechargeRate / 6) - (chunk.Terrain.Soil.Drainage / 10));

        return new WaterState(
            new SurfaceWaterState(newWaterLevel, newFlowSpeed, newFlowDirection, newWaterVolume),
            new GroundWaterState(waterTable, saturation, rechargeRate, chunk.Water.GroundWater.ExtractionRate),
            CreateRiverState(chunk, newWaterLevel, newFlowSpeed),
            CreateLakeState(chunk, newWaterLevel, newWaterVolume),
            chunk.Water.Ocean is null
                ? null
                : new OceanState(
                    chunk.Water.Ocean.SeaLevel,
                    chunk.Water.Ocean.Salinity,
                    chunk.Climate.Temperature.CurrentTemperature));
    }

    private int CalculateNeighbourRunoff(GaiaEngine.Domain.World.World world, Chunk chunk)
    {
        ChunkCoordinates coordinates = chunk.Metadata.Coordinates;
        ChunkCoordinates[] neighbours =
        {
            new(coordinates.X - 1, coordinates.Y),
            new(coordinates.X + 1, coordinates.Y),
            new(coordinates.X, coordinates.Y - 1),
            new(coordinates.X, coordinates.Y + 1),
        };

        int runoff = 0;
        foreach (ChunkCoordinates neighbourCoordinates in neighbours)
        {
            if (!world.TryGetChunk(neighbourCoordinates, out Chunk? neighbour))
            {
                continue;
            }

            int elevationDelta = neighbour!.Terrain.Elevation.Height - chunk.Terrain.Elevation.Height;
            if (elevationDelta > 0)
            {
                runoff += neighbour.Water.SurfaceWater.WaterLevel / settings.RunoffDivider;
            }
        }

        return runoff;
    }

    private static int ResolveFlowDirection(GaiaEngine.Domain.World.World world, Chunk chunk)
    {
        ChunkCoordinates coordinates = chunk.Metadata.Coordinates;
        (ChunkCoordinates Offset, int Direction)[] neighbours =
        {
            (new ChunkCoordinates(coordinates.X, coordinates.Y - 1), 0),
            (new ChunkCoordinates(coordinates.X + 1, coordinates.Y), 90),
            (new ChunkCoordinates(coordinates.X, coordinates.Y + 1), 180),
            (new ChunkCoordinates(coordinates.X - 1, coordinates.Y), 270),
        };

        int lowestHeight = chunk.Terrain.Elevation.Height;
        int flowDirection = chunk.Water.SurfaceWater.FlowDirection;
        foreach ((ChunkCoordinates offset, int direction) in neighbours)
        {
            if (world.TryGetChunk(offset, out Chunk? neighbour) && neighbour!.Terrain.Elevation.Height < lowestHeight)
            {
                lowestHeight = neighbour.Terrain.Elevation.Height;
                flowDirection = direction;
            }
        }

        return flowDirection;
    }

    private static RiverState? CreateRiverState(Chunk chunk, int waterLevel, int flowSpeed)
    {
        if (chunk.Water.Ocean is not null)
        {
            return null;
        }

        if (chunk.Terrain.Slope.Gradient >= 10 && waterLevel >= 360)
        {
            return new RiverState(
                chunk.Water.River?.RiverId ?? $"river-{chunk.Metadata.Coordinates.X}-{chunk.Metadata.Coordinates.Y}",
                width: Math.Max(1, waterLevel / 180),
                depth: Math.Max(1, waterLevel / 220),
                flowRate: Math.Max(1, flowSpeed * 3),
                currentVelocity: Math.Max(1, flowSpeed));
        }

        return null;
    }

    private static LakeState? CreateLakeState(Chunk chunk, int waterLevel, int waterVolume)
    {
        if (chunk.Water.Ocean is not null)
        {
            return null;
        }

        if (chunk.Terrain.Soil.Drainage <= 48 && waterLevel >= 420)
        {
            return new LakeState(
                surfaceArea: Math.Max(1, (waterLevel * chunk.Metadata.Size) / 20),
                maximumDepth: Math.Max(1, waterLevel / 150),
                waterVolume: Math.Max(1, waterVolume / 2),
                overflowLevel: Math.Clamp(waterLevel + 120, 0, 1000));
        }

        return null;
    }
}

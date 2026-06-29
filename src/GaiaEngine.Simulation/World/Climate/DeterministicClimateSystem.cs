using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.World.Climate;

/// <summary>
/// Updates deterministic per-chunk climate state using only explicit world data and climate settings.
/// </summary>
public sealed class DeterministicClimateSystem : IClimateSystem
{
    private readonly ClimateSystemSettings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicClimateSystem"/> class.
    /// </summary>
    /// <param name="settings">The explicit deterministic climate settings.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="settings"/> is <see langword="null"/>.</exception>
    public DeterministicClimateSystem(ClimateSystemSettings settings)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    /// <summary>
    /// Updates the climate state of every chunk in the supplied world.
    /// </summary>
    /// <param name="world">The world to update.</param>
    /// <returns>A new world instance containing the updated climate state.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="world"/> is <see langword="null"/>.</exception>
    public GaiaEngine.Domain.World.World UpdateWorld(GaiaEngine.Domain.World.World world)
    {
        ArgumentNullException.ThrowIfNull(world);

        IReadOnlyList<Chunk> existingChunks = world.GetChunks();
        List<Chunk> updatedChunks = new(existingChunks.Count);

        foreach (Chunk chunk in existingChunks)
        {
            ClimateState updatedClimate = CreateClimateState(world, chunk);
            updatedChunks.Add(new Chunk(chunk.Metadata, chunk.State, chunk.Terrain, updatedClimate, chunk.Resources, chunk.OrganismIds));
        }

        return new GaiaEngine.Domain.World.World(world.Metadata, world.Dimensions, world.TimeState, updatedChunks.AsReadOnly());
    }

    private ClimateState CreateClimateState(GaiaEngine.Domain.World.World world, Chunk chunk)
    {
        int seedOffset = (int)(Math.Abs(chunk.Metadata.Seed.Value) % 11) - 5;
        int zoneTemperatureOffset = GetZoneTemperatureOffset(chunk.Climate.Zone);
        int zoneHumidityOffset = GetZoneHumidityOffset(chunk.Climate.Zone);
        int seasonalTemperatureOffset = GetSeasonTemperatureOffset(world.TimeState.CurrentSeason);
        int seasonalHumidityOffset = GetSeasonHumidityOffset(world.TimeState.CurrentSeason);
        int dailyVariation = GetDailyVariation(world.TimeState.CurrentTick);
        (int neighbourTemperature, int neighbourHumidity) = GetNeighbourInfluence(world, chunk.Metadata.Coordinates);

        int seasonalAverage = settings.BaseTemperature + zoneTemperatureOffset + seasonalTemperatureOffset;
        int dailyAverage = seasonalAverage + seedOffset;
        int currentTemperature = dailyAverage + dailyVariation + neighbourTemperature;

        int relativeHumidity = ClampHumidity(settings.BaseHumidity + zoneHumidityOffset + seasonalHumidityOffset - seedOffset + neighbourHumidity);
        int evaporationRate = Math.Max(0, currentTemperature / 2);
        int condensationRate = Math.Max(0, (relativeHumidity - 50) / 2);
        int pressure = settings.BasePressure - ((relativeHumidity - 50) / 4) + (seedOffset * 2);

        WeatherState weatherState = ResolveWeatherState(currentTemperature, relativeHumidity, pressure);
        PrecipitationState precipitation = CreatePrecipitationState(weatherState, relativeHumidity);
        WindState wind = new(
            direction: CreateWindDirection(chunk.Metadata.Coordinates, world.TimeState.CurrentTick),
            speed: Math.Max(0, settings.BaseWindSpeed + Math.Abs(seedOffset) + (relativeHumidity / 25)),
            gustStrength: Math.Max(0, settings.BaseWindSpeed + Math.Abs(seedOffset) + (relativeHumidity / 20) + (weatherState == WeatherState.Storm ? 6 : 2)));

        return new ClimateState(
            chunk.Climate.Zone,
            weatherState,
            new TemperatureState(currentTemperature, dailyAverage, seasonalAverage, dailyVariation),
            new HumidityState(relativeHumidity, evaporationRate, condensationRate),
            wind,
            precipitation,
            new PressureState(pressure));
    }

    private (int Temperature, int Humidity) GetNeighbourInfluence(GaiaEngine.Domain.World.World world, ChunkCoordinates coordinates)
    {
        ChunkCoordinates[] offsets =
        {
            new(coordinates.X - 1, coordinates.Y),
            new(coordinates.X + 1, coordinates.Y),
            new(coordinates.X, coordinates.Y - 1),
            new(coordinates.X, coordinates.Y + 1),
        };

        int neighbourCount = 0;
        int temperatureSum = 0;
        int humiditySum = 0;

        foreach (ChunkCoordinates offset in offsets)
        {
            if (world.TryGetChunk(offset, out Chunk? neighbour))
            {
                neighbourCount++;
                temperatureSum += neighbour!.Climate.Temperature.CurrentTemperature;
                humiditySum += neighbour.Climate.Humidity.RelativeHumidity;
            }
        }

        if (neighbourCount == 0)
        {
            return (0, 0);
        }

        return ((temperatureSum / neighbourCount) / 10, ((humiditySum / neighbourCount) - settings.BaseHumidity) / 10);
    }

    private int GetDailyVariation(long currentTick)
    {
        int tickInDay = (int)(currentTick % settings.TicksPerDay);
        int centeredTick = tickInDay - (settings.TicksPerDay / 2);
        return centeredTick / Math.Max(1, settings.TicksPerDay / 8);
    }

    private static int CreateWindDirection(ChunkCoordinates coordinates, long currentTick)
    {
        long rawValue = (coordinates.X * 37L) + (coordinates.Y * 53L) + currentTick;
        int direction = (int)(rawValue % 360);
        return direction < 0 ? direction + 360 : direction;
    }

    private static int ClampHumidity(int relativeHumidity)
    {
        return Math.Clamp(relativeHumidity, 0, 100);
    }

    private int GetZoneTemperatureOffset(ClimateZone zone)
    {
        return zone switch
        {
            ClimateZone.Tropical => 8,
            ClimateZone.Temperate => 0,
            ClimateZone.Continental => -2,
            ClimateZone.Polar => -14,
            ClimateZone.Arid => 10,
            _ => throw new ArgumentOutOfRangeException(nameof(zone), "The supplied climate zone is not supported."),
        };
    }

    private int GetZoneHumidityOffset(ClimateZone zone)
    {
        return zone switch
        {
            ClimateZone.Tropical => 18,
            ClimateZone.Temperate => 0,
            ClimateZone.Continental => -4,
            ClimateZone.Polar => -10,
            ClimateZone.Arid => -24,
            _ => throw new ArgumentOutOfRangeException(nameof(zone), "The supplied climate zone is not supported."),
        };
    }

    private int GetSeasonTemperatureOffset(string season)
    {
        return season switch
        {
            "Spring" => settings.SeasonalTemperatureDelta / 2,
            "Summer" => settings.SeasonalTemperatureDelta,
            "Autumn" => -(settings.SeasonalTemperatureDelta / 2),
            "Winter" => -settings.SeasonalTemperatureDelta,
            _ => throw new ArgumentException("The supplied season is not supported.", nameof(season)),
        };
    }

    private int GetSeasonHumidityOffset(string season)
    {
        return season switch
        {
            "Spring" => 8,
            "Summer" => -6,
            "Autumn" => 4,
            "Winter" => 2,
            _ => throw new ArgumentException("The supplied season is not supported.", nameof(season)),
        };
    }

    private static WeatherState ResolveWeatherState(int currentTemperature, int relativeHumidity, int pressure)
    {
        if (relativeHumidity < 18)
        {
            return WeatherState.Drought;
        }

        if (currentTemperature <= 0 && relativeHumidity >= 65)
        {
            return WeatherState.Snow;
        }

        if (relativeHumidity >= 85 && pressure < 995)
        {
            return WeatherState.Storm;
        }

        if (relativeHumidity >= 70)
        {
            return WeatherState.Rain;
        }

        if (relativeHumidity >= 60)
        {
            return WeatherState.Cloudy;
        }

        if (relativeHumidity >= 50 && pressure < 1000)
        {
            return WeatherState.Fog;
        }

        return WeatherState.Clear;
    }

    private static PrecipitationState CreatePrecipitationState(WeatherState weatherState, int relativeHumidity)
    {
        return weatherState switch
        {
            WeatherState.Rain => new PrecipitationState(PrecipitationType.Rain, 5 + (relativeHumidity / 10), 3, Math.Min(100, 40 + relativeHumidity)),
            WeatherState.Storm => new PrecipitationState(PrecipitationType.Rain, 10 + (relativeHumidity / 8), 5, Math.Min(100, 60 + relativeHumidity)),
            WeatherState.Snow => new PrecipitationState(PrecipitationType.Snow, 4 + (relativeHumidity / 12), 4, Math.Min(100, 50 + relativeHumidity)),
            _ => new PrecipitationState(PrecipitationType.None, 0, 0, 0),
        };
    }
}

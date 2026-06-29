using System;
using System.Collections.Generic;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.World.Resources;

/// <summary>
/// Updates deterministic per-chunk resource state from climate and seasonal conditions.
/// </summary>
public sealed class DeterministicResourceSystem : IResourceSystem
{
    private readonly ResourceSystemSettings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicResourceSystem"/> class.
    /// </summary>
    /// <param name="settings">The explicit deterministic resource settings.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="settings"/> is <see langword="null"/>.</exception>
    public DeterministicResourceSystem(ResourceSystemSettings settings)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    /// <summary>
    /// Updates the resource state of every chunk in the supplied world.
    /// </summary>
    /// <param name="world">The world to update.</param>
    /// <returns>A new world instance containing the updated resource state.</returns>
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
                    UpdateResources(world, chunk),
                    chunk.OrganismIds));
        }

        return new GaiaEngine.Domain.World.World(world.Metadata, world.Dimensions, world.TimeState, updatedChunks.AsReadOnly());
    }

    private ChunkResources UpdateResources(GaiaEngine.Domain.World.World world, Chunk chunk)
    {
        List<ResourceState> updatedResources = new(chunk.Resources.Count);
        foreach (ResourceState resource in chunk.Resources.GetAll())
        {
            updatedResources.Add(UpdateResource(world.TimeState.CurrentSeason, chunk.Climate, resource));
        }

        return new ChunkResources(updatedResources.AsReadOnly());
    }

    private ResourceState UpdateResource(string season, ClimateState climate, ResourceState resource)
    {
        return resource.Type switch
        {
            ResourceType.Vegetation => UpdateVegetation(season, climate, resource),
            ResourceType.FreshWater => UpdateFreshWater(season, climate, resource),
            ResourceType.Minerals => UpdateMinerals(resource),
            _ => throw new ArgumentOutOfRangeException(nameof(resource), "The supplied resource type is not supported."),
        };
    }

    private ResourceState UpdateVegetation(string season, ClimateState climate, ResourceState resource)
    {
        int seasonBonus = season switch
        {
            "Spring" => settings.VegetationSeasonBonus,
            "Summer" => settings.VegetationSeasonBonus / 2,
            "Autumn" => 0,
            "Winter" => -settings.VegetationSeasonBonus,
            _ => throw new ArgumentException("The supplied season is not supported.", nameof(season)),
        };

        int humidityBonus = Math.Max(0, (climate.Humidity.RelativeHumidity - 40) / 10);
        int precipitationBonus = climate.Precipitation.Intensity / settings.PrecipitationDivider;
        int weatherModifier = climate.WeatherState switch
        {
            WeatherState.Drought => -resource.RegenerationRate,
            WeatherState.Rain => 2,
            WeatherState.Storm => 1,
            WeatherState.Snow => -1,
            _ => 0,
        };

        int regeneration = Math.Max(0, resource.RegenerationRate + seasonBonus + humidityBonus + precipitationBonus + weatherModifier);
        int currentAmount = Math.Min(resource.MaximumCapacity, resource.CurrentAmount + regeneration);
        int quality = Math.Clamp(resource.Quality + seasonBonus + (climate.WeatherState == WeatherState.Drought ? -4 : 2), 0, 100);
        int availability = CalculateAvailability(currentAmount, resource.MaximumCapacity, quality, precipitationBonus);

        return new ResourceState(
            resource.ResourceId,
            resource.Type,
            resource.Category,
            currentAmount,
            resource.MaximumCapacity,
            resource.RegenerationRate,
            quality,
            availability);
    }

    private ResourceState UpdateFreshWater(string season, ClimateState climate, ResourceState resource)
    {
        int seasonBonus = season switch
        {
            "Spring" => settings.WaterSeasonBonus,
            "Summer" => 0,
            "Autumn" => 1,
            "Winter" => 0,
            _ => throw new ArgumentException("The supplied season is not supported.", nameof(season)),
        };

        int precipitationBonus = climate.Precipitation.Intensity / Math.Max(1, settings.PrecipitationDivider - 1);
        int evaporationPenalty = climate.Humidity.EvaporationRate / settings.EvaporationDivider;
        int weatherModifier = climate.WeatherState switch
        {
            WeatherState.Drought => -resource.RegenerationRate,
            WeatherState.Rain => 2,
            WeatherState.Storm => 3,
            WeatherState.Snow => 1,
            _ => 0,
        };

        int regeneration = Math.Max(0, resource.RegenerationRate + seasonBonus + precipitationBonus + weatherModifier - evaporationPenalty);
        int currentAmount = Math.Min(resource.MaximumCapacity, resource.CurrentAmount + regeneration);
        int quality = Math.Clamp(resource.Quality + (climate.WeatherState == WeatherState.Storm ? -2 : 1), 0, 100);
        int availability = CalculateAvailability(currentAmount, resource.MaximumCapacity, quality, precipitationBonus);

        return new ResourceState(
            resource.ResourceId,
            resource.Type,
            resource.Category,
            currentAmount,
            resource.MaximumCapacity,
            resource.RegenerationRate,
            quality,
            availability);
    }

    private static ResourceState UpdateMinerals(ResourceState resource)
    {
        int availability = CalculateAvailability(resource.CurrentAmount, resource.MaximumCapacity, resource.Quality, 0);
        return new ResourceState(
            resource.ResourceId,
            resource.Type,
            resource.Category,
            resource.CurrentAmount,
            resource.MaximumCapacity,
            resource.RegenerationRate,
            resource.Quality,
            availability);
    }

    private static int CalculateAvailability(int currentAmount, int maximumCapacity, int quality, int climateModifier)
    {
        int baseAvailability = (currentAmount * 1000) / maximumCapacity;
        int qualityModifier = (quality - 50) * 4;
        return Math.Clamp(baseAvailability + qualityModifier + (climateModifier * 10), 0, 1000);
    }
}

using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.Events;

namespace GaiaEngine.Gameplay.Discovery;

/// <summary>
/// Creates the default deterministic discovery rule set for the current gameplay slice.
/// </summary>
public static class DefaultDiscoveryRuleSetFactory
{
    /// <summary>
    /// Creates the default deterministic discovery rules for the supplied world bootstrap state.
    /// </summary>
    /// <param name="world">The world used to derive observable gameplay discoveries.</param>
    /// <param name="species">The species used to derive observable gameplay discoveries.</param>
    /// <returns>The generated deterministic discovery rule set.</returns>
    public static DiscoveryRuleSet Create(GaiaEngine.Domain.World.World world, SpeciesCollection species)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(species);

        List<DiscoveryRuleDefinition> rules = new();
        foreach (Species currentSpecies in species.GetAll())
        {
            rules.Add(
                new DiscoveryRuleDefinition(
                    $"species.{currentSpecies.Id}",
                    DiscoveryCategory.Species,
                    DiscoverySignalSource.Observation,
                    currentSpecies.Id.ToString(),
                    $"Species {currentSpecies.Id}",
                    "Observed a new species in the ecosystem.",
                    rewardExperience: 10));
        }

        HashSet<ulong> knownBiomes = new();
        HashSet<ResourceType> knownResources = new();
        HashSet<WeatherState> knownWeatherStates = new();
        foreach (Chunk chunk in world.GetChunks())
        {
            if (knownBiomes.Add(chunk.Biome.BiomeId.Value))
            {
                rules.Add(
                    new DiscoveryRuleDefinition(
                        $"biome.{chunk.Biome.BiomeId}",
                        DiscoveryCategory.Biomes,
                        DiscoverySignalSource.Observation,
                        chunk.Biome.BiomeId.ToString(),
                        chunk.Biome.Name,
                        $"Observed the biome '{chunk.Biome.Name}'.",
                        rewardExperience: 5));
            }

            if (knownWeatherStates.Add(chunk.Climate.WeatherState))
            {
                rules.Add(
                    new DiscoveryRuleDefinition(
                        $"climate.{chunk.Climate.WeatherState}",
                        DiscoveryCategory.Climate,
                        DiscoverySignalSource.Observation,
                        chunk.Climate.WeatherState.ToString(),
                        chunk.Climate.WeatherState.ToString(),
                        $"Observed the climate state '{chunk.Climate.WeatherState}'.",
                        rewardExperience: 3));
            }

            foreach (ResourceState resource in chunk.Resources.GetAll())
            {
                if (!knownResources.Add(resource.Type))
                {
                    continue;
                }

                rules.Add(
                    new DiscoveryRuleDefinition(
                        $"resource.{resource.Type}",
                        DiscoveryCategory.Resources,
                        DiscoverySignalSource.Observation,
                        resource.Type.ToString(),
                        resource.Type.ToString(),
                        $"Observed the resource '{resource.Type}'.",
                        rewardExperience: 4));
            }
        }

        foreach (SimulationActionType actionType in Enum.GetValues<SimulationActionType>())
        {
            if (actionType == SimulationActionType.Idle)
            {
                continue;
            }

            rules.Add(
                new DiscoveryRuleDefinition(
                    $"behaviour.{actionType}",
                    DiscoveryCategory.Behaviours,
                    DiscoverySignalSource.SimulationEvent,
                    actionType.ToString(),
                    actionType.ToString(),
                    $"Witnessed the behaviour '{actionType}'.",
                    rewardExperience: 2));
        }

        rules.Add(
            new DiscoveryRuleDefinition(
                "worldevent.newday",
                DiscoveryCategory.WorldEvents,
                DiscoverySignalSource.SimulationEvent,
                nameof(NewDaySimulationEvent),
                "New Day",
                "Observed the beginning of a new simulated day.",
                rewardExperience: 1));
        rules.Add(
            new DiscoveryRuleDefinition(
                "worldevent.newseason",
                DiscoveryCategory.WorldEvents,
                DiscoverySignalSource.SimulationEvent,
                nameof(NewSeasonSimulationEvent),
                "New Season",
                "Observed the beginning of a new simulated season.",
                rewardExperience: 2));
        rules.Add(
            new DiscoveryRuleDefinition(
                "worldevent.newyear",
                DiscoveryCategory.WorldEvents,
                DiscoverySignalSource.SimulationEvent,
                nameof(NewYearSimulationEvent),
                "New Year",
                "Observed the beginning of a new simulated year.",
                rewardExperience: 3));

        return new DiscoveryRuleSet(rules.AsReadOnly());
    }
}

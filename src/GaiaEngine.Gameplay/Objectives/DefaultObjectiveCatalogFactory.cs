using System;
using System.Collections.Generic;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Simulation.Events;

namespace GaiaEngine.Gameplay.Objectives;

/// <summary>
/// Creates the default deterministic objective catalog for the current gameplay slice.
/// </summary>
public static class DefaultObjectiveCatalogFactory
{
    /// <summary>
    /// Creates the ordered default objective catalog.
    /// </summary>
    /// <returns>The ordered default objective definitions.</returns>
    public static IReadOnlyList<ObjectiveDefinition> Create()
    {
        return new List<ObjectiveDefinition>
        {
            new(
                "objective.discovery.first-species",
                ObjectiveCategory.Discovery,
                "Discover your first species",
                "Unlock one species discovery by observing the ecosystem.",
                new[]
                {
                    new ObjectiveRequirementDefinition(
                        "requirement.discovery.first-species",
                        ObjectiveRequirementType.Counter,
                        targetCount: 1,
                        discoveryCategory: DiscoveryCategory.Species),
                },
                new ObjectiveRewardDefinition(25, Array.Empty<string>()),
                ObjectiveStatus.Active),
            new(
                "objective.ecology.first-biome",
                ObjectiveCategory.Ecology,
                "Observe your first biome",
                "Unlock one biome discovery while studying the world.",
                new[]
                {
                    new ObjectiveRequirementDefinition(
                        "requirement.ecology.first-biome",
                        ObjectiveRequirementType.Counter,
                        targetCount: 1,
                        discoveryCategory: DiscoveryCategory.Biomes),
                },
                new ObjectiveRewardDefinition(15, Array.Empty<string>()),
                ObjectiveStatus.Active),
            new(
                "objective.observation.first-season",
                ObjectiveCategory.Observation,
                "Observe a season change",
                "Witness the first transition into a new season.",
                new[]
                {
                    new ObjectiveRequirementDefinition(
                        "requirement.observation.first-season",
                        ObjectiveRequirementType.WorldEvent,
                        targetCount: 1,
                        discoveryCategory: DiscoveryCategory.WorldEvents,
                        signalKey: nameof(NewSeasonSimulationEvent)),
                },
                new ObjectiveRewardDefinition(20, Array.Empty<string>()),
                ObjectiveStatus.Active),
            new(
                "objective.mastery.first-year",
                ObjectiveCategory.Mastery,
                "Witness a full simulated year",
                "Stay with the ecosystem long enough to observe the first new year.",
                new[]
                {
                    new ObjectiveRequirementDefinition(
                        "requirement.mastery.first-year",
                        ObjectiveRequirementType.WorldEvent,
                        targetCount: 1,
                        discoveryCategory: DiscoveryCategory.WorldEvents,
                        signalKey: nameof(NewYearSimulationEvent)),
                },
                new ObjectiveRewardDefinition(30, Array.Empty<string>()),
                ObjectiveStatus.Hidden),
        }.AsReadOnly();
    }
}

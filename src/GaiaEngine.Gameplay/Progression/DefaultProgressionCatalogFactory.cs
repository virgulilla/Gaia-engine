using System.Collections.Generic;
using GaiaEngine.Gameplay.Discovery;

namespace GaiaEngine.Gameplay.Progression;

/// <summary>
/// Creates the default deterministic progression catalog for the current gameplay slice.
/// </summary>
public static class DefaultProgressionCatalogFactory
{
    /// <summary>
    /// Creates the ordered default progression catalog.
    /// </summary>
    /// <returns>The ordered default progression catalog.</returns>
    public static ProgressionCatalog Create()
    {
        return new ProgressionCatalog(
            new List<ProgressionLevelDefinition>
            {
                new(
                    0,
                    requiredExperience: 0,
                    new[]
                    {
                        new ProgressionUnlockDefinition(
                            "analysis.organism-inspector",
                            ProgressionUnlockCategory.AnalysisTools,
                            "Organism Inspector",
                            "Allows detailed inspection of observed organisms."),
                    }),
                new(
                    1,
                    requiredExperience: 25,
                    new[]
                    {
                        new ProgressionUnlockDefinition(
                            "simulation.time-acceleration",
                            ProgressionUnlockCategory.SimulationControls,
                            "Time Acceleration",
                            "Allows faster deterministic simulation playback."),
                    }),
                new(
                    2,
                    requiredExperience: 50,
                    new[]
                    {
                        new ProgressionUnlockDefinition(
                            "statistics.population-graphs",
                            ProgressionUnlockCategory.Statistics,
                            "Population Graphs",
                            "Unlocks persistent population graph analysis."),
                        new ProgressionUnlockDefinition(
                            "visualization.climate-overlay",
                            ProgressionUnlockCategory.Visualization,
                            "Climate Overlay",
                            "Unlocks a world climate overlay for observation."),
                    }),
            }.AsReadOnly(),
            new List<ProgressionMilestoneDefinition>
            {
                new(
                    "milestone.first-discovery",
                    "First Discovery",
                    "Unlock the first permanent discovery.",
                    ProgressionMilestoneRequirementType.TotalDiscoveries,
                    targetValue: 1),
                new(
                    "milestone.first-species",
                    "First Species",
                    "Discover the first species in the ecosystem.",
                    ProgressionMilestoneRequirementType.CategoryDiscoveries,
                    targetValue: 1,
                    discoveryCategory: DiscoveryCategory.Species),
                new(
                    "milestone.first-objective",
                    "First Objective",
                    "Complete the first gameplay objective.",
                    ProgressionMilestoneRequirementType.CompletedObjectives,
                    targetValue: 1),
                new(
                    "milestone.field-researcher",
                    "Field Researcher",
                    "Accumulate meaningful observation experience.",
                    ProgressionMilestoneRequirementType.Experience,
                    targetValue: 50),
            }.AsReadOnly());
    }
}

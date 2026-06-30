using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Gameplay.Discovery;

/// <summary>
/// Creates deterministic discovery signals from the currently observed world snapshot.
/// </summary>
public static class DiscoveryObservationSnapshotFactory
{
    /// <summary>
    /// Creates discovery signals from the currently observed world and species snapshot.
    /// </summary>
    /// <param name="world">The observed world state.</param>
    /// <param name="species">The observed species state.</param>
    /// <returns>The generated observation signals in deterministic order.</returns>
    public static IReadOnlyList<DiscoverySignal> CreateSignals(GaiaEngine.Domain.World.World world, SpeciesCollection species)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(species);

        Dictionary<string, DiscoverySignal> signals = new(StringComparer.Ordinal);
        foreach (Species currentSpecies in species.GetAll())
        {
            AddSignal(signals, SimulationDiscoverySignalFactory.CreateSpeciesSignal(currentSpecies.Id));
        }

        foreach (Chunk chunk in world.GetChunks())
        {
            AddSignal(signals, SimulationDiscoverySignalFactory.CreateBiomeSignal(chunk.Biome.BiomeId));
            AddSignal(signals, SimulationDiscoverySignalFactory.CreateClimateSignal(chunk.Climate.WeatherState));
            foreach (ResourceState resource in chunk.Resources.GetAll())
            {
                AddSignal(signals, SimulationDiscoverySignalFactory.CreateResourceSignal(resource.Type));
            }
        }

        List<DiscoverySignal> ordered = new(signals.Values);
        ordered.Sort(static (left, right) => CompareSignals(left, right));
        return ordered.AsReadOnly();
    }

    private static void AddSignal(Dictionary<string, DiscoverySignal> signals, DiscoverySignal signal)
    {
        string key = $"{(int)signal.Source}:{(int)signal.Category}:{signal.Key}";
        signals.TryAdd(key, signal);
    }

    private static int CompareSignals(DiscoverySignal left, DiscoverySignal right)
    {
        int sourceComparison = left.Source.CompareTo(right.Source);
        if (sourceComparison != 0)
        {
            return sourceComparison;
        }

        int categoryComparison = left.Category.CompareTo(right.Category);
        if (categoryComparison != 0)
        {
            return categoryComparison;
        }

        return string.CompareOrdinal(left.Key, right.Key);
    }
}

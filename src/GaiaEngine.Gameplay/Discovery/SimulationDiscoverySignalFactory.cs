using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.Events;

namespace GaiaEngine.Gameplay.Discovery;

/// <summary>
/// Creates discovery signals from deterministic simulation observations and events.
/// </summary>
public static class SimulationDiscoverySignalFactory
{
    /// <summary>
    /// Creates a species discovery signal from one observed species identifier.
    /// </summary>
    /// <param name="speciesId">The observed species identifier.</param>
    /// <returns>The corresponding discovery signal.</returns>
    public static DiscoverySignal CreateSpeciesSignal(SpeciesId speciesId)
    {
        return new DiscoverySignal(DiscoveryCategory.Species, DiscoverySignalSource.Observation, speciesId.ToString());
    }

    /// <summary>
    /// Creates a biome discovery signal from one observed biome identifier.
    /// </summary>
    /// <param name="biomeId">The observed biome identifier.</param>
    /// <returns>The corresponding discovery signal.</returns>
    public static DiscoverySignal CreateBiomeSignal(BiomeId biomeId)
    {
        return new DiscoverySignal(DiscoveryCategory.Biomes, DiscoverySignalSource.Observation, biomeId.ToString());
    }

    /// <summary>
    /// Creates a resource discovery signal from one observed resource type.
    /// </summary>
    /// <param name="resourceType">The observed resource type.</param>
    /// <returns>The corresponding discovery signal.</returns>
    public static DiscoverySignal CreateResourceSignal(ResourceType resourceType)
    {
        return new DiscoverySignal(DiscoveryCategory.Resources, DiscoverySignalSource.Observation, resourceType.ToString());
    }

    /// <summary>
    /// Creates a climate discovery signal from one observed weather state.
    /// </summary>
    /// <param name="weatherState">The observed weather state.</param>
    /// <returns>The corresponding discovery signal.</returns>
    public static DiscoverySignal CreateClimateSignal(WeatherState weatherState)
    {
        return new DiscoverySignal(DiscoveryCategory.Climate, DiscoverySignalSource.Observation, weatherState.ToString());
    }

    /// <summary>
    /// Creates a trait discovery signal from one observed trait key.
    /// </summary>
    /// <param name="traitKey">The observed trait key.</param>
    /// <returns>The corresponding discovery signal.</returns>
    public static DiscoverySignal CreateTraitSignal(string traitKey)
    {
        return new DiscoverySignal(DiscoveryCategory.Traits, DiscoverySignalSource.Observation, traitKey);
    }

    /// <summary>
    /// Creates deterministic discovery signals from simulation events that represent behaviours or world events.
    /// </summary>
    /// <param name="events">The simulation events to inspect.</param>
    /// <returns>The generated discovery signals in deterministic order.</returns>
    public static IReadOnlyList<DiscoverySignal> CreateSignals(IReadOnlyList<IEvent> events)
    {
        ArgumentNullException.ThrowIfNull(events);

        List<DiscoverySignal> signals = new(events.Count);
        foreach (IEvent currentEvent in events)
        {
            ArgumentNullException.ThrowIfNull(currentEvent);

            if (currentEvent is ActionStartedSimulationEvent started)
            {
                signals.Add(new DiscoverySignal(DiscoveryCategory.Behaviours, DiscoverySignalSource.SimulationEvent, started.ActionType.ToString()));
                continue;
            }

            if (currentEvent is ActionCompletedSimulationEvent completed)
            {
                signals.Add(new DiscoverySignal(DiscoveryCategory.Behaviours, DiscoverySignalSource.SimulationEvent, completed.ActionType.ToString()));
                continue;
            }

            signals.Add(new DiscoverySignal(DiscoveryCategory.WorldEvents, DiscoverySignalSource.SimulationEvent, currentEvent.EventType));
        }

        return signals.AsReadOnly();
    }
}

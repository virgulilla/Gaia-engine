using System;
using System.Collections.Generic;
using GaiaEngine.Domain.AI;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.App.Configuration;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.World;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Objectives;
using GaiaEngine.Gameplay.Player;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.Pipeline;
using GaiaEngine.Simulation.Runtime;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Represents the initialized Gaia Engine runtime services required by the host process.
/// </summary>
public sealed class GaiaEngineRuntime
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GaiaEngineRuntime"/> class.
    /// </summary>
    /// <param name="engineConfiguration">The loaded engine configuration.</param>
    /// <param name="simulationConfiguration">The loaded simulation configuration.</param>
    /// <param name="simulationSession">The initialized simulation session.</param>
    /// <param name="discoverySystem">The gameplay discovery system used to update permanent knowledge.</param>
    /// <param name="objectiveSystem">The gameplay objective system used to update player progression goals.</param>
    /// <param name="playerProfile">The current player profile owned by the gameplay layer.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any required dependency is <see langword="null"/>.
    /// </exception>
    public GaiaEngineRuntime(
        EngineConfiguration engineConfiguration,
        SimulationConfiguration simulationConfiguration,
        ISimulationSession simulationSession,
        IDiscoverySystem discoverySystem,
        IObjectiveSystem objectiveSystem,
        PlayerProfile playerProfile)
    {
        EngineConfiguration = engineConfiguration ?? throw new ArgumentNullException(nameof(engineConfiguration));
        SimulationConfiguration = simulationConfiguration ?? throw new ArgumentNullException(nameof(simulationConfiguration));
        SimulationSession = simulationSession ?? throw new ArgumentNullException(nameof(simulationSession));
        this.discoverySystem = discoverySystem ?? throw new ArgumentNullException(nameof(discoverySystem));
        this.objectiveSystem = objectiveSystem ?? throw new ArgumentNullException(nameof(objectiveSystem));
        PlayerProfile = playerProfile ?? throw new ArgumentNullException(nameof(playerProfile));
    }

    private readonly IDiscoverySystem discoverySystem;
    private readonly IObjectiveSystem objectiveSystem;

    /// <summary>
    /// Gets the loaded engine configuration.
    /// </summary>
    public EngineConfiguration EngineConfiguration { get; }

    /// <summary>
    /// Gets the loaded simulation configuration.
    /// </summary>
    public SimulationConfiguration SimulationConfiguration { get; }

    /// <summary>
    /// Gets the initialized simulation session.
    /// </summary>
    public ISimulationSession SimulationSession { get; }

    /// <summary>
    /// Gets the initialized world bootstrap state.
    /// </summary>
    public GaiaEngine.Domain.World.World World => SimulationSession.CurrentWorld;

    /// <summary>
    /// Gets the initialized organism bootstrap state.
    /// </summary>
    public OrganismCollection Organisms => SimulationSession.CurrentOrganisms;

    /// <summary>
    /// Gets the initialized genome bootstrap state.
    /// </summary>
    public GenomeCollection Genomes => SimulationSession.CurrentGenomes;

    /// <summary>
    /// Gets the initialized species bootstrap state.
    /// </summary>
    public SpeciesCollection Species => SimulationSession.CurrentSpecies;

    /// <summary>
    /// Gets the initialized memory bootstrap state.
    /// </summary>
    public MemoryCollection Memories => SimulationSession.CurrentMemories;

    /// <summary>
    /// Gets the initialized common action request state.
    /// </summary>
    public SimulationActionRequestCollection ActionRequests => SimulationSession.CurrentActionRequests;

    /// <summary>
    /// Gets the current player profile owned by the host gameplay layer.
    /// </summary>
    public PlayerProfile PlayerProfile { get; private set; }

    /// <summary>
    /// Advances one deterministic simulation tick and updates gameplay discoveries from the resulting observations.
    /// </summary>
    /// <returns>The deterministic simulation tick result.</returns>
    public SimulationTickResult AdvanceTick()
    {
        SimulationTickResult result = SimulationSession.AdvanceTick();
        PlayerProfile = EvaluateGameplay(PlayerProfile, result.World, result.Species, result.TimeState.CurrentTick, result.EventPublicationResult.PublishedEvents);
        return result;
    }

    /// <summary>
    /// Counts the unlocked discoveries owned by the current player profile for the supplied category.
    /// </summary>
    /// <param name="category">The category to count.</param>
    /// <returns>The number of unlocked discoveries in the supplied category.</returns>
    public int CountDiscoveries(DiscoveryCategory category)
    {
        int count = 0;
        foreach (DiscoveryEntry entry in PlayerProfile.Knowledge.Discoveries.GetAll())
        {
            if (entry.Category == category)
            {
                count++;
            }
        }

        return count;
    }

    private PlayerProfile EvaluateGameplay(
        PlayerProfile profile,
        GaiaEngine.Domain.World.World world,
        SpeciesCollection species,
        long tick,
        IReadOnlyList<GaiaEngine.Engine.Events.IEvent> events)
    {
        List<DiscoverySignal> signals = new();
        foreach (DiscoverySignal signal in DiscoveryObservationSnapshotFactory.CreateSignals(world, species))
        {
            signals.Add(signal);
        }

        foreach (DiscoverySignal signal in SimulationDiscoverySignalFactory.CreateSignals(events))
        {
            signals.Add(signal);
        }

        DiscoveryEvaluationResult discoveryResult = discoverySystem.Evaluate(profile, world.Id, tick, signals.AsReadOnly());
        IReadOnlyList<ObjectiveSignal> objectiveSignals = ObjectiveSignalFactory.CreateSignals(discoveryResult.UnlockedDiscoveries, events);
        return objectiveSystem.Evaluate(discoveryResult.Profile, tick, objectiveSignals).Profile;
    }
}

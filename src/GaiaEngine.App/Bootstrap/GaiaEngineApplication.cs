using System;
using GaiaEngine.App.Configuration;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Pipeline;
using GaiaEngine.Simulation.Runtime;
using GaiaEngine.Simulation.Scheduling;
using GaiaEngine.Simulation.Time;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Coordinates deterministic application startup for the Gaia Engine host.
/// </summary>
public sealed class GaiaEngineApplication
{
    private readonly IEngineConfigurationProvider configurationProvider;
    private readonly ISimulationConfigurationProvider simulationConfigurationProvider;
    private GaiaEngineRuntime? runtime;

    /// <summary>
    /// Initializes a new instance of the <see cref="GaiaEngineApplication"/> class.
    /// </summary>
    /// <param name="configurationProvider">The configuration provider used during startup.</param>
    /// <param name="simulationConfigurationProvider">The simulation configuration provider used during startup.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="configurationProvider"/> or <paramref name="simulationConfigurationProvider"/> is <see langword="null"/>.
    /// </exception>
    public GaiaEngineApplication(
        IEngineConfigurationProvider configurationProvider,
        ISimulationConfigurationProvider simulationConfigurationProvider)
    {
        this.configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        this.simulationConfigurationProvider = simulationConfigurationProvider ?? throw new ArgumentNullException(nameof(simulationConfigurationProvider));
    }

    /// <summary>
    /// Loads the immutable runtime graph for the current host process.
    /// </summary>
    /// <returns>The initialized runtime graph.</returns>
    public GaiaEngineRuntime Initialize()
    {
        runtime ??= CreateRuntime();
        return runtime;
    }

    private GaiaEngineRuntime CreateRuntime()
    {
        EngineConfiguration engineConfiguration = configurationProvider.Load();
        SimulationConfiguration simulationConfiguration = simulationConfigurationProvider.Load();
        SimulationCalendar calendar = new(simulationConfiguration.TicksPerDay, simulationConfiguration.DaysPerSeason);
        DeterministicTimeSystem timeSystem = new(calendar);
        EventBus eventBus = new();
        DeterministicEntityIdGenerator eventIdGenerator = new();
        SimulationEventPublisher eventPublisher = new(eventBus, eventIdGenerator);
        DeterministicSimulationScheduler scheduler = new(Array.Empty<ScheduledSimulationSystemDefinition>());
        DeterministicSimulationTickPipeline tickPipeline = new(
            new ISimulationTickPhase[]
            {
                new NoOpSimulationTickPhase(SimulationTickPhase.InputCollection),
                new NoOpSimulationTickPhase(SimulationTickPhase.PreUpdate),
                new WorldUpdateTimePhase(timeSystem, eventPublisher),
                new NoOpSimulationTickPhase(SimulationTickPhase.OrganismUpdate),
                new NoOpSimulationTickPhase(SimulationTickPhase.InteractionSystems),
                new NoOpSimulationTickPhase(SimulationTickPhase.EnvironmentUpdate),
                new EventDispatchPhase(eventBus),
                new NoOpSimulationTickPhase(SimulationTickPhase.PostUpdate),
            },
            scheduler);
        DeterministicSimulationSession simulationSession = new(
            tickPipeline,
            new WorldTimeState(
                currentTick: 0,
                currentDay: simulationConfiguration.StartingDay,
                currentSeason: simulationConfiguration.StartingSeason,
                currentYear: simulationConfiguration.StartingYear));

        return new GaiaEngineRuntime(engineConfiguration, simulationConfiguration, simulationSession);
    }
}

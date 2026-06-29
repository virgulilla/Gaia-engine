using System;
using GaiaEngine.App.Configuration;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Genetics;
using GaiaEngine.Simulation.Interactions.Feeding;
using GaiaEngine.Simulation.Interactions.Hydration;
using GaiaEngine.Simulation.Interactions.Movement;
using GaiaEngine.Simulation.Organisms;
using GaiaEngine.Simulation.Pipeline;
using GaiaEngine.Simulation.Runtime;
using GaiaEngine.Simulation.Scheduling;
using GaiaEngine.Simulation.Time;
using GaiaEngine.Simulation.World.Climate;
using GaiaEngine.Simulation.World.Queries;
using GaiaEngine.Simulation.World.Resources;
using GaiaEngine.Simulation.World.Water;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Coordinates deterministic application startup for the Gaia Engine host.
/// </summary>
public sealed class GaiaEngineApplication
{
    private readonly IEngineConfigurationProvider configurationProvider;
    private readonly ISimulationConfigurationProvider simulationConfigurationProvider;
    private readonly IWorldConfigurationProvider worldConfigurationProvider;
    private GaiaEngineRuntime? runtime;

    /// <summary>
    /// Initializes a new instance of the <see cref="GaiaEngineApplication"/> class.
    /// </summary>
    /// <param name="configurationProvider">The configuration provider used during startup.</param>
    /// <param name="simulationConfigurationProvider">The simulation configuration provider used during startup.</param>
    /// <param name="worldConfigurationProvider">The world configuration provider used during startup.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any required provider is <see langword="null"/>.
    /// </exception>
    public GaiaEngineApplication(
        IEngineConfigurationProvider configurationProvider,
        ISimulationConfigurationProvider simulationConfigurationProvider,
        IWorldConfigurationProvider worldConfigurationProvider)
    {
        this.configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
        this.simulationConfigurationProvider = simulationConfigurationProvider ?? throw new ArgumentNullException(nameof(simulationConfigurationProvider));
        this.worldConfigurationProvider = worldConfigurationProvider ?? throw new ArgumentNullException(nameof(worldConfigurationProvider));
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
        WorldConfiguration worldConfiguration = worldConfigurationProvider.Load();
        SimulationCalendar calendar = new(simulationConfiguration.TicksPerDay, simulationConfiguration.DaysPerSeason);
        DeterministicTimeSystem timeSystem = new(calendar);
        EventBus eventBus = new();
        DeterministicEntityIdGenerator eventIdGenerator = new();
        SimulationEventPublisher eventPublisher = new(eventBus, eventIdGenerator);
        SimulationDiagnosticsCollector diagnosticsCollector = new();
        ClimateSystemSettings climateSettings = new(
            simulationConfiguration.TicksPerDay,
            baseTemperature: 18,
            seasonalTemperatureDelta: 10,
            baseHumidity: 55,
            basePressure: 1012,
            baseWindSpeed: 4);
        DeterministicClimateSystem climateSystem = new(climateSettings);
        WaterSystemSettings waterSettings = new(
            precipitationMultiplier: 8,
            evaporationDivider: 4,
            runoffDivider: 12,
            infiltrationDivider: 6);
        DeterministicWaterSystem waterSystem = new(waterSettings);
        ResourceSystemSettings resourceSettings = new(
            vegetationSeasonBonus: 3,
            waterSeasonBonus: 2,
            precipitationDivider: 3,
            evaporationDivider: 4);
        DeterministicResourceSystem resourceSystem = new(resourceSettings);
        DeterministicOrganismUpdateSystem organismUpdateSystem = new();
        SpeciesRecognitionSettings speciesRecognitionSettings = new(
            simulationConfiguration.SpeciesRecognition.EvaluationFrequency,
            simulationConfiguration.SpeciesRecognition.MinimumGenomeSimilarity,
            simulationConfiguration.SpeciesRecognition.MinimumTraitSimilarity,
            simulationConfiguration.SpeciesRecognition.MinimumMorphologySimilarity,
            simulationConfiguration.SpeciesRecognition.MinimumReproductiveCompatibility,
            simulationConfiguration.SpeciesRecognition.RequiredFailedMetricCount);
        DeterministicSpeciesRecognitionSystem speciesRecognitionSystem = new(speciesRecognitionSettings, eventIdGenerator);
        DeterministicSpeciesLifecycleSystem speciesLifecycleSystem = new();
        DeterministicSpatialQueryService spatialQueryService = new();
        DeterministicMovementSystem movementSystem = new(spatialQueryService);
        DeterministicFeedingSystem feedingSystem = new();
        DeterministicHydrationSystem hydrationSystem = new();
        DeterministicActionRequestDispatcher actionRequestDispatcher = new();
        DeterministicSimulationScheduler scheduler = new(
            new[]
            {
                new ScheduledSimulationSystemDefinition(
                    SimulationSystemNames.Climate,
                    SimulationTickPhase.WorldUpdate,
                    frequency: 10,
                    priority: 0),
                new ScheduledSimulationSystemDefinition(
                    SimulationSystemNames.Water,
                    SimulationTickPhase.WorldUpdate,
                    frequency: 10,
                    priority: 1),
                new ScheduledSimulationSystemDefinition(
                    SimulationSystemNames.Resources,
                    SimulationTickPhase.WorldUpdate,
                    frequency: 20,
                    priority: 2),
                new ScheduledSimulationSystemDefinition(
                    SimulationSystemNames.Organisms,
                    SimulationTickPhase.OrganismUpdate,
                    frequency: 1,
                    priority: 0),
                new ScheduledSimulationSystemDefinition(
                    SimulationSystemNames.Species,
                    SimulationTickPhase.OrganismUpdate,
                    frequency: speciesRecognitionSettings.EvaluationFrequency,
                    priority: 1),
                new ScheduledSimulationSystemDefinition(
                    SimulationSystemNames.Movement,
                    SimulationTickPhase.InteractionSystems,
                    frequency: 1,
                    priority: 0),
                new ScheduledSimulationSystemDefinition(
                    SimulationSystemNames.Feeding,
                    SimulationTickPhase.InteractionSystems,
                    frequency: 1,
                    priority: 1),
                new ScheduledSimulationSystemDefinition(
                    SimulationSystemNames.Hydration,
                    SimulationTickPhase.InteractionSystems,
                    frequency: 1,
                    priority: 2),
                new ScheduledSimulationSystemDefinition(
                    SimulationSystemNames.Statistics,
                    SimulationTickPhase.PostUpdate,
                    frequency: 100,
                    priority: 0),
            });
        DeterministicWorldBootstrapFactory worldBootstrapFactory = new(worldConfiguration, engineConfiguration, simulationConfiguration, eventIdGenerator);
        IGenomeBootstrapFactory genomeBootstrapFactory = new DeterministicGenomeBootstrapFactory(eventIdGenerator);
        ITraitExpressionService traitExpressionService = new DeterministicTraitExpressionService();
        IMorphogenesisService morphogenesisService = new DeterministicMorphogenesisService();
        DeterministicOrganismBootstrapFactory organismBootstrapFactory = new(eventIdGenerator, genomeBootstrapFactory, traitExpressionService, morphogenesisService);
        GaiaEngine.Domain.World.World bootstrapWorld = worldBootstrapFactory.CreateWorld();
        OrganismBootstrapState bootstrapOrganismState = organismBootstrapFactory.CreateInitialPopulation(bootstrapWorld);
        DeterministicSimulationTickPipeline tickPipeline = new(
            new ISimulationTickPhase[]
            {
                new NoOpSimulationTickPhase(SimulationTickPhase.InputCollection),
                new NoOpSimulationTickPhase(SimulationTickPhase.PreUpdate),
                new WorldUpdateTimePhase(timeSystem, scheduler, climateSystem, waterSystem, resourceSystem, eventPublisher),
                new OrganismUpdatePhase(organismUpdateSystem, speciesRecognitionSystem, speciesLifecycleSystem),
                new InteractionSystemsPhase(movementSystem, feedingSystem, hydrationSystem, actionRequestDispatcher),
                new NoOpSimulationTickPhase(SimulationTickPhase.EnvironmentUpdate),
                new EventDispatchPhase(eventBus),
                new PostUpdateStatisticsPhase(diagnosticsCollector),
            },
            scheduler);
        DeterministicSimulationSession simulationSession = new(
            tickPipeline,
            bootstrapOrganismState.World,
            bootstrapOrganismState.Organisms,
            bootstrapOrganismState.Genomes,
            bootstrapOrganismState.Species);

        return new GaiaEngineRuntime(engineConfiguration, simulationConfiguration, simulationSession);
    }
}

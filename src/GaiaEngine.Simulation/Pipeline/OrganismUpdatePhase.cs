using System;
using GaiaEngine.Simulation.AI.Execution;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Genetics;
using GaiaEngine.Simulation.Organisms;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Executes deterministic organism systems during the organism update phase.
/// </summary>
public sealed class OrganismUpdatePhase : ISimulationTickPhase
{
    private readonly IOrganismUpdateSystem organismUpdateSystem;
    private readonly ISpeciesRecognitionSystem speciesRecognitionSystem;
    private readonly ISpeciesLifecycleSystem speciesLifecycleSystem;
    private readonly IAutonomousBehaviourSystem autonomousBehaviourSystem;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganismUpdatePhase"/> class.
    /// </summary>
    /// <param name="organismUpdateSystem">The organism system executed during this phase.</param>
    /// <param name="speciesRecognitionSystem">The species recognition system executed during this phase.</param>
    /// <param name="speciesLifecycleSystem">The species lifecycle system executed during this phase.</param>
    /// <param name="autonomousBehaviourSystem">The autonomous behaviour system executed during this phase.</param>
    /// <exception cref="ArgumentNullException">Thrown when any dependency is <see langword="null"/>.</exception>
    public OrganismUpdatePhase(
        IOrganismUpdateSystem organismUpdateSystem,
        ISpeciesRecognitionSystem speciesRecognitionSystem,
        ISpeciesLifecycleSystem speciesLifecycleSystem,
        IAutonomousBehaviourSystem autonomousBehaviourSystem)
    {
        this.organismUpdateSystem = organismUpdateSystem ?? throw new ArgumentNullException(nameof(organismUpdateSystem));
        this.speciesRecognitionSystem = speciesRecognitionSystem ?? throw new ArgumentNullException(nameof(speciesRecognitionSystem));
        this.speciesLifecycleSystem = speciesLifecycleSystem ?? throw new ArgumentNullException(nameof(speciesLifecycleSystem));
        this.autonomousBehaviourSystem = autonomousBehaviourSystem ?? throw new ArgumentNullException(nameof(autonomousBehaviourSystem));
    }

    /// <summary>
    /// Gets the deterministic phase represented by the implementation.
    /// </summary>
    public SimulationTickPhase Phase => SimulationTickPhase.OrganismUpdate;

    /// <summary>
    /// Executes deterministic organism updates for the current tick.
    /// </summary>
    /// <param name="context">The mutable context shared by the current tick.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <see langword="null"/>.</exception>
    public void Execute(SimulationTickContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        foreach (Scheduling.ScheduledSimulationSystem scheduledSystem in context.Schedule.GetSystemsForPhase(SimulationTickPhase.OrganismUpdate))
        {
            if (scheduledSystem.SystemName == SimulationSystemNames.Organisms)
            {
                context.ApplyOrganisms(organismUpdateSystem.Update(context.CurrentWorld, context.CurrentOrganisms));
            }

            if (scheduledSystem.SystemName == SimulationSystemNames.Species)
            {
                SpeciesRecognitionResult recognitionResult = speciesRecognitionSystem.Update(
                    context.CurrentWorld,
                    context.CurrentOrganisms,
                    context.CurrentGenomes,
                    context.CurrentSpecies);
                context.ApplyOrganisms(recognitionResult.Organisms);
                context.ApplySpecies(recognitionResult.Species);
                context.ApplySpecies(speciesLifecycleSystem.Update(context.CurrentOrganisms, context.CurrentSpecies, context.CurrentTimeState.CurrentTick));
            }

            if (scheduledSystem.SystemName == SimulationSystemNames.AI)
            {
                context.ApplyActionRequests(
                    autonomousBehaviourSystem.Update(
                        context.CurrentWorld,
                        context.CurrentOrganisms,
                        context.CurrentActionRequests));
            }
        }
    }
}

using System;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Organisms;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Executes deterministic organism systems during the organism update phase.
/// </summary>
public sealed class OrganismUpdatePhase : ISimulationTickPhase
{
    private readonly IOrganismUpdateSystem organismUpdateSystem;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganismUpdatePhase"/> class.
    /// </summary>
    /// <param name="organismUpdateSystem">The organism system executed during this phase.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="organismUpdateSystem"/> is <see langword="null"/>.</exception>
    public OrganismUpdatePhase(IOrganismUpdateSystem organismUpdateSystem)
    {
        this.organismUpdateSystem = organismUpdateSystem ?? throw new ArgumentNullException(nameof(organismUpdateSystem));
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
        }
    }
}

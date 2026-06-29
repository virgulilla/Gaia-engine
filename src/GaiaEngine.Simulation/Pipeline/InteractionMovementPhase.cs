using System;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Interactions.Movement;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Executes deterministic movement interaction requests during the interaction phase.
/// </summary>
public sealed class InteractionMovementPhase : ISimulationTickPhase
{
    private readonly IMovementSystem movementSystem;

    /// <summary>
    /// Initializes a new instance of the <see cref="InteractionMovementPhase"/> class.
    /// </summary>
    /// <param name="movementSystem">The movement system executed during this phase.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="movementSystem"/> is <see langword="null"/>.</exception>
    public InteractionMovementPhase(IMovementSystem movementSystem)
    {
        this.movementSystem = movementSystem ?? throw new ArgumentNullException(nameof(movementSystem));
    }

    /// <summary>
    /// Gets the deterministic phase represented by the implementation.
    /// </summary>
    public SimulationTickPhase Phase => SimulationTickPhase.InteractionSystems;

    /// <summary>
    /// Executes deterministic movement interactions for the current tick.
    /// </summary>
    /// <param name="context">The mutable context shared by the current tick.</param>
    public void Execute(SimulationTickContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        foreach (Scheduling.ScheduledSimulationSystem scheduledSystem in context.Schedule.GetSystemsForPhase(SimulationTickPhase.InteractionSystems))
        {
            if (scheduledSystem.SystemName == SimulationSystemNames.Movement)
            {
                MovementSystemResult result = movementSystem.Execute(
                    context.CurrentWorld,
                    context.CurrentOrganisms,
                    context.CurrentMovementRequests);
                context.ApplyWorld(result.World);
                context.ApplyOrganisms(result.Organisms);
                context.ApplyMovementRequests(result.RemainingRequests);
            }
        }
    }
}

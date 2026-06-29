using System;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Interactions.Hydration;
using GaiaEngine.Simulation.Interactions.Movement;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Executes deterministic interaction systems during the interaction phase.
/// </summary>
public sealed class InteractionSystemsPhase : ISimulationTickPhase
{
    private readonly IMovementSystem movementSystem;
    private readonly IHydrationSystem hydrationSystem;

    /// <summary>
    /// Initializes a new instance of the <see cref="InteractionSystemsPhase"/> class.
    /// </summary>
    /// <param name="movementSystem">The movement system executed during this phase.</param>
    /// <param name="hydrationSystem">The hydration system executed during this phase.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="movementSystem"/> or <paramref name="hydrationSystem"/> is <see langword="null"/>.
    /// </exception>
    public InteractionSystemsPhase(IMovementSystem movementSystem, IHydrationSystem hydrationSystem)
    {
        this.movementSystem = movementSystem ?? throw new ArgumentNullException(nameof(movementSystem));
        this.hydrationSystem = hydrationSystem ?? throw new ArgumentNullException(nameof(hydrationSystem));
    }

    /// <summary>
    /// Gets the deterministic phase represented by the implementation.
    /// </summary>
    public SimulationTickPhase Phase => SimulationTickPhase.InteractionSystems;

    /// <summary>
    /// Executes deterministic interaction systems for the current tick.
    /// </summary>
    /// <param name="context">The mutable context shared by the current tick.</param>
    public void Execute(SimulationTickContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        foreach (Scheduling.ScheduledSimulationSystem scheduledSystem in context.Schedule.GetSystemsForPhase(SimulationTickPhase.InteractionSystems))
        {
            if (scheduledSystem.SystemName == SimulationSystemNames.Movement)
            {
                MovementSystemResult movementResult = movementSystem.Execute(
                    context.CurrentWorld,
                    context.CurrentOrganisms,
                    context.CurrentMovementRequests);
                context.ApplyWorld(movementResult.World);
                context.ApplyOrganisms(movementResult.Organisms);
                context.ApplyMovementRequests(movementResult.RemainingRequests);
                continue;
            }

            if (scheduledSystem.SystemName == SimulationSystemNames.Hydration)
            {
                HydrationSystemResult hydrationResult = hydrationSystem.Execute(
                    context.CurrentWorld,
                    context.CurrentOrganisms,
                    context.CurrentHydrationRequests);
                context.ApplyWorld(hydrationResult.World);
                context.ApplyOrganisms(hydrationResult.Organisms);
                context.ApplyHydrationRequests(hydrationResult.RemainingRequests);
            }
        }
    }
}

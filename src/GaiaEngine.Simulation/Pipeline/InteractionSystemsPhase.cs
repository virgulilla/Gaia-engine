using System;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Interactions.Feeding;
using GaiaEngine.Simulation.Interactions.Hydration;
using GaiaEngine.Simulation.Interactions.Movement;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Executes deterministic interaction systems during the interaction phase.
/// </summary>
public sealed class InteractionSystemsPhase : ISimulationTickPhase
{
    private readonly IMovementSystem movementSystem;
    private readonly IFeedingSystem feedingSystem;
    private readonly IHydrationSystem hydrationSystem;
    private readonly IActionRequestDispatcher actionRequestDispatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="InteractionSystemsPhase"/> class.
    /// </summary>
    /// <param name="movementSystem">The movement system executed during this phase.</param>
    /// <param name="feedingSystem">The feeding system executed during this phase.</param>
    /// <param name="hydrationSystem">The hydration system executed during this phase.</param>
    /// <param name="actionRequestDispatcher">The dispatcher that routes common action requests to specialized systems.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="movementSystem"/>, <paramref name="feedingSystem"/>, or <paramref name="hydrationSystem"/> is <see langword="null"/>.
    /// </exception>
    public InteractionSystemsPhase(
        IMovementSystem movementSystem,
        IFeedingSystem feedingSystem,
        IHydrationSystem hydrationSystem,
        IActionRequestDispatcher actionRequestDispatcher)
    {
        this.movementSystem = movementSystem ?? throw new ArgumentNullException(nameof(movementSystem));
        this.feedingSystem = feedingSystem ?? throw new ArgumentNullException(nameof(feedingSystem));
        this.hydrationSystem = hydrationSystem ?? throw new ArgumentNullException(nameof(hydrationSystem));
        this.actionRequestDispatcher = actionRequestDispatcher ?? throw new ArgumentNullException(nameof(actionRequestDispatcher));
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

        ActionRequestDispatchResult dispatchResult = actionRequestDispatcher.Dispatch(context.CurrentActionRequests);
        context.ApplyMovementRequests(dispatchResult.MovementRequests);
        context.ApplyFeedingRequests(dispatchResult.FeedingRequests);
        context.ApplyHydrationRequests(dispatchResult.HydrationRequests);

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

            if (scheduledSystem.SystemName == SimulationSystemNames.Feeding)
            {
                FeedingSystemResult feedingResult = feedingSystem.Execute(
                    context.CurrentWorld,
                    context.CurrentOrganisms,
                    context.CurrentFeedingRequests);
                context.ApplyWorld(feedingResult.World);
                context.ApplyOrganisms(feedingResult.Organisms);
                context.ApplyFeedingRequests(feedingResult.RemainingRequests);
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

        context.ApplyActionRequests(
            actionRequestDispatcher.Rebuild(
                dispatchResult.DeferredRequests,
                context.CurrentMovementRequests,
                context.CurrentFeedingRequests,
                context.CurrentHydrationRequests));
    }
}

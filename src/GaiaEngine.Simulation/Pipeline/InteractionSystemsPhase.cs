using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Events;
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
    private readonly ISimulationEventPublisher eventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="InteractionSystemsPhase"/> class.
    /// </summary>
    /// <param name="movementSystem">The movement system executed during this phase.</param>
    /// <param name="feedingSystem">The feeding system executed during this phase.</param>
    /// <param name="hydrationSystem">The hydration system executed during this phase.</param>
    /// <param name="actionRequestDispatcher">The dispatcher that routes common action requests to specialized systems.</param>
    /// <param name="eventPublisher">The simulation event publisher used for action lifecycle events.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="movementSystem"/>, <paramref name="feedingSystem"/>, or <paramref name="hydrationSystem"/> is <see langword="null"/>.
    /// </exception>
    public InteractionSystemsPhase(
        IMovementSystem movementSystem,
        IFeedingSystem feedingSystem,
        IHydrationSystem hydrationSystem,
        IActionRequestDispatcher actionRequestDispatcher,
        ISimulationEventPublisher eventPublisher)
    {
        this.movementSystem = movementSystem ?? throw new ArgumentNullException(nameof(movementSystem));
        this.feedingSystem = feedingSystem ?? throw new ArgumentNullException(nameof(feedingSystem));
        this.hydrationSystem = hydrationSystem ?? throw new ArgumentNullException(nameof(hydrationSystem));
        this.actionRequestDispatcher = actionRequestDispatcher ?? throw new ArgumentNullException(nameof(actionRequestDispatcher));
        this.eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
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

        SimulationActionRequestCollection originalActionRequests = context.CurrentActionRequests;
        GaiaEngine.Domain.World.World preInteractionWorld = context.CurrentWorld;
        OrganismCollection preInteractionOrganisms = context.CurrentOrganisms;
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

        PublishActionOutcomeEvents(context, originalActionRequests, preInteractionWorld, preInteractionOrganisms);
    }

    private void PublishActionOutcomeEvents(
        SimulationTickContext context,
        SimulationActionRequestCollection originalActionRequests,
        GaiaEngine.Domain.World.World preInteractionWorld,
        OrganismCollection preInteractionOrganisms)
    {
        List<ActionEventDescriptor> completedActions = new();
        List<ActionEventDescriptor> failedActions = new();

        foreach (SimulationActionRequest request in originalActionRequests.GetAll())
        {
            if (!IsSupportedInteractionRequest(request, context.CurrentTimeState.CurrentTick))
            {
                continue;
            }

            if (DidActionCompleteSuccessfully(request, preInteractionWorld, preInteractionOrganisms, context.CurrentWorld, context.CurrentOrganisms))
            {
                completedActions.Add(ToDescriptor(request));
            }
            else
            {
                failedActions.Add(ToDescriptor(request));
            }
        }

        if (completedActions.Count > 0)
        {
            context.AppendEventPublicationResult(
                eventPublisher.PublishActionCompletedEvents(
                    completedActions.AsReadOnly(),
                    context.CurrentTimeState.CurrentTick,
                    context.NextEventSequence));
        }

        if (failedActions.Count > 0)
        {
            context.AppendEventPublicationResult(
                eventPublisher.PublishActionFailedEvents(
                    failedActions.AsReadOnly(),
                    context.CurrentTimeState.CurrentTick,
                    context.NextEventSequence));
        }
    }

    private static bool IsSupportedInteractionRequest(SimulationActionRequest request, long currentTick)
    {
        if (request.StartTick > currentTick)
        {
            return false;
        }

        if (request.Target.Kind != ActionTargetKind.Chunk)
        {
            return false;
        }

        if (request.Status != ActionExecutionState.Waiting
            && request.Status != ActionExecutionState.Accepted
            && request.Status != ActionExecutionState.Running)
        {
            return false;
        }

        return request.ActionType == SimulationActionType.Move
            || request.ActionType == SimulationActionType.Eat
            || request.ActionType == SimulationActionType.Drink;
    }

    private static bool DidActionCompleteSuccessfully(
        SimulationActionRequest request,
        GaiaEngine.Domain.World.World preInteractionWorld,
        OrganismCollection preInteractionOrganisms,
        GaiaEngine.Domain.World.World postInteractionWorld,
        OrganismCollection postInteractionOrganisms)
    {
        if (!preInteractionOrganisms.TryGet(request.OrganismId, out Organism? beforeOrganism)
            || !postInteractionOrganisms.TryGet(request.OrganismId, out Organism? afterOrganism))
        {
            return false;
        }

        ChunkId targetChunkId = ChunkId.Parse(request.Target.TargetId);
        if (request.ActionType == SimulationActionType.Move)
        {
            return afterOrganism!.CurrentChunkId == targetChunkId;
        }

        if (!TryResolveChunk(preInteractionWorld, beforeOrganism!.CurrentChunkId, out Chunk? beforeChunk)
            || !TryResolveChunk(postInteractionWorld, afterOrganism!.CurrentChunkId, out Chunk? afterChunk))
        {
            return false;
        }

        if (request.ActionType == SimulationActionType.Eat)
        {
            if (!beforeChunk!.Resources.TryGet(ResourceType.Vegetation, out ResourceState? beforeVegetation)
                || !afterChunk!.Resources.TryGet(ResourceType.Vegetation, out ResourceState? afterVegetation))
            {
                return false;
            }

            return afterOrganism.Needs.Hunger < beforeOrganism.Needs.Hunger
                && afterVegetation!.CurrentAmount < beforeVegetation!.CurrentAmount;
        }

        if (request.ActionType == SimulationActionType.Drink)
        {
            if (!beforeChunk!.Resources.TryGet(ResourceType.FreshWater, out ResourceState? beforeWater)
                || !afterChunk!.Resources.TryGet(ResourceType.FreshWater, out ResourceState? afterWater))
            {
                return false;
            }

            return afterOrganism.Needs.Hydration < beforeOrganism.Needs.Hydration
                && afterWater!.CurrentAmount < beforeWater!.CurrentAmount;
        }

        return false;
    }

    private static bool TryResolveChunk(GaiaEngine.Domain.World.World world, ChunkId chunkId, out Chunk? chunk)
    {
        foreach (Chunk candidate in world.GetChunks())
        {
            if (candidate.Id == chunkId)
            {
                chunk = candidate;
                return true;
            }
        }

        chunk = null;
        return false;
    }

    private static ActionEventDescriptor ToDescriptor(SimulationActionRequest request)
    {
        return new ActionEventDescriptor(request.ActionId, request.OrganismId, request.ActionType, request.Target);
    }
}

using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Simulation.Interactions.Feeding;
using GaiaEngine.Simulation.Interactions.Hydration;
using GaiaEngine.Simulation.Interactions.Movement;

namespace GaiaEngine.Simulation.Actions;

/// <summary>
/// Dispatches common deterministic action requests to the currently supported specialized simulation systems.
/// </summary>
public sealed class DeterministicActionRequestDispatcher : IActionRequestDispatcher
{
    /// <summary>
    /// Dispatches common deterministic action requests to specialized system request collections.
    /// </summary>
    /// <param name="actionRequests">The current common action requests.</param>
    /// <returns>The deterministic dispatch result.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="actionRequests"/> is <see langword="null"/>.</exception>
    public ActionRequestDispatchResult Dispatch(SimulationActionRequestCollection actionRequests)
    {
        ArgumentNullException.ThrowIfNull(actionRequests);

        List<SimulationActionRequest> deferredRequests = new();
        List<MovementRequest> movementRequests = new();
        List<FeedingRequest> feedingRequests = new();
        List<HydrationRequest> hydrationRequests = new();

        foreach (SimulationActionRequest actionRequest in actionRequests.GetAll())
        {
            if (actionRequest.Target.Kind != ActionTargetKind.Chunk)
            {
                deferredRequests.Add(actionRequest);
                continue;
            }

            if (actionRequest.Status != ActionExecutionState.Waiting
                && actionRequest.Status != ActionExecutionState.Accepted
                && actionRequest.Status != ActionExecutionState.Running)
            {
                continue;
            }

            if (actionRequest.ActionType == SimulationActionType.Move)
            {
                movementRequests.Add(
                    new MovementRequest(
                        actionRequest.OrganismId,
                        ChunkId.Parse(actionRequest.Target.TargetId),
                        actionRequest.StartTick,
                        actionRequest.ExpectedDuration,
                        actionRequest.Priority));
                continue;
            }

            if (actionRequest.ActionType == SimulationActionType.Eat)
            {
                feedingRequests.Add(
                    new FeedingRequest(
                        actionRequest.OrganismId,
                        ChunkId.Parse(actionRequest.Target.TargetId),
                        actionRequest.StartTick,
                        actionRequest.ExpectedDuration,
                        actionRequest.Priority));
                continue;
            }

            if (actionRequest.ActionType == SimulationActionType.Drink)
            {
                hydrationRequests.Add(
                    new HydrationRequest(
                        actionRequest.OrganismId,
                        ChunkId.Parse(actionRequest.Target.TargetId),
                        actionRequest.StartTick,
                        actionRequest.ExpectedDuration,
                        actionRequest.Priority));
                continue;
            }

            deferredRequests.Add(actionRequest);
        }

        return new ActionRequestDispatchResult(
            new SimulationActionRequestCollection(deferredRequests.AsReadOnly()),
            new MovementRequestCollection(movementRequests.AsReadOnly()),
            new FeedingRequestCollection(feedingRequests.AsReadOnly()),
            new HydrationRequestCollection(hydrationRequests.AsReadOnly()));
    }

    /// <summary>
    /// Rebuilds a common action request collection from specialized system request collections and deferred requests.
    /// </summary>
    public SimulationActionRequestCollection Rebuild(
        SimulationActionRequestCollection deferredRequests,
        MovementRequestCollection movementRequests,
        FeedingRequestCollection feedingRequests,
        HydrationRequestCollection hydrationRequests)
    {
        ArgumentNullException.ThrowIfNull(deferredRequests);
        ArgumentNullException.ThrowIfNull(movementRequests);
        ArgumentNullException.ThrowIfNull(feedingRequests);
        ArgumentNullException.ThrowIfNull(hydrationRequests);

        List<SimulationActionRequest> requests = new(deferredRequests.Count + movementRequests.Count + feedingRequests.Count + hydrationRequests.Count);
        requests.AddRange(deferredRequests.GetAll());

        foreach (MovementRequest movementRequest in movementRequests.GetAll())
        {
            requests.Add(
                new SimulationActionRequest(
                    ActionId.FromSequence(new EntitySequence(movementRequest.OrganismId.Sequence.Value + 1000UL + (ulong)movementRequest.Priority)),
                    movementRequest.OrganismId,
                    SimulationActionType.Move,
                    new SimulationActionTarget(ActionTargetKind.Chunk, movementRequest.TargetChunkId.ToString()),
                    movementRequest.StartTick,
                    movementRequest.ExpectedDuration,
                    movementRequest.Priority,
                    ActionExecutionState.Waiting,
                    interruptible: true));
        }

        foreach (FeedingRequest feedingRequest in feedingRequests.GetAll())
        {
            requests.Add(
                new SimulationActionRequest(
                    ActionId.FromSequence(new EntitySequence(feedingRequest.OrganismId.Sequence.Value + 2000UL + (ulong)feedingRequest.Priority)),
                    feedingRequest.OrganismId,
                    SimulationActionType.Eat,
                    new SimulationActionTarget(ActionTargetKind.Chunk, feedingRequest.TargetChunkId.ToString()),
                    feedingRequest.StartTick,
                    feedingRequest.ExpectedDuration,
                    feedingRequest.Priority,
                    ActionExecutionState.Waiting,
                    interruptible: true));
        }

        foreach (HydrationRequest hydrationRequest in hydrationRequests.GetAll())
        {
            requests.Add(
                new SimulationActionRequest(
                    ActionId.FromSequence(new EntitySequence(hydrationRequest.OrganismId.Sequence.Value + 3000UL + (ulong)hydrationRequest.Priority)),
                    hydrationRequest.OrganismId,
                    SimulationActionType.Drink,
                    new SimulationActionTarget(ActionTargetKind.Chunk, hydrationRequest.TargetChunkId.ToString()),
                    hydrationRequest.StartTick,
                    hydrationRequest.ExpectedDuration,
                    hydrationRequest.Priority,
                    ActionExecutionState.Waiting,
                    interruptible: true));
        }

        return new SimulationActionRequestCollection(requests.AsReadOnly());
    }
}

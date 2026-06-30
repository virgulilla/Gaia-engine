using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.AI.Decision;
using GaiaEngine.Simulation.Events;

namespace GaiaEngine.Simulation.AI.Execution;

/// <summary>
/// Translates selected decisions into immutable deterministic action execution requests.
/// </summary>
public sealed class DeterministicBehaviourExecutionSystem : IBehaviourExecutionSystem
{
    /// <summary>
    /// Applies one or more selected decisions to the current action request collection.
    /// </summary>
    /// <param name="currentActionRequests">The current action request collection.</param>
    /// <param name="decisions">The selected decisions to translate.</param>
    /// <returns>The deterministic translation result.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="currentActionRequests"/> or <paramref name="decisions"/> is <see langword="null"/>.
    /// </exception>
    public BehaviourExecutionResult Execute(
        SimulationActionRequestCollection currentActionRequests,
        IReadOnlyList<SelectedDecision> decisions)
    {
        ArgumentNullException.ThrowIfNull(currentActionRequests);
        ArgumentNullException.ThrowIfNull(decisions);

        Dictionary<OrganismId, SimulationActionRequest> activeRequestsByOrganism = new();
        List<ActionEventDescriptor> startedActions = new();
        List<ActionEventDescriptor> cancelledActions = new();
        foreach (SimulationActionRequest request in currentActionRequests.GetAll())
        {
            if (IsTerminal(request.Status))
            {
                continue;
            }

            activeRequestsByOrganism[request.OrganismId] = request;
        }

        foreach (SelectedDecision decision in decisions)
        {
            ArgumentNullException.ThrowIfNull(decision);

            if (decision.IsIdleFallback)
            {
                continue;
            }

            if (activeRequestsByOrganism.TryGetValue(decision.OrganismId, out SimulationActionRequest? activeRequest))
            {
                if (IsEquivalent(activeRequest, decision))
                {
                    continue;
                }

                if (!activeRequest.Interruptible)
                {
                    continue;
                }

                cancelledActions.Add(ToDescriptor(activeRequest));
            }

            SimulationActionRequest createdRequest = CreateRequest(decision);
            activeRequestsByOrganism[decision.OrganismId] = createdRequest;
            startedActions.Add(ToDescriptor(createdRequest));
        }

        List<SimulationActionRequest> requests = new(activeRequestsByOrganism.Values);
        return new BehaviourExecutionResult(
            new SimulationActionRequestCollection(requests.AsReadOnly()),
            startedActions.AsReadOnly(),
            cancelledActions.AsReadOnly());
    }

    private static bool IsEquivalent(SimulationActionRequest request, SelectedDecision decision)
    {
        return request.ActionType == decision.ActionType
            && request.Target.Equals(decision.Target)
            && request.Status != ActionExecutionState.Failed
            && request.Status != ActionExecutionState.Cancelled
            && request.Status != ActionExecutionState.Completed;
    }

    private static SimulationActionRequest CreateRequest(SelectedDecision decision)
    {
        return new SimulationActionRequest(
            decision.ActionId,
            decision.OrganismId,
            decision.ActionType,
            decision.Target,
            startTick: decision.DecisionTick,
            expectedDuration: decision.ExpectedDuration,
            priority: 1000 - decision.UtilityScore,
            status: ActionExecutionState.Waiting,
            interruptible: IsInterruptible(decision.ActionType));
    }

    private static bool IsInterruptible(SimulationActionType actionType)
    {
        return actionType switch
        {
            SimulationActionType.Move => true,
            SimulationActionType.Eat => true,
            SimulationActionType.Drink => true,
            SimulationActionType.Idle => true,
            _ => true,
        };
    }

    private static bool IsTerminal(ActionExecutionState status)
    {
        return status == ActionExecutionState.Completed
            || status == ActionExecutionState.Cancelled
            || status == ActionExecutionState.Failed;
    }

    private static ActionEventDescriptor ToDescriptor(SimulationActionRequest request)
    {
        return new ActionEventDescriptor(request.ActionId, request.OrganismId, request.ActionType, request.Target);
    }
}

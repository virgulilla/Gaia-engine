using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.AI.Decision;
using GaiaEngine.Simulation.AI.Execution;
using Xunit;

namespace GaiaEngine.Simulation.Tests.AI.Execution;

public sealed class DeterministicBehaviourExecutionSystemTests
{
    [Fact]
    public void Execute_ShouldCreateActionRequestFromSelectedDecision()
    {
        DeterministicBehaviourExecutionSystem system = new();

        BehaviourExecutionResult result = system.Execute(
            SimulationActionRequestCollection.Empty,
            new[]
            {
                CreateDecision(10, 100, SimulationActionType.Eat, "144115188075855874", utilityScore: 900, duration: 2, isIdleFallback: false),
            });

        SimulationActionRequest request = Assert.Single(result.ActionRequests.GetAll());
        Assert.Equal(SimulationActionType.Eat, request.ActionType);
        Assert.Equal(100, request.Priority);
        Assert.Equal(ActionExecutionState.Waiting, request.Status);
    }

    [Fact]
    public void Execute_ShouldKeepEquivalentActiveRequestWithoutDuplicatingIt()
    {
        DeterministicBehaviourExecutionSystem system = new();
        SimulationActionRequest existingRequest = new(
            ActionId.FromSequence(new EntitySequence(10)),
            OrganismId.FromSequence(new EntitySequence(100)),
            SimulationActionType.Drink,
            new SimulationActionTarget(ActionTargetKind.Chunk, "144115188075855874"),
            40,
            1,
            300,
            ActionExecutionState.Running,
            interruptible: true);

        BehaviourExecutionResult result = system.Execute(
            new SimulationActionRequestCollection(new[] { existingRequest }),
            new[]
            {
                CreateDecision(11, 100, SimulationActionType.Drink, "144115188075855874", utilityScore: 700, duration: 1, isIdleFallback: false),
            });

        SimulationActionRequest request = Assert.Single(result.ActionRequests.GetAll());
        Assert.Equal(existingRequest.ActionId, request.ActionId);
        Assert.Equal(ActionExecutionState.Running, request.Status);
    }

    [Fact]
    public void Execute_ShouldReplaceInterruptibleRequestWhenDecisionChanges()
    {
        DeterministicBehaviourExecutionSystem system = new();
        SimulationActionRequest existingRequest = new(
            ActionId.FromSequence(new EntitySequence(10)),
            OrganismId.FromSequence(new EntitySequence(100)),
            SimulationActionType.Move,
            new SimulationActionTarget(ActionTargetKind.Chunk, "144115188075855874"),
            40,
            1,
            300,
            ActionExecutionState.Accepted,
            interruptible: true);

        BehaviourExecutionResult result = system.Execute(
            new SimulationActionRequestCollection(new[] { existingRequest }),
            new[]
            {
                CreateDecision(20, 100, SimulationActionType.Eat, "144115188075855875", utilityScore: 950, duration: 1, isIdleFallback: false),
            });

        SimulationActionRequest request = Assert.Single(result.ActionRequests.GetAll());
        Assert.Equal(ActionId.FromSequence(new EntitySequence(20)), request.ActionId);
        Assert.Equal(SimulationActionType.Eat, request.ActionType);
    }

    [Fact]
    public void Execute_ShouldKeepNonInterruptibleRequestWhenDecisionChanges()
    {
        DeterministicBehaviourExecutionSystem system = new();
        SimulationActionRequest existingRequest = new(
            ActionId.FromSequence(new EntitySequence(10)),
            OrganismId.FromSequence(new EntitySequence(100)),
            SimulationActionType.Eat,
            new SimulationActionTarget(ActionTargetKind.Chunk, "144115188075855874"),
            40,
            1,
            300,
            ActionExecutionState.Running,
            interruptible: false);

        BehaviourExecutionResult result = system.Execute(
            new SimulationActionRequestCollection(new[] { existingRequest }),
            new[]
            {
                CreateDecision(20, 100, SimulationActionType.Move, "144115188075855875", utilityScore: 950, duration: 1, isIdleFallback: false),
            });

        SimulationActionRequest request = Assert.Single(result.ActionRequests.GetAll());
        Assert.Equal(existingRequest.ActionId, request.ActionId);
        Assert.Equal(SimulationActionType.Eat, request.ActionType);
    }

    [Fact]
    public void Execute_ShouldIgnoreIdleFallbackWhenNoActionShouldBeQueued()
    {
        DeterministicBehaviourExecutionSystem system = new();

        BehaviourExecutionResult result = system.Execute(
            SimulationActionRequestCollection.Empty,
            new[]
            {
                CreateDecision(10, 100, SimulationActionType.Idle, "144115188075855874", utilityScore: 0, duration: 1, isIdleFallback: true),
            });

        Assert.Empty(result.ActionRequests.GetAll());
    }

    private static SelectedDecision CreateDecision(ulong actionSequence, ulong organismSequence, SimulationActionType actionType, string targetId, int utilityScore, int duration, bool isIdleFallback)
    {
        return new SelectedDecision(
            OrganismId.FromSequence(new EntitySequence(organismSequence)),
            ActionId.FromSequence(new EntitySequence(actionSequence)),
            actionType,
            new SimulationActionTarget(ActionTargetKind.Chunk, targetId),
            utilityScore,
            estimatedCost: 10,
            expectedDuration: duration,
            decisionTick: 40,
            isIdleFallback: isIdleFallback);
    }
}

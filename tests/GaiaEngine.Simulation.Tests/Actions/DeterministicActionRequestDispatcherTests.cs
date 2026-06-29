using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Simulation.Actions;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Actions;

public sealed class DeterministicActionRequestDispatcherTests
{
    [Fact]
    public void Dispatch_ShouldRouteSupportedChunkActionsToSpecializedRequests()
    {
        DeterministicActionRequestDispatcher dispatcher = new();
        SimulationActionRequestCollection requests = new(
            new[]
            {
                CreateRequest(1, 100, SimulationActionType.Move, "144115188075855874"),
                CreateRequest(2, 101, SimulationActionType.Eat, "144115188075855875"),
                CreateRequest(3, 102, SimulationActionType.Drink, "144115188075855876"),
            });

        ActionRequestDispatchResult result = dispatcher.Dispatch(requests);

        Assert.Empty(result.DeferredRequests.GetAll());
        Assert.Single(result.MovementRequests.GetAll());
        Assert.Single(result.FeedingRequests.GetAll());
        Assert.Single(result.HydrationRequests.GetAll());
    }

    [Fact]
    public void Dispatch_ShouldKeepUnsupportedTargetsDeferred()
    {
        DeterministicActionRequestDispatcher dispatcher = new();
        SimulationActionRequestCollection requests = new(
            new[]
            {
                new SimulationActionRequest(
                    ActionId.FromSequence(new EntitySequence(1)),
                    OrganismId.FromSequence(new EntitySequence(100)),
                    SimulationActionType.Move,
                    new SimulationActionTarget(ActionTargetKind.Position, "10,20"),
                    0,
                    1,
                    0,
                    ActionExecutionState.Waiting,
                    true),
            });

        ActionRequestDispatchResult result = dispatcher.Dispatch(requests);

        Assert.Single(result.DeferredRequests.GetAll());
        Assert.Empty(result.MovementRequests.GetAll());
    }

    private static SimulationActionRequest CreateRequest(ulong actionSequence, ulong organismSequence, SimulationActionType actionType, string chunkId)
    {
        return new SimulationActionRequest(
            ActionId.FromSequence(new EntitySequence(actionSequence)),
            OrganismId.FromSequence(new EntitySequence(organismSequence)),
            actionType,
            new SimulationActionTarget(ActionTargetKind.Chunk, chunkId),
            0,
            1,
            0,
            ActionExecutionState.Waiting,
            true);
    }
}

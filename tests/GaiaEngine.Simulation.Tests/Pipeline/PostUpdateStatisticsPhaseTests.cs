using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Pipeline;
using GaiaEngine.Simulation.Scheduling;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Pipeline;

public sealed class PostUpdateStatisticsPhaseTests
{
    [Fact]
    public void Execute_ShouldCaptureDiagnosticsWhenStatisticsSystemIsScheduled()
    {
        SimulationTickContext context = new(new WorldTimeState(100, 2, "Summer", 1), 4);
        context.ApplySchedule(
            new SimulationTickSchedule(
                100,
                new[]
                {
                    new ScheduledSimulationSystem(SimulationSystemNames.Statistics, SimulationTickPhase.PostUpdate, 0, 100),
                }));
        context.ApplyEventPublicationResult(
            new SimulationEventPublicationResult(
                5,
                new IEvent[]
                {
                    new NewDaySimulationEvent(GaiaEngine.Domain.Identifiers.EventId.FromSequence(new GaiaEngine.Domain.Identifiers.EntitySequence(4)), 100, 100, 2, "Summer", 1),
                }));
        context.ApplyEventDispatchResult(new EventDispatchResult(1, 0, System.Array.Empty<EventDispatchFailure>()));
        context.RegisterExecutedPhase(SimulationTickPhase.InputCollection);
        context.RegisterExecutedPhase(SimulationTickPhase.PreUpdate);
        context.RegisterExecutedPhase(SimulationTickPhase.WorldUpdate);
        context.RegisterExecutedPhase(SimulationTickPhase.EventDispatch);

        PostUpdateStatisticsPhase phase = new(new SimulationDiagnosticsCollector());

        phase.Execute(context);

        Assert.NotNull(context.Diagnostics);
        Assert.Equal(100, context.Diagnostics!.Tick);
        Assert.Equal(1, context.Diagnostics.ProcessedEventCount);
    }

    [Fact]
    public void Execute_ShouldSkipDiagnosticsWhenStatisticsSystemIsNotScheduled()
    {
        SimulationTickContext context = new(new WorldTimeState(99, 2, "Summer", 1), 4);
        context.ApplySchedule(new SimulationTickSchedule(99, System.Array.Empty<ScheduledSimulationSystem>()));

        PostUpdateStatisticsPhase phase = new(new SimulationDiagnosticsCollector());

        phase.Execute(context);

        Assert.Null(context.Diagnostics);
    }
}

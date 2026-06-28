using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Pipeline;
using GaiaEngine.Simulation.Scheduling;
using GaiaEngine.Simulation.Time;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Diagnostics;

public sealed class SimulationDiagnosticsCollectorTests
{
    [Fact]
    public void Capture_ShouldCreateDeterministicSnapshotFromTickContext()
    {
        SimulationTickContext context = new(new WorldTimeState(100, 2, "Summer", 1), 4);
        context.ApplySchedule(
            new SimulationTickSchedule(
                100,
                new[]
                {
                    new ScheduledSimulationSystem("Statistics", SimulationTickPhase.PostUpdate, 0, 100),
                }));
        context.ApplyEventPublicationResult(
            new SimulationEventPublicationResult(
                6,
                new IEvent[]
                {
                    new NewDaySimulationEvent(GaiaEngine.Domain.Identifiers.EventId.FromSequence(new GaiaEngine.Domain.Identifiers.EntitySequence(4)), 100, 100, 2, "Summer", 1),
                }));
        context.ApplyEventDispatchResult(new EventDispatchResult(1, 0, System.Array.Empty<EventDispatchFailure>()));
        context.RegisterExecutedPhase(SimulationTickPhase.InputCollection);
        context.RegisterExecutedPhase(SimulationTickPhase.PreUpdate);
        context.RegisterExecutedPhase(SimulationTickPhase.WorldUpdate);
        context.RegisterExecutedPhase(SimulationTickPhase.EventDispatch);

        SimulationDiagnosticsCollector collector = new();
        SimulationTickDiagnostics diagnostics = collector.Capture(context);

        Assert.Equal(100, diagnostics.Tick);
        Assert.Equal(2, diagnostics.Day);
        Assert.Equal("Summer", diagnostics.Season);
        Assert.Equal(1, diagnostics.Year);
        Assert.Equal(4, diagnostics.ExecutedPhaseCount);
        Assert.Equal(1, diagnostics.ScheduledSystemCount);
        Assert.Equal(1, diagnostics.PublishedEventCount);
        Assert.Equal(1, diagnostics.ProcessedEventCount);
    }
}

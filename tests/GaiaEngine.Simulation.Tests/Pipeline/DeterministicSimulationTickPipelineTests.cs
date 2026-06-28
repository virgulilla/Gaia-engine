using System;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Pipeline;
using GaiaEngine.Simulation.Scheduling;
using GaiaEngine.Simulation.Time;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Pipeline;

public sealed class DeterministicSimulationTickPipelineTests
{
    [Fact]
    public void Execute_ShouldRunAllPhasesInApprovedOrder()
    {
        DeterministicTimeSystem timeSystem = new(new SimulationCalendar(4, 3));
        DeterministicSimulationTickPipeline pipeline = CreatePipeline(timeSystem, new DeterministicSimulationScheduler(Array.Empty<ScheduledSimulationSystemDefinition>()));

        SimulationTickResult result = pipeline.Execute(new WorldTimeState(0, 0, "Spring", 0));

        Assert.Equal(8, result.ExecutedPhases.Count);
        Assert.Equal(SimulationTickPhase.InputCollection, result.ExecutedPhases[0]);
        Assert.Equal(SimulationTickPhase.PreUpdate, result.ExecutedPhases[1]);
        Assert.Equal(SimulationTickPhase.WorldUpdate, result.ExecutedPhases[2]);
        Assert.Equal(SimulationTickPhase.OrganismUpdate, result.ExecutedPhases[3]);
        Assert.Equal(SimulationTickPhase.InteractionSystems, result.ExecutedPhases[4]);
        Assert.Equal(SimulationTickPhase.EnvironmentUpdate, result.ExecutedPhases[5]);
        Assert.Equal(SimulationTickPhase.EventDispatch, result.ExecutedPhases[6]);
        Assert.Equal(SimulationTickPhase.PostUpdate, result.ExecutedPhases[7]);
        Assert.Equal(1, result.TimeState.CurrentTick);
        Assert.Empty(result.Schedule.Systems);
    }

    [Fact]
    public void Execute_ShouldExposeTimeAdvanceProducedByWorldUpdate()
    {
        DeterministicTimeSystem timeSystem = new(new SimulationCalendar(4, 3));
        DeterministicSimulationScheduler scheduler = new(
            new[]
            {
                new ScheduledSimulationSystemDefinition("Climate", SimulationTickPhase.WorldUpdate, 4, 1),
                new ScheduledSimulationSystemDefinition("Terrain", SimulationTickPhase.WorldUpdate, 2, 0),
            });
        DeterministicSimulationTickPipeline pipeline = CreatePipeline(timeSystem, scheduler);

        SimulationTickResult result = pipeline.Execute(new WorldTimeState(3, 0, "Spring", 0));

        Assert.NotNull(result.TimeAdvanceResult);
        Assert.Equal(4, result.TimeAdvanceResult!.TimeState.CurrentTick);
        Assert.Single(result.TimeAdvanceResult.Transitions);
        Assert.Equal(2, result.Schedule.Systems.Count);
        Assert.Equal(4, result.Schedule.ExecutingTick);
    }

    [Fact]
    public void Constructor_ShouldRejectInvalidPhaseOrder()
    {
        Assert.Throws<ArgumentException>(
            () => new DeterministicSimulationTickPipeline(
                new ISimulationTickPhase[]
                {
                    new NoOpSimulationTickPhase(SimulationTickPhase.PreUpdate),
                    new NoOpSimulationTickPhase(SimulationTickPhase.InputCollection),
                    new NoOpSimulationTickPhase(SimulationTickPhase.WorldUpdate),
                    new NoOpSimulationTickPhase(SimulationTickPhase.OrganismUpdate),
                    new NoOpSimulationTickPhase(SimulationTickPhase.InteractionSystems),
                    new NoOpSimulationTickPhase(SimulationTickPhase.EnvironmentUpdate),
                    new NoOpSimulationTickPhase(SimulationTickPhase.EventDispatch),
                    new NoOpSimulationTickPhase(SimulationTickPhase.PostUpdate),
                },
                new DeterministicSimulationScheduler(Array.Empty<ScheduledSimulationSystemDefinition>())));
    }

    private static DeterministicSimulationTickPipeline CreatePipeline(ITimeSystem timeSystem, ISimulationScheduler scheduler)
    {
        return new DeterministicSimulationTickPipeline(
            new ISimulationTickPhase[]
            {
                new NoOpSimulationTickPhase(SimulationTickPhase.InputCollection),
                new NoOpSimulationTickPhase(SimulationTickPhase.PreUpdate),
                new WorldUpdateTimePhase(timeSystem),
                new NoOpSimulationTickPhase(SimulationTickPhase.OrganismUpdate),
                new NoOpSimulationTickPhase(SimulationTickPhase.InteractionSystems),
                new NoOpSimulationTickPhase(SimulationTickPhase.EnvironmentUpdate),
                new NoOpSimulationTickPhase(SimulationTickPhase.EventDispatch),
                new NoOpSimulationTickPhase(SimulationTickPhase.PostUpdate),
            },
            scheduler);
    }
}

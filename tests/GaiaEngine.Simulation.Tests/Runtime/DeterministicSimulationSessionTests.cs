using System;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Pipeline;
using GaiaEngine.Simulation.Runtime;
using GaiaEngine.Simulation.Scheduling;
using GaiaEngine.Simulation.Time;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Runtime;

public sealed class DeterministicSimulationSessionTests
{
    [Fact]
    public void AdvanceTick_ShouldUpdateCurrentTimeState()
    {
        DeterministicTimeSystem timeSystem = new(new SimulationCalendar(4, 3));
        DeterministicSimulationScheduler scheduler = new(Array.Empty<ScheduledSimulationSystemDefinition>());
        DeterministicSimulationTickPipeline pipeline = new(
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
        DeterministicSimulationSession session = new(pipeline, new WorldTimeState(0, 0, "Spring", 0));

        SimulationTickResult result = session.AdvanceTick();

        Assert.Equal(result.TimeState, session.CurrentTimeState);
        Assert.Equal(1, session.CurrentTimeState.CurrentTick);
        Assert.Equal(8, result.ExecutedPhases.Count);
    }
}

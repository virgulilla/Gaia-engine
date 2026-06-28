using System;
using GaiaEngine.Simulation.Pipeline;
using GaiaEngine.Simulation.Scheduling;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Scheduling;

public sealed class DeterministicSimulationSchedulerTests
{
    [Fact]
    public void CreateSchedule_ShouldSelectSystemsByFrequencyAndPriority()
    {
        DeterministicSimulationScheduler scheduler = new(
            new[]
            {
                new ScheduledSimulationSystemDefinition("Statistics", SimulationTickPhase.PostUpdate, 100, 2),
                new ScheduledSimulationSystemDefinition("Climate", SimulationTickPhase.WorldUpdate, 10, 1),
                new ScheduledSimulationSystemDefinition("Terrain", SimulationTickPhase.WorldUpdate, 10, 0),
            });

        SimulationTickSchedule schedule = scheduler.CreateSchedule(100);

        Assert.Equal(3, schedule.Systems.Count);
        Assert.Equal("Terrain", schedule.Systems[0].SystemName);
        Assert.Equal("Climate", schedule.Systems[1].SystemName);
        Assert.Equal("Statistics", schedule.Systems[2].SystemName);
    }

    [Fact]
    public void CreateSchedule_ShouldExcludeSystemsWhenFrequencyDoesNotMatch()
    {
        DeterministicSimulationScheduler scheduler = new(
            new[]
            {
                new ScheduledSimulationSystemDefinition("Climate", SimulationTickPhase.WorldUpdate, 10, 0),
                new ScheduledSimulationSystemDefinition("Plants", SimulationTickPhase.EnvironmentUpdate, 20, 0),
            });

        SimulationTickSchedule schedule = scheduler.CreateSchedule(11);

        Assert.Empty(schedule.Systems);
    }

    [Fact]
    public void Constructor_ShouldRejectDuplicateSystemNames()
    {
        Assert.Throws<ArgumentException>(
            () => new DeterministicSimulationScheduler(
                new[]
                {
                    new ScheduledSimulationSystemDefinition("Climate", SimulationTickPhase.WorldUpdate, 10, 0),
                    new ScheduledSimulationSystemDefinition("Climate", SimulationTickPhase.EnvironmentUpdate, 20, 1),
                }));
    }
}

using System;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Time;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Time;

public sealed class DeterministicTimeSystemTests
{
    [Fact]
    public void Advance_ShouldIncrementTickWithoutTransitions_WhenNoBoundaryIsReached()
    {
        DeterministicTimeSystem timeSystem = new(new SimulationCalendar(4, 3));
        WorldTimeState initialState = new(0, 0, "Spring", 0);

        TimeAdvanceResult result = timeSystem.Advance(initialState);

        Assert.Equal(1, result.TimeState.CurrentTick);
        Assert.Equal(0, result.TimeState.CurrentDay);
        Assert.Equal("Spring", result.TimeState.CurrentSeason);
        Assert.Equal(0, result.TimeState.CurrentYear);
        Assert.Empty(result.Transitions);
    }

    [Fact]
    public void Advance_ShouldEmitNewDay_WhenDayBoundaryIsReached()
    {
        DeterministicTimeSystem timeSystem = new(new SimulationCalendar(4, 3));
        WorldTimeState initialState = new(3, 0, "Spring", 0);

        TimeAdvanceResult result = timeSystem.Advance(initialState);

        Assert.Equal(4, result.TimeState.CurrentTick);
        Assert.Equal(1, result.TimeState.CurrentDay);
        Assert.Single(result.Transitions);
        Assert.Equal(TemporalTransitionKind.NewDay, result.Transitions[0].Kind);
    }

    [Fact]
    public void Advance_ShouldEmitNewSeason_WhenSeasonBoundaryIsReached()
    {
        DeterministicTimeSystem timeSystem = new(new SimulationCalendar(4, 3));
        WorldTimeState initialState = new(11, 2, "Spring", 0);

        TimeAdvanceResult result = timeSystem.Advance(initialState);

        Assert.Equal(12, result.TimeState.CurrentTick);
        Assert.Equal(0, result.TimeState.CurrentDay);
        Assert.Equal("Summer", result.TimeState.CurrentSeason);
        Assert.Equal(2, result.Transitions.Count);
        Assert.Equal(TemporalTransitionKind.NewDay, result.Transitions[0].Kind);
        Assert.Equal(TemporalTransitionKind.NewSeason, result.Transitions[1].Kind);
    }

    [Fact]
    public void Advance_ShouldEmitNewYear_WhenYearBoundaryIsReached()
    {
        DeterministicTimeSystem timeSystem = new(new SimulationCalendar(2, 1));
        WorldTimeState initialState = new(7, 0, "Winter", 0);

        TimeAdvanceResult result = timeSystem.Advance(initialState);

        Assert.Equal(8, result.TimeState.CurrentTick);
        Assert.Equal(0, result.TimeState.CurrentDay);
        Assert.Equal("Spring", result.TimeState.CurrentSeason);
        Assert.Equal(1, result.TimeState.CurrentYear);
        Assert.Equal(3, result.Transitions.Count);
        Assert.Equal(TemporalTransitionKind.NewDay, result.Transitions[0].Kind);
        Assert.Equal(TemporalTransitionKind.NewSeason, result.Transitions[1].Kind);
        Assert.Equal(TemporalTransitionKind.NewYear, result.Transitions[2].Kind);
    }

    [Fact]
    public void Advance_ShouldRejectUnsupportedSeason()
    {
        DeterministicTimeSystem timeSystem = new(new SimulationCalendar(4, 3));
        WorldTimeState initialState = new(0, 0, "Monsoon", 0);

        Assert.Throws<ArgumentException>(() => timeSystem.Advance(initialState));
    }
}

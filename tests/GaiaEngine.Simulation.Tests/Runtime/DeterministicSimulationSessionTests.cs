using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Runtime;
using GaiaEngine.Simulation.Time;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Runtime;

public sealed class DeterministicSimulationSessionTests
{
    [Fact]
    public void AdvanceTick_ShouldUpdateCurrentTimeState()
    {
        DeterministicTimeSystem timeSystem = new(new SimulationCalendar(4, 3));
        DeterministicSimulationSession session = new(timeSystem, new WorldTimeState(0, 0, "Spring", 0));

        TimeAdvanceResult result = session.AdvanceTick();

        Assert.Equal(result.TimeState, session.CurrentTimeState);
        Assert.Equal(1, session.CurrentTimeState.CurrentTick);
    }
}

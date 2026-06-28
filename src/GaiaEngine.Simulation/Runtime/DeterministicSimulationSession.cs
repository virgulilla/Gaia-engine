using System;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Time;

namespace GaiaEngine.Simulation.Runtime;

/// <summary>
/// Provides a minimal deterministic simulation session driven by the Time System.
/// </summary>
public sealed class DeterministicSimulationSession : ISimulationSession
{
    private readonly ITimeSystem timeSystem;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicSimulationSession"/> class.
    /// </summary>
    /// <param name="timeSystem">The Time System responsible for advancing simulation time.</param>
    /// <param name="initialTimeState">The initial world time state.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="timeSystem"/> or <paramref name="initialTimeState"/> is <see langword="null"/>.
    /// </exception>
    public DeterministicSimulationSession(ITimeSystem timeSystem, WorldTimeState initialTimeState)
    {
        this.timeSystem = timeSystem ?? throw new ArgumentNullException(nameof(timeSystem));
        CurrentTimeState = initialTimeState ?? throw new ArgumentNullException(nameof(initialTimeState));
    }

    /// <summary>
    /// Gets the current simulation world time state.
    /// </summary>
    public WorldTimeState CurrentTimeState { get; private set; }

    /// <summary>
    /// Advances the simulation by one deterministic tick.
    /// </summary>
    /// <returns>The advance result produced by the Time System.</returns>
    public TimeAdvanceResult AdvanceTick()
    {
        TimeAdvanceResult result = timeSystem.Advance(CurrentTimeState);
        CurrentTimeState = result.TimeState;
        return result;
    }
}

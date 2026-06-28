using System;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Pipeline;

namespace GaiaEngine.Simulation.Runtime;

/// <summary>
/// Provides a minimal deterministic simulation session driven by the Time System.
/// </summary>
public sealed class DeterministicSimulationSession : ISimulationSession
{
    private readonly ISimulationTickPipeline tickPipeline;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicSimulationSession"/> class.
    /// </summary>
    /// <param name="tickPipeline">The deterministic tick pipeline responsible for advancing simulation state.</param>
    /// <param name="initialTimeState">The initial world time state.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="tickPipeline"/> or <paramref name="initialTimeState"/> is <see langword="null"/>.
    /// </exception>
    public DeterministicSimulationSession(ISimulationTickPipeline tickPipeline, WorldTimeState initialTimeState)
    {
        this.tickPipeline = tickPipeline ?? throw new ArgumentNullException(nameof(tickPipeline));
        CurrentTimeState = initialTimeState ?? throw new ArgumentNullException(nameof(initialTimeState));
    }

    /// <summary>
    /// Gets the current simulation world time state.
    /// </summary>
    public WorldTimeState CurrentTimeState { get; private set; }

    /// <summary>
    /// Advances the simulation by one deterministic tick.
    /// </summary>
    /// <returns>The deterministic tick pipeline result.</returns>
    public SimulationTickResult AdvanceTick()
    {
        SimulationTickResult result = tickPipeline.Execute(CurrentTimeState);
        CurrentTimeState = result.TimeState;
        return result;
    }
}

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
    private ulong nextEventSequence = 1;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicSimulationSession"/> class.
    /// </summary>
    /// <param name="tickPipeline">The deterministic tick pipeline responsible for advancing simulation state.</param>
    /// <param name="initialWorld">The initial world state.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="tickPipeline"/> or <paramref name="initialWorld"/> is <see langword="null"/>.
    /// </exception>
    public DeterministicSimulationSession(ISimulationTickPipeline tickPipeline, GaiaEngine.Domain.World.World initialWorld)
    {
        this.tickPipeline = tickPipeline ?? throw new ArgumentNullException(nameof(tickPipeline));
        CurrentWorld = initialWorld ?? throw new ArgumentNullException(nameof(initialWorld));
    }

    /// <summary>
    /// Gets the current simulation world state.
    /// </summary>
    public GaiaEngine.Domain.World.World CurrentWorld { get; private set; }

    /// <summary>
    /// Gets the current simulation world time state.
    /// </summary>
    public WorldTimeState CurrentTimeState => CurrentWorld.TimeState;

    /// <summary>
    /// Advances the simulation by one deterministic tick.
    /// </summary>
    /// <returns>The deterministic tick pipeline result.</returns>
    public SimulationTickResult AdvanceTick()
    {
        SimulationTickResult result = tickPipeline.Execute(CurrentWorld, nextEventSequence);
        CurrentWorld = result.World;
        nextEventSequence = result.NextEventSequence;
        return result;
    }
}

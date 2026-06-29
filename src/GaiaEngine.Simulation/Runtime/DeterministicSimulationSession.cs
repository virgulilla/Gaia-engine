using System;
using GaiaEngine.Domain.Organisms;
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
        : this(tickPipeline, initialWorld, OrganismCollection.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicSimulationSession"/> class.
    /// </summary>
    /// <param name="tickPipeline">The deterministic tick pipeline responsible for advancing simulation state.</param>
    /// <param name="initialWorld">The initial world state.</param>
    /// <param name="initialOrganisms">The initial organism state.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="tickPipeline"/>, <paramref name="initialWorld"/>, or <paramref name="initialOrganisms"/> is <see langword="null"/>.
    /// </exception>
    public DeterministicSimulationSession(ISimulationTickPipeline tickPipeline, GaiaEngine.Domain.World.World initialWorld, OrganismCollection initialOrganisms)
    {
        this.tickPipeline = tickPipeline ?? throw new ArgumentNullException(nameof(tickPipeline));
        CurrentWorld = initialWorld ?? throw new ArgumentNullException(nameof(initialWorld));
        CurrentOrganisms = initialOrganisms ?? throw new ArgumentNullException(nameof(initialOrganisms));
    }

    /// <summary>
    /// Gets the current simulation world state.
    /// </summary>
    public GaiaEngine.Domain.World.World CurrentWorld { get; private set; }

    /// <summary>
    /// Gets the current simulation organism state.
    /// </summary>
    public OrganismCollection CurrentOrganisms { get; private set; }

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
        SimulationTickResult result = tickPipeline.Execute(CurrentWorld, CurrentOrganisms, nextEventSequence);
        CurrentWorld = result.World;
        CurrentOrganisms = result.Organisms;
        nextEventSequence = result.NextEventSequence;
        return result;
    }
}

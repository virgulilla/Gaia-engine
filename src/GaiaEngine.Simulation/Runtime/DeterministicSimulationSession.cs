using System;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.Interactions.Feeding;
using GaiaEngine.Simulation.Interactions.Hydration;
using GaiaEngine.Simulation.Interactions.Movement;
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
        : this(tickPipeline, initialWorld, OrganismCollection.Empty, GenomeCollection.Empty, SpeciesCollection.Empty, SimulationActionRequestCollection.Empty, MovementRequestCollection.Empty, FeedingRequestCollection.Empty, HydrationRequestCollection.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicSimulationSession"/> class.
    /// </summary>
    /// <param name="tickPipeline">The deterministic tick pipeline responsible for advancing simulation state.</param>
    /// <param name="initialWorld">The initial world state.</param>
    /// <param name="initialOrganisms">The initial organism state.</param>
    /// <param name="initialGenomes">The initial genome state.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="tickPipeline"/>, <paramref name="initialWorld"/>, <paramref name="initialOrganisms"/>, or <paramref name="initialGenomes"/> is <see langword="null"/>.
    /// </exception>
    public DeterministicSimulationSession(
        ISimulationTickPipeline tickPipeline,
        GaiaEngine.Domain.World.World initialWorld,
        OrganismCollection initialOrganisms,
        GenomeCollection initialGenomes)
        : this(tickPipeline, initialWorld, initialOrganisms, initialGenomes, SpeciesCollection.Empty, SimulationActionRequestCollection.Empty, MovementRequestCollection.Empty, FeedingRequestCollection.Empty, HydrationRequestCollection.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicSimulationSession"/> class.
    /// </summary>
    /// <param name="tickPipeline">The deterministic tick pipeline responsible for advancing simulation state.</param>
    /// <param name="initialWorld">The initial world state.</param>
    /// <param name="initialOrganisms">The initial organism state.</param>
    /// <param name="initialGenomes">The initial genome state.</param>
    /// <param name="initialSpecies">The initial species state.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="tickPipeline"/>, <paramref name="initialWorld"/>, <paramref name="initialOrganisms"/>, <paramref name="initialGenomes"/>, or <paramref name="initialSpecies"/> is <see langword="null"/>.
    /// </exception>
    public DeterministicSimulationSession(
        ISimulationTickPipeline tickPipeline,
        GaiaEngine.Domain.World.World initialWorld,
        OrganismCollection initialOrganisms,
        GenomeCollection initialGenomes,
        SpeciesCollection initialSpecies)
        : this(tickPipeline, initialWorld, initialOrganisms, initialGenomes, initialSpecies, SimulationActionRequestCollection.Empty, MovementRequestCollection.Empty, FeedingRequestCollection.Empty, HydrationRequestCollection.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicSimulationSession"/> class.
    /// </summary>
    /// <param name="tickPipeline">The deterministic tick pipeline responsible for advancing simulation state.</param>
    /// <param name="initialWorld">The initial world state.</param>
    /// <param name="initialOrganisms">The initial organism state.</param>
    /// <param name="initialGenomes">The initial genome state.</param>
    /// <param name="initialSpecies">The initial species state.</param>
    /// <param name="initialActionRequests">The initial common action request state.</param>
    /// <param name="initialMovementRequests">The initial movement request state.</param>
    /// <param name="initialFeedingRequests">The initial feeding request state.</param>
    /// <param name="initialHydrationRequests">The initial hydration request state.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="tickPipeline"/>, <paramref name="initialWorld"/>, <paramref name="initialOrganisms"/>, <paramref name="initialGenomes"/>, <paramref name="initialSpecies"/>,
    /// <paramref name="initialMovementRequests"/>, <paramref name="initialFeedingRequests"/>, or <paramref name="initialHydrationRequests"/> is <see langword="null"/>.
    /// </exception>
    public DeterministicSimulationSession(
        ISimulationTickPipeline tickPipeline,
        GaiaEngine.Domain.World.World initialWorld,
        OrganismCollection initialOrganisms,
        GenomeCollection initialGenomes,
        SpeciesCollection initialSpecies,
        SimulationActionRequestCollection initialActionRequests,
        MovementRequestCollection initialMovementRequests,
        FeedingRequestCollection initialFeedingRequests,
        HydrationRequestCollection initialHydrationRequests)
    {
        this.tickPipeline = tickPipeline ?? throw new ArgumentNullException(nameof(tickPipeline));
        CurrentWorld = initialWorld ?? throw new ArgumentNullException(nameof(initialWorld));
        CurrentOrganisms = initialOrganisms ?? throw new ArgumentNullException(nameof(initialOrganisms));
        CurrentGenomes = initialGenomes ?? throw new ArgumentNullException(nameof(initialGenomes));
        CurrentSpecies = initialSpecies ?? throw new ArgumentNullException(nameof(initialSpecies));
        CurrentActionRequests = initialActionRequests ?? throw new ArgumentNullException(nameof(initialActionRequests));
        CurrentMovementRequests = initialMovementRequests ?? throw new ArgumentNullException(nameof(initialMovementRequests));
        CurrentFeedingRequests = initialFeedingRequests ?? throw new ArgumentNullException(nameof(initialFeedingRequests));
        CurrentHydrationRequests = initialHydrationRequests ?? throw new ArgumentNullException(nameof(initialHydrationRequests));
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
    /// Gets the current simulation genome state.
    /// </summary>
    public GenomeCollection CurrentGenomes { get; }

    /// <summary>
    /// Gets the current simulation species state.
    /// </summary>
    public SpeciesCollection CurrentSpecies { get; }

    /// <summary>
    /// Gets the current common action request state.
    /// </summary>
    public SimulationActionRequestCollection CurrentActionRequests { get; private set; }

    /// <summary>
    /// Gets the current movement request state.
    /// </summary>
    public MovementRequestCollection CurrentMovementRequests { get; private set; }

    /// <summary>
    /// Gets the current feeding request state.
    /// </summary>
    public FeedingRequestCollection CurrentFeedingRequests { get; private set; }

    /// <summary>
    /// Gets the current hydration request state.
    /// </summary>
    public HydrationRequestCollection CurrentHydrationRequests { get; private set; }

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
        SimulationTickResult result = tickPipeline.Execute(CurrentWorld, CurrentOrganisms, CurrentActionRequests, CurrentMovementRequests, CurrentFeedingRequests, CurrentHydrationRequests, nextEventSequence);
        CurrentWorld = result.World;
        CurrentOrganisms = result.Organisms;
        CurrentActionRequests = result.ActionRequests;
        CurrentMovementRequests = result.MovementRequests;
        CurrentFeedingRequests = result.FeedingRequests;
        CurrentHydrationRequests = result.HydrationRequests;
        nextEventSequence = result.NextEventSequence;
        return result;
    }
}

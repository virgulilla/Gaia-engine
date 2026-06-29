using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Interactions.Movement;
using GaiaEngine.Simulation.Pipeline;
using GaiaEngine.Simulation.Time;

namespace GaiaEngine.Simulation.Runtime;

/// <summary>
/// Represents a deterministic simulation session that advances runtime state tick by tick.
/// </summary>
public interface ISimulationSession
{
    /// <summary>
    /// Gets the current simulation world state.
    /// </summary>
    public GaiaEngine.Domain.World.World CurrentWorld { get; }

    /// <summary>
    /// Gets the current simulation organism state.
    /// </summary>
    public OrganismCollection CurrentOrganisms { get; }

    /// <summary>
    /// Gets the current movement request state.
    /// </summary>
    public MovementRequestCollection CurrentMovementRequests { get; }

    /// <summary>
    /// Gets the current simulation world time state.
    /// </summary>
    public WorldTimeState CurrentTimeState { get; }

    /// <summary>
    /// Advances the simulation by one deterministic tick.
    /// </summary>
    /// <returns>The deterministic tick pipeline result.</returns>
    public SimulationTickResult AdvanceTick();
}

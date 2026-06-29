using GaiaEngine.Domain.Organisms;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.Interactions.Feeding;
using GaiaEngine.Simulation.Interactions.Hydration;
using GaiaEngine.Simulation.Interactions.Movement;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Executes the deterministic sequence of phases that forms one simulation tick.
/// </summary>
public interface ISimulationTickPipeline
{
    /// <summary>
    /// Executes one deterministic simulation tick starting from the supplied world time state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <returns>The deterministic result of the tick pipeline execution.</returns>
    public SimulationTickResult Execute(GaiaEngine.Domain.World.World world, ulong nextEventSequence);

    /// <summary>
    /// Executes one deterministic simulation tick starting from the supplied world and organism state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <returns>The deterministic result of the tick pipeline execution.</returns>
    public SimulationTickResult Execute(GaiaEngine.Domain.World.World world, OrganismCollection organisms, ulong nextEventSequence);

    /// <summary>
    /// Executes one deterministic simulation tick starting from the supplied world, organism and movement request state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="actionRequests">The current common action request state.</param>
    /// <param name="movementRequests">The current movement request state.</param>
    /// <param name="feedingRequests">The current feeding request state.</param>
    /// <param name="hydrationRequests">The current hydration request state.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <returns>The deterministic result of the tick pipeline execution.</returns>
    public SimulationTickResult Execute(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        SimulationActionRequestCollection actionRequests,
        MovementRequestCollection movementRequests,
        FeedingRequestCollection feedingRequests,
        HydrationRequestCollection hydrationRequests,
        ulong nextEventSequence);
}

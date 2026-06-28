using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.Time;

/// <summary>
/// Advances deterministic simulation time using an explicit calendar model.
/// </summary>
public interface ITimeSystem
{
    /// <summary>
    /// Advances the supplied world time state by one deterministic simulation tick.
    /// </summary>
    /// <param name="timeState">The current world time state.</param>
    /// <returns>The deterministic advance result.</returns>
    public TimeAdvanceResult Advance(WorldTimeState timeState);
}

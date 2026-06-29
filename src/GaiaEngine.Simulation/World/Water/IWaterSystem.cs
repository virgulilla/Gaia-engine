namespace GaiaEngine.Simulation.World.Water;

/// <summary>
/// Updates deterministic per-chunk water state using only explicit world data.
/// </summary>
public interface IWaterSystem
{
    /// <summary>
    /// Updates the water state of every chunk in the supplied world.
    /// </summary>
    /// <param name="world">The world to update.</param>
    /// <returns>A new world instance containing the updated water state.</returns>
    public GaiaEngine.Domain.World.World UpdateWorld(GaiaEngine.Domain.World.World world);
}

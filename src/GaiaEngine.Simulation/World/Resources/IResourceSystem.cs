namespace GaiaEngine.Simulation.World.Resources;

/// <summary>
/// Updates deterministic per-chunk resource state using only explicit world data.
/// </summary>
public interface IResourceSystem
{
    /// <summary>
    /// Updates the resource state of every chunk in the supplied world.
    /// </summary>
    /// <param name="world">The world to update.</param>
    /// <returns>A new world instance containing the updated resource state.</returns>
    public GaiaEngine.Domain.World.World UpdateWorld(GaiaEngine.Domain.World.World world);
}

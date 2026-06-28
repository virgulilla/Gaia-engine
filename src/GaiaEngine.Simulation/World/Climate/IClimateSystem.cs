using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.World.Climate;

/// <summary>
/// Updates deterministic world climate state per chunk.
/// </summary>
public interface IClimateSystem
{
    /// <summary>
    /// Updates the climate state of every chunk in the supplied world.
    /// </summary>
    /// <param name="world">The world to update.</param>
    /// <returns>A new world instance containing the updated climate state.</returns>
    public GaiaEngine.Domain.World.World UpdateWorld(GaiaEngine.Domain.World.World world);
}

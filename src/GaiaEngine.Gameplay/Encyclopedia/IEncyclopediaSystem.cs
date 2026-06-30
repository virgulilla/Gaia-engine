using GaiaEngine.Gameplay.Discovery;

namespace GaiaEngine.Gameplay.Encyclopedia;

/// <summary>
/// Builds deterministic encyclopedia data from discoveries owned by the player profile.
/// </summary>
public interface IEncyclopediaSystem
{
    /// <summary>
    /// Builds one deterministic encyclopedia snapshot from the supplied discoveries.
    /// </summary>
    /// <param name="discoveries">The discoveries owned by the player profile.</param>
    /// <returns>The resulting encyclopedia snapshot.</returns>
    public EncyclopediaCollection Build(DiscoveryCollection discoveries);
}

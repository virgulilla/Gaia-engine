namespace GaiaEngine.App.Configuration;

/// <summary>
/// Loads immutable world bootstrap configuration for the Gaia Engine host application.
/// </summary>
public interface IWorldConfigurationProvider
{
    /// <summary>
    /// Loads the world bootstrap configuration from the configured source.
    /// </summary>
    /// <returns>The loaded world bootstrap configuration.</returns>
    public WorldConfiguration Load();
}

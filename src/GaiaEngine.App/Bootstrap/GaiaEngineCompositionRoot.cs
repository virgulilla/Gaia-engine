using GaiaEngine.App.Configuration;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Builds the dependency graph for the Gaia Engine host application.
/// </summary>
public static class GaiaEngineCompositionRoot
{
    /// <summary>
    /// Creates the application bootstrap graph for the supplied configuration path.
    /// </summary>
    /// <param name="configurationPath">The absolute path to the engine configuration file.</param>
    /// <returns>A configured application instance.</returns>
    public static GaiaEngineApplication CreateApplication(string configurationPath)
    {
        IEngineConfigurationProvider configurationProvider = new JsonEngineConfigurationProvider(configurationPath);
        return new GaiaEngineApplication(configurationProvider);
    }
}

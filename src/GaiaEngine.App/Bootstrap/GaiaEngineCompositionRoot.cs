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
    /// <param name="simulationConfigurationPath">The absolute path to the simulation configuration file.</param>
    /// <returns>A configured application instance.</returns>
    public static GaiaEngineApplication CreateApplication(string configurationPath, string simulationConfigurationPath)
    {
        IEngineConfigurationProvider configurationProvider = new JsonEngineConfigurationProvider(configurationPath);
        ISimulationConfigurationProvider simulationConfigurationProvider = new JsonSimulationConfigurationProvider(simulationConfigurationPath);
        return new GaiaEngineApplication(configurationProvider, simulationConfigurationProvider);
    }
}

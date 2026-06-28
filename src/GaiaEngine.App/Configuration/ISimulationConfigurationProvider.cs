namespace GaiaEngine.App.Configuration;

/// <summary>
/// Loads immutable simulation configuration for the Gaia Engine host application.
/// </summary>
public interface ISimulationConfigurationProvider
{
    /// <summary>
    /// Loads the simulation configuration from the configured source.
    /// </summary>
    /// <returns>The loaded simulation configuration.</returns>
    public SimulationConfiguration Load();
}

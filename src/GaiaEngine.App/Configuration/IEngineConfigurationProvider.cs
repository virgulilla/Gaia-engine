namespace GaiaEngine.App.Configuration;

/// <summary>
/// Loads immutable engine configuration data for the current host process.
/// </summary>
public interface IEngineConfigurationProvider
{
    /// <summary>
    /// Loads the engine configuration.
    /// </summary>
    /// <returns>The loaded engine configuration.</returns>
    public EngineConfiguration Load();
}

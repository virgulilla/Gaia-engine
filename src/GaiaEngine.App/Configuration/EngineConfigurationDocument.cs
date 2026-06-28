namespace GaiaEngine.App.Configuration;

/// <summary>
/// Represents the serialized engine configuration document.
/// </summary>
internal sealed class EngineConfigurationDocument
{
    /// <summary>
    /// Gets or sets the configuration version string.
    /// </summary>
    public string ConfigurationVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the engine version string.
    /// </summary>
    public string EngineVersion { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the deterministic simulation tick rate.
    /// </summary>
    public int TickRate { get; set; }

    /// <summary>
    /// Gets or sets the worker thread count.
    /// </summary>
    public int ThreadCount { get; set; }

    /// <summary>
    /// Gets or sets the logging level.
    /// </summary>
    public string LoggingLevel { get; set; } = string.Empty;
}

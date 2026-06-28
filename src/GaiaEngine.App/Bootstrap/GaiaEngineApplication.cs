using System;
using GaiaEngine.App.Configuration;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Coordinates deterministic application startup for the Gaia Engine host.
/// </summary>
public sealed class GaiaEngineApplication
{
    private readonly IEngineConfigurationProvider configurationProvider;
    private EngineConfiguration? configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="GaiaEngineApplication"/> class.
    /// </summary>
    /// <param name="configurationProvider">The configuration provider used during startup.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="configurationProvider"/> is <see langword="null"/>.</exception>
    public GaiaEngineApplication(IEngineConfigurationProvider configurationProvider)
    {
        this.configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
    }

    /// <summary>
    /// Loads the immutable engine configuration for the current host process.
    /// </summary>
    /// <returns>The loaded engine configuration.</returns>
    public EngineConfiguration Initialize()
    {
        configuration ??= configurationProvider.Load();
        return configuration;
    }
}

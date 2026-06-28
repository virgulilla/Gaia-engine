using System;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Versioning;

namespace GaiaEngine.App.Configuration;

/// <summary>
/// Represents the immutable startup configuration of the Gaia Engine host application.
/// </summary>
public sealed record EngineConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EngineConfiguration"/> class.
    /// </summary>
    /// <param name="configurationVersion">The version of the configuration set.</param>
    /// <param name="engineVersion">The engine version associated with the configuration.</param>
    /// <param name="tickRate">The deterministic simulation tick rate.</param>
    /// <param name="threadCount">The requested worker thread count.</param>
    /// <param name="loggingLevel">The configured logging verbosity.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when numeric values are outside their valid ranges.</exception>
    public EngineConfiguration(
        ConfigurationVersion configurationVersion,
        EngineVersion engineVersion,
        int tickRate,
        int threadCount,
        string loggingLevel)
    {
        if (tickRate <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tickRate), "The tick rate must be greater than zero.");
        }

        if (threadCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(threadCount), "The thread count must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(loggingLevel))
        {
            throw new ArgumentException("The logging level must contain a value.", nameof(loggingLevel));
        }

        ConfigurationVersion = configurationVersion;
        EngineVersion = engineVersion;
        TickRate = tickRate;
        ThreadCount = threadCount;
        LoggingLevel = loggingLevel;
    }

    /// <summary>
    /// Gets the configuration version.
    /// </summary>
    public ConfigurationVersion ConfigurationVersion { get; }

    /// <summary>
    /// Gets the engine version.
    /// </summary>
    public EngineVersion EngineVersion { get; }

    /// <summary>
    /// Gets the deterministic simulation tick rate.
    /// </summary>
    public int TickRate { get; }

    /// <summary>
    /// Gets the requested worker thread count.
    /// </summary>
    public int ThreadCount { get; }

    /// <summary>
    /// Gets the configured logging level.
    /// </summary>
    public string LoggingLevel { get; }
}

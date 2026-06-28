using System;
using System.IO;
using System.Text.Json;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Versioning;

namespace GaiaEngine.App.Configuration;

/// <summary>
/// Loads engine configuration from a JSON file.
/// </summary>
public sealed class JsonEngineConfigurationProvider : IEngineConfigurationProvider
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly string configurationPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonEngineConfigurationProvider"/> class.
    /// </summary>
    /// <param name="configurationPath">The absolute path to the configuration file.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="configurationPath"/> is empty.</exception>
    public JsonEngineConfigurationProvider(string configurationPath)
    {
        if (string.IsNullOrWhiteSpace(configurationPath))
        {
            throw new ArgumentException("The configuration path must contain a value.", nameof(configurationPath));
        }

        this.configurationPath = configurationPath;
    }

    /// <summary>
    /// Loads the engine configuration from disk.
    /// </summary>
    /// <returns>The loaded engine configuration.</returns>
    public EngineConfiguration Load()
    {
        if (!File.Exists(configurationPath))
        {
            throw new FileNotFoundException("The engine configuration file was not found.", configurationPath);
        }

        string json = File.ReadAllText(configurationPath);
        EngineConfigurationDocument? document = JsonSerializer.Deserialize<EngineConfigurationDocument>(json, SerializerOptions);

        if (document is null)
        {
            throw new InvalidOperationException("The engine configuration document could not be read.");
        }

        return new EngineConfiguration(
            new ConfigurationVersion(document.ConfigurationVersion),
            EngineVersion.Parse(document.EngineVersion),
            document.TickRate,
            document.ThreadCount,
            document.LoggingLevel);
    }
}

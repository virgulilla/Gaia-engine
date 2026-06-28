using System;
using System.IO;
using System.Text.Json;
using GaiaEngine.Domain.World;

namespace GaiaEngine.App.Configuration;

/// <summary>
/// Loads world bootstrap configuration from a JSON document on disk.
/// </summary>
public sealed class JsonWorldConfigurationProvider : IWorldConfigurationProvider
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly string configurationPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonWorldConfigurationProvider"/> class.
    /// </summary>
    /// <param name="configurationPath">The absolute configuration file path.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="configurationPath"/> is empty.</exception>
    public JsonWorldConfigurationProvider(string configurationPath)
    {
        if (string.IsNullOrWhiteSpace(configurationPath))
        {
            throw new ArgumentException("The configuration path must contain a value.", nameof(configurationPath));
        }

        this.configurationPath = configurationPath;
    }

    /// <summary>
    /// Loads the world bootstrap configuration from disk.
    /// </summary>
    /// <returns>The loaded world bootstrap configuration.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the configuration file does not exist.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the JSON payload is invalid.</exception>
    public WorldConfiguration Load()
    {
        if (!File.Exists(configurationPath))
        {
            throw new FileNotFoundException("The world configuration file could not be found.", configurationPath);
        }

        string json = File.ReadAllText(configurationPath);
        WorldConfigurationDocument? document = JsonSerializer.Deserialize<WorldConfigurationDocument>(json, SerializerOptions);
        if (document is null)
        {
            throw new InvalidOperationException("The world configuration document could not be deserialized.");
        }

        return new WorldConfiguration(
            document.WorldName,
            document.WorldSeed,
            document.ChunkColumns,
            document.ChunkRows,
            document.ChunkSize,
            document.MaximumElevation,
            Enum.Parse<ClimateZone>(document.DefaultClimateZone, ignoreCase: false));
    }
}

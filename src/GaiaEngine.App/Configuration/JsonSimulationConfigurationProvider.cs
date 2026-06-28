using System;
using System.IO;
using System.Text.Json;

namespace GaiaEngine.App.Configuration;

/// <summary>
/// Loads simulation configuration from a JSON document on disk.
/// </summary>
public sealed class JsonSimulationConfigurationProvider : ISimulationConfigurationProvider
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly string configurationPath;

    /// <summary>
    /// Initializes a new instance of the <see cref="JsonSimulationConfigurationProvider"/> class.
    /// </summary>
    /// <param name="configurationPath">The absolute configuration file path.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="configurationPath"/> is empty.</exception>
    public JsonSimulationConfigurationProvider(string configurationPath)
    {
        if (string.IsNullOrWhiteSpace(configurationPath))
        {
            throw new ArgumentException("The configuration path must contain a value.", nameof(configurationPath));
        }

        this.configurationPath = configurationPath;
    }

    /// <summary>
    /// Loads the simulation configuration from disk.
    /// </summary>
    /// <returns>The loaded simulation configuration.</returns>
    /// <exception cref="FileNotFoundException">Thrown when the configuration file does not exist.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the JSON payload is invalid.</exception>
    public SimulationConfiguration Load()
    {
        if (!File.Exists(configurationPath))
        {
            throw new FileNotFoundException("The simulation configuration file could not be found.", configurationPath);
        }

        string json = File.ReadAllText(configurationPath);
        SimulationConfigurationDocument? document = JsonSerializer.Deserialize<SimulationConfigurationDocument>(json, SerializerOptions);
        if (document is null)
        {
            throw new InvalidOperationException("The simulation configuration document could not be deserialized.");
        }

        return new SimulationConfiguration(
            document.TicksPerDay,
            document.DaysPerSeason,
            document.StartingDay,
            document.StartingSeason,
            document.StartingYear);
    }
}

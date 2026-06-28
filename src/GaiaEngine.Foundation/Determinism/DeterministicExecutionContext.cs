using System;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Versioning;

namespace GaiaEngine.Foundation.Determinism;

/// <summary>
/// Captures the immutable inputs required to reproduce a deterministic simulation execution.
/// </summary>
public readonly record struct DeterministicExecutionContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicExecutionContext"/> struct.
    /// </summary>
    /// <param name="worldSeed">The world seed used to create the simulation.</param>
    /// <param name="engineVersion">The engine version used during execution.</param>
    /// <param name="configurationVersion">The configuration version used during execution.</param>
    /// <param name="simulationTick">The current simulation tick.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the simulation tick is negative.</exception>
    public DeterministicExecutionContext(
        WorldSeed worldSeed,
        EngineVersion engineVersion,
        ConfigurationVersion configurationVersion,
        long simulationTick)
    {
        if (simulationTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(simulationTick), "The simulation tick must be zero or greater.");
        }

        WorldSeed = worldSeed;
        EngineVersion = engineVersion;
        ConfigurationVersion = configurationVersion;
        SimulationTick = simulationTick;
    }

    /// <summary>
    /// Gets the immutable world seed.
    /// </summary>
    public WorldSeed WorldSeed { get; }

    /// <summary>
    /// Gets the immutable engine version.
    /// </summary>
    public EngineVersion EngineVersion { get; }

    /// <summary>
    /// Gets the immutable configuration version.
    /// </summary>
    public ConfigurationVersion ConfigurationVersion { get; }

    /// <summary>
    /// Gets the simulation tick associated with the execution state.
    /// </summary>
    public long SimulationTick { get; }
}

using System;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.App.Configuration;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Runtime;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Represents the initialized Gaia Engine runtime services required by the host process.
/// </summary>
public sealed class GaiaEngineRuntime
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GaiaEngineRuntime"/> class.
    /// </summary>
    /// <param name="engineConfiguration">The loaded engine configuration.</param>
    /// <param name="simulationConfiguration">The loaded simulation configuration.</param>
    /// <param name="simulationSession">The initialized simulation session.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any required dependency is <see langword="null"/>.
    /// </exception>
    public GaiaEngineRuntime(
        EngineConfiguration engineConfiguration,
        SimulationConfiguration simulationConfiguration,
        ISimulationSession simulationSession)
    {
        EngineConfiguration = engineConfiguration ?? throw new ArgumentNullException(nameof(engineConfiguration));
        SimulationConfiguration = simulationConfiguration ?? throw new ArgumentNullException(nameof(simulationConfiguration));
        SimulationSession = simulationSession ?? throw new ArgumentNullException(nameof(simulationSession));
    }

    /// <summary>
    /// Gets the loaded engine configuration.
    /// </summary>
    public EngineConfiguration EngineConfiguration { get; }

    /// <summary>
    /// Gets the loaded simulation configuration.
    /// </summary>
    public SimulationConfiguration SimulationConfiguration { get; }

    /// <summary>
    /// Gets the initialized simulation session.
    /// </summary>
    public ISimulationSession SimulationSession { get; }

    /// <summary>
    /// Gets the initialized world bootstrap state.
    /// </summary>
    public GaiaEngine.Domain.World.World World => SimulationSession.CurrentWorld;

    /// <summary>
    /// Gets the initialized organism bootstrap state.
    /// </summary>
    public OrganismCollection Organisms => SimulationSession.CurrentOrganisms;
}

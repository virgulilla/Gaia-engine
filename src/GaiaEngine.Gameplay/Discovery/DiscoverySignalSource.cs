namespace GaiaEngine.Gameplay.Discovery;

/// <summary>
/// Defines the deterministic origin of one discovery signal.
/// </summary>
public enum DiscoverySignalSource
{
    /// <summary>
    /// Represents a signal produced from direct observation.
    /// </summary>
    Observation = 0,

    /// <summary>
    /// Represents a signal produced from a simulation event.
    /// </summary>
    SimulationEvent = 1,
}

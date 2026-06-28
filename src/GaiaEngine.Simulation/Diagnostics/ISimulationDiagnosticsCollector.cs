using GaiaEngine.Simulation.Pipeline;

namespace GaiaEngine.Simulation.Diagnostics;

/// <summary>
/// Captures deterministic diagnostics snapshots from the current simulation tick context.
/// </summary>
public interface ISimulationDiagnosticsCollector
{
    /// <summary>
    /// Captures a deterministic diagnostics snapshot from the supplied tick context.
    /// </summary>
    /// <param name="context">The mutable context shared by the current tick.</param>
    /// <returns>The captured diagnostics snapshot.</returns>
    public SimulationTickDiagnostics Capture(SimulationTickContext context);
}

using System;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Represents a deterministic placeholder phase that preserves pipeline structure without mutating state.
/// </summary>
public sealed class NoOpSimulationTickPhase : ISimulationTickPhase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NoOpSimulationTickPhase"/> class.
    /// </summary>
    /// <param name="phase">The represented deterministic phase.</param>
    public NoOpSimulationTickPhase(SimulationTickPhase phase)
    {
        Phase = phase;
    }

    /// <summary>
    /// Gets the deterministic phase represented by the implementation.
    /// </summary>
    public SimulationTickPhase Phase { get; }

    /// <summary>
    /// Executes the phase without mutating state.
    /// </summary>
    /// <param name="context">The mutable context shared by the current tick.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <see langword="null"/>.</exception>
    public void Execute(SimulationTickContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
    }
}

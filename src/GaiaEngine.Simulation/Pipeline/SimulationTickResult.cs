using System;
using System.Collections.Generic;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Time;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Represents the deterministic result produced by one simulation tick pipeline execution.
/// </summary>
public sealed record SimulationTickResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationTickResult"/> class.
    /// </summary>
    /// <param name="timeState">The resulting world time state.</param>
    /// <param name="executedPhases">The deterministic list of executed phases.</param>
    /// <param name="timeAdvanceResult">The time advancement produced during the tick, when available.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="timeState"/> or <paramref name="executedPhases"/> is <see langword="null"/>.
    /// </exception>
    public SimulationTickResult(
        WorldTimeState timeState,
        IReadOnlyList<SimulationTickPhase> executedPhases,
        TimeAdvanceResult? timeAdvanceResult)
    {
        TimeState = timeState ?? throw new ArgumentNullException(nameof(timeState));
        ExecutedPhases = executedPhases ?? throw new ArgumentNullException(nameof(executedPhases));
        TimeAdvanceResult = timeAdvanceResult;
    }

    /// <summary>
    /// Gets the resulting world time state.
    /// </summary>
    public WorldTimeState TimeState { get; }

    /// <summary>
    /// Gets the deterministic list of executed phases.
    /// </summary>
    public IReadOnlyList<SimulationTickPhase> ExecutedPhases { get; }

    /// <summary>
    /// Gets the time advancement produced during the tick, when available.
    /// </summary>
    public TimeAdvanceResult? TimeAdvanceResult { get; }
}

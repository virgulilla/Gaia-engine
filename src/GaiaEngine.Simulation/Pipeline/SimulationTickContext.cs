using System;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Time;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Represents the mutable runtime context shared across deterministic tick phases.
/// </summary>
public sealed class SimulationTickContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationTickContext"/> class.
    /// </summary>
    /// <param name="timeState">The initial world time state for the tick.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="timeState"/> is <see langword="null"/>.</exception>
    public SimulationTickContext(WorldTimeState timeState)
    {
        CurrentTimeState = timeState ?? throw new ArgumentNullException(nameof(timeState));
    }

    /// <summary>
    /// Gets the current world time state being updated by the pipeline.
    /// </summary>
    public WorldTimeState CurrentTimeState { get; private set; }

    /// <summary>
    /// Gets the time advancement produced during the current tick, when available.
    /// </summary>
    public TimeAdvanceResult? TimeAdvanceResult { get; private set; }

    /// <summary>
    /// Applies the time advancement produced by the Time System to the current context.
    /// </summary>
    /// <param name="result">The time advancement result to apply.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public void ApplyTimeAdvance(TimeAdvanceResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        TimeAdvanceResult = result;
        CurrentTimeState = result.TimeState;
    }
}

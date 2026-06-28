using System;
using System.Collections.Generic;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.Time;

/// <summary>
/// Represents the deterministic result of a Time System advance operation.
/// </summary>
public sealed record TimeAdvanceResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeAdvanceResult"/> class.
    /// </summary>
    /// <param name="timeState">The resulting world time state.</param>
    /// <param name="transitions">The ordered transitions emitted during the advance.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="timeState"/> or <paramref name="transitions"/> is <see langword="null"/>.
    /// </exception>
    public TimeAdvanceResult(WorldTimeState timeState, IReadOnlyList<TemporalTransition> transitions)
    {
        TimeState = timeState ?? throw new ArgumentNullException(nameof(timeState));
        Transitions = transitions ?? throw new ArgumentNullException(nameof(transitions));
    }

    /// <summary>
    /// Gets the resulting world time state.
    /// </summary>
    public WorldTimeState TimeState { get; }

    /// <summary>
    /// Gets the ordered temporal transitions emitted during the advance.
    /// </summary>
    public IReadOnlyList<TemporalTransition> Transitions { get; }
}

using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.Time;

/// <summary>
/// Represents a deterministic temporal transition observed while advancing simulation time.
/// </summary>
public sealed record TemporalTransition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TemporalTransition"/> class.
    /// </summary>
    /// <param name="kind">The transition kind.</param>
    /// <param name="timeState">The resulting world time state after the transition.</param>
    public TemporalTransition(TemporalTransitionKind kind, WorldTimeState timeState)
    {
        Kind = kind;
        TimeState = timeState;
    }

    /// <summary>
    /// Gets the transition kind.
    /// </summary>
    public TemporalTransitionKind Kind { get; }

    /// <summary>
    /// Gets the resulting world time state after the transition.
    /// </summary>
    public WorldTimeState TimeState { get; }
}

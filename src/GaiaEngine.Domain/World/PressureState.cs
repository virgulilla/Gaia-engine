namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the passive atmospheric pressure values stored for one chunk climate state.
/// </summary>
public sealed record PressureState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PressureState"/> class.
    /// </summary>
    /// <param name="currentPressure">The current atmospheric pressure value.</param>
    public PressureState(int currentPressure)
    {
        CurrentPressure = currentPressure;
    }

    /// <summary>
    /// Gets the current atmospheric pressure value.
    /// </summary>
    public int CurrentPressure { get; }
}

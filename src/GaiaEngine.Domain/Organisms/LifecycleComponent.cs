using System;

namespace GaiaEngine.Domain.Organisms;

/// <summary>
/// Stores deterministic lifecycle progression data for one organism.
/// </summary>
public sealed record LifecycleComponent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LifecycleComponent"/> class.
    /// </summary>
    /// <param name="birthTick">The birth tick of the organism.</param>
    /// <param name="ageTicks">The current age in ticks.</param>
    /// <param name="maturityAgeTicks">The maturity threshold in ticks.</param>
    /// <param name="stage">The current lifecycle stage.</param>
    /// <param name="isAlive">A value indicating whether the organism is alive.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="birthTick"/>, <paramref name="ageTicks"/>, or <paramref name="maturityAgeTicks"/> is negative.
    /// </exception>
    public LifecycleComponent(long birthTick, long ageTicks, long maturityAgeTicks, LifecycleStage stage, bool isAlive)
    {
        if (birthTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(birthTick), "The birth tick must be zero or greater.");
        }

        if (ageTicks < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(ageTicks), "The age in ticks must be zero or greater.");
        }

        if (maturityAgeTicks < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maturityAgeTicks), "The maturity age must be zero or greater.");
        }

        BirthTick = birthTick;
        AgeTicks = ageTicks;
        MaturityAgeTicks = maturityAgeTicks;
        Stage = stage;
        IsAlive = isAlive;
    }

    /// <summary>
    /// Gets the birth tick of the organism.
    /// </summary>
    public long BirthTick { get; }

    /// <summary>
    /// Gets the current age in ticks.
    /// </summary>
    public long AgeTicks { get; }

    /// <summary>
    /// Gets the maturity threshold in ticks.
    /// </summary>
    public long MaturityAgeTicks { get; }

    /// <summary>
    /// Gets the current lifecycle stage.
    /// </summary>
    public LifecycleStage Stage { get; }

    /// <summary>
    /// Gets a value indicating whether the organism is alive.
    /// </summary>
    public bool IsAlive { get; }
}

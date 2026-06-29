using System;

namespace GaiaEngine.Domain.Organisms;

/// <summary>
/// Stores deterministic organism needs scaled to the inclusive range [0, 1000].
/// </summary>
public sealed record NeedsComponent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NeedsComponent"/> class.
    /// </summary>
    /// <param name="hunger">The current hunger urgency.</param>
    /// <param name="hydration">The current hydration urgency.</param>
    /// <param name="rest">The current rest urgency.</param>
    /// <param name="reproductionUrge">The current reproduction urgency.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when any supplied value is outside the supported deterministic range.</exception>
    public NeedsComponent(int hunger, int hydration, int rest, int reproductionUrge)
    {
        Hunger = ValidateRange(hunger, nameof(hunger));
        Hydration = ValidateRange(hydration, nameof(hydration));
        Rest = ValidateRange(rest, nameof(rest));
        ReproductionUrge = ValidateRange(reproductionUrge, nameof(reproductionUrge));
    }

    /// <summary>
    /// Gets the current hunger urgency.
    /// </summary>
    public int Hunger { get; }

    /// <summary>
    /// Gets the current hydration urgency.
    /// </summary>
    public int Hydration { get; }

    /// <summary>
    /// Gets the current rest urgency.
    /// </summary>
    public int Rest { get; }

    /// <summary>
    /// Gets the current reproduction urgency.
    /// </summary>
    public int ReproductionUrge { get; }

    private static int ValidateRange(int value, string parameterName)
    {
        if (value < 0 || value > 1000)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Need values must be between 0 and 1000.");
        }

        return value;
    }
}

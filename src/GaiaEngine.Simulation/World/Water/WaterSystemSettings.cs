using System;

namespace GaiaEngine.Simulation.World.Water;

/// <summary>
/// Stores the explicit deterministic tuning values used by the Water System.
/// </summary>
public sealed record WaterSystemSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WaterSystemSettings"/> class.
    /// </summary>
    /// <param name="precipitationMultiplier">The multiplier applied to precipitation intensity.</param>
    /// <param name="evaporationDivider">The divisor applied to evaporation.</param>
    /// <param name="runoffDivider">The divisor applied to neighbouring runoff.</param>
    /// <param name="infiltrationDivider">The divisor applied to groundwater recharge.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when one or more divisors are less than one.
    /// </exception>
    public WaterSystemSettings(
        int precipitationMultiplier,
        int evaporationDivider,
        int runoffDivider,
        int infiltrationDivider)
    {
        if (evaporationDivider < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(evaporationDivider), "The evaporation divider must be greater than zero.");
        }

        if (runoffDivider < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(runoffDivider), "The runoff divider must be greater than zero.");
        }

        if (infiltrationDivider < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(infiltrationDivider), "The infiltration divider must be greater than zero.");
        }

        PrecipitationMultiplier = precipitationMultiplier;
        EvaporationDivider = evaporationDivider;
        RunoffDivider = runoffDivider;
        InfiltrationDivider = infiltrationDivider;
    }

    /// <summary>
    /// Gets the multiplier applied to precipitation intensity.
    /// </summary>
    public int PrecipitationMultiplier { get; }

    /// <summary>
    /// Gets the divisor applied to evaporation.
    /// </summary>
    public int EvaporationDivider { get; }

    /// <summary>
    /// Gets the divisor applied to neighbouring runoff.
    /// </summary>
    public int RunoffDivider { get; }

    /// <summary>
    /// Gets the divisor applied to groundwater recharge.
    /// </summary>
    public int InfiltrationDivider { get; }
}

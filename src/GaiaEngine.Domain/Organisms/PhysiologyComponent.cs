using System;

namespace GaiaEngine.Domain.Organisms;

/// <summary>
/// Stores deterministic physiological parameters for one organism.
/// </summary>
public sealed record PhysiologyComponent
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PhysiologyComponent"/> class.
    /// </summary>
    /// <param name="metabolismRate">The deterministic metabolism rate.</param>
    /// <param name="growthRate">The deterministic growth rate.</param>
    /// <param name="lifespanTicks">The deterministic lifespan in ticks.</param>
    /// <param name="waterEfficiency">The deterministic water efficiency value in the inclusive range [0, 100].</param>
    /// <param name="digestionEfficiency">The deterministic digestion efficiency value in the inclusive range [0, 100].</param>
    /// <param name="bodyTemperature">The deterministic body temperature value.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when any supplied value is outside the supported deterministic range.
    /// </exception>
    public PhysiologyComponent(
        int metabolismRate,
        int growthRate,
        int lifespanTicks,
        int waterEfficiency,
        int digestionEfficiency,
        int bodyTemperature)
    {
        if (metabolismRate <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(metabolismRate), "The metabolism rate must be greater than zero.");
        }

        if (growthRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(growthRate), "The growth rate must be zero or greater.");
        }

        if (lifespanTicks <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(lifespanTicks), "The lifespan must be greater than zero.");
        }

        if (waterEfficiency < 0 || waterEfficiency > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(waterEfficiency), "The water efficiency must be between 0 and 100.");
        }

        if (digestionEfficiency < 0 || digestionEfficiency > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(digestionEfficiency), "The digestion efficiency must be between 0 and 100.");
        }

        MetabolismRate = metabolismRate;
        GrowthRate = growthRate;
        LifespanTicks = lifespanTicks;
        WaterEfficiency = waterEfficiency;
        DigestionEfficiency = digestionEfficiency;
        BodyTemperature = bodyTemperature;
    }

    /// <summary>
    /// Gets the deterministic metabolism rate.
    /// </summary>
    public int MetabolismRate { get; }

    /// <summary>
    /// Gets the deterministic growth rate.
    /// </summary>
    public int GrowthRate { get; }

    /// <summary>
    /// Gets the deterministic lifespan in ticks.
    /// </summary>
    public int LifespanTicks { get; }

    /// <summary>
    /// Gets the deterministic water efficiency value in the inclusive range [0, 100].
    /// </summary>
    public int WaterEfficiency { get; }

    /// <summary>
    /// Gets the deterministic digestion efficiency value in the inclusive range [0, 100].
    /// </summary>
    public int DigestionEfficiency { get; }

    /// <summary>
    /// Gets the deterministic body temperature value.
    /// </summary>
    public int BodyTemperature { get; }
}

using System;

namespace GaiaEngine.Simulation.World.Resources;

/// <summary>
/// Stores the explicit deterministic tuning values used by the Resource System.
/// </summary>
public sealed record ResourceSystemSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceSystemSettings"/> class.
    /// </summary>
    /// <param name="vegetationSeasonBonus">The spring and summer vegetation regeneration bonus.</param>
    /// <param name="waterSeasonBonus">The spring fresh-water regeneration bonus.</param>
    /// <param name="precipitationDivider">The divisor used to convert precipitation intensity into regeneration.</param>
    /// <param name="evaporationDivider">The divisor used to convert evaporation into water loss mitigation.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="precipitationDivider"/> or <paramref name="evaporationDivider"/> is less than one.
    /// </exception>
    public ResourceSystemSettings(
        int vegetationSeasonBonus,
        int waterSeasonBonus,
        int precipitationDivider,
        int evaporationDivider)
    {
        if (precipitationDivider < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(precipitationDivider), "The precipitation divider must be greater than zero.");
        }

        if (evaporationDivider < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(evaporationDivider), "The evaporation divider must be greater than zero.");
        }

        VegetationSeasonBonus = vegetationSeasonBonus;
        WaterSeasonBonus = waterSeasonBonus;
        PrecipitationDivider = precipitationDivider;
        EvaporationDivider = evaporationDivider;
    }

    /// <summary>
    /// Gets the spring and summer vegetation regeneration bonus.
    /// </summary>
    public int VegetationSeasonBonus { get; }

    /// <summary>
    /// Gets the spring fresh-water regeneration bonus.
    /// </summary>
    public int WaterSeasonBonus { get; }

    /// <summary>
    /// Gets the divisor used to convert precipitation intensity into regeneration.
    /// </summary>
    public int PrecipitationDivider { get; }

    /// <summary>
    /// Gets the divisor used to convert evaporation into water mitigation.
    /// </summary>
    public int EvaporationDivider { get; }
}

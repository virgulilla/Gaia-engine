using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the local deterministic groundwater state stored by one chunk.
/// </summary>
public sealed record GroundWaterState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GroundWaterState"/> class.
    /// </summary>
    /// <param name="waterTable">The groundwater table level.</param>
    /// <param name="saturation">The soil saturation in the inclusive range [0, 100].</param>
    /// <param name="rechargeRate">The groundwater recharge rate.</param>
    /// <param name="extractionRate">The groundwater extraction rate.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when one or more values are outside the supported deterministic range.
    /// </exception>
    public GroundWaterState(int waterTable, int saturation, int rechargeRate, int extractionRate)
    {
        if (waterTable < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(waterTable), "The groundwater table must be zero or greater.");
        }

        if (saturation < 0 || saturation > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(saturation), "The groundwater saturation must be between 0 and 100.");
        }

        if (rechargeRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rechargeRate), "The groundwater recharge rate must be zero or greater.");
        }

        if (extractionRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(extractionRate), "The groundwater extraction rate must be zero or greater.");
        }

        WaterTable = waterTable;
        Saturation = saturation;
        RechargeRate = rechargeRate;
        ExtractionRate = extractionRate;
    }

    /// <summary>
    /// Gets the groundwater table level.
    /// </summary>
    public int WaterTable { get; }

    /// <summary>
    /// Gets the soil saturation.
    /// </summary>
    public int Saturation { get; }

    /// <summary>
    /// Gets the recharge rate.
    /// </summary>
    public int RechargeRate { get; }

    /// <summary>
    /// Gets the extraction rate.
    /// </summary>
    public int ExtractionRate { get; }
}

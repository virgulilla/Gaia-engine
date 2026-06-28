using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the passive humidity values stored for one chunk climate state.
/// </summary>
public sealed record HumidityState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HumidityState"/> class.
    /// </summary>
    /// <param name="relativeHumidity">The relative humidity percentage.</param>
    /// <param name="evaporationRate">The deterministic evaporation rate.</param>
    /// <param name="condensationRate">The deterministic condensation rate.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when humidity is outside the inclusive range [0, 100].</exception>
    public HumidityState(int relativeHumidity, int evaporationRate, int condensationRate)
    {
        if (relativeHumidity < 0 || relativeHumidity > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(relativeHumidity), "The relative humidity must be between 0 and 100.");
        }

        RelativeHumidity = relativeHumidity;
        EvaporationRate = evaporationRate;
        CondensationRate = condensationRate;
    }

    /// <summary>
    /// Gets the relative humidity percentage.
    /// </summary>
    public int RelativeHumidity { get; }

    /// <summary>
    /// Gets the deterministic evaporation rate.
    /// </summary>
    public int EvaporationRate { get; }

    /// <summary>
    /// Gets the deterministic condensation rate.
    /// </summary>
    public int CondensationRate { get; }
}

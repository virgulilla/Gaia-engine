using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the optional deterministic ocean state stored by one chunk.
/// </summary>
public sealed record OceanState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OceanState"/> class.
    /// </summary>
    /// <param name="seaLevel">The sea level.</param>
    /// <param name="salinity">The salinity in the inclusive range [0, 1000].</param>
    /// <param name="temperature">The ocean temperature.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="seaLevel"/> is negative or when <paramref name="salinity"/> is outside [0, 1000].
    /// </exception>
    public OceanState(int seaLevel, int salinity, int temperature)
    {
        if (seaLevel < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(seaLevel), "The sea level must be zero or greater.");
        }

        if (salinity < 0 || salinity > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(salinity), "The ocean salinity must be between 0 and 1000.");
        }

        SeaLevel = seaLevel;
        Salinity = salinity;
        Temperature = temperature;
    }

    /// <summary>
    /// Gets the sea level.
    /// </summary>
    public int SeaLevel { get; }

    /// <summary>
    /// Gets the salinity.
    /// </summary>
    public int Salinity { get; }

    /// <summary>
    /// Gets the ocean temperature.
    /// </summary>
    public int Temperature { get; }
}

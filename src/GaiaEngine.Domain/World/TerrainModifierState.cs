using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents one deterministic terrain modifier applied to a chunk terrain slice.
/// </summary>
public sealed record TerrainModifierState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TerrainModifierState"/> class.
    /// </summary>
    /// <param name="type">The deterministic terrain modifier type.</param>
    /// <param name="intensity">The modifier intensity in the inclusive range [0, 100].</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="intensity"/> is outside [0, 100].</exception>
    public TerrainModifierState(TerrainModifierType type, int intensity)
    {
        if (intensity < 0 || intensity > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(intensity), "The terrain modifier intensity must be between 0 and 100.");
        }

        Type = type;
        Intensity = intensity;
    }

    /// <summary>
    /// Gets the deterministic terrain modifier type.
    /// </summary>
    public TerrainModifierType Type { get; }

    /// <summary>
    /// Gets the terrain modifier intensity.
    /// </summary>
    public int Intensity { get; }
}

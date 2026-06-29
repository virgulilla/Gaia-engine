using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents immutable deterministic soil data for one chunk terrain slice.
/// </summary>
public sealed record SoilState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoilState"/> class.
    /// </summary>
    /// <param name="soilType">The deterministic soil type.</param>
    /// <param name="fertility">The fertility value in the inclusive range [0, 100].</param>
    /// <param name="drainage">The drainage value in the inclusive range [0, 100].</param>
    /// <param name="moistureCapacity">The moisture capacity value in the inclusive range [0, 100].</param>
    /// <param name="organicMatter">The organic matter value in the inclusive range [0, 100].</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when one or more numeric values are outside the inclusive range [0, 100].
    /// </exception>
    public SoilState(SoilType soilType, int fertility, int drainage, int moistureCapacity, int organicMatter)
    {
        ValidatePercentage(fertility, nameof(fertility));
        ValidatePercentage(drainage, nameof(drainage));
        ValidatePercentage(moistureCapacity, nameof(moistureCapacity));
        ValidatePercentage(organicMatter, nameof(organicMatter));

        SoilType = soilType;
        Fertility = fertility;
        Drainage = drainage;
        MoistureCapacity = moistureCapacity;
        OrganicMatter = organicMatter;
    }

    /// <summary>
    /// Gets the deterministic soil type.
    /// </summary>
    public SoilType SoilType { get; }

    /// <summary>
    /// Gets the fertility value.
    /// </summary>
    public int Fertility { get; }

    /// <summary>
    /// Gets the drainage value.
    /// </summary>
    public int Drainage { get; }

    /// <summary>
    /// Gets the moisture capacity value.
    /// </summary>
    public int MoistureCapacity { get; }

    /// <summary>
    /// Gets the organic matter value.
    /// </summary>
    public int OrganicMatter { get; }

    private static void ValidatePercentage(int value, string parameterName)
    {
        if (value < 0 || value > 100)
        {
            throw new ArgumentOutOfRangeException(parameterName, "The terrain soil value must be between 0 and 100.");
        }
    }
}

using System;
using System.Collections.Generic;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the passive deterministic terrain state stored by one chunk.
/// </summary>
public sealed record TerrainState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TerrainState"/> class.
    /// </summary>
    /// <param name="elevation">The immutable elevation data.</param>
    /// <param name="slope">The immutable slope data.</param>
    /// <param name="soil">The immutable soil data.</param>
    /// <param name="surface">The current surface type.</param>
    /// <param name="geology">The permanent geology type.</param>
    /// <param name="modifiers">The deterministic list of terrain modifiers.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="elevation"/>, <paramref name="slope"/>, <paramref name="soil"/>, or <paramref name="modifiers"/> is <see langword="null"/>.
    /// </exception>
    public TerrainState(
        ElevationState elevation,
        SlopeState slope,
        SoilState soil,
        SurfaceType surface,
        GeologyType geology,
        IReadOnlyList<TerrainModifierState> modifiers)
    {
        Elevation = elevation ?? throw new ArgumentNullException(nameof(elevation));
        Slope = slope ?? throw new ArgumentNullException(nameof(slope));
        Soil = soil ?? throw new ArgumentNullException(nameof(soil));
        Modifiers = modifiers ?? throw new ArgumentNullException(nameof(modifiers));
        foreach (TerrainModifierState modifier in Modifiers)
        {
            ArgumentNullException.ThrowIfNull(modifier);
        }

        Surface = surface;
        Geology = geology;
    }

    /// <summary>
    /// Gets the immutable elevation data.
    /// </summary>
    public ElevationState Elevation { get; }

    /// <summary>
    /// Gets the immutable slope data.
    /// </summary>
    public SlopeState Slope { get; }

    /// <summary>
    /// Gets the immutable soil data.
    /// </summary>
    public SoilState Soil { get; }

    /// <summary>
    /// Gets the current surface type.
    /// </summary>
    public SurfaceType Surface { get; }

    /// <summary>
    /// Gets the permanent geology type.
    /// </summary>
    public GeologyType Geology { get; }

    /// <summary>
    /// Gets the deterministic terrain modifiers.
    /// </summary>
    public IReadOnlyList<TerrainModifierState> Modifiers { get; }
}

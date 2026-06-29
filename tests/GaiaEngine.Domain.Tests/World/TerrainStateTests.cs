using System;
using GaiaEngine.Domain.World;
using Xunit;

namespace GaiaEngine.Domain.Tests.WorldModel;

public sealed class TerrainStateTests
{
    [Fact]
    public void Constructor_ShouldStoreImmutableTerrainData()
    {
        TerrainState terrain = new(
            new ElevationState(70, 10, 10),
            new SlopeState(15, 180, 130),
            new SoilState(SoilType.Loam, 80, 65, 72, 70),
            SurfaceType.Grass,
            GeologyType.Granite,
            Array.Empty<TerrainModifierState>());

        Assert.Equal(70, terrain.Elevation.Height);
        Assert.Equal(15, terrain.Slope.Gradient);
        Assert.Equal(80, terrain.Soil.Fertility);
        Assert.Equal(SurfaceType.Grass, terrain.Surface);
        Assert.Equal(GeologyType.Granite, terrain.Geology);
        Assert.Empty(terrain.Modifiers);
    }

    [Fact]
    public void SoilState_ShouldRejectValuesOutsideSupportedRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SoilState(SoilType.Loam, 101, 50, 50, 50));
    }

    [Fact]
    public void SlopeState_ShouldRejectInvalidAspect()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SlopeState(12, 360, 120));
    }
}

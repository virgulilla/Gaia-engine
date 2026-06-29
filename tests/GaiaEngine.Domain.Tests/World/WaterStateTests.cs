using System;
using GaiaEngine.Domain.World;
using Xunit;

namespace GaiaEngine.Domain.Tests.WorldModel;

public sealed class WaterStateTests
{
    [Fact]
    public void Constructor_ShouldStoreLocalHydrology()
    {
        WaterState water = new(
            new SurfaceWaterState(300, 4, 90, 600),
            new GroundWaterState(40, 60, 5, 0),
            new RiverState("river-0-0", 2, 1, 12, 4),
            null,
            null);

        Assert.Equal(300, water.SurfaceWater.WaterLevel);
        Assert.Equal(60, water.GroundWater.Saturation);
        Assert.Equal("river-0-0", water.River!.RiverId);
    }

    [Fact]
    public void SurfaceWaterState_ShouldRejectInvalidDirection()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SurfaceWaterState(100, 1, 360, 100));
    }
}

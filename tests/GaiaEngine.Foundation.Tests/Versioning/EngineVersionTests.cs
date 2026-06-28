using System;
using GaiaEngine.Foundation.Versioning;
using Xunit;

namespace GaiaEngine.Foundation.Tests.Versioning;

public sealed class EngineVersionTests
{
    [Fact]
    public void Constructor_ShouldStoreSemanticVersionComponents()
    {
        EngineVersion version = new(1, 2, 3);

        Assert.Equal(1, version.Major);
        Assert.Equal(2, version.Minor);
        Assert.Equal(3, version.Patch);
        Assert.Equal("1.2.3", version.ToString());
    }

    [Theory]
    [InlineData(-1, 0, 0, "major")]
    [InlineData(0, -1, 0, "minor")]
    [InlineData(0, 0, -1, "patch")]
    public void Constructor_ShouldRejectNegativeComponents(int major, int minor, int patch, string expectedParameterName)
    {
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => new EngineVersion(major, minor, patch));

        Assert.Equal(expectedParameterName, exception.ParamName);
    }
}

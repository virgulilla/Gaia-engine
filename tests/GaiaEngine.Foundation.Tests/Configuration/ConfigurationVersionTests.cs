using System;
using GaiaEngine.Foundation.Configuration;
using Xunit;

namespace GaiaEngine.Foundation.Tests.Configuration;

public sealed class ConfigurationVersionTests
{
    [Fact]
    public void Constructor_ShouldStoreValue()
    {
        ConfigurationVersion version = new("2026.06.27");

        Assert.Equal("2026.06.27", version.Value);
        Assert.Equal("2026.06.27", version.ToString());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_ShouldRejectInvalidValues(string? value)
    {
        Assert.Throws<ArgumentException>(() => new ConfigurationVersion(value!));
    }
}

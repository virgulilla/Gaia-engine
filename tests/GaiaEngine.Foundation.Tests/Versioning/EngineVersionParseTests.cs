using System;
using GaiaEngine.Foundation.Versioning;
using Xunit;

namespace GaiaEngine.Foundation.Tests.Versioning;

public sealed class EngineVersionParseTests
{
    [Fact]
    public void Parse_ShouldReadSemanticVersion()
    {
        EngineVersion version = EngineVersion.Parse("1.2.3");

        Assert.Equal(1, version.Major);
        Assert.Equal(2, version.Minor);
        Assert.Equal(3, version.Patch);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Parse_ShouldRejectEmptyValues(string value)
    {
        Assert.Throws<ArgumentException>(() => EngineVersion.Parse(value));
    }

    [Theory]
    [InlineData("1")]
    [InlineData("1.2")]
    [InlineData("1.2.3.4")]
    [InlineData("1.two.3")]
    public void Parse_ShouldRejectInvalidFormats(string value)
    {
        Assert.Throws<FormatException>(() => EngineVersion.Parse(value));
    }
}

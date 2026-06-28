using System;
using GaiaEngine.Engine.Identifiers;
using Xunit;

namespace GaiaEngine.Engine.Tests.Identifiers;

public sealed class EntitySequenceTests
{
    [Fact]
    public void Constructor_ShouldStoreValueWithinTheSupportedRange()
    {
        EntitySequence sequence = new(1024);

        Assert.Equal((ulong)1024, sequence.Value);
    }

    [Fact]
    public void Constructor_ShouldRejectValuesOutsideTheSupportedRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new EntitySequence(EntitySequence.MAX_VALUE + 1));
    }
}

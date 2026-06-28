using System;
using GaiaEngine.Domain.Identifiers;
using Xunit;

namespace GaiaEngine.Engine.Tests.Identifiers;

public sealed class WorldIdTests
{
    [Fact]
    public void ToStringAndParse_ShouldRoundTripTheSerializedValue()
    {
        WorldId worldId = WorldId.FromSequence(new EntitySequence(17));

        WorldId parsed = WorldId.Parse(worldId.ToString());

        Assert.Equal(worldId, parsed);
        Assert.Equal((ulong)17, parsed.Sequence.Value);
    }

    [Fact]
    public void Constructor_ShouldRejectAValueFromAnotherCategory()
    {
        OrganismId organismId = OrganismId.FromSequence(new EntitySequence(3));

        Assert.Throws<ArgumentException>(() => new WorldId(organismId.Value));
    }
}

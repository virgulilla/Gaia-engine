using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;
using Xunit;

namespace GaiaEngine.Engine.Tests.Identifiers;

public sealed class DeterministicEntityIdGeneratorTests
{
    [Fact]
    public void CreateOrganismId_ShouldBeDeterministicForTheSameContext()
    {
        DeterministicEntityIdGenerator generator = new();
        IdentifierGenerationContext context = new(new WorldSeed(42), 12, new EntitySequence(7));

        OrganismId first = generator.CreateOrganismId(context);
        OrganismId second = generator.CreateOrganismId(context);

        Assert.Equal(first, second);
    }

    [Fact]
    public void CreateIds_ShouldRemainGloballyUniqueAcrossCategoriesForTheSameSequence()
    {
        DeterministicEntityIdGenerator generator = new();
        IdentifierGenerationContext context = new(new WorldSeed(42), 12, new EntitySequence(9));

        WorldId worldId = generator.CreateWorldId(context);
        ChunkId chunkId = generator.CreateChunkId(context);
        OrganismId organismId = generator.CreateOrganismId(context);
        SpeciesId speciesId = generator.CreateSpeciesId(context);

        Assert.NotEqual(worldId.Value, chunkId.Value);
        Assert.NotEqual(chunkId.Value, organismId.Value);
        Assert.NotEqual(organismId.Value, speciesId.Value);
    }

    [Fact]
    public void CreateEventId_ShouldPreserveTheProvidedSequence()
    {
        DeterministicEntityIdGenerator generator = new();
        IdentifierGenerationContext context = new(new WorldSeed(5), 20, new EntitySequence(123));

        EventId eventId = generator.CreateEventId(context);

        Assert.Equal((ulong)123, eventId.Sequence.Value);
    }
}

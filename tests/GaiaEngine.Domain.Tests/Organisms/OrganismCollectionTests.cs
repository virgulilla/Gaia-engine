using System;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using Xunit;

namespace GaiaEngine.Domain.Tests.Organisms;

public sealed class OrganismCollectionTests
{
    [Fact]
    public void Constructor_ShouldOrderOrganismsDeterministicallyByIdentifier()
    {
        Organism second = CreateOrganism(2, 12);
        Organism first = CreateOrganism(1, 11);
        OrganismCollection collection = new(new[] { second, first });

        Assert.Equal(first.Id, collection.GetAll()[0].Id);
        Assert.Equal(second.Id, collection.GetAll()[1].Id);
    }

    [Fact]
    public void Constructor_ShouldRejectDuplicateIdentifiers()
    {
        Organism organism = CreateOrganism(1, 11);

        Assert.Throws<ArgumentException>(() => new OrganismCollection(new[] { organism, organism }));
    }

    private static Organism CreateOrganism(ulong organismSequence, ulong chunkSequence)
    {
        return new Organism(
            OrganismId.FromSequence(new EntitySequence(organismSequence)),
            SpeciesId.FromSequence(new EntitySequence(1)),
            GenomeId.FromSequence(new EntitySequence(10 + organismSequence)),
            ChunkId.FromSequence(new EntitySequence(chunkSequence)),
            new PhysiologyComponent(3, 2, 500, 60, 55, 18),
            new NeedsComponent(100, 100, 100, 0),
            new LifecycleComponent(0, 0, 100, LifecycleStage.Juvenile, true),
            new HealthComponent(100, 100));
    }
}

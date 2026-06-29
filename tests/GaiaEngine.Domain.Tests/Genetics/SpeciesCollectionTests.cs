using System;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using Xunit;

namespace GaiaEngine.Domain.Tests.Genetics;

public sealed class SpeciesCollectionTests
{
    [Fact]
    public void Constructor_ShouldOrderSpeciesDeterministicallyByIdentifier()
    {
        Species second = CreateSpecies(2, 12, 10);
        Species first = CreateSpecies(1, 11, 0);
        SpeciesCollection collection = new(new[] { second, first });

        Assert.Equal(first.Id, collection.GetAll()[0].Id);
        Assert.Equal(second.Id, collection.GetAll()[1].Id);
    }

    [Fact]
    public void Constructor_ShouldRejectDuplicateIdentifiers()
    {
        Species species = CreateSpecies(1, 11, 0);

        Assert.Throws<ArgumentException>(() => new SpeciesCollection(new[] { species, species }));
    }

    [Fact]
    public void Constructor_ShouldSortFounderPopulationDeterministically()
    {
        Species species = new(
            SpeciesId.FromSequence(new EntitySequence(1)),
            parentSpeciesId: null,
            originTick: 0,
            extinctionTick: null,
            new[]
            {
                OrganismId.FromSequence(new EntitySequence(20)),
                OrganismId.FromSequence(new EntitySequence(10)),
            });

        Assert.Equal(OrganismId.FromSequence(new EntitySequence(10)), species.GetFounderPopulation()[0]);
        Assert.Equal(OrganismId.FromSequence(new EntitySequence(20)), species.GetFounderPopulation()[1]);
    }

    private static Species CreateSpecies(ulong speciesSequence, ulong founderSequence, long originTick)
    {
        return new Species(
            SpeciesId.FromSequence(new EntitySequence(speciesSequence)),
            parentSpeciesId: null,
            originTick,
            extinctionTick: null,
            new[] { OrganismId.FromSequence(new EntitySequence(founderSequence)) });
    }
}

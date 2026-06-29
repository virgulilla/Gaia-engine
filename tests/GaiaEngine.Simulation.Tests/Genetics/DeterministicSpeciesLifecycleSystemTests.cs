using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Simulation.Genetics;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Genetics;

public sealed class DeterministicSpeciesLifecycleSystemTests
{
    [Fact]
    public void Update_ShouldMarkSpeciesExtinctWhenNoLivingOrganismsRemain()
    {
        DeterministicSpeciesLifecycleSystem system = new();
        Species species = CreateSpecies(1, extinctionTick: null);
        Organism deadOrganism = CreateOrganism(1, isAlive: false);

        SpeciesCollection updated = system.Update(new OrganismCollection(new[] { deadOrganism }), new SpeciesCollection(new[] { species }), currentTick: 120);

        Assert.True(updated.GetAll()[0].IsExtinct);
        Assert.Equal(120, updated.GetAll()[0].ExtinctionTick);
    }

    [Fact]
    public void Update_ShouldKeepSpeciesAliveWhenAtLeastOneLivingOrganismExists()
    {
        DeterministicSpeciesLifecycleSystem system = new();
        Species species = CreateSpecies(1, extinctionTick: null);
        Organism livingOrganism = CreateOrganism(1, isAlive: true);

        SpeciesCollection updated = system.Update(new OrganismCollection(new[] { livingOrganism }), new SpeciesCollection(new[] { species }), currentTick: 120);

        Assert.False(updated.GetAll()[0].IsExtinct);
        Assert.Null(updated.GetAll()[0].ExtinctionTick);
    }

    [Fact]
    public void Update_ShouldPreserveExistingExtinctionTick()
    {
        DeterministicSpeciesLifecycleSystem system = new();
        Species species = CreateSpecies(1, extinctionTick: 80);

        SpeciesCollection updated = system.Update(OrganismCollection.Empty, new SpeciesCollection(new[] { species }), currentTick: 120);

        Assert.True(updated.GetAll()[0].IsExtinct);
        Assert.Equal(80, updated.GetAll()[0].ExtinctionTick);
    }

    private static Species CreateSpecies(ulong speciesSequence, long? extinctionTick)
    {
        return new Species(
            SpeciesId.FromSequence(new EntitySequence(speciesSequence)),
            parentSpeciesId: null,
            originTick: 0,
            extinctionTick,
            new[] { OrganismId.FromSequence(new EntitySequence(100)) });
    }

    private static Organism CreateOrganism(ulong speciesSequence, bool isAlive)
    {
        return new Organism(
            OrganismId.FromSequence(new EntitySequence(100)),
            SpeciesId.FromSequence(new EntitySequence(speciesSequence)),
            GenomeId.FromSequence(new EntitySequence(200)),
            ChunkId.FromSequence(new EntitySequence(2)),
            new PhysiologyComponent(3, 2, 500, 60, 55, 18),
            new NeedsComponent(100, 100, 100, 0),
            new LifecycleComponent(0, 0, 100, isAlive ? LifecycleStage.Juvenile : LifecycleStage.Dead, isAlive),
            new HealthComponent(isAlive ? 100 : 0, 100));
    }
}

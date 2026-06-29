using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Determinism;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Creates the initial deterministic organism population for a freshly bootstrapped world.
/// </summary>
public sealed class DeterministicOrganismBootstrapFactory
{
    private readonly IEntityIdGenerator idGenerator;
    private readonly IGenomeBootstrapFactory genomeBootstrapFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicOrganismBootstrapFactory"/> class.
    /// </summary>
    /// <param name="idGenerator">The deterministic identifier generator.</param>
    /// <param name="genomeBootstrapFactory">The deterministic genome bootstrap factory.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="idGenerator"/> or <paramref name="genomeBootstrapFactory"/> is <see langword="null"/>.
    /// </exception>
    public DeterministicOrganismBootstrapFactory(IEntityIdGenerator idGenerator, IGenomeBootstrapFactory genomeBootstrapFactory)
    {
        this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
        this.genomeBootstrapFactory = genomeBootstrapFactory ?? throw new ArgumentNullException(nameof(genomeBootstrapFactory));
    }

    /// <summary>
    /// Creates the initial deterministic organism state for the supplied world.
    /// </summary>
    /// <param name="world">The bootstrap world state.</param>
    /// <returns>The updated world and initial organism population.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="world"/> is <see langword="null"/>.</exception>
    public OrganismBootstrapState CreateInitialPopulation(GaiaEngine.Domain.World.World world)
    {
        ArgumentNullException.ThrowIfNull(world);

        SpeciesId starterSpeciesId = idGenerator.CreateSpeciesId(new IdentifierGenerationContext(world.Metadata.Seed, 0, new EntitySequence(1)));
        List<Organism> organisms = new(world.ChunkCount);
        List<Genome> genomes = new(world.ChunkCount);
        List<Chunk> updatedChunks = new(world.ChunkCount);

        int index = 0;
        foreach (Chunk chunk in world.GetChunks())
        {
            index++;
            Genome genome = genomeBootstrapFactory.CreateGenome(world.Metadata.Seed, chunk, index);
            Organism organism = CreateOrganism(world.Metadata.Seed, starterSpeciesId, genome.Id, chunk, index);
            organisms.Add(organism);
            genomes.Add(genome);
            updatedChunks.Add(
                new Chunk(
                    chunk.Metadata,
                    chunk.State,
                    chunk.Terrain,
                    chunk.Biome,
                    chunk.Climate,
                    chunk.Water,
                    chunk.Resources,
                    new[] { organism.Id }));
        }

        GaiaEngine.Domain.World.World updatedWorld = new(
            world.Metadata,
            world.Dimensions,
            world.TimeState,
            updatedChunks.AsReadOnly());

        return new OrganismBootstrapState(updatedWorld, new OrganismCollection(organisms.AsReadOnly()), new GenomeCollection(genomes.AsReadOnly()));
    }

    private Organism CreateOrganism(WorldSeed worldSeed, SpeciesId speciesId, GenomeId genomeId, Chunk chunk, int index)
    {
        EntitySequence organismSequence = new((ulong)(1000 + index));
        OrganismId organismId = idGenerator.CreateOrganismId(new IdentifierGenerationContext(worldSeed, 0, organismSequence));

        int biomeTemperature = chunk.Biome.ClimateProfile.AverageTemperature;
        int waterEfficiency = Math.Clamp(45 + (chunk.Resources.GetAll()[1].Availability / 40), 0, 100);
        int digestionEfficiency = Math.Clamp(40 + (chunk.Resources.GetAll()[0].Quality / 2), 0, 100);
        int growthRate = Math.Max(1, 2 + (chunk.Biome.VegetationProfile.Density / 25));
        int metabolismRate = Math.Max(1, 3 + (Math.Abs(chunk.Terrain.Elevation.RelativeHeight) / 20));
        int lifespanTicks = 600 + (chunk.Biome.ResourceProfile.Biomass / 4);
        int maturityAgeTicks = Math.Max(60, lifespanTicks / 4);

        return new Organism(
            organismId,
            speciesId,
            genomeId,
            chunk.Id,
            new PhysiologyComponent(
                metabolismRate,
                growthRate,
                lifespanTicks,
                waterEfficiency,
                digestionEfficiency,
                biomeTemperature),
            new NeedsComponent(120, 100, 80, 0),
            new LifecycleComponent(
                birthTick: 0,
                ageTicks: 0,
                maturityAgeTicks,
                LifecycleStage.Juvenile,
                isAlive: true),
            new HealthComponent(100, 100));
    }
}

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
    private readonly ITraitExpressionService traitExpressionService;
    private readonly IMorphogenesisService morphogenesisService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicOrganismBootstrapFactory"/> class.
    /// </summary>
    /// <param name="idGenerator">The deterministic identifier generator.</param>
    /// <param name="genomeBootstrapFactory">The deterministic genome bootstrap factory.</param>
    /// <param name="traitExpressionService">The deterministic trait expression service.</param>
    /// <param name="morphogenesisService">The deterministic morphogenesis service.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="idGenerator"/>, <paramref name="genomeBootstrapFactory"/>, <paramref name="traitExpressionService"/>, or <paramref name="morphogenesisService"/> is <see langword="null"/>.
    /// </exception>
    public DeterministicOrganismBootstrapFactory(
        IEntityIdGenerator idGenerator,
        IGenomeBootstrapFactory genomeBootstrapFactory,
        ITraitExpressionService traitExpressionService,
        IMorphogenesisService morphogenesisService)
    {
        this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
        this.genomeBootstrapFactory = genomeBootstrapFactory ?? throw new ArgumentNullException(nameof(genomeBootstrapFactory));
        this.traitExpressionService = traitExpressionService ?? throw new ArgumentNullException(nameof(traitExpressionService));
        this.morphogenesisService = morphogenesisService ?? throw new ArgumentNullException(nameof(morphogenesisService));
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
            TraitProfile traits = traitExpressionService.Evaluate(genome);
            DevelopmentConditions developmentConditions = CreateDevelopmentConditions(world.TimeState.CurrentSeason, chunk);
            MorphogenesisResult morphogenesis = morphogenesisService.Generate(traits, developmentConditions);
            Organism organism = CreateOrganism(world.Metadata.Seed, starterSpeciesId, genome.Id, chunk, index, morphogenesis);
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

        Species starterSpecies = new(
            starterSpeciesId,
            parentSpeciesId: null,
            world.TimeState.CurrentTick,
            extinctionTick: null,
            organisms.ConvertAll(static organism => organism.Id).AsReadOnly());

        return new OrganismBootstrapState(
            updatedWorld,
            new OrganismCollection(organisms.AsReadOnly()),
            new GenomeCollection(genomes.AsReadOnly()),
            new SpeciesCollection(new[] { starterSpecies }));
    }

    private Organism CreateOrganism(
        WorldSeed worldSeed,
        SpeciesId speciesId,
        GenomeId genomeId,
        Chunk chunk,
        int index,
        MorphogenesisResult morphogenesis)
    {
        EntitySequence organismSequence = new((ulong)(1000 + index));
        OrganismId organismId = idGenerator.CreateOrganismId(new IdentifierGenerationContext(worldSeed, 0, organismSequence));

        return new Organism(
            organismId,
            speciesId,
            genomeId,
            chunk.Id,
            morphogenesis.Physiology,
            new NeedsComponent(120, 100, 80, 0),
            new LifecycleComponent(
                birthTick: 0,
                ageTicks: 0,
                morphogenesis.MaturityAgeTicks,
                LifecycleStage.Juvenile,
                isAlive: true),
            new HealthComponent(100, 100));
    }

    private static DevelopmentConditions CreateDevelopmentConditions(string season, Chunk chunk)
    {
        return new DevelopmentConditions(
            chunk.Biome.ClimateProfile.AverageTemperature,
            chunk.Biome.ResourceProfile.Food,
            chunk.Climate.Humidity.RelativeHumidity * 10,
            chunk.Terrain.Elevation.Height,
            season);
    }
}

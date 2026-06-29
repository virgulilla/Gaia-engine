using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Simulation.Genetics;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Genetics;

public sealed class DeterministicSpeciesRecognitionSystemTests
{
    [Fact]
    public void Update_ShouldCreateNewSpeciesWhenAdultGenomeFailsRecognitionThresholds()
    {
        DeterministicSpeciesRecognitionSystem system = new(CreateSettings(), new DeterministicEntityIdGenerator());
        GaiaEngine.Domain.World.World world = CreateWorld(tick: 100);
        Organism founder = CreateOrganism(100, 1, 200, LifecycleStage.Adult, isAlive: true);
        Organism candidate = CreateOrganism(101, 1, 201, LifecycleStage.Adult, isAlive: true);
        Genome founderGenome = CreateFounderGenome(founder.GenomeId);
        Genome divergentGenome = CreateDivergentGenome(candidate.GenomeId);
        Species rootSpecies = new(founder.SpeciesId, null, 0, null, new[] { founder.Id });

        SpeciesRecognitionResult result = system.Update(
            world,
            new OrganismCollection(new[] { founder, candidate }),
            new GenomeCollection(new[] { founderGenome, divergentGenome }),
            new SpeciesCollection(new[] { rootSpecies }));

        Assert.Equal(2, result.Species.Count);
        Assert.NotEqual(founder.SpeciesId, result.Organisms.Get(candidate.Id).SpeciesId);
        Assert.Equal(founder.SpeciesId, result.Species.GetAll()[1].ParentSpeciesId);
        Assert.Equal(world.TimeState.CurrentTick, result.Species.GetAll()[1].OriginTick);
    }

    [Fact]
    public void Update_ShouldKeepSpeciesWhenReproductiveCompatibilityRemainsHigh()
    {
        DeterministicSpeciesRecognitionSystem system = new(CreateSettings(), new DeterministicEntityIdGenerator());
        GaiaEngine.Domain.World.World world = CreateWorld(tick: 100);
        Organism founder = CreateOrganism(100, 1, 200, LifecycleStage.Adult, isAlive: true);
        Organism candidate = CreateOrganism(101, 1, 201, LifecycleStage.Adult, isAlive: true);
        Genome founderGenome = CreateFounderGenome(founder.GenomeId);
        Genome compatibleGenome = CreateCompatibleGenome(candidate.GenomeId);
        Species rootSpecies = new(founder.SpeciesId, null, 0, null, new[] { founder.Id });

        SpeciesRecognitionResult result = system.Update(
            world,
            new OrganismCollection(new[] { founder, candidate }),
            new GenomeCollection(new[] { founderGenome, compatibleGenome }),
            new SpeciesCollection(new[] { rootSpecies }));

        Assert.Single(result.Species.GetAll());
        Assert.Equal(founder.SpeciesId, result.Organisms.Get(candidate.Id).SpeciesId);
    }

    private static SpeciesRecognitionSettings CreateSettings()
    {
        return new SpeciesRecognitionSettings(
            evaluationFrequency: 100,
            minimumGenomeSimilarity: 780,
            minimumTraitSimilarity: 760,
            minimumMorphologySimilarity: 740,
            minimumReproductiveCompatibility: 700,
            requiredFailedMetricCount: 3);
    }

    private static GaiaEngine.Domain.World.World CreateWorld(long tick)
    {
        WorldId worldId = WorldId.FromSequence(new EntitySequence(1));
        return new GaiaEngine.Domain.World.World(
            new WorldMetadata(
                worldId,
                "Gaia",
                new WorldSeed(42),
                "2026-06-29",
                new EngineVersion(1, 0, 0),
                new ConfigurationVersion("2026.06.29")),
            new WorldDimensions(16, 16, 16, 1, 200),
            new WorldTimeState(tick, 0, "Spring", 0),
            new[]
            {
                new Chunk(
                    new ChunkMetadata(
                        ChunkId.FromSequence(new EntitySequence(2)),
                        worldId,
                        new ChunkCoordinates(0, 0),
                        new WorldSeed(100),
                        16),
                    ChunkState.Active,
                    new TerrainState(
                        new ElevationState(50, 0, 0),
                        new SlopeState(4, 90, 110),
                        new SoilState(SoilType.Loam, 70, 60, 70, 65),
                        SurfaceType.Grass,
                        GeologyType.Granite,
                        System.Array.Empty<TerrainModifierState>()),
                    new BiomeState(
                        BiomeId.FromSequence(new EntitySequence(5)),
                        "Grassland",
                        BiomeCategory.Plains,
                        "Open plains biome.",
                        new BiomeClimateProfile(18, 2, 55, 4, 8),
                        new BiomeTerrainProfile(40, 80, SoilType.Loam, SurfaceType.Grass, 60),
                        new BiomeResourceProfile(750, 800, 500, 800),
                        new BiomeVegetationProfile(VegetationType.Grassland, 62),
                        new BiomeSpeciesAffinityProfile(72, 46, 60, 20)),
                    new ClimateState(
                        ClimateZone.Temperate,
                        WeatherState.Clear,
                        new TemperatureState(18, 18, 18, 0),
                        new HumidityState(55, 3, 2),
                        new WindState(90, 4, 6),
                        new PrecipitationState(PrecipitationType.None, 0, 0, 0),
                        new PressureState(1012)),
                    new WaterState(
                        new SurfaceWaterState(220, 3, 90, 400),
                        new GroundWaterState(42, 58, 6, 0),
                        null,
                        null,
                        null),
                    new ChunkResources(System.Array.Empty<ResourceState>()),
                    new[] { OrganismId.FromSequence(new EntitySequence(100)), OrganismId.FromSequence(new EntitySequence(101)) }),
            });
    }

    private static Organism CreateOrganism(ulong organismSequence, ulong speciesSequence, ulong genomeSequence, LifecycleStage stage, bool isAlive)
    {
        return new Organism(
            OrganismId.FromSequence(new EntitySequence(organismSequence)),
            SpeciesId.FromSequence(new EntitySequence(speciesSequence)),
            GenomeId.FromSequence(new EntitySequence(genomeSequence)),
            ChunkId.FromSequence(new EntitySequence(2)),
            new PhysiologyComponent(3, 2, 500, 60, 55, 18),
            new NeedsComponent(100, 100, 100, 0),
            new LifecycleComponent(0, 200, 100, stage, isAlive),
            new HealthComponent(100, 100));
    }

    private static Genome CreateFounderGenome(GenomeId genomeId)
    {
        return CreateGenome(
            genomeId,
            bodySize: 500,
            metabolism: 500,
            fertility: 500,
            maturityAge: 500,
            visionRange: 500,
            desertAdaptation: 500,
            primaryColor: 500,
            curiosity: 500);
    }

    private static Genome CreateDivergentGenome(GenomeId genomeId)
    {
        return CreateGenome(
            genomeId,
            bodySize: 120,
            metabolism: 880,
            fertility: 0,
            maturityAge: 1000,
            visionRange: 160,
            desertAdaptation: 900,
            primaryColor: 120,
            curiosity: 880);
    }

    private static Genome CreateCompatibleGenome(GenomeId genomeId)
    {
        return CreateGenome(
            genomeId,
            bodySize: 300,
            metabolism: 700,
            fertility: 520,
            maturityAge: 520,
            visionRange: 260,
            desertAdaptation: 700,
            primaryColor: 240,
            curiosity: 720);
    }

    private static Genome CreateGenome(
        GenomeId genomeId,
        int bodySize,
        int metabolism,
        int fertility,
        int maturityAge,
        int visionRange,
        int desertAdaptation,
        int primaryColor,
        int curiosity)
    {
        return new Genome(
            new GenomeIdentity(genomeId, 1, null, null, 0, 0),
            new GenomeGeneGroup(GenomeGroupType.Morphology, new GenomeGene[]
            {
                new(GenomeGeneKey.BodySize, new NormalizedGeneValue(bodySize), GeneDominance.Dominant, true),
                new(GenomeGeneKey.LimbCount, new NormalizedGeneValue(bodySize), GeneDominance.Dominant, true),
                new(GenomeGeneKey.NeckLength, new NormalizedGeneValue(bodySize), GeneDominance.Dominant, true),
                new(GenomeGeneKey.TailLength, new NormalizedGeneValue(bodySize), GeneDominance.Dominant, true),
                new(GenomeGeneKey.SkeletalDensity, new NormalizedGeneValue(bodySize), GeneDominance.Dominant, true),
                new(GenomeGeneKey.MuscleDistribution, new NormalizedGeneValue(bodySize), GeneDominance.Dominant, true),
            }),
            new GenomeGeneGroup(GenomeGroupType.Physiology, new GenomeGene[]
            {
                new(GenomeGeneKey.Metabolism, new NormalizedGeneValue(metabolism), GeneDominance.Dominant, true),
                new(GenomeGeneKey.GrowthRate, new NormalizedGeneValue(metabolism), GeneDominance.Dominant, true),
                new(GenomeGeneKey.Lifespan, new NormalizedGeneValue(metabolism), GeneDominance.Dominant, true),
                new(GenomeGeneKey.HeatResistance, new NormalizedGeneValue(metabolism), GeneDominance.Dominant, true),
                new(GenomeGeneKey.ColdResistance, new NormalizedGeneValue(metabolism), GeneDominance.Dominant, true),
                new(GenomeGeneKey.WaterEfficiency, new NormalizedGeneValue(metabolism), GeneDominance.Dominant, true),
                new(GenomeGeneKey.DigestionEfficiency, new NormalizedGeneValue(metabolism), GeneDominance.Dominant, true),
            }),
            new GenomeGeneGroup(GenomeGroupType.Reproduction, new GenomeGene[]
            {
                new(GenomeGeneKey.MaturityAge, new NormalizedGeneValue(maturityAge), GeneDominance.Dominant, true),
                new(GenomeGeneKey.Fertility, new NormalizedGeneValue(fertility), GeneDominance.Dominant, true),
                new(GenomeGeneKey.GestationTime, new NormalizedGeneValue(maturityAge), GeneDominance.Dominant, true),
                new(GenomeGeneKey.EggCount, new NormalizedGeneValue(fertility), GeneDominance.Dominant, true),
                new(GenomeGeneKey.BreedingCooldown, new NormalizedGeneValue(maturityAge), GeneDominance.Dominant, true),
            }),
            new GenomeGeneGroup(GenomeGroupType.Senses, new GenomeGene[]
            {
                new(GenomeGeneKey.VisionRange, new NormalizedGeneValue(visionRange), GeneDominance.Dominant, true),
                new(GenomeGeneKey.NightVision, new NormalizedGeneValue(visionRange), GeneDominance.Dominant, true),
                new(GenomeGeneKey.SmellSensitivity, new NormalizedGeneValue(visionRange), GeneDominance.Dominant, true),
                new(GenomeGeneKey.HearingRange, new NormalizedGeneValue(visionRange), GeneDominance.Dominant, true),
                new(GenomeGeneKey.ThreatDetection, new NormalizedGeneValue(visionRange), GeneDominance.Dominant, true),
            }),
            new GenomeGeneGroup(GenomeGroupType.Adaptation, new GenomeGene[]
            {
                new(GenomeGeneKey.DesertAdaptation, new NormalizedGeneValue(desertAdaptation), GeneDominance.Dominant, true),
                new(GenomeGeneKey.ColdAdaptation, new NormalizedGeneValue(desertAdaptation), GeneDominance.Dominant, true),
                new(GenomeGeneKey.WetlandAdaptation, new NormalizedGeneValue(desertAdaptation), GeneDominance.Dominant, true),
                new(GenomeGeneKey.MountainAdaptation, new NormalizedGeneValue(desertAdaptation), GeneDominance.Dominant, true),
                new(GenomeGeneKey.AquaticAffinity, new NormalizedGeneValue(desertAdaptation), GeneDominance.Dominant, true),
            }),
            new GenomeGeneGroup(GenomeGroupType.Appearance, new GenomeGene[]
            {
                new(GenomeGeneKey.PrimaryColor, new NormalizedGeneValue(primaryColor), GeneDominance.Dominant, true),
                new(GenomeGeneKey.SecondaryColor, new NormalizedGeneValue(primaryColor), GeneDominance.Dominant, true),
                new(GenomeGeneKey.Pattern, new NormalizedGeneValue(primaryColor), GeneDominance.Dominant, true),
                new(GenomeGeneKey.FurDensity, new NormalizedGeneValue(primaryColor), GeneDominance.Dominant, true),
                new(GenomeGeneKey.ScaleDensity, new NormalizedGeneValue(primaryColor), GeneDominance.Dominant, true),
            }),
            new GenomeGeneGroup(GenomeGroupType.BehaviourBias, new GenomeGene[]
            {
                new(GenomeGeneKey.Curiosity, new NormalizedGeneValue(curiosity), GeneDominance.Dominant, true),
                new(GenomeGeneKey.AggressionBias, new NormalizedGeneValue(curiosity), GeneDominance.Dominant, true),
                new(GenomeGeneKey.SocialBias, new NormalizedGeneValue(curiosity), GeneDominance.Dominant, true),
                new(GenomeGeneKey.RiskTolerance, new NormalizedGeneValue(curiosity), GeneDominance.Dominant, true),
                new(GenomeGeneKey.ExplorationBias, new NormalizedGeneValue(curiosity), GeneDominance.Dominant, true),
            }));
    }
}

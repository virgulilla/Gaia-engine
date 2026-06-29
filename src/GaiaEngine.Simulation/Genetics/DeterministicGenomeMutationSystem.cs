using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;

namespace GaiaEngine.Simulation.Genetics;

/// <summary>
/// Applies deterministic mutations to a newly inherited genome.
/// </summary>
public sealed class DeterministicGenomeMutationSystem : IGenomeMutationSystem
{
    private static readonly MutationCategory[] MutationCategories =
    [
        MutationCategory.Parameter,
        MutationCategory.Dominance,
        MutationCategory.Activation,
        MutationCategory.Structural,
    ];

    private readonly GenomeMutationSettings settings;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicGenomeMutationSystem"/> class.
    /// </summary>
    /// <param name="settings">The immutable mutation settings.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="settings"/> is <see langword="null"/>.</exception>
    public DeterministicGenomeMutationSystem(GenomeMutationSettings settings)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
    }

    /// <summary>
    /// Applies deterministic mutations to a newly inherited genome.
    /// </summary>
    /// <param name="genome">The genome produced by inheritance.</param>
    /// <param name="worldSeed">The world seed that drives deterministic mutation decisions.</param>
    /// <returns>The mutated genome.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="genome"/> is <see langword="null"/>.</exception>
    public Genome Mutate(Genome genome, WorldSeed worldSeed)
    {
        ArgumentNullException.ThrowIfNull(genome);

        List<GenomeMutationRecord> records = new();
        int startingSequence = genome.MutationHistory.Count;
        GenomeGeneGroup morphology = MutateGroup(genome.Morphology, genome.Id, worldSeed, records, startingSequence);
        GenomeGeneGroup physiology = MutateGroup(genome.Physiology, genome.Id, worldSeed, records, startingSequence);
        GenomeGeneGroup reproduction = MutateGroup(genome.Reproduction, genome.Id, worldSeed, records, startingSequence);
        GenomeGeneGroup senses = MutateGroup(genome.Senses, genome.Id, worldSeed, records, startingSequence);
        GenomeGeneGroup adaptation = MutateGroup(genome.Adaptation, genome.Id, worldSeed, records, startingSequence);
        GenomeGeneGroup appearance = MutateGroup(genome.Appearance, genome.Id, worldSeed, records, startingSequence);
        GenomeGeneGroup behaviourBias = MutateGroup(genome.BehaviourBias, genome.Id, worldSeed, records, startingSequence);

        List<GenomeMutationRecord> allRecords = new(genome.MutationHistory.Count + records.Count);
        foreach (GenomeMutationRecord record in genome.MutationHistory.GetAll())
        {
            allRecords.Add(record);
        }

        foreach (GenomeMutationRecord record in records)
        {
            allRecords.Add(record);
        }

        GenomeIdentity identity = new(
            genome.Identity.GenomeId,
            genome.Identity.Version,
            genome.Identity.ParentGenomeA,
            genome.Identity.ParentGenomeB,
            genome.Identity.MutationCount + records.Count,
            genome.Identity.Generation);

        return new Genome(
            identity,
            morphology,
            physiology,
            reproduction,
            senses,
            adaptation,
            appearance,
            behaviourBias,
            settings.MutationVersion,
            new GenomeMutationHistory(allRecords.AsReadOnly()));
    }

    private GenomeGeneGroup MutateGroup(GenomeGeneGroup group, GenomeId genomeId, WorldSeed worldSeed, List<GenomeMutationRecord> records, int startingSequence)
    {
        List<GenomeGene> genes = new(group.Count);
        IReadOnlyList<GenomeGene> sourceGenes = group.GetGenes();
        foreach (GenomeGene gene in sourceGenes)
        {
            GenomeGene currentGene = gene;
            foreach (MutationCategory category in MutationCategories)
            {
                if (records.Count >= settings.MaxMutationsPerGenome)
                {
                    break;
                }

                if (!ShouldMutate(genomeId, worldSeed, group.GroupType, gene.Key, category))
                {
                    continue;
                }

                GenomeGene mutatedGene = ApplyMutation(currentGene, sourceGenes, group.GroupType, worldSeed, category);
                if (currentGene == mutatedGene)
                {
                    continue;
                }

                records.Add(
                    new GenomeMutationRecord(
                        sequence: startingSequence + records.Count,
                        group.GroupType,
                        gene.Key,
                        category,
                        currentGene.Value.ScaledValue,
                        mutatedGene.Value.ScaledValue,
                        currentGene.Dominance,
                        mutatedGene.Dominance,
                        currentGene.IsActive,
                        mutatedGene.IsActive));
                currentGene = mutatedGene;
            }

            genes.Add(currentGene);
        }

        return new GenomeGeneGroup(group.GroupType, genes.AsReadOnly());
    }

    private bool ShouldMutate(GenomeId genomeId, WorldSeed worldSeed, GenomeGroupType groupType, GenomeGeneKey geneKey, MutationCategory category)
    {
        if (settings.GlobalMutationChance == 0 || settings.MaxMutationsPerGenome == 0)
        {
            return false;
        }

        int effectiveChance = (settings.GlobalMutationChance * settings.GetGroupWeight(groupType) * settings.GetCategoryWeight(category)) / 1_000_000;
        if (effectiveChance <= 0)
        {
            return false;
        }

        int roll = ComputeScore(genomeId, worldSeed, groupType, geneKey, category, salt: 0);
        return roll < effectiveChance;
    }

    private GenomeGene ApplyMutation(GenomeGene gene, IReadOnlyList<GenomeGene> groupGenes, GenomeGroupType groupType, WorldSeed worldSeed, MutationCategory category)
    {
        return category switch
        {
            MutationCategory.Parameter => ApplyParameterMutation(gene, worldSeed, groupType),
            MutationCategory.Dominance => ApplyDominanceMutation(gene),
            MutationCategory.Activation => ApplyActivationMutation(gene),
            MutationCategory.Structural => ApplyStructuralMutation(gene, groupGenes, worldSeed, groupType),
            _ => gene,
        };
    }

    private GenomeGene ApplyParameterMutation(GenomeGene gene, WorldSeed worldSeed, GenomeGroupType groupType)
    {
        int amplitude = Math.Max(1, settings.MutationStrength / 10);
        int signedOffset = ComputeSignedOffset(gene.Key, worldSeed, groupType, MutationCategory.Parameter, amplitude);
        int newValue = Math.Clamp(gene.Value.ScaledValue + signedOffset, 0, 1000);
        return newValue == gene.Value.ScaledValue
            ? gene
            : new GenomeGene(gene.Key, new NormalizedGeneValue(newValue), gene.Dominance, gene.IsActive);
    }

    private static GenomeGene ApplyDominanceMutation(GenomeGene gene)
    {
        GeneDominance newDominance = gene.Dominance switch
        {
            GeneDominance.Dominant => GeneDominance.Recessive,
            GeneDominance.Recessive => GeneDominance.CoDominant,
            GeneDominance.CoDominant => GeneDominance.Blended,
            _ => GeneDominance.Dominant,
        };

        return newDominance == gene.Dominance
            ? gene
            : new GenomeGene(gene.Key, gene.Value, newDominance, gene.IsActive);
    }

    private static GenomeGene ApplyActivationMutation(GenomeGene gene)
    {
        return new GenomeGene(gene.Key, gene.Value, gene.Dominance, !gene.IsActive);
    }

    private GenomeGene ApplyStructuralMutation(GenomeGene gene, IReadOnlyList<GenomeGene> groupGenes, WorldSeed worldSeed, GenomeGroupType groupType)
    {
        GenomeGene neighbour = ResolveNeighbour(groupGenes, gene.Key);
        int amplitude = Math.Max(1, settings.MutationStrength / 8);
        int signedOffset = ComputeSignedOffset(gene.Key, worldSeed, groupType, MutationCategory.Structural, amplitude);
        int newValue = Math.Clamp(((gene.Value.ScaledValue + neighbour.Value.ScaledValue) / 2) + signedOffset, 0, 1000);
        bool newIsActive = gene.IsActive || neighbour.IsActive;
        GeneDominance newDominance = ResolveStructuralDominance(gene.Dominance, neighbour.Dominance);

        if (newValue == gene.Value.ScaledValue && newIsActive == gene.IsActive && newDominance == gene.Dominance)
        {
            return gene;
        }

        return new GenomeGene(gene.Key, new NormalizedGeneValue(newValue), newDominance, newIsActive);
    }

    private static GenomeGene ResolveNeighbour(IReadOnlyList<GenomeGene> genes, GenomeGeneKey key)
    {
        for (int index = 0; index < genes.Count; index++)
        {
            if (genes[index].Key != key)
            {
                continue;
            }

            if (genes.Count == 1)
            {
                return genes[index];
            }

            int neighbourIndex = index == genes.Count - 1 ? index - 1 : index + 1;
            return genes[neighbourIndex];
        }

        return genes[0];
    }

    private static GeneDominance ResolveStructuralDominance(GeneDominance left, GeneDominance right)
    {
        if (left == right)
        {
            return left;
        }

        if (left == GeneDominance.Blended || right == GeneDominance.Blended)
        {
            return GeneDominance.Blended;
        }

        return GeneDominance.CoDominant;
    }

    private int ComputeSignedOffset(GenomeGeneKey geneKey, WorldSeed worldSeed, GenomeGroupType groupType, MutationCategory category, int amplitude)
    {
        int raw = ComputeScore(GenomeId.FromSequence(new EntitySequence((ulong)(int)geneKey + 1)), worldSeed, groupType, geneKey, category, salt: 1);
        int normalized = raw % ((amplitude * 2) + 1);
        return normalized - amplitude;
    }

    private static int ComputeScore(GenomeId genomeId, WorldSeed worldSeed, GenomeGroupType groupType, GenomeGeneKey geneKey, MutationCategory category, int salt)
    {
        long seed = worldSeed.Value;
        long value = seed;
        value = (value * 31) + (long)genomeId.Value;
        value = (value * 31) + (int)groupType;
        value = (value * 31) + (int)geneKey;
        value = (value * 31) + (int)category;
        value = (value * 31) + salt;
        value ^= value >> 13;
        value ^= value << 7;
        value ^= value >> 17;
        return (int)(Math.Abs(value) % 1000);
    }
}

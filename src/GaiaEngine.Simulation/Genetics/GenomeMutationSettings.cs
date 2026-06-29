using System;
using GaiaEngine.Domain.Genetics;

namespace GaiaEngine.Simulation.Genetics;

/// <summary>
/// Stores the immutable deterministic settings used by the genome mutation subsystem.
/// </summary>
public sealed record GenomeMutationSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenomeMutationSettings"/> class.
    /// </summary>
    public GenomeMutationSettings(
        int globalMutationChance,
        int mutationStrength,
        int maxMutationsPerGenome,
        int parameterMutationWeight,
        int dominanceMutationWeight,
        int activationMutationWeight,
        int structuralMutationWeight,
        int morphologyGroupWeight,
        int physiologyGroupWeight,
        int reproductionGroupWeight,
        int sensesGroupWeight,
        int adaptationGroupWeight,
        int appearanceGroupWeight,
        int behaviourBiasGroupWeight,
        int mutationVersion)
    {
        GlobalMutationChance = ValidateNormalized(globalMutationChance, nameof(globalMutationChance));
        MutationStrength = ValidateNormalized(mutationStrength, nameof(mutationStrength));
        ParameterMutationWeight = ValidateNormalized(parameterMutationWeight, nameof(parameterMutationWeight));
        DominanceMutationWeight = ValidateNormalized(dominanceMutationWeight, nameof(dominanceMutationWeight));
        ActivationMutationWeight = ValidateNormalized(activationMutationWeight, nameof(activationMutationWeight));
        StructuralMutationWeight = ValidateNormalized(structuralMutationWeight, nameof(structuralMutationWeight));
        MorphologyGroupWeight = ValidateNormalized(morphologyGroupWeight, nameof(morphologyGroupWeight));
        PhysiologyGroupWeight = ValidateNormalized(physiologyGroupWeight, nameof(physiologyGroupWeight));
        ReproductionGroupWeight = ValidateNormalized(reproductionGroupWeight, nameof(reproductionGroupWeight));
        SensesGroupWeight = ValidateNormalized(sensesGroupWeight, nameof(sensesGroupWeight));
        AdaptationGroupWeight = ValidateNormalized(adaptationGroupWeight, nameof(adaptationGroupWeight));
        AppearanceGroupWeight = ValidateNormalized(appearanceGroupWeight, nameof(appearanceGroupWeight));
        BehaviourBiasGroupWeight = ValidateNormalized(behaviourBiasGroupWeight, nameof(behaviourBiasGroupWeight));
        if (maxMutationsPerGenome < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maxMutationsPerGenome), "The maximum mutations per genome must be zero or greater.");
        }

        if (mutationVersion < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(mutationVersion), "The mutation version must be zero or greater.");
        }

        MaxMutationsPerGenome = maxMutationsPerGenome;
        MutationVersion = mutationVersion;
    }

    /// <summary>
    /// Gets the global normalized mutation chance applied before category and group weights.
    /// </summary>
    public int GlobalMutationChance { get; }

    /// <summary>
    /// Gets the normalized mutation strength that controls parameter and structural amplitude.
    /// </summary>
    public int MutationStrength { get; }

    /// <summary>
    /// Gets the maximum number of mutations allowed in one generated genome.
    /// </summary>
    public int MaxMutationsPerGenome { get; }

    /// <summary>
    /// Gets the normalized weight assigned to parameter mutations.
    /// </summary>
    public int ParameterMutationWeight { get; }

    /// <summary>
    /// Gets the normalized weight assigned to dominance mutations.
    /// </summary>
    public int DominanceMutationWeight { get; }

    /// <summary>
    /// Gets the normalized weight assigned to activation mutations.
    /// </summary>
    public int ActivationMutationWeight { get; }

    /// <summary>
    /// Gets the normalized weight assigned to structural mutations.
    /// </summary>
    public int StructuralMutationWeight { get; }

    /// <summary>
    /// Gets the normalized weight assigned to morphology genes.
    /// </summary>
    public int MorphologyGroupWeight { get; }

    /// <summary>
    /// Gets the normalized weight assigned to physiology genes.
    /// </summary>
    public int PhysiologyGroupWeight { get; }

    /// <summary>
    /// Gets the normalized weight assigned to reproduction genes.
    /// </summary>
    public int ReproductionGroupWeight { get; }

    /// <summary>
    /// Gets the normalized weight assigned to senses genes.
    /// </summary>
    public int SensesGroupWeight { get; }

    /// <summary>
    /// Gets the normalized weight assigned to adaptation genes.
    /// </summary>
    public int AdaptationGroupWeight { get; }

    /// <summary>
    /// Gets the normalized weight assigned to appearance genes.
    /// </summary>
    public int AppearanceGroupWeight { get; }

    /// <summary>
    /// Gets the normalized weight assigned to behaviour bias genes.
    /// </summary>
    public int BehaviourBiasGroupWeight { get; }

    /// <summary>
    /// Gets the mutation schema version written into produced genomes.
    /// </summary>
    public int MutationVersion { get; }

    /// <summary>
    /// Resolves the normalized mutation weight for the specified genome group.
    /// </summary>
    /// <param name="groupType">The group type to evaluate.</param>
    /// <returns>The configured normalized group weight.</returns>
    public int GetGroupWeight(GenomeGroupType groupType)
    {
        return groupType switch
        {
            GenomeGroupType.Morphology => MorphologyGroupWeight,
            GenomeGroupType.Physiology => PhysiologyGroupWeight,
            GenomeGroupType.Reproduction => ReproductionGroupWeight,
            GenomeGroupType.Senses => SensesGroupWeight,
            GenomeGroupType.Adaptation => AdaptationGroupWeight,
            GenomeGroupType.Appearance => AppearanceGroupWeight,
            GenomeGroupType.BehaviourBias => BehaviourBiasGroupWeight,
            _ => 0,
        };
    }

    /// <summary>
    /// Resolves the normalized mutation weight for the specified mutation category.
    /// </summary>
    /// <param name="category">The mutation category to evaluate.</param>
    /// <returns>The configured normalized category weight.</returns>
    public int GetCategoryWeight(MutationCategory category)
    {
        return category switch
        {
            MutationCategory.Parameter => ParameterMutationWeight,
            MutationCategory.Dominance => DominanceMutationWeight,
            MutationCategory.Activation => ActivationMutationWeight,
            MutationCategory.Structural => StructuralMutationWeight,
            _ => 0,
        };
    }

    private static int ValidateNormalized(int value, string parameterName)
    {
        if (value < 0 || value > 1000)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Mutation setting values must be between 0 and 1000.");
        }

        return value;
    }
}

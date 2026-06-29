using System;

namespace GaiaEngine.App.Configuration;

/// <summary>
/// Represents the immutable startup configuration of the genetics mutation subsystem.
/// </summary>
public sealed record MutationConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MutationConfiguration"/> class.
    /// </summary>
    /// <param name="globalMutationChance">The global mutation chance scaled to the inclusive range [0, 1000].</param>
    /// <param name="mutationStrength">The mutation strength scaled to the inclusive range [0, 1000].</param>
    /// <param name="maxMutationsPerGenome">The maximum number of mutations allowed for one genome.</param>
    /// <param name="parameterMutationWeight">The parameter mutation weight scaled to the inclusive range [0, 1000].</param>
    /// <param name="dominanceMutationWeight">The dominance mutation weight scaled to the inclusive range [0, 1000].</param>
    /// <param name="activationMutationWeight">The activation mutation weight scaled to the inclusive range [0, 1000].</param>
    /// <param name="structuralMutationWeight">The structural mutation weight scaled to the inclusive range [0, 1000].</param>
    /// <param name="morphologyGroupWeight">The morphology group weight scaled to the inclusive range [0, 1000].</param>
    /// <param name="physiologyGroupWeight">The physiology group weight scaled to the inclusive range [0, 1000].</param>
    /// <param name="reproductionGroupWeight">The reproduction group weight scaled to the inclusive range [0, 1000].</param>
    /// <param name="sensesGroupWeight">The senses group weight scaled to the inclusive range [0, 1000].</param>
    /// <param name="adaptationGroupWeight">The adaptation group weight scaled to the inclusive range [0, 1000].</param>
    /// <param name="appearanceGroupWeight">The appearance group weight scaled to the inclusive range [0, 1000].</param>
    /// <param name="behaviourBiasGroupWeight">The behaviour bias group weight scaled to the inclusive range [0, 1000].</param>
    /// <param name="mutationVersion">The deterministic mutation schema version.</param>
    public MutationConfiguration(
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
    /// Gets the shared default mutation configuration used when legacy configuration files omit the mutation section.
    /// </summary>
    public static MutationConfiguration Default { get; } = new(
        globalMutationChance: 90,
        mutationStrength: 120,
        maxMutationsPerGenome: 2,
        parameterMutationWeight: 700,
        dominanceMutationWeight: 180,
        activationMutationWeight: 80,
        structuralMutationWeight: 40,
        morphologyGroupWeight: 550,
        physiologyGroupWeight: 500,
        reproductionGroupWeight: 350,
        sensesGroupWeight: 400,
        adaptationGroupWeight: 450,
        appearanceGroupWeight: 300,
        behaviourBiasGroupWeight: 250,
        mutationVersion: 1);

    /// <summary>
    /// Gets the global mutation chance scaled to the inclusive range [0, 1000].
    /// </summary>
    public int GlobalMutationChance { get; }

    /// <summary>
    /// Gets the mutation strength scaled to the inclusive range [0, 1000].
    /// </summary>
    public int MutationStrength { get; }

    /// <summary>
    /// Gets the maximum number of mutations allowed for one genome.
    /// </summary>
    public int MaxMutationsPerGenome { get; }

    /// <summary>
    /// Gets the parameter mutation weight.
    /// </summary>
    public int ParameterMutationWeight { get; }

    /// <summary>
    /// Gets the dominance mutation weight.
    /// </summary>
    public int DominanceMutationWeight { get; }

    /// <summary>
    /// Gets the activation mutation weight.
    /// </summary>
    public int ActivationMutationWeight { get; }

    /// <summary>
    /// Gets the structural mutation weight.
    /// </summary>
    public int StructuralMutationWeight { get; }

    /// <summary>
    /// Gets the morphology group weight.
    /// </summary>
    public int MorphologyGroupWeight { get; }

    /// <summary>
    /// Gets the physiology group weight.
    /// </summary>
    public int PhysiologyGroupWeight { get; }

    /// <summary>
    /// Gets the reproduction group weight.
    /// </summary>
    public int ReproductionGroupWeight { get; }

    /// <summary>
    /// Gets the senses group weight.
    /// </summary>
    public int SensesGroupWeight { get; }

    /// <summary>
    /// Gets the adaptation group weight.
    /// </summary>
    public int AdaptationGroupWeight { get; }

    /// <summary>
    /// Gets the appearance group weight.
    /// </summary>
    public int AppearanceGroupWeight { get; }

    /// <summary>
    /// Gets the behaviour bias group weight.
    /// </summary>
    public int BehaviourBiasGroupWeight { get; }

    /// <summary>
    /// Gets the deterministic mutation schema version.
    /// </summary>
    public int MutationVersion { get; }

    private static int ValidateNormalized(int value, string parameterName)
    {
        if (value < 0 || value > 1000)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Mutation configuration values must be between 0 and 1000.");
        }

        return value;
    }
}

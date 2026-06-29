namespace GaiaEngine.App.Configuration;

/// <summary>
/// Represents the raw JSON document used to deserialize mutation configuration.
/// </summary>
internal sealed class MutationConfigurationDocument
{
    /// <summary>
    /// Gets or sets the global mutation chance scaled to [0, 1000].
    /// </summary>
    public int GlobalMutationChance { get; set; } = MutationConfiguration.Default.GlobalMutationChance;

    /// <summary>
    /// Gets or sets the mutation strength scaled to [0, 1000].
    /// </summary>
    public int MutationStrength { get; set; } = MutationConfiguration.Default.MutationStrength;

    /// <summary>
    /// Gets or sets the maximum mutations allowed for one genome.
    /// </summary>
    public int MaxMutationsPerGenome { get; set; } = MutationConfiguration.Default.MaxMutationsPerGenome;

    /// <summary>
    /// Gets or sets the parameter mutation weight.
    /// </summary>
    public int ParameterMutationWeight { get; set; } = MutationConfiguration.Default.ParameterMutationWeight;

    /// <summary>
    /// Gets or sets the dominance mutation weight.
    /// </summary>
    public int DominanceMutationWeight { get; set; } = MutationConfiguration.Default.DominanceMutationWeight;

    /// <summary>
    /// Gets or sets the activation mutation weight.
    /// </summary>
    public int ActivationMutationWeight { get; set; } = MutationConfiguration.Default.ActivationMutationWeight;

    /// <summary>
    /// Gets or sets the structural mutation weight.
    /// </summary>
    public int StructuralMutationWeight { get; set; } = MutationConfiguration.Default.StructuralMutationWeight;

    /// <summary>
    /// Gets or sets the morphology group weight.
    /// </summary>
    public int MorphologyGroupWeight { get; set; } = MutationConfiguration.Default.MorphologyGroupWeight;

    /// <summary>
    /// Gets or sets the physiology group weight.
    /// </summary>
    public int PhysiologyGroupWeight { get; set; } = MutationConfiguration.Default.PhysiologyGroupWeight;

    /// <summary>
    /// Gets or sets the reproduction group weight.
    /// </summary>
    public int ReproductionGroupWeight { get; set; } = MutationConfiguration.Default.ReproductionGroupWeight;

    /// <summary>
    /// Gets or sets the senses group weight.
    /// </summary>
    public int SensesGroupWeight { get; set; } = MutationConfiguration.Default.SensesGroupWeight;

    /// <summary>
    /// Gets or sets the adaptation group weight.
    /// </summary>
    public int AdaptationGroupWeight { get; set; } = MutationConfiguration.Default.AdaptationGroupWeight;

    /// <summary>
    /// Gets or sets the appearance group weight.
    /// </summary>
    public int AppearanceGroupWeight { get; set; } = MutationConfiguration.Default.AppearanceGroupWeight;

    /// <summary>
    /// Gets or sets the behaviour bias group weight.
    /// </summary>
    public int BehaviourBiasGroupWeight { get; set; } = MutationConfiguration.Default.BehaviourBiasGroupWeight;

    /// <summary>
    /// Gets or sets the deterministic mutation schema version.
    /// </summary>
    public int MutationVersion { get; set; } = MutationConfiguration.Default.MutationVersion;
}

using System;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Represents one immutable deterministic genome aggregate.
/// </summary>
public sealed class Genome
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Genome"/> class.
    /// </summary>
    /// <param name="identity">The genome identity metadata.</param>
    /// <param name="morphology">The morphology group.</param>
    /// <param name="physiology">The physiology group.</param>
    /// <param name="reproduction">The reproduction group.</param>
    /// <param name="senses">The senses group.</param>
    /// <param name="adaptation">The adaptation group.</param>
    /// <param name="appearance">The appearance group.</param>
    /// <param name="behaviourBias">The behaviour bias group.</param>
    /// <param name="mutationVersion">The mutation schema version used to produce this genome.</param>
    /// <param name="mutationHistory">The deterministic mutation history.</param>
    /// <exception cref="ArgumentNullException">Thrown when any supplied group is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when a supplied group has the wrong group type.</exception>
    public Genome(
        GenomeIdentity identity,
        GenomeGeneGroup morphology,
        GenomeGeneGroup physiology,
        GenomeGeneGroup reproduction,
        GenomeGeneGroup senses,
        GenomeGeneGroup adaptation,
        GenomeGeneGroup appearance,
        GenomeGeneGroup behaviourBias,
        int mutationVersion,
        GenomeMutationHistory mutationHistory)
    {
        Identity = identity ?? throw new ArgumentNullException(nameof(identity));
        Morphology = ValidateGroup(morphology, GenomeGroupType.Morphology, nameof(morphology));
        Physiology = ValidateGroup(physiology, GenomeGroupType.Physiology, nameof(physiology));
        Reproduction = ValidateGroup(reproduction, GenomeGroupType.Reproduction, nameof(reproduction));
        Senses = ValidateGroup(senses, GenomeGroupType.Senses, nameof(senses));
        Adaptation = ValidateGroup(adaptation, GenomeGroupType.Adaptation, nameof(adaptation));
        Appearance = ValidateGroup(appearance, GenomeGroupType.Appearance, nameof(appearance));
        BehaviourBias = ValidateGroup(behaviourBias, GenomeGroupType.BehaviourBias, nameof(behaviourBias));
        if (mutationVersion < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(mutationVersion), "The mutation version must be zero or greater.");
        }

        MutationVersion = mutationVersion;
        MutationHistory = mutationHistory ?? throw new ArgumentNullException(nameof(mutationHistory));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Genome"/> class.
    /// </summary>
    /// <param name="identity">The genome identity metadata.</param>
    /// <param name="morphology">The morphology group.</param>
    /// <param name="physiology">The physiology group.</param>
    /// <param name="reproduction">The reproduction group.</param>
    /// <param name="senses">The senses group.</param>
    /// <param name="adaptation">The adaptation group.</param>
    /// <param name="appearance">The appearance group.</param>
    /// <param name="behaviourBias">The behaviour bias group.</param>
    public Genome(
        GenomeIdentity identity,
        GenomeGeneGroup morphology,
        GenomeGeneGroup physiology,
        GenomeGeneGroup reproduction,
        GenomeGeneGroup senses,
        GenomeGeneGroup adaptation,
        GenomeGeneGroup appearance,
        GenomeGeneGroup behaviourBias)
        : this(identity, morphology, physiology, reproduction, senses, adaptation, appearance, behaviourBias, mutationVersion: 0, GenomeMutationHistory.Empty)
    {
    }

    /// <summary>
    /// Gets the immutable genome identifier.
    /// </summary>
    public GenomeId Id => Identity.GenomeId;

    /// <summary>
    /// Gets the immutable genome identity metadata.
    /// </summary>
    public GenomeIdentity Identity { get; }

    /// <summary>
    /// Gets the morphology group.
    /// </summary>
    public GenomeGeneGroup Morphology { get; }

    /// <summary>
    /// Gets the physiology group.
    /// </summary>
    public GenomeGeneGroup Physiology { get; }

    /// <summary>
    /// Gets the reproduction group.
    /// </summary>
    public GenomeGeneGroup Reproduction { get; }

    /// <summary>
    /// Gets the senses group.
    /// </summary>
    public GenomeGeneGroup Senses { get; }

    /// <summary>
    /// Gets the adaptation group.
    /// </summary>
    public GenomeGeneGroup Adaptation { get; }

    /// <summary>
    /// Gets the appearance group.
    /// </summary>
    public GenomeGeneGroup Appearance { get; }

    /// <summary>
    /// Gets the behaviour bias group.
    /// </summary>
    public GenomeGeneGroup BehaviourBias { get; }

    /// <summary>
    /// Gets the mutation schema version used to produce this genome.
    /// </summary>
    public int MutationVersion { get; }

    /// <summary>
    /// Gets the deterministic mutation history.
    /// </summary>
    public GenomeMutationHistory MutationHistory { get; }

    private static GenomeGeneGroup ValidateGroup(GenomeGeneGroup group, GenomeGroupType expectedType, string parameterName)
    {
        ArgumentNullException.ThrowIfNull(group);
        if (group.GroupType != expectedType)
        {
            throw new ArgumentException($"The supplied group must be '{expectedType}'.", parameterName);
        }

        return group;
    }
}

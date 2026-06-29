using System.Collections.Generic;

namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized payload of one genome aggregate.
/// </summary>
internal sealed class GenomeDocument
{
    /// <summary>
    /// Gets or sets the mutation schema version.
    /// </summary>
    public int MutationVersion { get; set; }

    /// <summary>
    /// Gets or sets the serialized mutation history.
    /// </summary>
    public List<GenomeMutationRecordDocument> MutationHistory { get; set; } = new();

    /// <summary>
    /// Gets or sets the serialized genome identity.
    /// </summary>
    public GenomeIdentityDocument? Identity { get; set; }

    /// <summary>
    /// Gets or sets the morphology gene group.
    /// </summary>
    public GenomeGeneGroupDocument? Morphology { get; set; }

    /// <summary>
    /// Gets or sets the physiology gene group.
    /// </summary>
    public GenomeGeneGroupDocument? Physiology { get; set; }

    /// <summary>
    /// Gets or sets the reproduction gene group.
    /// </summary>
    public GenomeGeneGroupDocument? Reproduction { get; set; }

    /// <summary>
    /// Gets or sets the senses gene group.
    /// </summary>
    public GenomeGeneGroupDocument? Senses { get; set; }

    /// <summary>
    /// Gets or sets the adaptation gene group.
    /// </summary>
    public GenomeGeneGroupDocument? Adaptation { get; set; }

    /// <summary>
    /// Gets or sets the appearance gene group.
    /// </summary>
    public GenomeGeneGroupDocument? Appearance { get; set; }

    /// <summary>
    /// Gets or sets the behaviour bias gene group.
    /// </summary>
    public GenomeGeneGroupDocument? BehaviourBias { get; set; }
}

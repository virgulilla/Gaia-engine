namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Identifies one functional genome group.
/// </summary>
public enum GenomeGroupType
{
    /// <summary>
    /// Morphological body construction values.
    /// </summary>
    Morphology = 0,

    /// <summary>
    /// Physiological operating values.
    /// </summary>
    Physiology = 1,

    /// <summary>
    /// Reproductive values.
    /// </summary>
    Reproduction = 2,

    /// <summary>
    /// Sensory values.
    /// </summary>
    Senses = 3,

    /// <summary>
    /// Environmental adaptation values.
    /// </summary>
    Adaptation = 4,

    /// <summary>
    /// Presentation-oriented appearance values.
    /// </summary>
    Appearance = 5,

    /// <summary>
    /// Behaviour tendency bias values.
    /// </summary>
    BehaviourBias = 6,
}

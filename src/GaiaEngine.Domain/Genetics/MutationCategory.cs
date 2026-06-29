namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Identifies one supported genome mutation category.
/// </summary>
public enum MutationCategory
{
    /// <summary>
    /// Modifies the normalized gene value.
    /// </summary>
    Parameter = 0,

    /// <summary>
    /// Modifies the gene dominance mode.
    /// </summary>
    Dominance = 1,

    /// <summary>
    /// Modifies the gene activation state.
    /// </summary>
    Activation = 2,

    /// <summary>
    /// Introduces variation derived from neighbouring genes in the same group.
    /// </summary>
    Structural = 3,
}

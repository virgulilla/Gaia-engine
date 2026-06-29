namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Represents the deterministic dominance mode used by one inherited gene.
/// </summary>
public enum GeneDominance
{
    /// <summary>
    /// Indicates that the gene is dominant.
    /// </summary>
    Dominant = 0,

    /// <summary>
    /// Indicates that the gene is recessive.
    /// </summary>
    Recessive = 1,

    /// <summary>
    /// Indicates that the gene is co-dominant.
    /// </summary>
    CoDominant = 2,

    /// <summary>
    /// Indicates that the gene is blended.
    /// </summary>
    Blended = 3,
}

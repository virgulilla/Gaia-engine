namespace GaiaEngine.Domain.World;

/// <summary>
/// Defines the supported deterministic resource categories stored per chunk.
/// </summary>
public enum ResourceCategory
{
    /// <summary>
    /// Identifies renewable biological resources.
    /// </summary>
    Renewable,

    /// <summary>
    /// Identifies non-renewable geological resources.
    /// </summary>
    NonRenewable,

    /// <summary>
    /// Identifies calculated environmental resources.
    /// </summary>
    Environmental,
}

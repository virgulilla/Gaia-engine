namespace GaiaEngine.Domain.World;

/// <summary>
/// Defines the runtime state of a chunk.
/// </summary>
public enum ChunkState
{
    /// <summary>
    /// Indicates that the chunk executes all simulation updates.
    /// </summary>
    Active,

    /// <summary>
    /// Indicates that the chunk executes reduced-frequency simulation updates.
    /// </summary>
    Dormant,

    /// <summary>
    /// Indicates that the chunk executes only required simulation updates.
    /// </summary>
    Sleeping,

    /// <summary>
    /// Indicates a future unloaded chunk state.
    /// </summary>
    Unloaded,
}

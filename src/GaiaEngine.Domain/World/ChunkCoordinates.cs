namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the immutable coordinates of a chunk inside a world grid.
/// </summary>
public readonly record struct ChunkCoordinates(int X, int Y);

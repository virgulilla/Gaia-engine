using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents immutable world dimensions.
/// </summary>
public sealed record WorldDimensions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorldDimensions"/> class.
    /// </summary>
    /// <param name="width">The immutable world width.</param>
    /// <param name="height">The immutable world height.</param>
    /// <param name="chunkSize">The immutable chunk size shared by the world.</param>
    /// <param name="chunkCount">The immutable chunk count.</param>
    /// <param name="maximumElevation">The immutable maximum elevation.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when numeric values are out of range.</exception>
    public WorldDimensions(int width, int height, int chunkSize, int chunkCount, int maximumElevation)
    {
        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "The world width must be greater than zero.");
        }

        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height), "The world height must be greater than zero.");
        }

        if (chunkSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(chunkSize), "The chunk size must be greater than zero.");
        }

        if (chunkCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(chunkCount), "The chunk count must be greater than zero.");
        }

        if (maximumElevation < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumElevation), "The maximum elevation must be zero or greater.");
        }

        Width = width;
        Height = height;
        ChunkSize = chunkSize;
        ChunkCount = chunkCount;
        MaximumElevation = maximumElevation;
    }

    /// <summary>
    /// Gets the immutable world width.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the immutable world height.
    /// </summary>
    public int Height { get; }

    /// <summary>
    /// Gets the immutable chunk size shared by the world.
    /// </summary>
    public int ChunkSize { get; }

    /// <summary>
    /// Gets the immutable chunk count.
    /// </summary>
    public int ChunkCount { get; }

    /// <summary>
    /// Gets the immutable maximum elevation.
    /// </summary>
    public int MaximumElevation { get; }
}

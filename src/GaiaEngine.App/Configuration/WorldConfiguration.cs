using System;
using GaiaEngine.Domain.World;

namespace GaiaEngine.App.Configuration;

/// <summary>
/// Represents the immutable startup configuration of the initial world bootstrap state.
/// </summary>
public sealed record WorldConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorldConfiguration"/> class.
    /// </summary>
    /// <param name="worldName">The deterministic world name.</param>
    /// <param name="worldSeed">The deterministic world seed.</param>
    /// <param name="chunkColumns">The number of chunk columns in the initial world grid.</param>
    /// <param name="chunkRows">The number of chunk rows in the initial world grid.</param>
    /// <param name="chunkSize">The chunk size.</param>
    /// <param name="maximumElevation">The maximum elevation value.</param>
    /// <param name="defaultClimateZone">The default initial climate zone assigned to new chunks.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="worldName"/> is empty.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when any required numeric value is not greater than zero.
    /// </exception>
    public WorldConfiguration(
        string worldName,
        long worldSeed,
        int chunkColumns,
        int chunkRows,
        int chunkSize,
        int maximumElevation,
        ClimateZone defaultClimateZone)
    {
        if (string.IsNullOrWhiteSpace(worldName))
        {
            throw new ArgumentException("The world name must contain a value.", nameof(worldName));
        }

        if (chunkColumns <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(chunkColumns), "The chunk column count must be greater than zero.");
        }

        if (chunkRows <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(chunkRows), "The chunk row count must be greater than zero.");
        }

        if (chunkSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(chunkSize), "The chunk size must be greater than zero.");
        }

        if (maximumElevation <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumElevation), "The maximum elevation must be greater than zero.");
        }

        WorldName = worldName;
        WorldSeed = worldSeed;
        ChunkColumns = chunkColumns;
        ChunkRows = chunkRows;
        ChunkSize = chunkSize;
        MaximumElevation = maximumElevation;
        DefaultClimateZone = defaultClimateZone;
    }

    /// <summary>
    /// Gets the deterministic world name.
    /// </summary>
    public string WorldName { get; }

    /// <summary>
    /// Gets the deterministic world seed.
    /// </summary>
    public long WorldSeed { get; }

    /// <summary>
    /// Gets the number of chunk columns in the initial world grid.
    /// </summary>
    public int ChunkColumns { get; }

    /// <summary>
    /// Gets the number of chunk rows in the initial world grid.
    /// </summary>
    public int ChunkRows { get; }

    /// <summary>
    /// Gets the chunk size.
    /// </summary>
    public int ChunkSize { get; }

    /// <summary>
    /// Gets the maximum elevation value.
    /// </summary>
    public int MaximumElevation { get; }

    /// <summary>
    /// Gets the default initial climate zone assigned to new chunks.
    /// </summary>
    public ClimateZone DefaultClimateZone { get; }
}

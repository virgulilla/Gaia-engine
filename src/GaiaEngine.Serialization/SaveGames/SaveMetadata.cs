using System;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;

namespace GaiaEngine.Serialization.SaveGames;

/// <summary>
/// Represents the persistent metadata stored alongside a serialized save document.
/// </summary>
public sealed record SaveMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SaveMetadata"/> class.
    /// </summary>
    /// <param name="saveName">The human-readable save name.</param>
    /// <param name="creationDate">The save creation date string.</param>
    /// <param name="lastModified">The save last-modified date string.</param>
    /// <param name="worldSeed">The deterministic world seed.</param>
    /// <param name="engineVersion">The engine version used by the save.</param>
    /// <param name="saveVersion">The save format version string.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when any required string value is null, empty, or whitespace.
    /// </exception>
    public SaveMetadata(
        string saveName,
        string creationDate,
        string lastModified,
        WorldSeed worldSeed,
        EngineVersion engineVersion,
        string saveVersion)
    {
        if (string.IsNullOrWhiteSpace(saveName))
        {
            throw new ArgumentException("The save name must contain a value.", nameof(saveName));
        }

        if (string.IsNullOrWhiteSpace(creationDate))
        {
            throw new ArgumentException("The creation date must contain a value.", nameof(creationDate));
        }

        if (string.IsNullOrWhiteSpace(lastModified))
        {
            throw new ArgumentException("The last-modified value must contain a value.", nameof(lastModified));
        }

        if (string.IsNullOrWhiteSpace(saveVersion))
        {
            throw new ArgumentException("The save version must contain a value.", nameof(saveVersion));
        }

        SaveName = saveName;
        CreationDate = creationDate;
        LastModified = lastModified;
        WorldSeed = worldSeed;
        EngineVersion = engineVersion;
        SaveVersion = saveVersion;
    }

    /// <summary>
    /// Gets the human-readable save name.
    /// </summary>
    public string SaveName { get; }

    /// <summary>
    /// Gets the save creation date string.
    /// </summary>
    public string CreationDate { get; }

    /// <summary>
    /// Gets the save last-modified date string.
    /// </summary>
    public string LastModified { get; }

    /// <summary>
    /// Gets the deterministic world seed.
    /// </summary>
    public WorldSeed WorldSeed { get; }

    /// <summary>
    /// Gets the engine version used by the save.
    /// </summary>
    public EngineVersion EngineVersion { get; }

    /// <summary>
    /// Gets the save format version string.
    /// </summary>
    public string SaveVersion { get; }
}

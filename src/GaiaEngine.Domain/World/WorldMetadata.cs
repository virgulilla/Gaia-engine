using System;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents immutable metadata associated with a world aggregate.
/// </summary>
public sealed record WorldMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorldMetadata"/> class.
    /// </summary>
    /// <param name="worldId">The immutable world identifier.</param>
    /// <param name="worldName">The immutable world name.</param>
    /// <param name="seed">The deterministic world seed.</param>
    /// <param name="creationDate">The serialized world creation date.</param>
    /// <param name="engineVersion">The engine version associated with the world.</param>
    /// <param name="configurationVersion">The configuration version associated with the world.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="worldName"/> or <paramref name="creationDate"/> is empty.</exception>
    public WorldMetadata(
        WorldId worldId,
        string worldName,
        WorldSeed seed,
        string creationDate,
        EngineVersion engineVersion,
        ConfigurationVersion configurationVersion)
    {
        if (string.IsNullOrWhiteSpace(worldName))
        {
            throw new ArgumentException("The world name must contain a value.", nameof(worldName));
        }

        if (string.IsNullOrWhiteSpace(creationDate))
        {
            throw new ArgumentException("The creation date must contain a value.", nameof(creationDate));
        }

        WorldId = worldId;
        WorldName = worldName;
        Seed = seed;
        CreationDate = creationDate;
        EngineVersion = engineVersion;
        ConfigurationVersion = configurationVersion;
    }

    /// <summary>
    /// Gets the immutable world identifier.
    /// </summary>
    public WorldId WorldId { get; }

    /// <summary>
    /// Gets the immutable world name.
    /// </summary>
    public string WorldName { get; }

    /// <summary>
    /// Gets the deterministic world seed.
    /// </summary>
    public WorldSeed Seed { get; }

    /// <summary>
    /// Gets the serialized world creation date.
    /// </summary>
    public string CreationDate { get; }

    /// <summary>
    /// Gets the engine version associated with the world.
    /// </summary>
    public EngineVersion EngineVersion { get; }

    /// <summary>
    /// Gets the configuration version associated with the world.
    /// </summary>
    public ConfigurationVersion ConfigurationVersion { get; }
}

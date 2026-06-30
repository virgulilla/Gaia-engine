using System;
using GaiaEngine.Domain.AI;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Simulation.Actions;

namespace GaiaEngine.Serialization.SaveGames;

/// <summary>
/// Represents the persistent save document used for the current world serialization slice.
/// </summary>
public sealed record WorldSaveGame
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorldSaveGame"/> class.
    /// </summary>
    /// <param name="metadata">The save metadata.</param>
    /// <param name="world">The serialized world aggregate.</param>
    /// <param name="organisms">The serialized organism aggregate.</param>
    /// <param name="genomes">The serialized genome aggregate.</param>
    /// <param name="actionRequests">The serialized common action request aggregate.</param>
    /// <param name="configurationVersion">The serialized configuration version string.</param>
    /// <param name="version">The embedded version information.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="metadata"/>, <paramref name="world"/>, or <paramref name="version"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="configurationVersion"/> is empty.</exception>
    public WorldSaveGame(
        SaveMetadata metadata,
        World world,
        OrganismCollection organisms,
        GenomeCollection genomes,
        SimulationActionRequestCollection actionRequests,
        ConfigurationVersion configurationVersion,
        SaveVersionInfo version)
        : this(metadata, world, organisms, genomes, SpeciesCollection.Empty, MemoryCollection.Empty, actionRequests, configurationVersion, version)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldSaveGame"/> class.
    /// </summary>
    /// <param name="metadata">The save metadata.</param>
    /// <param name="world">The serialized world aggregate.</param>
    /// <param name="organisms">The serialized organism aggregate.</param>
    /// <param name="genomes">The serialized genome aggregate.</param>
    /// <param name="species">The serialized species aggregate.</param>
    /// <param name="memories">The serialized memory aggregate.</param>
    /// <param name="actionRequests">The serialized common action request aggregate.</param>
    /// <param name="configurationVersion">The serialized configuration version string.</param>
    /// <param name="version">The embedded version information.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="metadata"/>, <paramref name="world"/>, or <paramref name="version"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">Thrown when <paramref name="configurationVersion"/> is empty.</exception>
    public WorldSaveGame(
        SaveMetadata metadata,
        World world,
        OrganismCollection organisms,
        GenomeCollection genomes,
        SpeciesCollection species,
        MemoryCollection memories,
        SimulationActionRequestCollection actionRequests,
        ConfigurationVersion configurationVersion,
        SaveVersionInfo version)
    {
        Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
        World = world ?? throw new ArgumentNullException(nameof(world));
        Organisms = organisms ?? throw new ArgumentNullException(nameof(organisms));
        Genomes = genomes ?? throw new ArgumentNullException(nameof(genomes));
        Species = species ?? throw new ArgumentNullException(nameof(species));
        Memories = memories ?? throw new ArgumentNullException(nameof(memories));
        ActionRequests = actionRequests ?? throw new ArgumentNullException(nameof(actionRequests));
        ConfigurationVersion = configurationVersion;
        Version = version ?? throw new ArgumentNullException(nameof(version));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldSaveGame"/> class.
    /// </summary>
    /// <param name="metadata">The save metadata.</param>
    /// <param name="world">The serialized world aggregate.</param>
    /// <param name="configurationVersion">The serialized configuration version string.</param>
    /// <param name="version">The embedded version information.</param>
    public WorldSaveGame(
        SaveMetadata metadata,
        World world,
        ConfigurationVersion configurationVersion,
        SaveVersionInfo version)
        : this(metadata, world, OrganismCollection.Empty, GenomeCollection.Empty, SpeciesCollection.Empty, MemoryCollection.Empty, SimulationActionRequestCollection.Empty, configurationVersion, version)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldSaveGame"/> class.
    /// </summary>
    /// <param name="metadata">The save metadata.</param>
    /// <param name="world">The serialized world aggregate.</param>
    /// <param name="organisms">The serialized organism aggregate.</param>
    /// <param name="configurationVersion">The serialized configuration version string.</param>
    /// <param name="version">The embedded version information.</param>
    public WorldSaveGame(
        SaveMetadata metadata,
        World world,
        OrganismCollection organisms,
        ConfigurationVersion configurationVersion,
        SaveVersionInfo version)
        : this(metadata, world, organisms, GenomeCollection.Empty, SpeciesCollection.Empty, MemoryCollection.Empty, SimulationActionRequestCollection.Empty, configurationVersion, version)
    {
    }

    /// <summary>
    /// Gets the save metadata.
    /// </summary>
    public SaveMetadata Metadata { get; }

    /// <summary>
    /// Gets the serialized world aggregate.
    /// </summary>
    public World World { get; }

    /// <summary>
    /// Gets the serialized organism aggregate.
    /// </summary>
    public OrganismCollection Organisms { get; }

    /// <summary>
    /// Gets the serialized configuration version.
    /// </summary>
    public ConfigurationVersion ConfigurationVersion { get; }

    /// <summary>
    /// Gets the serialized genome aggregate.
    /// </summary>
    public GenomeCollection Genomes { get; }

    /// <summary>
    /// Gets the serialized species aggregate.
    /// </summary>
    public SpeciesCollection Species { get; }

    /// <summary>
    /// Gets the serialized memory aggregate.
    /// </summary>
    public MemoryCollection Memories { get; }

    /// <summary>
    /// Gets the serialized common action request aggregate.
    /// </summary>
    public SimulationActionRequestCollection ActionRequests { get; }

    /// <summary>
    /// Gets the embedded version information.
    /// </summary>
    public SaveVersionInfo Version { get; }
}

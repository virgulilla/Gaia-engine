using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.World.Queries;

/// <summary>
/// Provides deterministic spatial queries over world and organism state.
/// </summary>
public interface ISpatialQueryService
{
    /// <summary>
    /// Tries to resolve one chunk by coordinates.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="coordinates">The chunk coordinates.</param>
    /// <param name="chunk">The resolved chunk when present.</param>
    /// <returns><see langword="true"/> when the chunk exists; otherwise <see langword="false"/>.</returns>
    public bool TryGetChunk(GaiaEngine.Domain.World.World world, ChunkCoordinates coordinates, out Chunk? chunk);

    /// <summary>
    /// Returns the neighbouring chunks of the supplied origin coordinates in deterministic order.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="origin">The origin chunk coordinates.</param>
    /// <returns>The neighbouring chunks in deterministic order.</returns>
    public IReadOnlyList<Chunk> GetAdjacentChunks(GaiaEngine.Domain.World.World world, ChunkCoordinates origin);

    /// <summary>
    /// Returns the chunks inside one rectangular area in deterministic order.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="minimum">The inclusive minimum coordinates.</param>
    /// <param name="maximum">The inclusive maximum coordinates.</param>
    /// <returns>The chunks inside the supplied area in deterministic order.</returns>
    public IReadOnlyList<Chunk> GetChunksInArea(GaiaEngine.Domain.World.World world, ChunkCoordinates minimum, ChunkCoordinates maximum);

    /// <summary>
    /// Returns the organisms currently referenced by the supplied chunk in deterministic order.
    /// </summary>
    /// <param name="chunk">The chunk whose organisms are queried.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <returns>The organisms referenced by the supplied chunk in deterministic order.</returns>
    public IReadOnlyList<Organism> GetOrganismsInChunk(Chunk chunk, OrganismCollection organisms);

    /// <summary>
    /// Returns the resources stored by the supplied chunk in deterministic order.
    /// </summary>
    /// <param name="chunk">The chunk whose resources are queried.</param>
    /// <returns>The resources stored by the supplied chunk in deterministic order.</returns>
    public IReadOnlyList<ResourceState> GetResourcesInChunk(Chunk chunk);

    /// <summary>
    /// Returns the nearest chunk that contains the requested resource type.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="origin">The origin chunk coordinates.</param>
    /// <param name="resourceType">The resource type to search for.</param>
    /// <returns>The nearest valid chunk when found; otherwise <see langword="null"/>.</returns>
    public Chunk? FindNearestChunkWithResource(GaiaEngine.Domain.World.World world, ChunkCoordinates origin, ResourceType resourceType);
}

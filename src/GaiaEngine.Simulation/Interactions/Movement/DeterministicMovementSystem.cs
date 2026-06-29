using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.World.Queries;

namespace GaiaEngine.Simulation.Interactions.Movement;

/// <summary>
/// Executes deterministic organism movement requests between adjacent traversable chunks.
/// </summary>
public sealed class DeterministicMovementSystem : IMovementSystem
{
    private readonly ISpatialQueryService spatialQueryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicMovementSystem"/> class.
    /// </summary>
    /// <param name="spatialQueryService">The spatial query service used to resolve movement constraints.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="spatialQueryService"/> is <see langword="null"/>.</exception>
    public DeterministicMovementSystem(ISpatialQueryService spatialQueryService)
    {
        this.spatialQueryService = spatialQueryService ?? throw new ArgumentNullException(nameof(spatialQueryService));
    }

    /// <summary>
    /// Executes deterministic movement requests against the supplied world and organism state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="requests">The current movement requests.</param>
    /// <returns>The deterministic movement execution result.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/>, <paramref name="organisms"/>, or <paramref name="requests"/> is <see langword="null"/>.
    /// </exception>
    public MovementSystemResult Execute(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        MovementRequestCollection requests)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(organisms);
        ArgumentNullException.ThrowIfNull(requests);

        Dictionary<ChunkId, Chunk> chunksById = new();
        foreach (Chunk chunk in world.GetChunks())
        {
            chunksById.Add(chunk.Id, chunk);
        }

        Dictionary<OrganismId, Organism> updatedOrganisms = new();
        foreach (Organism organism in organisms.GetAll())
        {
            updatedOrganisms.Add(organism.Id, organism);
        }

        Dictionary<ChunkId, List<OrganismId>> chunkOccupancy = CreateChunkOccupancy(world);
        List<MovementRequest> remainingRequests = new();

        foreach (MovementRequest request in requests.GetAll())
        {
            if (request.StartTick > world.TimeState.CurrentTick)
            {
                remainingRequests.Add(request);
                continue;
            }

            if (!updatedOrganisms.TryGetValue(request.OrganismId, out Organism? organism))
            {
                continue;
            }

            if (!chunksById.TryGetValue(organism.CurrentChunkId, out Chunk? sourceChunk))
            {
                continue;
            }

            if (!chunksById.TryGetValue(request.TargetChunkId, out Chunk? targetChunk))
            {
                continue;
            }

            if (!IsAdjacent(world, sourceChunk.Metadata.Coordinates, request.TargetChunkId))
            {
                continue;
            }

            if (!IsTraversable(targetChunk))
            {
                continue;
            }

            updatedOrganisms[organism.Id] = new Organism(
                organism.Id,
                organism.SpeciesId,
                organism.GenomeId,
                request.TargetChunkId,
                organism.Physiology,
                organism.Needs,
                organism.Lifecycle,
                organism.Health);

            chunkOccupancy[sourceChunk.Id].Remove(organism.Id);
            chunkOccupancy[targetChunk.Id].Add(organism.Id);
            chunkOccupancy[targetChunk.Id].Sort(static (left, right) => left.Value.CompareTo(right.Value));
        }

        GaiaEngine.Domain.World.World updatedWorld = RebuildWorld(world, chunkOccupancy);
        OrganismCollection updatedOrganismCollection = new(CreateOrderedOrganisms(updatedOrganisms));
        MovementRequestCollection remainingRequestCollection = new(remainingRequests.AsReadOnly());
        return new MovementSystemResult(updatedWorld, updatedOrganismCollection, remainingRequestCollection);
    }

    private static Dictionary<ChunkId, List<OrganismId>> CreateChunkOccupancy(GaiaEngine.Domain.World.World world)
    {
        Dictionary<ChunkId, List<OrganismId>> chunkOccupancy = new();
        foreach (Chunk chunk in world.GetChunks())
        {
            chunkOccupancy.Add(chunk.Id, new List<OrganismId>(chunk.OrganismIds));
        }

        return chunkOccupancy;
    }

    private bool IsAdjacent(GaiaEngine.Domain.World.World world, ChunkCoordinates sourceCoordinates, ChunkId targetChunkId)
    {
        foreach (Chunk adjacentChunk in spatialQueryService.GetAdjacentChunks(world, sourceCoordinates))
        {
            if (adjacentChunk.Id == targetChunkId)
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsTraversable(Chunk chunk)
    {
        return chunk.Water.Ocean is null;
    }

    private static GaiaEngine.Domain.World.World RebuildWorld(
        GaiaEngine.Domain.World.World world,
        Dictionary<ChunkId, List<OrganismId>> chunkOccupancy)
    {
        List<Chunk> updatedChunks = new(world.ChunkCount);
        foreach (Chunk chunk in world.GetChunks())
        {
            List<OrganismId> occupancy = chunkOccupancy[chunk.Id];
            updatedChunks.Add(
                new Chunk(
                    chunk.Metadata,
                    chunk.State,
                    chunk.Terrain,
                    chunk.Biome,
                    chunk.Climate,
                    chunk.Water,
                    chunk.Resources,
                    occupancy.AsReadOnly()));
        }

        return new GaiaEngine.Domain.World.World(
            world.Metadata,
            world.Dimensions,
            world.TimeState,
            updatedChunks.AsReadOnly());
    }

    private static IReadOnlyList<Organism> CreateOrderedOrganisms(Dictionary<OrganismId, Organism> updatedOrganisms)
    {
        List<Organism> orderedOrganisms = new(updatedOrganisms.Values);
        orderedOrganisms.Sort(static (left, right) => left.Id.Value.CompareTo(right.Id.Value));
        return orderedOrganisms.AsReadOnly();
    }
}

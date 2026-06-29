using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.World.Queries;

/// <summary>
/// Implements deterministic spatial lookups without exposing internal world storage.
/// </summary>
public sealed class DeterministicSpatialQueryService : ISpatialQueryService
{
    /// <summary>
    /// Tries to resolve one chunk by coordinates.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="coordinates">The chunk coordinates.</param>
    /// <param name="chunk">The resolved chunk when present.</param>
    /// <returns><see langword="true"/> when the chunk exists; otherwise <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="world"/> is <see langword="null"/>.</exception>
    public bool TryGetChunk(GaiaEngine.Domain.World.World world, ChunkCoordinates coordinates, out Chunk? chunk)
    {
        ArgumentNullException.ThrowIfNull(world);
        return world.TryGetChunk(coordinates, out chunk);
    }

    /// <summary>
    /// Returns the neighbouring chunks of the supplied origin coordinates in deterministic order.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="origin">The origin chunk coordinates.</param>
    /// <returns>The neighbouring chunks in deterministic order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="world"/> is <see langword="null"/>.</exception>
    public IReadOnlyList<Chunk> GetAdjacentChunks(GaiaEngine.Domain.World.World world, ChunkCoordinates origin)
    {
        ArgumentNullException.ThrowIfNull(world);

        List<Chunk> adjacentChunks = new(8);
        for (int y = origin.Y - 1; y <= origin.Y + 1; y++)
        {
            for (int x = origin.X - 1; x <= origin.X + 1; x++)
            {
                if (x == origin.X && y == origin.Y)
                {
                    continue;
                }

                if (world.TryGetChunk(new ChunkCoordinates(x, y), out Chunk? chunk))
                {
                    adjacentChunks.Add(chunk!);
                }
            }
        }

        return adjacentChunks.AsReadOnly();
    }

    /// <summary>
    /// Returns the chunks inside one rectangular area in deterministic order.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="minimum">The inclusive minimum coordinates.</param>
    /// <param name="maximum">The inclusive maximum coordinates.</param>
    /// <returns>The chunks inside the supplied area in deterministic order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="world"/> is <see langword="null"/>.</exception>
    public IReadOnlyList<Chunk> GetChunksInArea(GaiaEngine.Domain.World.World world, ChunkCoordinates minimum, ChunkCoordinates maximum)
    {
        ArgumentNullException.ThrowIfNull(world);

        int minimumX = Math.Min(minimum.X, maximum.X);
        int maximumX = Math.Max(minimum.X, maximum.X);
        int minimumY = Math.Min(minimum.Y, maximum.Y);
        int maximumY = Math.Max(minimum.Y, maximum.Y);

        List<Chunk> chunks = new();
        for (int y = minimumY; y <= maximumY; y++)
        {
            for (int x = minimumX; x <= maximumX; x++)
            {
                if (world.TryGetChunk(new ChunkCoordinates(x, y), out Chunk? chunk))
                {
                    chunks.Add(chunk!);
                }
            }
        }

        return chunks.AsReadOnly();
    }

    /// <summary>
    /// Returns the organisms currently referenced by the supplied chunk in deterministic order.
    /// </summary>
    /// <param name="chunk">The chunk whose organisms are queried.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <returns>The organisms referenced by the supplied chunk in deterministic order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="chunk"/> or <paramref name="organisms"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the chunk references an organism missing from the organism collection.</exception>
    public IReadOnlyList<Organism> GetOrganismsInChunk(Chunk chunk, OrganismCollection organisms)
    {
        ArgumentNullException.ThrowIfNull(chunk);
        ArgumentNullException.ThrowIfNull(organisms);

        List<Organism> resolvedOrganisms = new(chunk.OrganismIds.Count);
        foreach (OrganismId organismId in chunk.OrganismIds)
        {
            if (!organisms.TryGet(organismId, out Organism? organism))
            {
                throw new InvalidOperationException($"The chunk '{chunk.Id}' references an organism that does not exist in the organism collection.");
            }

            resolvedOrganisms.Add(organism!);
        }

        return resolvedOrganisms.AsReadOnly();
    }

    /// <summary>
    /// Returns the resources stored by the supplied chunk in deterministic order.
    /// </summary>
    /// <param name="chunk">The chunk whose resources are queried.</param>
    /// <returns>The resources stored by the supplied chunk in deterministic order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="chunk"/> is <see langword="null"/>.</exception>
    public IReadOnlyList<ResourceState> GetResourcesInChunk(Chunk chunk)
    {
        ArgumentNullException.ThrowIfNull(chunk);
        return chunk.Resources.GetAll();
    }

    /// <summary>
    /// Returns the nearest chunk that contains the requested resource type.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="origin">The origin chunk coordinates.</param>
    /// <param name="resourceType">The resource type to search for.</param>
    /// <returns>The nearest valid chunk when found; otherwise <see langword="null"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="world"/> is <see langword="null"/>.</exception>
    public Chunk? FindNearestChunkWithResource(GaiaEngine.Domain.World.World world, ChunkCoordinates origin, ResourceType resourceType)
    {
        ArgumentNullException.ThrowIfNull(world);

        Chunk? nearestChunk = null;
        int nearestDistanceSquared = int.MaxValue;

        foreach (Chunk chunk in world.GetChunks())
        {
            if (!chunk.Resources.TryGet(resourceType, out ResourceState? resource) || resource!.CurrentAmount <= 0)
            {
                continue;
            }

            int distanceSquared = GetDistanceSquared(origin, chunk.Metadata.Coordinates);
            if (distanceSquared < nearestDistanceSquared)
            {
                nearestChunk = chunk;
                nearestDistanceSquared = distanceSquared;
                continue;
            }

            if (distanceSquared == nearestDistanceSquared && nearestChunk is not null)
            {
                if (CompareCoordinates(chunk.Metadata.Coordinates, nearestChunk.Metadata.Coordinates) < 0)
                {
                    nearestChunk = chunk;
                }
            }
        }

        return nearestChunk;
    }

    private static int GetDistanceSquared(ChunkCoordinates origin, ChunkCoordinates target)
    {
        int deltaX = target.X - origin.X;
        int deltaY = target.Y - origin.Y;
        return (deltaX * deltaX) + (deltaY * deltaY);
    }

    private static int CompareCoordinates(ChunkCoordinates left, ChunkCoordinates right)
    {
        int yComparison = left.Y.CompareTo(right.Y);
        if (yComparison != 0)
        {
            return yComparison;
        }

        return left.X.CompareTo(right.X);
    }
}

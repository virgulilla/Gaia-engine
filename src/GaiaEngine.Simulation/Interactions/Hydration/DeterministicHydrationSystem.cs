using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.Interactions.Hydration;

/// <summary>
/// Executes deterministic hydration requests by consuming fresh water from chunk resources.
/// </summary>
public sealed class DeterministicHydrationSystem : IHydrationSystem
{
    private const int HydrationReduction = 220;
    private const int WaterConsumptionAmount = 10;

    /// <summary>
    /// Executes deterministic hydration requests against the supplied world and organism state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="requests">The current hydration requests.</param>
    /// <returns>The deterministic hydration execution result.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/>, <paramref name="organisms"/>, or <paramref name="requests"/> is <see langword="null"/>.
    /// </exception>
    public HydrationSystemResult Execute(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        HydrationRequestCollection requests)
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

        Dictionary<ChunkId, ChunkResources> updatedResources = new();
        foreach (Chunk chunk in world.GetChunks())
        {
            updatedResources.Add(chunk.Id, chunk.Resources);
        }

        List<HydrationRequest> remainingRequests = new();
        foreach (HydrationRequest request in requests.GetAll())
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

            if (organism.CurrentChunkId != request.TargetChunkId)
            {
                continue;
            }

            if (!updatedResources.TryGetValue(request.TargetChunkId, out ChunkResources? resources))
            {
                continue;
            }

            if (!resources.TryGet(ResourceType.FreshWater, out ResourceState? freshWater) || freshWater is null)
            {
                continue;
            }

            if (freshWater.CurrentAmount < WaterConsumptionAmount)
            {
                continue;
            }

            updatedOrganisms[organism.Id] = new Organism(
                organism.Id,
                organism.SpeciesId,
                organism.GenomeId,
                organism.CurrentChunkId,
                organism.Physiology,
                new NeedsComponent(
                    organism.Needs.Hunger,
                    Math.Max(0, organism.Needs.Hydration - HydrationReduction),
                    organism.Needs.Rest,
                    organism.Needs.ReproductionUrge),
                organism.Lifecycle,
                organism.Health);

            updatedResources[request.TargetChunkId] = CreateUpdatedResources(resources, freshWater, WaterConsumptionAmount);
        }

        GaiaEngine.Domain.World.World updatedWorld = RebuildWorld(world, updatedResources);
        OrganismCollection updatedOrganismCollection = new(CreateOrderedOrganisms(updatedOrganisms));
        HydrationRequestCollection remainingRequestCollection = new(remainingRequests.AsReadOnly());
        return new HydrationSystemResult(updatedWorld, updatedOrganismCollection, remainingRequestCollection);
    }

    private static ChunkResources CreateUpdatedResources(ChunkResources resources, ResourceState freshWater, int consumptionAmount)
    {
        List<ResourceState> updatedStates = new(resources.Count);
        foreach (ResourceState resource in resources.GetAll())
        {
            if (resource.ResourceId == freshWater.ResourceId)
            {
                updatedStates.Add(
                    new ResourceState(
                        resource.ResourceId,
                        resource.Type,
                        resource.Category,
                        resource.CurrentAmount - consumptionAmount,
                        resource.MaximumCapacity,
                        resource.RegenerationRate,
                        resource.Quality,
                        resource.Availability));
            }
            else
            {
                updatedStates.Add(resource);
            }
        }

        return new ChunkResources(updatedStates.AsReadOnly());
    }

    private static GaiaEngine.Domain.World.World RebuildWorld(
        GaiaEngine.Domain.World.World world,
        Dictionary<ChunkId, ChunkResources> updatedResources)
    {
        List<Chunk> updatedChunks = new(world.ChunkCount);
        foreach (Chunk chunk in world.GetChunks())
        {
            updatedChunks.Add(
                new Chunk(
                    chunk.Metadata,
                    chunk.State,
                    chunk.Terrain,
                    chunk.Biome,
                    chunk.Climate,
                    chunk.Water,
                    updatedResources[chunk.Id],
                    chunk.OrganismIds));
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

using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.Interactions.Feeding;

/// <summary>
/// Executes deterministic feeding requests by consuming vegetation from chunk resources.
/// </summary>
public sealed class DeterministicFeedingSystem : IFeedingSystem
{
    private const int HungerReduction = 240;
    private const int VegetationConsumptionAmount = 12;

    /// <summary>
    /// Executes deterministic feeding requests against the supplied world and organism state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="requests">The current feeding requests.</param>
    /// <returns>The deterministic feeding execution result.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/>, <paramref name="organisms"/>, or <paramref name="requests"/> is <see langword="null"/>.
    /// </exception>
    public FeedingSystemResult Execute(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        FeedingRequestCollection requests)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(organisms);
        ArgumentNullException.ThrowIfNull(requests);

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

        List<FeedingRequest> remainingRequests = new();
        foreach (FeedingRequest request in requests.GetAll())
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

            if (!resources.TryGet(ResourceType.Vegetation, out ResourceState? vegetation) || vegetation is null)
            {
                continue;
            }

            if (vegetation.CurrentAmount < VegetationConsumptionAmount)
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
                    Math.Max(0, organism.Needs.Hunger - HungerReduction),
                    organism.Needs.Hydration,
                    organism.Needs.Rest,
                    organism.Needs.ReproductionUrge),
                organism.Lifecycle,
                organism.Health);

            updatedResources[request.TargetChunkId] = CreateUpdatedResources(resources, vegetation, VegetationConsumptionAmount);
        }

        GaiaEngine.Domain.World.World updatedWorld = RebuildWorld(world, updatedResources);
        OrganismCollection updatedOrganismCollection = new(CreateOrderedOrganisms(updatedOrganisms));
        FeedingRequestCollection remainingRequestCollection = new(remainingRequests.AsReadOnly());
        return new FeedingSystemResult(updatedWorld, updatedOrganismCollection, remainingRequestCollection);
    }

    private static ChunkResources CreateUpdatedResources(ChunkResources resources, ResourceState vegetation, int consumptionAmount)
    {
        List<ResourceState> updatedStates = new(resources.Count);
        foreach (ResourceState resource in resources.GetAll())
        {
            if (resource.ResourceId == vegetation.ResourceId)
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

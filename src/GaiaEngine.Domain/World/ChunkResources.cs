using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the passive resource state collection stored by one chunk.
/// </summary>
public sealed record ChunkResources
{
    private readonly Dictionary<ResourceType, ResourceState> resources;
    private readonly Dictionary<ResourceId, ResourceState> resourcesById;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChunkResources"/> class.
    /// </summary>
    /// <param name="resources">The resource states owned by the chunk.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="resources"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when duplicate resource types are supplied.</exception>
    public ChunkResources(IReadOnlyList<ResourceState> resources)
    {
        ArgumentNullException.ThrowIfNull(resources);

        this.resources = new Dictionary<ResourceType, ResourceState>(resources.Count);
        resourcesById = new Dictionary<ResourceId, ResourceState>(resources.Count);
        foreach (ResourceState resource in resources)
        {
            ArgumentNullException.ThrowIfNull(resource);

            if (!this.resources.TryAdd(resource.Type, resource))
            {
                throw new ArgumentException("Chunk resource types must be unique.", nameof(resources));
            }

            if (!resourcesById.TryAdd(resource.ResourceId, resource))
            {
                throw new ArgumentException("Chunk resource identifiers must be unique.", nameof(resources));
            }
        }
    }

    /// <summary>
    /// Gets the number of stored resource states.
    /// </summary>
    public int Count => resources.Count;

    /// <summary>
    /// Tries to get one stored resource state by type.
    /// </summary>
    /// <param name="type">The resource type.</param>
    /// <param name="resource">The resolved resource state when present.</param>
    /// <returns><see langword="true"/> when the resource exists; otherwise <see langword="false"/>.</returns>
    public bool TryGet(ResourceType type, out ResourceState? resource)
    {
        return resources.TryGetValue(type, out resource);
    }

    /// <summary>
    /// Tries to get one stored resource state by identifier.
    /// </summary>
    /// <param name="resourceId">The resource identifier.</param>
    /// <param name="resource">The resolved resource state when present.</param>
    /// <returns><see langword="true"/> when the resource exists; otherwise <see langword="false"/>.</returns>
    public bool TryGet(ResourceId resourceId, out ResourceState? resource)
    {
        return resourcesById.TryGetValue(resourceId, out resource);
    }

    /// <summary>
    /// Returns the stored resource states in deterministic type order.
    /// </summary>
    /// <returns>The stored resource states in deterministic type order.</returns>
    public IReadOnlyList<ResourceState> GetAll()
    {
        List<ResourceState> orderedResources = new(resources.Count);
        foreach (ResourceType type in Enum.GetValues<ResourceType>())
        {
            if (resources.TryGetValue(type, out ResourceState? resource))
            {
                orderedResources.Add(resource);
            }
        }

        return orderedResources.AsReadOnly();
    }
}

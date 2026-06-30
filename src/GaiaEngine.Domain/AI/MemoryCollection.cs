using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Domain.AI;

/// <summary>
/// Represents the deterministic memory state for every organism in the current runtime slice.
/// </summary>
public sealed class MemoryCollection
{
    private readonly Dictionary<OrganismId, OrganismMemory> memoryByOrganismId;
    private readonly IReadOnlyList<OrganismMemory> orderedMemories;

    /// <summary>
    /// Initializes a new instance of the <see cref="MemoryCollection"/> class.
    /// </summary>
    /// <param name="memories">The organism memory records to store.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="memories"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when duplicated organism identifiers are detected.</exception>
    public MemoryCollection(IReadOnlyList<OrganismMemory> memories)
    {
        ArgumentNullException.ThrowIfNull(memories);

        memoryByOrganismId = new Dictionary<OrganismId, OrganismMemory>(memories.Count);
        List<OrganismMemory> ordered = new(memories.Count);
        foreach (OrganismMemory memory in memories)
        {
            ArgumentNullException.ThrowIfNull(memory);
            if (!memoryByOrganismId.TryAdd(memory.OrganismId, memory))
            {
                throw new ArgumentException($"The organism memory '{memory.OrganismId}' is duplicated.", nameof(memories));
            }

            ordered.Add(memory);
        }

        ordered.Sort(static (left, right) => left.OrganismId.Value.CompareTo(right.OrganismId.Value));
        orderedMemories = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the shared empty memory collection instance.
    /// </summary>
    public static MemoryCollection Empty { get; } = new(Array.Empty<OrganismMemory>());

    /// <summary>
    /// Gets the number of stored organism memory records.
    /// </summary>
    public int Count => orderedMemories.Count;

    /// <summary>
    /// Returns all stored organism memory records in deterministic order.
    /// </summary>
    /// <returns>The ordered memory set.</returns>
    public IReadOnlyList<OrganismMemory> GetAll()
    {
        return orderedMemories;
    }

    /// <summary>
    /// Attempts to resolve one organism memory by organism identifier.
    /// </summary>
    /// <param name="organismId">The organism identifier to resolve.</param>
    /// <param name="memory">The resolved memory when found; otherwise <see langword="null"/>.</param>
    /// <returns><see langword="true"/> when the memory exists; otherwise <see langword="false"/>.</returns>
    public bool TryGet(OrganismId organismId, out OrganismMemory? memory)
    {
        return memoryByOrganismId.TryGetValue(organismId, out memory);
    }
}

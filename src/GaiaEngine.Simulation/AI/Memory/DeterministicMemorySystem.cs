using System;
using System.Collections.Generic;
using GaiaEngine.Domain.AI;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.AI.Perception;

namespace GaiaEngine.Simulation.AI.Memory;

/// <summary>
/// Updates deterministic organism memories from current perception without introducing hidden state.
/// </summary>
public sealed class DeterministicMemorySystem : IMemorySystem
{
    private readonly MemorySettings settings;
    private readonly IPerceptionSystem perceptionSystem;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicMemorySystem"/> class.
    /// </summary>
    /// <param name="settings">The deterministic memory settings.</param>
    /// <param name="perceptionSystem">The deterministic perception system.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="settings"/> or <paramref name="perceptionSystem"/> is <see langword="null"/>.</exception>
    public DeterministicMemorySystem(MemorySettings settings, IPerceptionSystem perceptionSystem)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        this.perceptionSystem = perceptionSystem ?? throw new ArgumentNullException(nameof(perceptionSystem));
    }

    /// <summary>
    /// Updates the current memory collection from the current world and organism state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="memories">The current organism memory state.</param>
    /// <returns>The updated deterministic memory collection.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/>, <paramref name="organisms"/>, or <paramref name="memories"/> is <see langword="null"/>.
    /// </exception>
    public MemoryCollection Update(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        MemoryCollection memories)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(organisms);
        ArgumentNullException.ThrowIfNull(memories);

        long currentTick = world.TimeState.CurrentTick;
        List<OrganismMemory> updatedMemories = new();
        foreach (Organism organism in organisms.GetAll())
        {
            if (!organism.Lifecycle.IsAlive)
            {
                continue;
            }

            List<MemoryEntry> entries = new();
            if (memories.TryGet(organism.Id, out OrganismMemory? existingMemory) && existingMemory is not null)
            {
                entries.AddRange(DecayEntries(existingMemory.GetAll(), currentTick));
            }

            PerceptionResult perception = perceptionSystem.Evaluate(world, organisms, organism.Id);
            MergeObservations(entries, world, organisms, perception, currentTick);
            IReadOnlyList<MemoryEntry> cappedEntries = EnforceCapacities(entries);
            updatedMemories.Add(new OrganismMemory(organism.Id, cappedEntries));
        }

        return new MemoryCollection(updatedMemories.AsReadOnly());
    }

    private IReadOnlyList<MemoryEntry> DecayEntries(IReadOnlyList<MemoryEntry> entries, long currentTick)
    {
        List<MemoryEntry> updated = new(entries.Count);
        foreach (MemoryEntry entry in entries)
        {
            if (currentTick >= entry.ExpirationTick)
            {
                continue;
            }

            int elapsedTicks = (int)Math.Max(0, currentTick - entry.LastUpdateTick);
            int decayedConfidence = Math.Max(0, entry.Confidence - (ResolveDecay(entry.Category) * elapsedTicks));
            if (decayedConfidence < settings.MinimumConfidence)
            {
                continue;
            }

            updated.Add(
                new MemoryEntry(
                    entry.Identifier,
                    entry.Category,
                    entry.Position,
                    decayedConfidence,
                    entry.CreationTick,
                    entry.LastUpdateTick,
                    entry.ExpirationTick,
                    entry.EstimatedAvailability));
        }

        return updated.AsReadOnly();
    }

    private void MergeObservations(
        List<MemoryEntry> entries,
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        PerceptionResult perception,
        long currentTick)
    {
        foreach (PerceivedObject observation in perception.Observations)
        {
            if (!TryCreateObservedMemory(world, organisms, observation, currentTick, out MemoryEntry? observedEntry))
            {
                continue;
            }

            int existingIndex = FindEntryIndex(entries, observedEntry!.Category, observedEntry.Identifier);
            if (existingIndex >= 0)
            {
                MemoryEntry existing = entries[existingIndex];
                entries[existingIndex] = new MemoryEntry(
                    existing.Identifier,
                    existing.Category,
                    observedEntry.Position,
                    Math.Min(1000, Math.Max(existing.Confidence, observedEntry.Confidence) + settings.ConfidenceRefreshBonus),
                    existing.CreationTick,
                    currentTick,
                    currentTick + ResolveExpiration(existing.Category),
                    observedEntry.EstimatedAvailability);
                continue;
            }

            entries.Add(observedEntry);
        }
    }

    private bool TryCreateObservedMemory(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        PerceivedObject observation,
        long currentTick,
        out MemoryEntry? entry)
    {
        entry = null;
        if (observation.Confidence < settings.MinimumConfidence)
        {
            return false;
        }

        MemoryCategory category = observation.ObjectKind == PerceivedObjectKind.Organism ? MemoryCategory.Organism : MemoryCategory.Resource;
        if (!TryResolveObservationPosition(world, organisms, observation, out ChunkCoordinates position, out int? availability))
        {
            return false;
        }

        entry = new MemoryEntry(
            observation.ObjectId,
            category,
            position,
            observation.Confidence,
            currentTick,
            currentTick,
            currentTick + ResolveExpiration(category),
            availability);
        return true;
    }

    private static int FindEntryIndex(List<MemoryEntry> entries, MemoryCategory category, ulong identifier)
    {
        for (int index = 0; index < entries.Count; index++)
        {
            if (entries[index].Category == category && entries[index].Identifier == identifier)
            {
                return index;
            }
        }

        return -1;
    }

    private IReadOnlyList<MemoryEntry> EnforceCapacities(List<MemoryEntry> entries)
    {
        List<MemoryEntry> capped = new(entries.Count);
        foreach (MemoryCategory category in Enum.GetValues<MemoryCategory>())
        {
            List<MemoryEntry> categoryEntries = new();
            foreach (MemoryEntry entry in entries)
            {
                if (entry.Category == category)
                {
                    categoryEntries.Add(entry);
                }
            }

            categoryEntries.Sort(static (left, right) => ComparePriority(left, right));
            int keepCount = Math.Min(categoryEntries.Count, ResolveCapacity(category));
            for (int index = 0; index < keepCount; index++)
            {
                capped.Add(categoryEntries[index]);
            }
        }

        capped.Sort(static (left, right) => CompareStored(left, right));
        return capped.AsReadOnly();
    }

    private static int ComparePriority(MemoryEntry left, MemoryEntry right)
    {
        int confidenceComparison = right.Confidence.CompareTo(left.Confidence);
        if (confidenceComparison != 0)
        {
            return confidenceComparison;
        }

        int lastUpdateComparison = right.LastUpdateTick.CompareTo(left.LastUpdateTick);
        if (lastUpdateComparison != 0)
        {
            return lastUpdateComparison;
        }

        return left.Identifier.CompareTo(right.Identifier);
    }

    private static int CompareStored(MemoryEntry left, MemoryEntry right)
    {
        int categoryComparison = left.Category.CompareTo(right.Category);
        if (categoryComparison != 0)
        {
            return categoryComparison;
        }

        return left.Identifier.CompareTo(right.Identifier);
    }

    private int ResolveCapacity(MemoryCategory category)
    {
        return category switch
        {
            MemoryCategory.Organism => settings.OrganismCapacity,
            MemoryCategory.Resource => settings.ResourceCapacity,
            MemoryCategory.Location => settings.LocationCapacity,
            MemoryCategory.Hazard => settings.HazardCapacity,
            MemoryCategory.Event => settings.EventCapacity,
            _ => throw new ArgumentOutOfRangeException(nameof(category), "The supplied memory category is not supported."),
        };
    }

    private int ResolveExpiration(MemoryCategory category)
    {
        return category switch
        {
            MemoryCategory.Organism => settings.OrganismExpirationTicks,
            MemoryCategory.Resource => settings.ResourceExpirationTicks,
            MemoryCategory.Location => settings.LocationExpirationTicks,
            MemoryCategory.Hazard => settings.HazardExpirationTicks,
            MemoryCategory.Event => settings.EventExpirationTicks,
            _ => throw new ArgumentOutOfRangeException(nameof(category), "The supplied memory category is not supported."),
        };
    }

    private int ResolveDecay(MemoryCategory category)
    {
        return category switch
        {
            MemoryCategory.Organism => settings.OrganismDecayPerTick,
            MemoryCategory.Resource => settings.ResourceDecayPerTick,
            MemoryCategory.Location => settings.LocationDecayPerTick,
            MemoryCategory.Hazard => settings.HazardDecayPerTick,
            MemoryCategory.Event => settings.EventDecayPerTick,
            _ => throw new ArgumentOutOfRangeException(nameof(category), "The supplied memory category is not supported."),
        };
    }

    private static bool TryResolveObservationPosition(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        PerceivedObject observation,
        out ChunkCoordinates position,
        out int? availability)
    {
        if (observation.ObjectKind == PerceivedObjectKind.Organism)
        {
            OrganismId organismId;
            try
            {
                organismId = new OrganismId(observation.ObjectId);
            }
            catch (ArgumentException)
            {
                position = default;
                availability = null;
                return false;
            }

            if (!organisms.TryGet(organismId, out Organism? organism))
            {
                position = default;
                availability = null;
                return false;
            }

            foreach (Chunk chunk in world.GetChunks())
            {
                if (chunk.Id == organism!.CurrentChunkId)
                {
                    position = chunk.Metadata.Coordinates;
                    availability = null;
                    return true;
                }
            }

            position = default;
            availability = null;
            return false;
        }

        foreach (Chunk chunk in world.GetChunks())
        {
            if (observation.ObjectKind == PerceivedObjectKind.Water && chunk.Id.Value == observation.ObjectId)
            {
                position = chunk.Metadata.Coordinates;
                availability = Math.Min(1000, chunk.Water.SurfaceWater.WaterLevel);
                return true;
            }

            foreach (ResourceState resource in chunk.Resources.GetAll())
            {
                if (resource.ResourceId.Value == observation.ObjectId)
                {
                    position = chunk.Metadata.Coordinates;
                    availability = resource.Availability;
                    return true;
                }
            }
        }

        position = default;
        availability = null;
        return false;
    }
}

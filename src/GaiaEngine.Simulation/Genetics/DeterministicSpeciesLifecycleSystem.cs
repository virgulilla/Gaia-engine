using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Genetics;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;

namespace GaiaEngine.Simulation.Genetics;

/// <summary>
/// Maintains deterministic species lifecycle state such as extinction markers.
/// </summary>
public sealed class DeterministicSpeciesLifecycleSystem : ISpeciesLifecycleSystem
{
    /// <summary>
    /// Updates the supplied species state deterministically.
    /// </summary>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="species">The current species state.</param>
    /// <param name="currentTick">The current deterministic simulation tick.</param>
    /// <returns>The updated species state.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="organisms"/> or <paramref name="species"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="currentTick"/> is negative.</exception>
    public SpeciesCollection Update(OrganismCollection organisms, SpeciesCollection species, long currentTick)
    {
        ArgumentNullException.ThrowIfNull(organisms);
        ArgumentNullException.ThrowIfNull(species);
        if (currentTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currentTick), "The current tick must be zero or greater.");
        }

        HashSet<SpeciesId> livingSpecies = new();
        foreach (Organism organism in organisms.GetAll())
        {
            if (organism.Lifecycle.IsAlive && organism.Lifecycle.Stage != LifecycleStage.Dead)
            {
                livingSpecies.Add(organism.SpeciesId);
            }
        }

        List<Species> updatedSpecies = new(species.Count);
        foreach (Species entry in species.GetAll())
        {
            bool isLiving = livingSpecies.Contains(entry.Id);
            if (isLiving || entry.IsExtinct)
            {
                updatedSpecies.Add(entry);
                continue;
            }

            updatedSpecies.Add(
                new Species(
                    entry.Id,
                    entry.ParentSpeciesId,
                    entry.OriginTick,
                    currentTick,
                    entry.GetFounderPopulation()));
        }

        return new SpeciesCollection(updatedSpecies.AsReadOnly());
    }
}

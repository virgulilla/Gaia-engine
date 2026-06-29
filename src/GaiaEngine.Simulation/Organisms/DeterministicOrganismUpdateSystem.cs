using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.Organisms;

/// <summary>
/// Applies deterministic physiology, needs, health and lifecycle progression to organisms.
/// </summary>
public sealed class DeterministicOrganismUpdateSystem : IOrganismUpdateSystem
{
    private const int MaximumNeedValue = 1000;

    /// <summary>
    /// Updates the supplied organism state deterministically.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <returns>The updated organism state.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/> or <paramref name="organisms"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">Thrown when an organism references a chunk that does not exist.</exception>
    public OrganismCollection Update(GaiaEngine.Domain.World.World world, OrganismCollection organisms)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(organisms);

        HashSet<ChunkId> validChunkIds = new();
        foreach (Chunk chunk in world.GetChunks())
        {
            validChunkIds.Add(chunk.Id);
        }

        List<Organism> updatedOrganisms = new(organisms.Count);
        foreach (Organism organism in organisms.GetAll())
        {
            if (!validChunkIds.Contains(organism.CurrentChunkId))
            {
                throw new InvalidOperationException($"The organism '{organism.Id}' references a chunk that does not exist in the current world.");
            }

            updatedOrganisms.Add(UpdateOrganism(organism));
        }

        return new OrganismCollection(updatedOrganisms.AsReadOnly());
    }

    private static Organism UpdateOrganism(Organism organism)
    {
        if (!organism.Lifecycle.IsAlive || organism.Lifecycle.Stage == LifecycleStage.Dead)
        {
            LifecycleComponent deadLifecycle = new(
                organism.Lifecycle.BirthTick,
                organism.Lifecycle.AgeTicks,
                organism.Lifecycle.MaturityAgeTicks,
                LifecycleStage.Dead,
                isAlive: false);
            NeedsComponent deadNeeds = new(
                organism.Needs.Hunger,
                organism.Needs.Hydration,
                organism.Needs.Rest,
                reproductionUrge: 0);

            return new Organism(
                organism.Id,
                organism.SpeciesId,
                organism.GenomeId,
                organism.CurrentChunkId,
                organism.Physiology,
                deadNeeds,
                deadLifecycle,
                organism.Health);
        }

        long ageTicks = checked(organism.Lifecycle.AgeTicks + 1);
        NeedsComponent needs = UpdateNeeds(organism.Physiology, organism.Needs, organism.Lifecycle.Stage);
        HealthComponent health = UpdateHealth(organism.Health, needs);
        LifecycleComponent lifecycle = UpdateLifecycle(organism.Lifecycle, organism.Physiology, ageTicks, health);

        NeedsComponent finalizedNeeds = lifecycle.IsAlive
            ? needs
            : new NeedsComponent(needs.Hunger, needs.Hydration, needs.Rest, 0);

        return new Organism(
            organism.Id,
            organism.SpeciesId,
            organism.GenomeId,
            organism.CurrentChunkId,
            organism.Physiology,
            finalizedNeeds,
            lifecycle,
            health);
    }

    private static NeedsComponent UpdateNeeds(PhysiologyComponent physiology, NeedsComponent currentNeeds, LifecycleStage stage)
    {
        int hungerIncrease = Math.Max(1, physiology.MetabolismRate + (physiology.GrowthRate / 2) - (physiology.DigestionEfficiency / 20));
        int hydrationIncrease = Math.Max(1, physiology.MetabolismRate + 2 - (physiology.WaterEfficiency / 15));
        int restIncrease = Math.Max(1, 2 + (physiology.GrowthRate / 3));
        int reproductionIncrease = stage == LifecycleStage.Adult || stage == LifecycleStage.Elder ? 4 : 0;

        return new NeedsComponent(
            ClampNeed(currentNeeds.Hunger + hungerIncrease),
            ClampNeed(currentNeeds.Hydration + hydrationIncrease),
            ClampNeed(currentNeeds.Rest + restIncrease),
            ClampNeed(currentNeeds.ReproductionUrge + reproductionIncrease));
    }

    private static HealthComponent UpdateHealth(HealthComponent currentHealth, NeedsComponent needs)
    {
        int penalty = 0;
        penalty += GetNeedPenalty(needs.Hunger);
        penalty += GetNeedPenalty(needs.Hydration);
        penalty += GetNeedPenalty(needs.Rest);

        int recovery = needs.Hunger <= 250 && needs.Hydration <= 250 && needs.Rest <= 250 ? 2 : 0;
        int nextValue = Math.Clamp(currentHealth.CurrentValue - penalty + recovery, 0, currentHealth.MaximumValue);
        return new HealthComponent(nextValue, currentHealth.MaximumValue);
    }

    private static LifecycleComponent UpdateLifecycle(
        LifecycleComponent currentLifecycle,
        PhysiologyComponent physiology,
        long ageTicks,
        HealthComponent health)
    {
        if (health.CurrentValue == 0 || ageTicks >= physiology.LifespanTicks)
        {
            return new LifecycleComponent(
                currentLifecycle.BirthTick,
                ageTicks,
                currentLifecycle.MaturityAgeTicks,
                LifecycleStage.Dead,
                isAlive: false);
        }

        long elderThreshold = Math.Max(currentLifecycle.MaturityAgeTicks + 1, physiology.LifespanTicks * 3L / 4L);
        LifecycleStage stage = ageTicks >= elderThreshold
            ? LifecycleStage.Elder
            : ageTicks >= currentLifecycle.MaturityAgeTicks
                ? LifecycleStage.Adult
                : LifecycleStage.Juvenile;

        return new LifecycleComponent(
            currentLifecycle.BirthTick,
            ageTicks,
            currentLifecycle.MaturityAgeTicks,
            stage,
            isAlive: true);
    }

    private static int GetNeedPenalty(int value)
    {
        if (value >= 950)
        {
            return 12;
        }

        if (value >= 800)
        {
            return 5;
        }

        if (value >= 650)
        {
            return 2;
        }

        return 0;
    }

    private static int ClampNeed(int value)
    {
        return Math.Clamp(value, 0, MaximumNeedValue);
    }
}

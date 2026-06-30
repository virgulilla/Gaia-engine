using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.AI.Utility;
using GaiaEngine.Foundation.Determinism;

namespace GaiaEngine.Simulation.AI.Decision;

/// <summary>
/// Selects exactly one deterministic decision from utility-evaluated candidates.
/// </summary>
public sealed class DeterministicDecisionMakingSystem : IDecisionMakingSystem
{
    private readonly IEntityIdGenerator idGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicDecisionMakingSystem"/> class.
    /// </summary>
    /// <param name="idGenerator">The deterministic entity identifier generator.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="idGenerator"/> is <see langword="null"/>.</exception>
    public DeterministicDecisionMakingSystem(IEntityIdGenerator idGenerator)
    {
        this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    }

    /// <summary>
    /// Selects one deterministic decision for the supplied organism.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="utilityResult">The utility evaluation result to resolve.</param>
    /// <param name="organismId">The evaluated organism identifier.</param>
    /// <returns>The selected deterministic decision.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/>, <paramref name="organisms"/>, or <paramref name="utilityResult"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the organism does not exist or when <paramref name="utilityResult"/> belongs to another organism.
    /// </exception>
    public SelectedDecision Select(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        UtilityEvaluationResult utilityResult,
        OrganismId organismId)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(organisms);
        ArgumentNullException.ThrowIfNull(utilityResult);

        if (utilityResult.OrganismId != organismId)
        {
            throw new InvalidOperationException("The supplied utility result does not belong to the evaluated organism.");
        }

        if (!organisms.TryGet(organismId, out Organism? organism))
        {
            throw new InvalidOperationException("The evaluated organism does not exist in the organism collection.");
        }

        List<UtilityActionEvaluation> orderedCandidates = new(utilityResult.Candidates);
        orderedCandidates.Sort(static (left, right) => CompareCandidates(left, right));

        foreach (UtilityActionEvaluation candidate in orderedCandidates)
        {
            if (!candidate.IsValid)
            {
                continue;
            }

            return new SelectedDecision(
                organismId,
                candidate.ActionId,
                candidate.ActionType,
                candidate.Target,
                candidate.UtilityScore,
                candidate.EstimatedCost,
                candidate.ExpectedDuration,
                utilityResult.EvaluationTick,
                isIdleFallback: false);
        }

        Chunk currentChunk = ResolveChunkById(world, organism!.CurrentChunkId);
        ActionId idleActionId = CreateIdleActionId(world.Metadata.Seed, utilityResult.EvaluationTick, organismId, currentChunk.Id);
        return new SelectedDecision(
            organismId,
            idleActionId,
            SimulationActionType.Idle,
            new SimulationActionTarget(ActionTargetKind.Chunk, currentChunk.Id.ToString()),
            utilityScore: 0,
            estimatedCost: 0,
            expectedDuration: 1,
            decisionTick: utilityResult.EvaluationTick,
            isIdleFallback: true);
    }

    private ActionId CreateIdleActionId(WorldSeed worldSeed, long tick, OrganismId organismId, ChunkId chunkId)
    {
        ulong hash = 1469598103934665603UL;
        hash = Mix(hash, organismId.Value);
        hash = Mix(hash, chunkId.Value);
        hash = Mix(hash, (ulong)tick);
        hash = Mix(hash, (ulong)SimulationActionType.Idle);

        ulong sequenceValue = hash & EntitySequence.MAX_VALUE;
        if (sequenceValue == 0)
        {
            sequenceValue = 1;
        }

        return idGenerator.CreateActionId(new IdentifierGenerationContext(worldSeed, tick, new EntitySequence(sequenceValue)));
    }

    private static Chunk ResolveChunkById(GaiaEngine.Domain.World.World world, ChunkId chunkId)
    {
        foreach (Chunk chunk in world.GetChunks())
        {
            if (chunk.Id == chunkId)
            {
                return chunk;
            }
        }

        throw new InvalidOperationException("The current organism chunk could not be resolved in the world state.");
    }

    private static ulong Mix(ulong current, ulong value)
    {
        const ulong prime = 1099511628211UL;
        return (current ^ value) * prime;
    }

    private static int CompareCandidates(UtilityActionEvaluation left, UtilityActionEvaluation right)
    {
        int scoreComparison = right.UtilityScore.CompareTo(left.UtilityScore);
        if (scoreComparison != 0)
        {
            return scoreComparison;
        }

        int costComparison = left.EstimatedCost.CompareTo(right.EstimatedCost);
        if (costComparison != 0)
        {
            return costComparison;
        }

        int durationComparison = left.ExpectedDuration.CompareTo(right.ExpectedDuration);
        if (durationComparison != 0)
        {
            return durationComparison;
        }

        return left.ActionId.Value.CompareTo(right.ActionId.Value);
    }
}

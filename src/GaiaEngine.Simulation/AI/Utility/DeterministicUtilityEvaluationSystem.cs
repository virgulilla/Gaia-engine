using System;
using System.Collections.Generic;
using GaiaEngine.Domain.AI;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.AI.Perception;

namespace GaiaEngine.Simulation.AI.Utility;

/// <summary>
/// Evaluates deterministic utility scores for the currently supported simulation actions.
/// </summary>
public sealed class DeterministicUtilityEvaluationSystem : IUtilityEvaluationSystem
{
    private readonly UtilityEvaluationSettings settings;
    private readonly IUtilityCurveEvaluator curveEvaluator;
    private readonly IEntityIdGenerator idGenerator;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicUtilityEvaluationSystem"/> class.
    /// </summary>
    /// <param name="settings">The deterministic utility evaluation settings.</param>
    /// <param name="curveEvaluator">The deterministic utility curve evaluator.</param>
    /// <param name="idGenerator">The deterministic entity identifier generator.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="settings"/> or <paramref name="curveEvaluator"/> is <see langword="null"/>.
    /// </exception>
    public DeterministicUtilityEvaluationSystem(UtilityEvaluationSettings settings, IUtilityCurveEvaluator curveEvaluator, IEntityIdGenerator idGenerator)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        this.curveEvaluator = curveEvaluator ?? throw new ArgumentNullException(nameof(curveEvaluator));
        this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
    }

    /// <summary>
    /// Evaluates candidate action utilities for one organism based on current perception and world state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="memories">The current memory state.</param>
    /// <param name="perception">The current perception output for the evaluated organism.</param>
    /// <param name="organismId">The evaluated organism identifier.</param>
    /// <returns>The deterministic utility evaluation result.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/>, <paramref name="organisms"/>, or <paramref name="perception"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the organism does not exist or when <paramref name="perception"/> belongs to another organism.
    /// </exception>
    public UtilityEvaluationResult Evaluate(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        MemoryCollection memories,
        PerceptionResult perception,
        OrganismId organismId)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(organisms);
        ArgumentNullException.ThrowIfNull(memories);
        ArgumentNullException.ThrowIfNull(perception);

        if (perception.ObserverId != organismId)
        {
            throw new InvalidOperationException("The supplied perception result does not belong to the evaluated organism.");
        }

        if (!organisms.TryGet(organismId, out Organism? organism))
        {
            throw new InvalidOperationException("The evaluated organism does not exist in the organism collection.");
        }

        Chunk currentChunk = ResolveChunkById(world, organism!.CurrentChunkId);
        OrganismMemory? organismMemory = null;
        memories.TryGet(organismId, out organismMemory);
        UtilityActionEvaluation eat = EvaluateEat(world, perception, organism, currentChunk);
        UtilityActionEvaluation drink = EvaluateDrink(world, perception, organism, currentChunk);
        UtilityActionEvaluation move = EvaluateMove(world, perception, organismMemory, organism, currentChunk, eat.IsValid, drink.IsValid);

        List<UtilityActionEvaluation> candidates = new() { eat, drink, move };
        candidates.Sort(static (left, right) => CompareCandidates(left, right));
        return new UtilityEvaluationResult(organismId, world.TimeState.CurrentTick, candidates.AsReadOnly());
    }

    private UtilityActionEvaluation EvaluateEat(GaiaEngine.Domain.World.World world, PerceptionResult perception, Organism organism, Chunk currentChunk)
    {
        ResourceObservation? resourceObservation = TryFindCurrentResourceObservation(world, perception, currentChunk, ResourceType.Vegetation);
        if (resourceObservation is null)
        {
            return CreateInvalidCandidate(SimulationActionType.Eat, organism.Id, currentChunk.Id, currentChunk.Metadata.Seed, perception.DetectionTick);
        }

        int urgency = curveEvaluator.Evaluate(organism.Needs.Hunger, settings.EatCurve);
        int resourceFactor = resourceObservation.Resource!.Availability;
        int confidenceFactor = resourceObservation.Observation.Confidence;
        int score = CombineFactors(urgency, resourceFactor, confidenceFactor);
        int cost = Math.Max(0, 100 - organism.Physiology.DigestionEfficiency);
        return new UtilityActionEvaluation(
            CreateActionId(organism.Id, currentChunk.Id.ToString(), SimulationActionType.Eat, currentChunk.Metadata.Seed, perception.DetectionTick),
            SimulationActionType.Eat,
            new SimulationActionTarget(ActionTargetKind.Chunk, currentChunk.Id.ToString()),
            score,
            cost,
            expectedDuration: 1,
            isValid: true);
    }

    private UtilityActionEvaluation EvaluateDrink(GaiaEngine.Domain.World.World world, PerceptionResult perception, Organism organism, Chunk currentChunk)
    {
        ResourceObservation? resourceObservation = TryFindCurrentResourceObservation(world, perception, currentChunk, ResourceType.FreshWater);
        if (resourceObservation is null)
        {
            return CreateInvalidCandidate(SimulationActionType.Drink, organism.Id, currentChunk.Id, currentChunk.Metadata.Seed, perception.DetectionTick);
        }

        int urgency = curveEvaluator.Evaluate(organism.Needs.Hydration, settings.DrinkCurve);
        int resourceFactor = resourceObservation.Resource!.Availability;
        int confidenceFactor = resourceObservation.Observation.Confidence;
        int score = CombineFactors(urgency, resourceFactor, confidenceFactor);
        int cost = Math.Max(0, 100 - organism.Physiology.WaterEfficiency);
        return new UtilityActionEvaluation(
            CreateActionId(organism.Id, currentChunk.Id.ToString(), SimulationActionType.Drink, currentChunk.Metadata.Seed, perception.DetectionTick),
            SimulationActionType.Drink,
            new SimulationActionTarget(ActionTargetKind.Chunk, currentChunk.Id.ToString()),
            score,
            cost,
            expectedDuration: 1,
            isValid: true);
    }

    private UtilityActionEvaluation EvaluateMove(GaiaEngine.Domain.World.World world, PerceptionResult perception, OrganismMemory? memory, Organism organism, Chunk currentChunk, bool canEatHere, bool canDrinkHere)
    {
        MoveTargetCandidate? bestTarget = null;

        foreach (PerceivedObject observation in perception.Observations)
        {
            if (observation.SensorType == SensorType.Touch || observation.SensorType == SensorType.Hearing)
            {
                continue;
            }

            if (observation.Confidence < settings.MinimumPerceptionConfidence)
            {
                continue;
            }

            if (TryResolveObservedResourceChunk(world, observation.ObjectId, ResourceType.Vegetation, out Chunk? foodChunk, out ResourceState? foodResource)
                && foodChunk!.Id != currentChunk.Id
                && observation.Distance == 1)
            {
                int urgency = canEatHere ? organism.Needs.Hunger / 4 : organism.Needs.Hunger;
                int baseScore = curveEvaluator.Evaluate(urgency, settings.MoveCurve);
                int score = CombineFactors(baseScore, foodResource!.Availability, observation.Confidence);
                score = Math.Max(0, score - 150);
                TryPromoteMoveCandidate(ref bestTarget, foodChunk, score);
            }

            if (TryResolveObservedResourceChunk(world, observation.ObjectId, ResourceType.FreshWater, out Chunk? waterChunk, out ResourceState? waterResource)
                && waterChunk!.Id != currentChunk.Id
                && observation.Distance == 1)
            {
                int urgency = canDrinkHere ? organism.Needs.Hydration / 4 : organism.Needs.Hydration;
                int baseScore = curveEvaluator.Evaluate(urgency, settings.MoveCurve);
                int score = CombineFactors(baseScore, waterResource!.Availability, observation.Confidence);
                score = Math.Max(0, score - 150);
                TryPromoteMoveCandidate(ref bestTarget, waterChunk, score);
            }
        }

        if (memory is not null)
        {
            foreach (MemoryEntry entry in memory.GetAll())
            {
                if (entry.Category != MemoryCategory.Resource)
                {
                    continue;
                }

                if (TryResolveRememberedResourceChunk(world, entry, ResourceType.Vegetation, out Chunk? foodChunk, out int foodAvailability)
                    && foodChunk!.Id != currentChunk.Id)
                {
                    int urgency = canEatHere ? organism.Needs.Hunger / 4 : organism.Needs.Hunger;
                    int baseScore = curveEvaluator.Evaluate(urgency, settings.MoveCurve);
                    int score = CombineFactors(baseScore, foodAvailability, entry.Confidence);
                    score = Math.Max(0, score - 300);
                    TryPromoteMoveCandidate(ref bestTarget, foodChunk, score);
                }

                if (TryResolveRememberedResourceChunk(world, entry, ResourceType.FreshWater, out Chunk? waterChunk, out int waterAvailability)
                    && waterChunk!.Id != currentChunk.Id)
                {
                    int urgency = canDrinkHere ? organism.Needs.Hydration / 4 : organism.Needs.Hydration;
                    int baseScore = curveEvaluator.Evaluate(urgency, settings.MoveCurve);
                    int score = CombineFactors(baseScore, waterAvailability, entry.Confidence);
                    score = Math.Max(0, score - 300);
                    TryPromoteMoveCandidate(ref bestTarget, waterChunk, score);
                }
            }
        }

        if (bestTarget is null || bestTarget.Score <= 0)
        {
            return CreateInvalidCandidate(SimulationActionType.Move, organism.Id, currentChunk.Id, currentChunk.Metadata.Seed, perception.DetectionTick);
        }

        int estimatedCost = Math.Max(1, bestTarget.TargetChunk.Terrain.Slope.TraversalCost / 10);
        return new UtilityActionEvaluation(
            CreateActionId(organism.Id, bestTarget.TargetChunk.Id.ToString(), SimulationActionType.Move, currentChunk.Metadata.Seed, perception.DetectionTick),
            SimulationActionType.Move,
            new SimulationActionTarget(ActionTargetKind.Chunk, bestTarget.TargetChunk.Id.ToString()),
            bestTarget.Score,
            estimatedCost,
            expectedDuration: 1,
            isValid: true);
    }

    private static void TryPromoteMoveCandidate(ref MoveTargetCandidate? bestTarget, Chunk chunk, int score)
    {
        if (bestTarget is null
            || score > bestTarget.Score
            || (score == bestTarget.Score && chunk.Metadata.Coordinates.Y < bestTarget.TargetChunk.Metadata.Coordinates.Y)
            || (score == bestTarget.Score
                && chunk.Metadata.Coordinates.Y == bestTarget.TargetChunk.Metadata.Coordinates.Y
                && chunk.Metadata.Coordinates.X < bestTarget.TargetChunk.Metadata.Coordinates.X))
        {
            bestTarget = new MoveTargetCandidate(chunk, score);
        }
    }

    private ResourceObservation? TryFindCurrentResourceObservation(GaiaEngine.Domain.World.World world, PerceptionResult perception, Chunk currentChunk, ResourceType resourceType)
    {
        foreach (PerceivedObject observation in perception.Observations)
        {
            if (observation.SensorType == SensorType.Hearing || observation.SensorType == SensorType.Touch)
            {
                continue;
            }

            if (observation.Confidence < settings.MinimumPerceptionConfidence)
            {
                continue;
            }

            if (!TryResolveObservedResourceChunk(world, observation.ObjectId, resourceType, out Chunk? observedChunk, out ResourceState? resource))
            {
                continue;
            }

            if (observedChunk!.Id != currentChunk.Id)
            {
                continue;
            }

            if (resource!.CurrentAmount < settings.MinimumResourceAmount)
            {
                continue;
            }

            return new ResourceObservation(observation, observedChunk, resource);
        }

        return null;
    }

    private static int CombineFactors(int urgency, int resourceAvailability, int confidence)
    {
        return ((urgency * 6) + (resourceAvailability * 2) + (confidence * 2)) / 10;
    }

    private static int CompareCandidates(UtilityActionEvaluation left, UtilityActionEvaluation right)
    {
        int validityComparison = right.IsValid.CompareTo(left.IsValid);
        if (validityComparison != 0)
        {
            return validityComparison;
        }

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

    private static Chunk ResolveChunkById(GaiaEngine.Domain.World.World world, ChunkId chunkId)
    {
        foreach (Chunk chunk in world.GetChunks())
        {
            if (chunk.Id == chunkId)
            {
                return chunk;
            }
        }

        throw new InvalidOperationException("The requested chunk could not be resolved in the current world state.");
    }

    private static bool TryResolveObservedResourceChunk(GaiaEngine.Domain.World.World world, ulong objectId, ResourceType resourceType, out Chunk? observedChunk, out ResourceState? resource)
    {
        observedChunk = null;
        resource = null;

        ResourceId resourceId;
        try
        {
            resourceId = new ResourceId(objectId);
        }
        catch (ArgumentException)
        {
            return false;
        }

        foreach (Chunk chunk in world.GetChunks())
        {
            if (!chunk.Resources.TryGet(resourceId, out ResourceState? resolvedResource))
            {
                continue;
            }

            if (resolvedResource!.Type != resourceType)
            {
                return false;
            }

            observedChunk = chunk;
            resource = resolvedResource;
            return true;
        }

        return false;
    }

    private static bool TryResolveRememberedResourceChunk(
        GaiaEngine.Domain.World.World world,
        MemoryEntry entry,
        ResourceType resourceType,
        out Chunk? rememberedChunk,
        out int availability)
    {
        rememberedChunk = null;
        availability = 0;

        foreach (Chunk chunk in world.GetChunks())
        {
            if (chunk.Metadata.Coordinates != entry.Position)
            {
                continue;
            }

            if (resourceType == ResourceType.FreshWater && chunk.Id.Value == entry.Identifier)
            {
                int waterAvailability = Math.Min(1000, chunk.Water.SurfaceWater.WaterLevel);
                if (waterAvailability < 1)
                {
                    return false;
                }

                rememberedChunk = chunk;
                availability = waterAvailability;
                return true;
            }

            foreach (ResourceState resource in chunk.Resources.GetAll())
            {
                if (resource.ResourceId.Value != entry.Identifier || resource.Type != resourceType || resource.CurrentAmount < 1)
                {
                    continue;
                }

                rememberedChunk = chunk;
                availability = resource.Availability;
                return true;
            }

            return false;
        }

        return false;
    }
    private UtilityActionEvaluation CreateInvalidCandidate(SimulationActionType actionType, OrganismId organismId, ChunkId fallbackChunkId, GaiaEngine.Foundation.Determinism.WorldSeed worldSeed, long tick)
    {
        return new UtilityActionEvaluation(
            CreateActionId(organismId, fallbackChunkId.ToString(), actionType, worldSeed, tick),
            actionType,
            new SimulationActionTarget(ActionTargetKind.Chunk, fallbackChunkId.ToString()),
            utilityScore: 0,
            estimatedCost: 0,
            expectedDuration: 0,
            isValid: false);
    }

    private ActionId CreateActionId(OrganismId organismId, string targetId, SimulationActionType actionType, GaiaEngine.Foundation.Determinism.WorldSeed worldSeed, long tick)
    {
        ulong hash = 1469598103934665603UL;
        hash = Mix(hash, organismId.Value);
        hash = Mix(hash, (ulong)tick);
        hash = Mix(hash, (ulong)actionType);
        foreach (char character in targetId)
        {
            hash = Mix(hash, character);
        }

        ulong sequenceValue = hash & EntitySequence.MAX_VALUE;
        if (sequenceValue == 0)
        {
            sequenceValue = 1;
        }

        IdentifierGenerationContext context = new(worldSeed, tick, new EntitySequence(sequenceValue));
        return idGenerator.CreateActionId(context);
    }

    private static ulong Mix(ulong current, ulong value)
    {
        const ulong prime = 1099511628211UL;
        return (current ^ value) * prime;
    }

    private sealed record ResourceObservation(PerceivedObject Observation, Chunk Chunk, ResourceState Resource);

    private sealed record MoveTargetCandidate(Chunk TargetChunk, int Score);
}

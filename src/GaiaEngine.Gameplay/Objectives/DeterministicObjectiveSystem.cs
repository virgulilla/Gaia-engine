using System;
using System.Collections.Generic;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Player;

namespace GaiaEngine.Gameplay.Objectives;

/// <summary>
/// Evaluates objective progress deterministically from profile state and gameplay signals.
/// </summary>
public sealed class DeterministicObjectiveSystem : IObjectiveSystem
{
    private readonly IReadOnlyList<ObjectiveDefinition> definitions;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicObjectiveSystem"/> class.
    /// </summary>
    /// <param name="definitions">The ordered objective definitions to evaluate.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="definitions"/> is <see langword="null"/>.</exception>
    public DeterministicObjectiveSystem(IReadOnlyList<ObjectiveDefinition> definitions)
    {
        this.definitions = definitions ?? throw new ArgumentNullException(nameof(definitions));
    }

    /// <inheritdoc />
    public ObjectiveEvaluationResult Evaluate(PlayerProfile profile, long tick, IReadOnlyList<ObjectiveSignal> signals)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(signals);
        if (tick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tick), "The objective evaluation tick must be zero or greater.");
        }

        List<ObjectiveSignal> orderedSignals = new(signals.Count);
        foreach (ObjectiveSignal signal in signals)
        {
            orderedSignals.Add(signal ?? throw new ArgumentNullException(nameof(signals), "The objective signal list cannot contain null signals."));
        }

        orderedSignals.Sort(static (left, right) =>
        {
            int categoryComparison = left.Category.CompareTo(right.Category);
            if (categoryComparison != 0)
            {
                return categoryComparison;
            }

            return string.CompareOrdinal(left.Key, right.Key);
        });

        List<ObjectiveEntry> updatedEntries = new(definitions.Count);
        List<ObjectiveEntry> completedObjectives = new();
        int experience = profile.Progression.Experience;
        int completedObjectiveCount = 0;

        foreach (ObjectiveDefinition definition in definitions)
        {
            ObjectiveEntry currentEntry = profile.Objectives.TryGet(definition.ObjectiveId, out ObjectiveEntry? existingEntry) && existingEntry is not null
                ? existingEntry
                : CreateEntry(definition);

            ObjectiveEntry updatedEntry = EvaluateEntry(currentEntry, definition, profile, orderedSignals);
            updatedEntries.Add(updatedEntry);

            if (updatedEntry.Status == ObjectiveStatus.Completed)
            {
                completedObjectiveCount++;
            }

            if (currentEntry.Status != ObjectiveStatus.Completed && updatedEntry.Status == ObjectiveStatus.Completed)
            {
                completedObjectives.Add(updatedEntry);
                experience += updatedEntry.Reward.Experience;
            }
        }

        PlayerProfile updatedProfile = new(
            profile.Identity,
            profile.Knowledge,
            new ObjectiveCollection(updatedEntries.AsReadOnly()),
            new PlayerProgression(
                experience,
                profile.Progression.Discoveries,
                profile.Progression.UnlockLevel,
                completedObjectiveCount,
                profile.Progression.Unlocks,
                profile.Progression.CompletedMilestones),
            profile.Achievements,
            profile.Statistics,
            profile.Settings);
        return new ObjectiveEvaluationResult(updatedProfile, completedObjectives.AsReadOnly());
    }

    private static ObjectiveEntry CreateEntry(ObjectiveDefinition definition)
    {
        List<ObjectiveRequirementProgress> progress = new(definition.Requirements.Count);
        foreach (ObjectiveRequirementDefinition requirement in definition.Requirements)
        {
            progress.Add(new ObjectiveRequirementProgress(requirement.RequirementId, 0));
        }

        return new ObjectiveEntry(
            definition.ObjectiveId,
            definition.Category,
            definition.Title,
            definition.Description,
            definition.Requirements,
            progress.AsReadOnly(),
            definition.Reward,
            definition.InitialStatus);
    }

    private static ObjectiveEntry EvaluateEntry(
        ObjectiveEntry currentEntry,
        ObjectiveDefinition definition,
        PlayerProfile profile,
        IReadOnlyList<ObjectiveSignal> signals)
    {
        if (currentEntry.Status == ObjectiveStatus.Completed || currentEntry.Status == ObjectiveStatus.Failed)
        {
            return currentEntry;
        }

        List<ObjectiveRequirementProgress> updatedProgress = new(definition.Requirements.Count);
        bool anyProgress = false;
        bool allCompleted = true;

        for (int index = 0; index < definition.Requirements.Count; index++)
        {
            ObjectiveRequirementDefinition requirement = definition.Requirements[index];
            ObjectiveRequirementProgress currentProgress = currentEntry.GetProgress()[index];
            int nextValue = EvaluateRequirementProgress(requirement, currentProgress.CurrentValue, profile, signals);
            if (nextValue != currentProgress.CurrentValue)
            {
                anyProgress = true;
            }

            updatedProgress.Add(new ObjectiveRequirementProgress(requirement.RequirementId, nextValue));
            if (nextValue < requirement.TargetCount)
            {
                allCompleted = false;
            }
        }

        ObjectiveStatus status = currentEntry.Status;
        if (status == ObjectiveStatus.Hidden && anyProgress)
        {
            status = ObjectiveStatus.Active;
        }

        if (status == ObjectiveStatus.Locked && anyProgress)
        {
            status = ObjectiveStatus.Active;
        }

        if (status == ObjectiveStatus.Active || status == ObjectiveStatus.Hidden || status == ObjectiveStatus.Locked)
        {
            status = allCompleted ? ObjectiveStatus.Completed : status == ObjectiveStatus.Hidden && !anyProgress ? ObjectiveStatus.Hidden : ObjectiveStatus.Active;
        }

        return new ObjectiveEntry(
            currentEntry.ObjectiveId,
            currentEntry.Category,
            currentEntry.Title,
            currentEntry.Description,
            currentEntry.Requirements,
            updatedProgress.AsReadOnly(),
            currentEntry.Reward,
            status);
    }

    private static int EvaluateRequirementProgress(
        ObjectiveRequirementDefinition requirement,
        int currentValue,
        PlayerProfile profile,
        IReadOnlyList<ObjectiveSignal> signals)
    {
        return requirement.Type switch
        {
            ObjectiveRequirementType.Counter or ObjectiveRequirementType.Collection => EvaluateCountRequirement(requirement, profile),
            ObjectiveRequirementType.SingleEvent or ObjectiveRequirementType.Observation or ObjectiveRequirementType.WorldEvent => EvaluateSignalRequirement(requirement, currentValue, signals),
            _ => currentValue,
        };
    }

    private static int EvaluateCountRequirement(ObjectiveRequirementDefinition requirement, PlayerProfile profile)
    {
        if (!requirement.DiscoveryCategory.HasValue)
        {
            return 0;
        }

        int count = 0;
        foreach (DiscoveryEntry entry in profile.Knowledge.Discoveries.GetAll())
        {
            if (entry.Category == requirement.DiscoveryCategory.Value)
            {
                count++;
            }
        }

        return Math.Min(count, requirement.TargetCount);
    }

    private static int EvaluateSignalRequirement(
        ObjectiveRequirementDefinition requirement,
        int currentValue,
        IReadOnlyList<ObjectiveSignal> signals)
    {
        foreach (ObjectiveSignal signal in signals)
        {
            if (requirement.DiscoveryCategory.HasValue && signal.Category != requirement.DiscoveryCategory.Value)
            {
                continue;
            }

            if (requirement.SignalKey is not null && !string.Equals(signal.Key, requirement.SignalKey, StringComparison.Ordinal))
            {
                continue;
            }

            return Math.Min(currentValue + 1, requirement.TargetCount);
        }

        return currentValue;
    }
}

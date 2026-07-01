using System;
using System.Collections.Generic;
using System.Text.Json;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Encyclopedia;
using GaiaEngine.Gameplay.Objectives;
using GaiaEngine.Gameplay.Player;
using GaiaEngine.Gameplay.Progression;
using GaiaEngine.Gameplay.Achievements;
using GaiaEngine.Serialization.Profiles.Documents;

namespace GaiaEngine.Serialization.Profiles;

/// <summary>
/// Serializes player profiles using deterministic JSON payloads.
/// </summary>
public sealed class JsonPlayerProfileSerializer : IPlayerProfileSerializer
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
    };

    /// <summary>
    /// Serializes one player profile into a deterministic payload.
    /// </summary>
    /// <param name="profile">The player profile to serialize.</param>
    /// <returns>The serialized payload.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="profile"/> is <see langword="null"/>.</exception>
    public string Serialize(PlayerProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        return JsonSerializer.Serialize(CreateDocument(profile), SerializerOptions);
    }

    /// <summary>
    /// Deserializes one player profile from a payload.
    /// </summary>
    /// <param name="payload">The serialized payload.</param>
    /// <returns>The restored player profile.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="payload"/> is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the payload is invalid or incomplete.</exception>
    public PlayerProfile Deserialize(string payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
        {
            throw new ArgumentException("The payload must contain a value.", nameof(payload));
        }

        PlayerProfileDocument? document = JsonSerializer.Deserialize<PlayerProfileDocument>(payload, SerializerOptions);
        if (document is null)
        {
            throw new InvalidOperationException("The player profile document could not be deserialized.");
        }

        return CreateProfile(document);
    }

    private static PlayerProfileDocument CreateDocument(PlayerProfile profile)
    {
        List<DiscoveryEntryDocument> discoveries = new(profile.Knowledge.Discoveries.Count);
        List<EncyclopediaEntryDocument> encyclopedia = new(profile.Knowledge.Encyclopedia.Count);
        List<ObjectiveEntryDocument> objectives = new(profile.Objectives.Count);
        List<AchievementEntryDocument> achievements = new(profile.Achievements.Count);
        foreach (DiscoveryEntry entry in profile.Knowledge.Discoveries.GetAll())
        {
            discoveries.Add(
                new DiscoveryEntryDocument
                {
                    DiscoveryId = entry.DiscoveryId,
                    Category = entry.Category.ToString(),
                    Name = entry.Name,
                    Description = entry.Description,
                    UnlockTick = entry.UnlockTick,
                    WorldId = entry.WorldId.ToString(),
                    PlayerId = entry.PlayerId,
                });
        }

        foreach (EncyclopediaEntry entry in profile.Knowledge.Encyclopedia.GetAll())
        {
            List<EncyclopediaStatisticDocument> statistics = new(entry.GetStatistics().Count);
            foreach (EncyclopediaStatistic statistic in entry.GetStatistics())
            {
                statistics.Add(
                    new EncyclopediaStatisticDocument
                    {
                        Key = statistic.Key,
                        Value = statistic.Value,
                    });
            }

            encyclopedia.Add(
                new EncyclopediaEntryDocument
                {
                    EntryId = entry.EntryId,
                    Category = entry.Category.ToString(),
                    Title = entry.Title,
                    Description = entry.Description,
                    UnlockState = entry.UnlockState.ToString(),
                    DiscoveryDate = entry.DiscoveryDate,
                    RelatedEntries = new List<string>(entry.GetRelatedEntries()),
                    Statistics = statistics,
                });
        }

        foreach (ObjectiveEntry entry in profile.Objectives.GetAll())
        {
            List<ObjectiveRequirementDocument> requirements = new(entry.Requirements.Count);
            foreach (ObjectiveRequirementDefinition requirement in entry.Requirements)
            {
                requirements.Add(
                    new ObjectiveRequirementDocument
                    {
                        RequirementId = requirement.RequirementId,
                        Type = requirement.Type.ToString(),
                        TargetCount = requirement.TargetCount,
                        DiscoveryCategory = requirement.DiscoveryCategory?.ToString(),
                        SignalKey = requirement.SignalKey,
                    });
            }

            List<ObjectiveRequirementProgressDocument> progress = new(entry.GetProgress().Count);
            foreach (ObjectiveRequirementProgress requirementProgress in entry.GetProgress())
            {
                progress.Add(
                    new ObjectiveRequirementProgressDocument
                    {
                        RequirementId = requirementProgress.RequirementId,
                        CurrentValue = requirementProgress.CurrentValue,
                    });
            }

            objectives.Add(
                new ObjectiveEntryDocument
                {
                    ObjectiveId = entry.ObjectiveId,
                    Category = entry.Category.ToString(),
                    Title = entry.Title,
                    Description = entry.Description,
                    Requirements = requirements,
                    Progress = progress,
                    Reward = new ObjectiveRewardDocument
                    {
                        Experience = entry.Reward.Experience,
                        Unlocks = new List<string>(entry.Reward.Unlocks),
                    },
                    Status = entry.Status.ToString(),
                });
        }

        foreach (AchievementEntry entry in profile.Achievements.GetAll())
        {
            achievements.Add(
                new AchievementEntryDocument
                {
                    AchievementId = entry.AchievementId,
                    Category = entry.Category.ToString(),
                    Title = entry.Title,
                    Description = entry.Description,
                    TargetValue = entry.TargetValue,
                    CurrentValue = entry.CurrentValue,
                    Reward = new AchievementRewardDocument
                    {
                        RewardId = entry.Reward.RewardId,
                        Category = entry.Reward.Category.ToString(),
                        Name = entry.Reward.Name,
                    },
                    Hidden = entry.Hidden,
                    UnlockDate = entry.UnlockDate,
                });
        }

        return new PlayerProfileDocument
        {
            Identity = new PlayerIdentityDocument
            {
                PlayerId = profile.Identity.PlayerId,
                ProfileName = profile.Identity.ProfileName,
                CreationDate = profile.Identity.CreationDate,
            },
            Discoveries = discoveries,
            Encyclopedia = encyclopedia,
            Objectives = objectives,
            Achievements = achievements,
            Progression = new PlayerProgressionDocument
            {
                Experience = profile.Progression.Experience,
                Discoveries = profile.Progression.Discoveries,
                UnlockLevel = profile.Progression.UnlockLevel,
                CompletedObjectives = profile.Progression.CompletedObjectives,
                Unlocks = new List<string>(profile.Progression.Unlocks.GetAll()),
                CompletedMilestones = new List<string>(profile.Progression.CompletedMilestones.GetAll()),
            },
            Statistics = new PlayerStatisticsDocument
            {
                TotalDiscoveriesUnlocked = profile.Statistics.TotalDiscoveriesUnlocked,
                DuplicateDiscoveryObservations = profile.Statistics.DuplicateDiscoveryObservations,
            },
            Settings = new PlayerSettingsDocument
            {
                Language = profile.Settings.Language,
                BrightnessPercent = profile.Settings.BrightnessPercent,
                MasterVolumePercent = profile.Settings.MasterVolumePercent,
                MusicVolumePercent = profile.Settings.MusicVolumePercent,
                EffectsVolumePercent = profile.Settings.EffectsVolumePercent,
                ControllerSupportEnabled = profile.Settings.ControllerSupportEnabled,
                Accessibility = new AccessibilitySettingsDocument
                {
                    HighContrastMode = profile.Settings.Accessibility.HighContrastMode,
                    LargeText = profile.Settings.Accessibility.LargeText,
                    UiScalePercent = profile.Settings.Accessibility.UiScalePercent,
                    ColorProfile = profile.Settings.Accessibility.ColorProfile.ToString(),
                    ReducedMotion = profile.Settings.Accessibility.ReducedMotion,
                    SubtitleSizePercent = profile.Settings.Accessibility.SubtitleSizePercent,
                    SimplifiedNotifications = profile.Settings.Accessibility.SimplifiedNotifications,
                    VisualEventIndicators = profile.Settings.Accessibility.VisualEventIndicators,
                    LargeTouchTargets = profile.Settings.Accessibility.LargeTouchTargets,
                    ToggleInsteadOfHold = profile.Settings.Accessibility.ToggleInsteadOfHold,
                    HoldDurationMilliseconds = profile.Settings.Accessibility.HoldDurationMilliseconds,
                },
            },
        };
    }

    private static PlayerProfile CreateProfile(PlayerProfileDocument document)
    {
        if (document.Identity is null)
        {
            throw new InvalidOperationException("The player identity section is required.");
        }

        if (document.Progression is null)
        {
            throw new InvalidOperationException("The player progression section is required.");
        }

        if (document.Statistics is null)
        {
            throw new InvalidOperationException("The player statistics section is required.");
        }

        List<DiscoveryEntry> discoveries = new(document.Discoveries.Count);
        foreach (DiscoveryEntryDocument entry in document.Discoveries)
        {
            discoveries.Add(
                new DiscoveryEntry(
                    entry.DiscoveryId,
                    Enum.Parse<DiscoveryCategory>(entry.Category, ignoreCase: false),
                    entry.Name,
                    entry.Description,
                    entry.UnlockTick,
                    WorldId.Parse(entry.WorldId),
                    entry.PlayerId));
        }

        List<EncyclopediaEntry> encyclopediaEntries = new(document.Encyclopedia.Count);
        foreach (EncyclopediaEntryDocument entry in document.Encyclopedia)
        {
            List<EncyclopediaStatistic> statistics = new(entry.Statistics.Count);
            foreach (EncyclopediaStatisticDocument statistic in entry.Statistics)
            {
                statistics.Add(new EncyclopediaStatistic(statistic.Key, statistic.Value));
            }

            encyclopediaEntries.Add(
                new EncyclopediaEntry(
                    entry.EntryId,
                    Enum.Parse<EncyclopediaCategory>(entry.Category, ignoreCase: false),
                    entry.Title,
                    entry.Description,
                    Enum.Parse<EncyclopediaUnlockState>(entry.UnlockState, ignoreCase: false),
                    entry.DiscoveryDate,
                    entry.RelatedEntries.AsReadOnly(),
                    statistics.AsReadOnly()));
        }

        List<ObjectiveEntry> objectiveEntries = new(document.Objectives.Count);
        foreach (ObjectiveEntryDocument entry in document.Objectives)
        {
            if (entry.Reward is null)
            {
                throw new InvalidOperationException("Each objective entry requires a reward section.");
            }

            List<ObjectiveRequirementDefinition> requirements = new(entry.Requirements.Count);
            foreach (ObjectiveRequirementDocument requirement in entry.Requirements)
            {
                requirements.Add(
                    new ObjectiveRequirementDefinition(
                        requirement.RequirementId,
                        Enum.Parse<ObjectiveRequirementType>(requirement.Type, ignoreCase: false),
                        requirement.TargetCount,
                        requirement.DiscoveryCategory is null ? null : Enum.Parse<DiscoveryCategory>(requirement.DiscoveryCategory, ignoreCase: false),
                        requirement.SignalKey));
            }

            List<ObjectiveRequirementProgress> progress = new(entry.Progress.Count);
            foreach (ObjectiveRequirementProgressDocument requirementProgress in entry.Progress)
            {
                progress.Add(new ObjectiveRequirementProgress(requirementProgress.RequirementId, requirementProgress.CurrentValue));
            }

            objectiveEntries.Add(
                new ObjectiveEntry(
                    entry.ObjectiveId,
                    Enum.Parse<ObjectiveCategory>(entry.Category, ignoreCase: false),
                    entry.Title,
                    entry.Description,
                    requirements.AsReadOnly(),
                    progress.AsReadOnly(),
                    new ObjectiveRewardDefinition(entry.Reward.Experience, entry.Reward.Unlocks.AsReadOnly()),
                    Enum.Parse<ObjectiveStatus>(entry.Status, ignoreCase: false)));
        }

        List<AchievementEntry> achievementEntries = new(document.Achievements.Count);
        foreach (AchievementEntryDocument entry in document.Achievements)
        {
            if (entry.Reward is null)
            {
                throw new InvalidOperationException("Each achievement entry requires a reward section.");
            }

            achievementEntries.Add(
                new AchievementEntry(
                    entry.AchievementId,
                    Enum.Parse<AchievementCategory>(entry.Category, ignoreCase: false),
                    entry.Title,
                    entry.Description,
                    entry.TargetValue,
                    entry.CurrentValue,
                    new AchievementRewardDefinition(
                        entry.Reward.RewardId,
                        Enum.Parse<AchievementRewardCategory>(entry.Reward.Category, ignoreCase: false),
                        entry.Reward.Name),
                    entry.Hidden,
                    entry.UnlockDate));
        }

        PlayerSettings settings = document.Settings is null
            ? PlayerSettings.Default
            : new PlayerSettings(
                document.Settings.Language,
                document.Settings.Accessibility is null
                    ? AccessibilitySettings.Default
                    : new AccessibilitySettings(
                        document.Settings.Accessibility.HighContrastMode,
                        document.Settings.Accessibility.LargeText,
                        document.Settings.Accessibility.UiScalePercent,
                        Enum.Parse<AccessibilityColorProfile>(document.Settings.Accessibility.ColorProfile, ignoreCase: false),
                        document.Settings.Accessibility.ReducedMotion,
                        document.Settings.Accessibility.SubtitleSizePercent,
                        document.Settings.Accessibility.SimplifiedNotifications,
                        document.Settings.Accessibility.VisualEventIndicators,
                        document.Settings.Accessibility.LargeTouchTargets,
                        document.Settings.Accessibility.ToggleInsteadOfHold,
                        document.Settings.Accessibility.HoldDurationMilliseconds),
                document.Settings.BrightnessPercent,
                document.Settings.MasterVolumePercent,
                document.Settings.MusicVolumePercent,
                document.Settings.EffectsVolumePercent,
                document.Settings.ControllerSupportEnabled);

        return new PlayerProfile(
            new PlayerIdentity(document.Identity.PlayerId, document.Identity.ProfileName, document.Identity.CreationDate),
            new PlayerKnowledge(
                new DiscoveryCollection(discoveries.AsReadOnly()),
                new EncyclopediaCollection(encyclopediaEntries.AsReadOnly())),
            new ObjectiveCollection(objectiveEntries.AsReadOnly()),
            new PlayerProgression(
                document.Progression.Experience,
                document.Progression.Discoveries,
                document.Progression.UnlockLevel,
                document.Progression.CompletedObjectives,
                new ProgressionUnlockCollection(document.Progression.Unlocks.AsReadOnly()),
                new ProgressionMilestoneCollection(document.Progression.CompletedMilestones.AsReadOnly())),
            new AchievementCollection(achievementEntries.AsReadOnly()),
            new PlayerStatistics(document.Statistics.TotalDiscoveriesUnlocked, document.Statistics.DuplicateDiscoveryObservations),
            settings);
    }
}

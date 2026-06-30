using System;
using System.Collections.Generic;
using GaiaEngine.App.Bootstrap;
using GaiaEngine.Gameplay.Achievements;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Objectives;
using GaiaEngine.Godot.UI.Notifications;
using Godot;

namespace GaiaEngine.Godot.Bootstrap;

/// <summary>
/// Adapts the Godot scene lifecycle to the Gaia Engine application bootstrap.
/// </summary>
public sealed partial class GaiaEngineBootstrap : Node
{
    private const string WorldNameLabelPath = "HudLayer/HudRoot/TopBar/TopBarMargin/TopBarRow/WorldNameLabel";
    private const string TimeSummaryLabelPath = "HudLayer/HudRoot/TopBar/TopBarMargin/TopBarRow/TimeSummaryLabel";
    private const string TickRateLabelPath = "HudLayer/HudRoot/TopBar/TopBarMargin/TopBarRow/TickRateChip/TickRateChipMargin/TickRateLabel";
    private const string PopulationLabelPath = "HudLayer/HudRoot/RightPanel/RightPanelMargin/RightPanelColumn/PopulationLabel";
    private const string AlivePopulationLabelPath = "HudLayer/HudRoot/RightPanel/RightPanelMargin/RightPanelColumn/AlivePopulationLabel";
    private const string SpeciesCountLabelPath = "HudLayer/HudRoot/RightPanel/RightPanelMargin/RightPanelColumn/SpeciesCountLabel";
    private const string WeatherLabelPath = "HudLayer/HudRoot/RightPanel/RightPanelMargin/RightPanelColumn/WeatherLabel";
    private const string BiomeLabelPath = "HudLayer/HudRoot/RightPanel/RightPanelMargin/RightPanelColumn/BiomeLabel";
    private const string TemperatureLabelPath = "HudLayer/HudRoot/RightPanel/RightPanelMargin/RightPanelColumn/TemperatureLabel";
    private const string HumidityLabelPath = "HudLayer/HudRoot/RightPanel/RightPanelMargin/RightPanelColumn/HumidityLabel";
    private const string MemoryCountLabelPath = "HudLayer/HudRoot/RightPanel/RightPanelMargin/RightPanelColumn/MemoryCountLabel";
    private const string ActionCountLabelPath = "HudLayer/HudRoot/RightPanel/RightPanelMargin/RightPanelColumn/ActionCountLabel";
    private const string DiscoveryCountLabelPath = "HudLayer/HudRoot/RightPanel/RightPanelMargin/RightPanelColumn/DiscoveryCountLabel";
    private const string EncyclopediaCountLabelPath = "HudLayer/HudRoot/RightPanel/RightPanelMargin/RightPanelColumn/EncyclopediaCountLabel";
    private const string ObjectiveCountLabelPath = "HudLayer/HudRoot/RightPanel/RightPanelMargin/RightPanelColumn/ObjectiveCountLabel";
    private const string LevelLabelPath = "HudLayer/HudRoot/RightPanel/RightPanelMargin/RightPanelColumn/LevelLabel";
    private const string AchievementCountLabelPath = "HudLayer/HudRoot/RightPanel/RightPanelMargin/RightPanelColumn/AchievementCountLabel";
    private const string NotificationCard1Path = "HudLayer/HudRoot/NotificationArea/NotificationCard1";
    private const string NotificationCard2Path = "HudLayer/HudRoot/NotificationArea/NotificationCard2";
    private const string NotificationCard3Path = "HudLayer/HudRoot/NotificationArea/NotificationCard3";
    private const string NotificationTitleLabel1Path = "HudLayer/HudRoot/NotificationArea/NotificationCard1/NotificationMargin1/NotificationColumn1/NotificationTitleLabel1";
    private const string NotificationTitleLabel2Path = "HudLayer/HudRoot/NotificationArea/NotificationCard2/NotificationMargin2/NotificationColumn2/NotificationTitleLabel2";
    private const string NotificationTitleLabel3Path = "HudLayer/HudRoot/NotificationArea/NotificationCard3/NotificationMargin3/NotificationColumn3/NotificationTitleLabel3";
    private const string NotificationBodyLabel1Path = "HudLayer/HudRoot/NotificationArea/NotificationCard1/NotificationMargin1/NotificationColumn1/NotificationBodyLabel1";
    private const string NotificationBodyLabel2Path = "HudLayer/HudRoot/NotificationArea/NotificationCard2/NotificationMargin2/NotificationColumn2/NotificationBodyLabel2";
    private const string NotificationBodyLabel3Path = "HudLayer/HudRoot/NotificationArea/NotificationCard3/NotificationMargin3/NotificationColumn3/NotificationBodyLabel3";

    private GaiaEngineApplication? application;
    private GaiaEngineRuntime? runtime;
    private Label? worldNameLabel;
    private Label? timeSummaryLabel;
    private Label? tickRateLabel;
    private Label? populationLabel;
    private Label? alivePopulationLabel;
    private Label? speciesCountLabel;
    private Label? weatherLabel;
    private Label? biomeLabel;
    private Label? temperatureLabel;
    private Label? humidityLabel;
    private Label? memoryCountLabel;
    private Label? actionCountLabel;
    private Label? discoveryCountLabel;
    private Label? encyclopediaCountLabel;
    private Label? objectiveCountLabel;
    private Label? levelLabel;
    private Label? achievementCountLabel;
    private PanelContainer[]? notificationCards;
    private Label[]? notificationTitleLabels;
    private Label[]? notificationBodyLabels;
    private HudNotificationQueue? notificationQueue;
    private double tickAccumulator;
    private HudViewSnapshot? lastSnapshot;
    private RuntimeObservationSnapshot? lastObservedState;

    /// <summary>
    /// Initializes the application when the root scene enters the tree.
    /// </summary>
    public override void _Ready()
    {
        string engineConfigurationPath = ProjectSettings.GlobalizePath("res://Configuration/Engine/EngineConfiguration.json");
        string simulationConfigurationPath = ProjectSettings.GlobalizePath("res://Configuration/Simulation/SimulationConfiguration.json");
        string worldConfigurationPath = ProjectSettings.GlobalizePath("res://Configuration/World/WorldConfiguration.json");
        application = GaiaEngineCompositionRoot.CreateApplication(engineConfigurationPath, simulationConfigurationPath, worldConfigurationPath);
        runtime = application.Initialize();
        worldNameLabel = GetNode<Label>(WorldNameLabelPath);
        timeSummaryLabel = GetNode<Label>(TimeSummaryLabelPath);
        tickRateLabel = GetNode<Label>(TickRateLabelPath);
        populationLabel = GetNode<Label>(PopulationLabelPath);
        alivePopulationLabel = GetNode<Label>(AlivePopulationLabelPath);
        speciesCountLabel = GetNode<Label>(SpeciesCountLabelPath);
        weatherLabel = GetNode<Label>(WeatherLabelPath);
        biomeLabel = GetNode<Label>(BiomeLabelPath);
        temperatureLabel = GetNode<Label>(TemperatureLabelPath);
        humidityLabel = GetNode<Label>(HumidityLabelPath);
        memoryCountLabel = GetNode<Label>(MemoryCountLabelPath);
        actionCountLabel = GetNode<Label>(ActionCountLabelPath);
        discoveryCountLabel = GetNode<Label>(DiscoveryCountLabelPath);
        encyclopediaCountLabel = GetNode<Label>(EncyclopediaCountLabelPath);
        objectiveCountLabel = GetNode<Label>(ObjectiveCountLabelPath);
        levelLabel = GetNode<Label>(LevelLabelPath);
        achievementCountLabel = GetNode<Label>(AchievementCountLabelPath);
        notificationCards =
        [
            GetNode<PanelContainer>(NotificationCard1Path),
            GetNode<PanelContainer>(NotificationCard2Path),
            GetNode<PanelContainer>(NotificationCard3Path),
        ];
        notificationTitleLabels =
        [
            GetNode<Label>(NotificationTitleLabel1Path),
            GetNode<Label>(NotificationTitleLabel2Path),
            GetNode<Label>(NotificationTitleLabel3Path),
        ];
        notificationBodyLabels =
        [
            GetNode<Label>(NotificationBodyLabel1Path),
            GetNode<Label>(NotificationBodyLabel2Path),
            GetNode<Label>(NotificationBodyLabel3Path),
        ];
        notificationQueue = new HudNotificationQueue(activeLimit: 3, historyLimit: 24);
        lastObservedState = CreateObservationSnapshot();
        notificationQueue.Enqueue(
            new HudNotificationEntry(
                "system.startup",
                HudNotificationCategory.System,
                HudNotificationPriority.Low,
                "Observation Mode",
                "The simulation is running. Notifications will appear here as discoveries, goals and world changes happen.",
                runtime.World.TimeState.CurrentTick,
                GetDurationSeconds(HudNotificationPriority.Low),
                actionLabel: null));

        UpdateSimulationStatusText();
        UpdateNotificationWidgets();
        GD.Print($"Gaia Engine initialized with tick rate {runtime.EngineConfiguration.TickRate}.");
    }

    /// <summary>
    /// Advances the minimal simulation session and refreshes the HUD.
    /// </summary>
    /// <param name="delta">The real elapsed frame time.</param>
    public override void _Process(double delta)
    {
        if (runtime is null || notificationQueue is null)
        {
            return;
        }

        double secondsPerTick = 1d / runtime.EngineConfiguration.TickRate;
        tickAccumulator += delta;
        bool advancedSimulation = false;

        while (tickAccumulator >= secondsPerTick)
        {
            runtime.AdvanceTick();
            tickAccumulator -= secondsPerTick;
            advancedSimulation = true;
        }

        if (advancedSimulation)
        {
            IReadOnlyList<HudNotificationEntry> entries = BuildNotificationsFromObservedChanges();
            notificationQueue.EnqueueRange(entries);
        }

        notificationQueue.Advance(delta);
        UpdateSimulationStatusText();
        UpdateNotificationWidgets();
    }

    private void UpdateSimulationStatusText()
    {
        if (runtime is null
            || worldNameLabel is null
            || timeSummaryLabel is null
            || tickRateLabel is null
            || populationLabel is null
            || alivePopulationLabel is null
            || speciesCountLabel is null
            || weatherLabel is null
            || biomeLabel is null
            || temperatureLabel is null
            || humidityLabel is null
            || memoryCountLabel is null
            || actionCountLabel is null
            || discoveryCountLabel is null
            || encyclopediaCountLabel is null
            || objectiveCountLabel is null
            || levelLabel is null
            || achievementCountLabel is null)
        {
            return;
        }

        int aliveOrganisms = CountAliveOrganisms();
        int memoryEntries = CountMemoryEntries();
        GaiaEngine.Domain.World.Chunk primaryChunk = runtime.World.GetChunks()[0];
        HudViewSnapshot snapshot = new(
            runtime.World.Metadata.WorldName,
            $"Day {runtime.SimulationSession.CurrentTimeState.CurrentDay} - {runtime.SimulationSession.CurrentTimeState.CurrentSeason} - Year {runtime.SimulationSession.CurrentTimeState.CurrentYear} - Tick {runtime.SimulationSession.CurrentTimeState.CurrentTick}",
            $"Tick Rate: {runtime.EngineConfiguration.TickRate}",
            $"Population: {runtime.Organisms.Count}",
            $"Alive: {aliveOrganisms}",
            $"Species: {runtime.Species.Count}",
            $"Weather: {primaryChunk.Climate.WeatherState}",
            $"Biome: {primaryChunk.Biome.Name}",
            $"Temperature: {primaryChunk.Climate.Temperature.CurrentTemperature} C",
            $"Humidity: {primaryChunk.Climate.Humidity.RelativeHumidity}%",
            $"Memory Entries: {memoryEntries}",
            $"Actions: {runtime.ActionRequests.Count}",
            $"Discoveries: {runtime.PlayerProfile.Knowledge.Discoveries.Count}",
            $"Encyclopedia: {runtime.PlayerProfile.Knowledge.Encyclopedia.Count}",
            $"Objectives: {runtime.PlayerProfile.Progression.CompletedObjectives} / {runtime.PlayerProfile.Objectives.Count}",
            $"Level: {runtime.PlayerProfile.Progression.UnlockLevel}",
            $"Achievements: {runtime.PlayerProfile.Achievements.Count}");

        if (snapshot == lastSnapshot)
        {
            return;
        }

        lastSnapshot = snapshot;
        worldNameLabel.Text = snapshot.WorldName;
        timeSummaryLabel.Text = snapshot.TimeSummary;
        tickRateLabel.Text = snapshot.TickRate;
        populationLabel.Text = snapshot.Population;
        alivePopulationLabel.Text = snapshot.AlivePopulation;
        speciesCountLabel.Text = snapshot.SpeciesCount;
        weatherLabel.Text = snapshot.Weather;
        biomeLabel.Text = snapshot.Biome;
        temperatureLabel.Text = snapshot.Temperature;
        humidityLabel.Text = snapshot.Humidity;
        memoryCountLabel.Text = snapshot.MemoryCount;
        actionCountLabel.Text = snapshot.ActionCount;
        discoveryCountLabel.Text = snapshot.DiscoveryCount;
        encyclopediaCountLabel.Text = snapshot.EncyclopediaCount;
        objectiveCountLabel.Text = snapshot.ObjectiveCount;
        levelLabel.Text = snapshot.Level;
        achievementCountLabel.Text = snapshot.AchievementCount;
    }

    private IReadOnlyList<HudNotificationEntry> BuildNotificationsFromObservedChanges()
    {
        if (runtime is null)
        {
            return Array.Empty<HudNotificationEntry>();
        }

        RuntimeObservationSnapshot currentState = CreateObservationSnapshot();
        if (lastObservedState is null)
        {
            lastObservedState = currentState;
            return Array.Empty<HudNotificationEntry>();
        }

        List<HudNotificationEntry> notifications = new();
        AppendSeasonAndWeatherNotifications(notifications, lastObservedState, currentState);
        AppendDiscoveryNotifications(notifications, lastObservedState, currentState);
        AppendObjectiveNotifications(notifications, lastObservedState, currentState);
        AppendAchievementNotifications(notifications, lastObservedState, currentState);
        AppendProgressionNotifications(notifications, lastObservedState, currentState);
        AppendPopulationWarningNotifications(notifications, lastObservedState, currentState);
        lastObservedState = currentState;
        return notifications.AsReadOnly();
    }

    private void AppendSeasonAndWeatherNotifications(
        List<HudNotificationEntry> notifications,
        RuntimeObservationSnapshot previousState,
        RuntimeObservationSnapshot currentState)
    {
        if (previousState.Season != currentState.Season)
        {
            notifications.Add(
                new HudNotificationEntry(
                    $"season.{currentState.Tick}.{currentState.Season}",
                    HudNotificationCategory.Information,
                    HudNotificationPriority.Normal,
                    "Season Changed",
                    $"The world has entered {currentState.Season}.",
                    currentState.Tick,
                    GetDurationSeconds(HudNotificationPriority.Normal),
                    actionLabel: null));
        }

        if (previousState.Weather != currentState.Weather)
        {
            HudNotificationCategory category = currentState.Weather is GaiaEngine.Domain.World.WeatherState.Storm or GaiaEngine.Domain.World.WeatherState.Drought
                ? HudNotificationCategory.Warning
                : HudNotificationCategory.Information;
            HudNotificationPriority priority = currentState.Weather is GaiaEngine.Domain.World.WeatherState.Storm or GaiaEngine.Domain.World.WeatherState.Drought
                ? HudNotificationPriority.High
                : HudNotificationPriority.Normal;
            notifications.Add(
                new HudNotificationEntry(
                    $"weather.{currentState.Tick}.{currentState.Weather}",
                    category,
                    priority,
                    "Weather Update",
                    $"Weather changed to {currentState.Weather} in {currentState.BiomeName}.",
                    currentState.Tick,
                    GetDurationSeconds(priority),
                    actionLabel: null));
        }
    }

    private void AppendDiscoveryNotifications(
        List<HudNotificationEntry> notifications,
        RuntimeObservationSnapshot previousState,
        RuntimeObservationSnapshot currentState)
    {
        Dictionary<DiscoveryCategory, List<DiscoveryEntry>> groupedDiscoveries = new();
        foreach (DiscoveryEntry entry in currentState.Discoveries)
        {
            if (previousState.DiscoveryIds.Contains(entry.DiscoveryId))
            {
                continue;
            }

            if (!groupedDiscoveries.TryGetValue(entry.Category, out List<DiscoveryEntry>? entries))
            {
                entries = new List<DiscoveryEntry>();
                groupedDiscoveries.Add(entry.Category, entries);
            }

            entries.Add(entry);
        }

        foreach ((DiscoveryCategory category, List<DiscoveryEntry> entries) in groupedDiscoveries)
        {
            if (entries.Count == 1)
            {
                DiscoveryEntry entry = entries[0];
                notifications.Add(
                    new HudNotificationEntry(
                        $"discovery.{entry.DiscoveryId}",
                        HudNotificationCategory.Discovery,
                        HudNotificationPriority.High,
                        "New Discovery",
                        $"{entry.Name} discovered. {entry.Description}",
                        currentState.Tick,
                        GetDurationSeconds(HudNotificationPriority.High),
                        actionLabel: "Encyclopedia"));
                continue;
            }

            notifications.Add(
                new HudNotificationEntry(
                    $"discovery.group.{currentState.Tick}.{category}",
                    HudNotificationCategory.Discovery,
                    HudNotificationPriority.High,
                    "Discovery Group",
                    $"{entries.Count} new {GetDiscoveryCategoryLabel(category)} discoveries were recorded.",
                    currentState.Tick,
                    GetDurationSeconds(HudNotificationPriority.High),
                    actionLabel: "Encyclopedia"));
        }
    }

    private void AppendObjectiveNotifications(
        List<HudNotificationEntry> notifications,
        RuntimeObservationSnapshot previousState,
        RuntimeObservationSnapshot currentState)
    {
        List<ObjectiveEntry> completedObjectives = new();
        foreach (ObjectiveEntry entry in currentState.Objectives)
        {
            previousState.ObjectiveStatusById.TryGetValue(entry.ObjectiveId, out ObjectiveStatus previousStatus);
            if (entry.Status == ObjectiveStatus.Completed && previousStatus != ObjectiveStatus.Completed)
            {
                completedObjectives.Add(entry);
            }
        }

        if (completedObjectives.Count == 0)
        {
            return;
        }

        if (completedObjectives.Count == 1)
        {
            ObjectiveEntry entry = completedObjectives[0];
            notifications.Add(
                new HudNotificationEntry(
                    $"objective.{entry.ObjectiveId}",
                    HudNotificationCategory.Objective,
                    HudNotificationPriority.High,
                    "Objective Complete",
                    $"{entry.Title} completed.",
                    currentState.Tick,
                    GetDurationSeconds(HudNotificationPriority.High),
                    actionLabel: "Open Objective"));
            return;
        }

        notifications.Add(
            new HudNotificationEntry(
                $"objective.group.{currentState.Tick}",
                HudNotificationCategory.Objective,
                HudNotificationPriority.High,
                "Objectives Complete",
                $"{completedObjectives.Count} objectives were completed.",
                currentState.Tick,
                GetDurationSeconds(HudNotificationPriority.High),
                actionLabel: "Open Objective"));
    }

    private void AppendAchievementNotifications(
        List<HudNotificationEntry> notifications,
        RuntimeObservationSnapshot previousState,
        RuntimeObservationSnapshot currentState)
    {
        List<AchievementEntry> unlockedAchievements = new();
        foreach (AchievementEntry entry in currentState.Achievements)
        {
            if (!entry.IsUnlocked || previousState.UnlockedAchievementIds.Contains(entry.AchievementId))
            {
                continue;
            }

            unlockedAchievements.Add(entry);
        }

        if (unlockedAchievements.Count == 0)
        {
            return;
        }

        if (unlockedAchievements.Count == 1)
        {
            AchievementEntry entry = unlockedAchievements[0];
            notifications.Add(
                new HudNotificationEntry(
                    $"achievement.{entry.AchievementId}",
                    HudNotificationCategory.Achievement,
                    HudNotificationPriority.High,
                    "Achievement Unlocked",
                    $"{entry.Title} unlocked.",
                    currentState.Tick,
                    GetDurationSeconds(HudNotificationPriority.High),
                    actionLabel: null));
            return;
        }

        notifications.Add(
            new HudNotificationEntry(
                $"achievement.group.{currentState.Tick}",
                HudNotificationCategory.Achievement,
                HudNotificationPriority.High,
                "Achievements Unlocked",
                $"{unlockedAchievements.Count} achievements were unlocked.",
                currentState.Tick,
                GetDurationSeconds(HudNotificationPriority.High),
                actionLabel: null));
    }

    private void AppendProgressionNotifications(
        List<HudNotificationEntry> notifications,
        RuntimeObservationSnapshot previousState,
        RuntimeObservationSnapshot currentState)
    {
        if (currentState.UnlockLevel > previousState.UnlockLevel)
        {
            notifications.Add(
                new HudNotificationEntry(
                    $"level.{currentState.Tick}.{currentState.UnlockLevel}",
                    HudNotificationCategory.Achievement,
                    HudNotificationPriority.High,
                    "Level Reached",
                    $"Observer level {currentState.UnlockLevel} reached.",
                    currentState.Tick,
                    GetDurationSeconds(HudNotificationPriority.High),
                    actionLabel: null));
        }

        List<string> newUnlocks = new();
        foreach (string unlockId in currentState.UnlockIds)
        {
            if (!previousState.UnlockIdSet.Contains(unlockId))
            {
                newUnlocks.Add(unlockId);
            }
        }

        if (newUnlocks.Count == 1)
        {
            notifications.Add(
                new HudNotificationEntry(
                    $"unlock.{newUnlocks[0]}",
                    HudNotificationCategory.System,
                    HudNotificationPriority.Normal,
                    "New Unlock",
                    $"{HumanizeIdentifier(newUnlocks[0])} is now available.",
                    currentState.Tick,
                    GetDurationSeconds(HudNotificationPriority.Normal),
                    actionLabel: null));
        }
        else if (newUnlocks.Count > 1)
        {
            notifications.Add(
                new HudNotificationEntry(
                    $"unlock.group.{currentState.Tick}",
                    HudNotificationCategory.System,
                    HudNotificationPriority.Normal,
                    "New Unlocks",
                    $"{newUnlocks.Count} new unlocks became available.",
                    currentState.Tick,
                    GetDurationSeconds(HudNotificationPriority.Normal),
                    actionLabel: null));
        }

        List<string> newMilestones = new();
        foreach (string milestoneId in currentState.MilestoneIds)
        {
            if (!previousState.MilestoneIdSet.Contains(milestoneId))
            {
                newMilestones.Add(milestoneId);
            }
        }

        if (newMilestones.Count > 0)
        {
            notifications.Add(
                new HudNotificationEntry(
                    $"milestone.group.{currentState.Tick}",
                    HudNotificationCategory.Achievement,
                    HudNotificationPriority.High,
                    "Milestone Reached",
                    newMilestones.Count == 1
                        ? $"{HumanizeIdentifier(newMilestones[0])} was completed."
                        : $"{newMilestones.Count} progression milestones were completed.",
                    currentState.Tick,
                    GetDurationSeconds(HudNotificationPriority.High),
                    actionLabel: null));
        }
    }

    private void AppendPopulationWarningNotifications(
        List<HudNotificationEntry> notifications,
        RuntimeObservationSnapshot previousState,
        RuntimeObservationSnapshot currentState)
    {
        if (previousState.AlivePopulation <= 0)
        {
            return;
        }

        int populationLoss = previousState.AlivePopulation - currentState.AlivePopulation;
        if (populationLoss <= 0)
        {
            return;
        }

        if (currentState.AlivePopulation == 0)
        {
            notifications.Add(
                new HudNotificationEntry(
                    $"population.zero.{currentState.Tick}",
                    HudNotificationCategory.Critical,
                    HudNotificationPriority.Critical,
                    "Population Collapse",
                    "All observed organisms are gone from the current simulation state.",
                    currentState.Tick,
                    durationSeconds: 0d,
                    actionLabel: null));
            return;
        }

        if (populationLoss * 4 >= previousState.AlivePopulation)
        {
            notifications.Add(
                new HudNotificationEntry(
                    $"population.drop.{currentState.Tick}",
                    HudNotificationCategory.Warning,
                    HudNotificationPriority.High,
                    "Population Warning",
                    $"Observed population dropped from {previousState.AlivePopulation} to {currentState.AlivePopulation}.",
                    currentState.Tick,
                    GetDurationSeconds(HudNotificationPriority.High),
                    actionLabel: null));
        }
    }

    private RuntimeObservationSnapshot CreateObservationSnapshot()
    {
        if (runtime is null)
        {
            throw new InvalidOperationException("The runtime must be initialized before creating observations.");
        }

        List<DiscoveryEntry> discoveries = new();
        HashSet<string> discoveryIds = new(StringComparer.Ordinal);
        foreach (DiscoveryEntry entry in runtime.PlayerProfile.Knowledge.Discoveries.GetAll())
        {
            discoveries.Add(entry);
            discoveryIds.Add(entry.DiscoveryId);
        }

        List<ObjectiveEntry> objectives = new();
        Dictionary<string, ObjectiveStatus> objectiveStatusById = new(StringComparer.Ordinal);
        foreach (ObjectiveEntry entry in runtime.PlayerProfile.Objectives.GetAll())
        {
            objectives.Add(entry);
            objectiveStatusById[entry.ObjectiveId] = entry.Status;
        }

        List<AchievementEntry> achievements = new();
        HashSet<string> unlockedAchievementIds = new(StringComparer.Ordinal);
        foreach (AchievementEntry entry in runtime.PlayerProfile.Achievements.GetAll())
        {
            achievements.Add(entry);
            if (entry.IsUnlocked)
            {
                unlockedAchievementIds.Add(entry.AchievementId);
            }
        }

        List<string> unlockIds = new();
        HashSet<string> unlockIdSet = new(StringComparer.Ordinal);
        foreach (string unlockId in runtime.PlayerProfile.Progression.Unlocks.GetAll())
        {
            unlockIds.Add(unlockId);
            unlockIdSet.Add(unlockId);
        }

        List<string> milestoneIds = new();
        HashSet<string> milestoneIdSet = new(StringComparer.Ordinal);
        foreach (string milestoneId in runtime.PlayerProfile.Progression.CompletedMilestones.GetAll())
        {
            milestoneIds.Add(milestoneId);
            milestoneIdSet.Add(milestoneId);
        }

        GaiaEngine.Domain.World.Chunk primaryChunk = runtime.World.GetChunks()[0];
        return new RuntimeObservationSnapshot(
            runtime.World.TimeState.CurrentTick,
            runtime.World.TimeState.CurrentSeason,
            primaryChunk.Climate.WeatherState,
            primaryChunk.Biome.Name,
            CountAliveOrganisms(),
            discoveries.AsReadOnly(),
            discoveryIds,
            objectives.AsReadOnly(),
            objectiveStatusById,
            achievements.AsReadOnly(),
            unlockedAchievementIds,
            runtime.PlayerProfile.Progression.UnlockLevel,
            unlockIds.AsReadOnly(),
            unlockIdSet,
            milestoneIds.AsReadOnly(),
            milestoneIdSet);
    }

    private void UpdateNotificationWidgets()
    {
        if (notificationQueue is null
            || notificationCards is null
            || notificationTitleLabels is null
            || notificationBodyLabels is null)
        {
            return;
        }

        IReadOnlyList<HudNotificationEntry> activeNotifications = notificationQueue.GetActive();
        for (int index = 0; index < notificationCards.Length; index++)
        {
            bool isVisible = index < activeNotifications.Count;
            notificationCards[index].Visible = isVisible;
            if (!isVisible)
            {
                notificationTitleLabels[index].Text = string.Empty;
                notificationBodyLabels[index].Text = string.Empty;
                continue;
            }

            HudNotificationEntry entry = activeNotifications[index];
            notificationTitleLabels[index].Text = GetNotificationPrefix(entry.Category) + entry.Title;
            notificationBodyLabels[index].Text = entry.Message;
        }
    }

    private int CountAliveOrganisms()
    {
        if (runtime is null)
        {
            return 0;
        }

        int aliveOrganisms = 0;
        foreach (GaiaEngine.Domain.Organisms.Organism organism in runtime.Organisms.GetAll())
        {
            if (organism.Lifecycle.IsAlive)
            {
                aliveOrganisms++;
            }
        }

        return aliveOrganisms;
    }

    private int CountMemoryEntries()
    {
        if (runtime is null)
        {
            return 0;
        }

        int memoryEntries = 0;
        foreach (GaiaEngine.Domain.AI.OrganismMemory memory in runtime.Memories.GetAll())
        {
            memoryEntries += memory.Count;
        }

        return memoryEntries;
    }

    private static string GetDiscoveryCategoryLabel(DiscoveryCategory category)
    {
        return category switch
        {
            DiscoveryCategory.Species => "species",
            DiscoveryCategory.Traits => "trait",
            DiscoveryCategory.Biomes => "biome",
            DiscoveryCategory.Resources => "resource",
            DiscoveryCategory.Behaviours => "behaviour",
            DiscoveryCategory.Climate => "climate",
            DiscoveryCategory.WorldEvents => "world event",
            _ => "discovery",
        };
    }

    private static string GetNotificationPrefix(HudNotificationCategory category)
    {
        return category switch
        {
            HudNotificationCategory.Warning => "Warning - ",
            HudNotificationCategory.Critical => "Critical - ",
            HudNotificationCategory.Achievement => "Achievement - ",
            HudNotificationCategory.Discovery => "Discovery - ",
            HudNotificationCategory.Objective => "Objective - ",
            _ => string.Empty,
        };
    }

    private static double GetDurationSeconds(HudNotificationPriority priority)
    {
        return priority switch
        {
            HudNotificationPriority.Low => 3d,
            HudNotificationPriority.Normal => 5d,
            HudNotificationPriority.High => 8d,
            _ => 0d,
        };
    }

    private static string HumanizeIdentifier(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
        {
            return "Unknown";
        }

        return identifier.Replace(".", " ", StringComparison.Ordinal).Replace("-", " ", StringComparison.Ordinal);
    }

    private sealed record HudViewSnapshot(
        string WorldName,
        string TimeSummary,
        string TickRate,
        string Population,
        string AlivePopulation,
        string SpeciesCount,
        string Weather,
        string Biome,
        string Temperature,
        string Humidity,
        string MemoryCount,
        string ActionCount,
        string DiscoveryCount,
        string EncyclopediaCount,
        string ObjectiveCount,
        string Level,
        string AchievementCount);

    private sealed record RuntimeObservationSnapshot(
        long Tick,
        string Season,
        GaiaEngine.Domain.World.WeatherState Weather,
        string BiomeName,
        int AlivePopulation,
        IReadOnlyList<DiscoveryEntry> Discoveries,
        HashSet<string> DiscoveryIds,
        IReadOnlyList<ObjectiveEntry> Objectives,
        Dictionary<string, ObjectiveStatus> ObjectiveStatusById,
        IReadOnlyList<AchievementEntry> Achievements,
        HashSet<string> UnlockedAchievementIds,
        int UnlockLevel,
        IReadOnlyList<string> UnlockIds,
        HashSet<string> UnlockIdSet,
        IReadOnlyList<string> MilestoneIds,
        HashSet<string> MilestoneIdSet);
}

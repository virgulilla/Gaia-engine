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
    private const string LeftPanelPath = "HudLayer/HudRoot/LeftPanel";
    private const string ContextPanelPath = "HudLayer/HudRoot/ContextPanel";
    private const string SelectionHintLabelPath = "HudLayer/HudRoot/LeftPanel/LeftPanelMargin/LeftPanelColumn/SelectionHintLabel";
    private const string SelectionTypeLabelPath = "HudLayer/HudRoot/LeftPanel/LeftPanelMargin/LeftPanelColumn/SelectionTypeLabel";
    private const string SelectionPrimaryLabelPath = "HudLayer/HudRoot/LeftPanel/LeftPanelMargin/LeftPanelColumn/SelectionPrimaryLabel";
    private const string SelectionSecondaryLabelPath = "HudLayer/HudRoot/LeftPanel/LeftPanelMargin/LeftPanelColumn/SelectionSecondaryLabel";
    private const string SelectionTertiaryLabelPath = "HudLayer/HudRoot/LeftPanel/LeftPanelMargin/LeftPanelColumn/SelectionTertiaryLabel";
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
    private const string ContextTitleLabelPath = "HudLayer/HudRoot/ContextPanel/ContextPanelMargin/ContextPanelColumn/ContextTitleLabel";
    private const string ContextBodyLabelPath = "HudLayer/HudRoot/ContextPanel/ContextPanelMargin/ContextPanelColumn/ContextBodyLabel";
    private const string ContextLine1LabelPath = "HudLayer/HudRoot/ContextPanel/ContextPanelMargin/ContextPanelColumn/ContextLine1Label";
    private const string ContextLine2LabelPath = "HudLayer/HudRoot/ContextPanel/ContextPanelMargin/ContextPanelColumn/ContextLine2Label";
    private const string ContextLine3LabelPath = "HudLayer/HudRoot/ContextPanel/ContextPanelMargin/ContextPanelColumn/ContextLine3Label";
    private const string ContextLine4LabelPath = "HudLayer/HudRoot/ContextPanel/ContextPanelMargin/ContextPanelColumn/ContextLine4Label";
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
    private PanelContainer? leftPanel;
    private PanelContainer? contextPanel;
    private Label? selectionHintLabel;
    private Label? selectionTypeLabel;
    private Label? selectionPrimaryLabel;
    private Label? selectionSecondaryLabel;
    private Label? selectionTertiaryLabel;
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
    private Label? contextTitleLabel;
    private Label? contextBodyLabel;
    private Label? contextLine1Label;
    private Label? contextLine2Label;
    private Label? contextLine3Label;
    private Label? contextLine4Label;
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
        leftPanel = GetNode<PanelContainer>(LeftPanelPath);
        contextPanel = GetNode<PanelContainer>(ContextPanelPath);
        selectionHintLabel = GetNode<Label>(SelectionHintLabelPath);
        selectionTypeLabel = GetNode<Label>(SelectionTypeLabelPath);
        selectionPrimaryLabel = GetNode<Label>(SelectionPrimaryLabelPath);
        selectionSecondaryLabel = GetNode<Label>(SelectionSecondaryLabelPath);
        selectionTertiaryLabel = GetNode<Label>(SelectionTertiaryLabelPath);
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
        contextTitleLabel = GetNode<Label>(ContextTitleLabelPath);
        contextBodyLabel = GetNode<Label>(ContextBodyLabelPath);
        contextLine1Label = GetNode<Label>(ContextLine1LabelPath);
        contextLine2Label = GetNode<Label>(ContextLine2LabelPath);
        contextLine3Label = GetNode<Label>(ContextLine3LabelPath);
        contextLine4Label = GetNode<Label>(ContextLine4LabelPath);
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
            || leftPanel is null
            || contextPanel is null
            || selectionHintLabel is null
            || selectionTypeLabel is null
            || selectionPrimaryLabel is null
            || selectionSecondaryLabel is null
            || selectionTertiaryLabel is null
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

        ObservationSelection selection = ResolveObservationSelection();

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
            $"Achievements: {runtime.PlayerProfile.Achievements.Count}",
            selection.IsVisible,
            selection.SelectionHint,
            selection.SelectionType,
            selection.SelectionPrimary,
            selection.SelectionSecondary,
            selection.SelectionTertiary,
            selection.ContextTitle,
            selection.ContextBody,
            selection.ContextLine1,
            selection.ContextLine2,
            selection.ContextLine3,
            selection.ContextLine4);

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
        leftPanel.Visible = snapshot.SelectionVisible;
        contextPanel.Visible = snapshot.SelectionVisible;
        selectionHintLabel.Text = snapshot.SelectionHint;
        selectionTypeLabel.Text = snapshot.SelectionType;
        selectionPrimaryLabel.Text = snapshot.SelectionPrimary;
        selectionSecondaryLabel.Text = snapshot.SelectionSecondary;
        selectionTertiaryLabel.Text = snapshot.SelectionTertiary;
        if (contextTitleLabel is not null
            && contextBodyLabel is not null
            && contextLine1Label is not null
            && contextLine2Label is not null
            && contextLine3Label is not null
            && contextLine4Label is not null)
        {
            contextTitleLabel.Text = snapshot.ContextTitle;
            contextBodyLabel.Text = snapshot.ContextBody;
            contextLine1Label.Text = snapshot.ContextLine1;
            contextLine2Label.Text = snapshot.ContextLine2;
            contextLine3Label.Text = snapshot.ContextLine3;
            contextLine4Label.Text = snapshot.ContextLine4;
        }
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

    private ObservationSelection ResolveObservationSelection()
    {
        if (runtime is null)
        {
            return ObservationSelection.Hidden;
        }

        GaiaEngine.Domain.Organisms.Organism? selectedOrganism = TryResolveObservedOrganism();
        if (selectedOrganism is not null)
        {
            GaiaEngine.Domain.World.Chunk selectedChunk = ResolveChunk(selectedOrganism.CurrentChunkId);
            GaiaEngine.Domain.World.ResourceState primaryResource = ResolvePrimaryResource(selectedChunk);
            return new ObservationSelection(
                IsVisible: true,
                SelectionHint: "Observed focus updates automatically from the current simulation state.",
                SelectionType: "Focus: Organism",
                SelectionPrimary: $"Organism: {selectedOrganism.Id}",
                SelectionSecondary: $"Chunk: {selectedChunk.Metadata.Coordinates.X}, {selectedChunk.Metadata.Coordinates.Y}",
                SelectionTertiary: $"Biome: {selectedChunk.Biome.Name}",
                ContextTitle: "Organism Context",
                ContextBody: $"Tracking organism {selectedOrganism.Id} in species {selectedOrganism.SpeciesId}.",
                ContextLine1: $"Stage: {selectedOrganism.Lifecycle.Stage}   Age: {selectedOrganism.Lifecycle.AgeTicks} ticks",
                ContextLine2: $"Health: {selectedOrganism.Health.CurrentValue}/{selectedOrganism.Health.MaximumValue}   Hunger: {selectedOrganism.Needs.Hunger}/1000",
                ContextLine3: $"Hydration: {selectedOrganism.Needs.Hydration}/1000   Rest: {selectedOrganism.Needs.Rest}/1000",
                ContextLine4: $"Resource: {primaryResource.Type} {primaryResource.CurrentAmount}/{primaryResource.MaximumCapacity}   Weather: {selectedChunk.Climate.WeatherState}");
        }

        GaiaEngine.Domain.World.Chunk primaryChunk = runtime.World.GetChunks()[0];
        GaiaEngine.Domain.World.ResourceState primaryChunkResource = ResolvePrimaryResource(primaryChunk);
        return new ObservationSelection(
            IsVisible: true,
            SelectionHint: "No living organism is available, so the HUD is observing the primary world chunk.",
            SelectionType: "Focus: Chunk",
            SelectionPrimary: $"Chunk: {primaryChunk.Metadata.Coordinates.X}, {primaryChunk.Metadata.Coordinates.Y}",
            SelectionSecondary: $"Biome: {primaryChunk.Biome.Name}",
            SelectionTertiary: $"Weather: {primaryChunk.Climate.WeatherState}",
            ContextTitle: "Biome Context",
            ContextBody: primaryChunk.Biome.Description,
            ContextLine1: $"Temperature: {primaryChunk.Climate.Temperature.CurrentTemperature} C   Humidity: {primaryChunk.Climate.Humidity.RelativeHumidity}%",
            ContextLine2: $"Wind: {primaryChunk.Climate.Wind.Speed}   Pressure: {primaryChunk.Climate.Pressure.CurrentPressure}",
            ContextLine3: $"Vegetation density: {primaryChunk.Biome.VegetationProfile.Density}   Plant diversity: {primaryChunk.Biome.SpeciesAffinity.PlantDiversity}",
            ContextLine4: $"Primary resource: {primaryChunkResource.Type} {primaryChunkResource.CurrentAmount}/{primaryChunkResource.MaximumCapacity}");
    }

    private GaiaEngine.Domain.Organisms.Organism? TryResolveObservedOrganism()
    {
        if (runtime is null)
        {
            return null;
        }

        foreach (GaiaEngine.Domain.Organisms.Organism organism in runtime.Organisms.GetAll())
        {
            if (organism.Lifecycle.IsAlive)
            {
                return organism;
            }
        }

        return null;
    }

    private GaiaEngine.Domain.World.Chunk ResolveChunk(GaiaEngine.Domain.Identifiers.ChunkId chunkId)
    {
        if (runtime is null)
        {
            throw new InvalidOperationException("Runtime must exist to resolve chunks.");
        }

        foreach (GaiaEngine.Domain.World.Chunk chunk in runtime.World.GetChunks())
        {
            if (chunk.Id == chunkId)
            {
                return chunk;
            }
        }

        return runtime.World.GetChunks()[0];
    }

    private static GaiaEngine.Domain.World.ResourceState ResolvePrimaryResource(GaiaEngine.Domain.World.Chunk chunk)
    {
        GaiaEngine.Domain.World.ResourceState? selectedResource = null;
        foreach (GaiaEngine.Domain.World.ResourceState resource in chunk.Resources.GetAll())
        {
            if (selectedResource is null || resource.CurrentAmount > selectedResource.CurrentAmount)
            {
                selectedResource = resource;
            }
        }

        return selectedResource ?? throw new InvalidOperationException("The observed chunk must contain at least one resource.");
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
        string AchievementCount,
        bool SelectionVisible,
        string SelectionHint,
        string SelectionType,
        string SelectionPrimary,
        string SelectionSecondary,
        string SelectionTertiary,
        string ContextTitle,
        string ContextBody,
        string ContextLine1,
        string ContextLine2,
        string ContextLine3,
        string ContextLine4);

    private sealed record ObservationSelection(
        bool IsVisible,
        string SelectionHint,
        string SelectionType,
        string SelectionPrimary,
        string SelectionSecondary,
        string SelectionTertiary,
        string ContextTitle,
        string ContextBody,
        string ContextLine1,
        string ContextLine2,
        string ContextLine3,
        string ContextLine4)
    {
        public static ObservationSelection Hidden { get; } = new(
            false,
            "No selection is available.",
            "Focus: None",
            "Primary: -",
            "Secondary: -",
            "Tertiary: -",
            "Context",
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty);
    }

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

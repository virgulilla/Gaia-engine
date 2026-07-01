using System;
using System.Collections.Generic;
using System.Text;
using GaiaEngine.App.Bootstrap;
using GaiaEngine.Gameplay.Achievements;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Encyclopedia;
using GaiaEngine.Gameplay.Objectives;
using GaiaEngine.Gameplay.Player;
using GaiaEngine.Godot.UI.Notifications;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Pipeline;
using Godot;

namespace GaiaEngine.Godot.Bootstrap;

/// <summary>
/// Adapts the Godot scene lifecycle to the Gaia Engine application bootstrap.
/// </summary>
public sealed partial class GaiaEngineBootstrap : Node
{
    private const string HudRootPath = "HudLayer/HudRoot";
    private const string WorldNameLabelPath = "HudLayer/HudRoot/TopBar/TopBarMargin/TopBarRow/WorldNameLabel";
    private const string TimeSummaryLabelPath = "HudLayer/HudRoot/TopBar/TopBarMargin/TopBarRow/TimeSummaryLabel";
    private const string TickRateLabelPath = "HudLayer/HudRoot/TopBar/TopBarMargin/TopBarRow/TickRateChip/TickRateChipMargin/TickRateLabel";
    private const string InspectButtonPath = "HudLayer/HudRoot/BottomToolbar/BottomToolbarMargin/BottomToolbarRow/InspectButton";
    private const string TimeControlsButtonPath = "HudLayer/HudRoot/BottomToolbar/BottomToolbarMargin/BottomToolbarRow/TimeControlsButton";
    private const string StepTickButtonPath = "HudLayer/HudRoot/BottomToolbar/BottomToolbarMargin/BottomToolbarRow/StepTickButton";
    private const string EncyclopediaButtonPath = "HudLayer/HudRoot/BottomToolbar/BottomToolbarMargin/BottomToolbarRow/EncyclopediaButton";
    private const string StatisticsButtonPath = "HudLayer/HudRoot/BottomToolbar/BottomToolbarMargin/BottomToolbarRow/StatisticsButton";
    private const string SettingsButtonPath = "HudLayer/HudRoot/BottomToolbar/BottomToolbarMargin/BottomToolbarRow/SettingsButton";
    private const string LeftPanelPath = "HudLayer/HudRoot/LeftPanel";
    private const string ContextPanelPath = "HudLayer/HudRoot/ContextPanel";
    private const string StatisticsOverlayPath = "HudLayer/HudRoot/StatisticsOverlay";
    private const string EncyclopediaOverlayPath = "HudLayer/HudRoot/EncyclopediaOverlay";
    private const string SettingsOverlayPath = "HudLayer/HudRoot/SettingsOverlay";
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
    private const string StatisticsStatusLabelPath = "HudLayer/HudRoot/StatisticsOverlay/StatisticsOverlayMargin/StatisticsOverlayColumn/StatisticsStatusLabel";
    private const string StatisticsPopulationLabelPath = "HudLayer/HudRoot/StatisticsOverlay/StatisticsOverlayMargin/StatisticsOverlayColumn/StatisticsPopulationLabel";
    private const string StatisticsClimateLabelPath = "HudLayer/HudRoot/StatisticsOverlay/StatisticsOverlayMargin/StatisticsOverlayColumn/StatisticsClimateLabel";
    private const string StatisticsDiscoveryLabelPath = "HudLayer/HudRoot/StatisticsOverlay/StatisticsOverlayMargin/StatisticsOverlayColumn/StatisticsDiscoveryLabel";
    private const string StatisticsPerformanceLabelPath = "HudLayer/HudRoot/StatisticsOverlay/StatisticsOverlayMargin/StatisticsOverlayColumn/StatisticsPerformanceLabel";
    private const string StatisticsHistoryLabelPath = "HudLayer/HudRoot/StatisticsOverlay/StatisticsOverlayMargin/StatisticsOverlayColumn/StatisticsHistoryLabel";
    private const string EncyclopediaSearchBarPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaHeaderRow/EncyclopediaSearchBar";
    private const string EncyclopediaFilterButtonPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaHeaderRow/EncyclopediaFilterButton";
    private const string EncyclopediaSortButtonPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaHeaderRow/EncyclopediaSortButton";
    private const string EncyclopediaCompareButtonPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaHeaderRow/EncyclopediaCompareButton";
    private const string SpeciesCategoryButtonPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaNavigationPanel/EncyclopediaNavigationMargin/EncyclopediaNavigationColumn/SpeciesCategoryButton";
    private const string TraitsCategoryButtonPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaNavigationPanel/EncyclopediaNavigationMargin/EncyclopediaNavigationColumn/TraitsCategoryButton";
    private const string BiomesCategoryButtonPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaNavigationPanel/EncyclopediaNavigationMargin/EncyclopediaNavigationColumn/BiomesCategoryButton";
    private const string ResourcesCategoryButtonPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaNavigationPanel/EncyclopediaNavigationMargin/EncyclopediaNavigationColumn/ResourcesCategoryButton";
    private const string ClimateCategoryButtonPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaNavigationPanel/EncyclopediaNavigationMargin/EncyclopediaNavigationColumn/ClimateCategoryButton";
    private const string BehavioursCategoryButtonPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaNavigationPanel/EncyclopediaNavigationMargin/EncyclopediaNavigationColumn/BehavioursCategoryButton";
    private const string EvolutionCategoryButtonPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaNavigationPanel/EncyclopediaNavigationMargin/EncyclopediaNavigationColumn/EvolutionCategoryButton";
    private const string WorldHistoryCategoryButtonPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaNavigationPanel/EncyclopediaNavigationMargin/EncyclopediaNavigationColumn/WorldHistoryCategoryButton";
    private const string EntryListStatusLabelPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaEntryPanel/EncyclopediaEntryMargin/EncyclopediaEntryColumn/EntryListStatusLabel";
    private const string EncyclopediaEntryListPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaEntryPanel/EncyclopediaEntryMargin/EncyclopediaEntryColumn/EncyclopediaEntryList";
    private const string EntryDetailsStatusLabelPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaDetailsPanel/EncyclopediaDetailsMargin/EncyclopediaDetailsColumn/EntryDetailsStatusLabel";
    private const string EntryDetailsBodyLabelPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaDetailsPanel/EncyclopediaDetailsMargin/EncyclopediaDetailsColumn/EntryDetailsBodyLabel";
    private const string EntryDetailsStatisticsLabelPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaDetailsPanel/EncyclopediaDetailsMargin/EncyclopediaDetailsColumn/EntryDetailsStatisticsLabel";
    private const string RelatedEntriesListPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaDetailsPanel/EncyclopediaDetailsMargin/EncyclopediaDetailsColumn/RelatedEntriesList";
    private const string ComparisonSummaryLabelPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaDetailsPanel/EncyclopediaDetailsMargin/EncyclopediaDetailsColumn/ComparisonSummaryLabel";
    private const string ProgressSummaryLabelPath = "HudLayer/HudRoot/EncyclopediaOverlay/EncyclopediaOverlayMargin/EncyclopediaOverlayColumn/EncyclopediaBodyRow/EncyclopediaDetailsPanel/EncyclopediaDetailsMargin/EncyclopediaDetailsColumn/ProgressSummaryLabel";
    private const string AccessibilitySettingsCategoryButtonPath = "HudLayer/HudRoot/SettingsOverlay/SettingsOverlayMargin/SettingsOverlayColumn/SettingsBodyRow/SettingsNavigationPanel/SettingsNavigationMargin/SettingsNavigationColumn/AccessibilitySettingsCategoryButton";
    private const string AudioSettingsCategoryButtonPath = "HudLayer/HudRoot/SettingsOverlay/SettingsOverlayMargin/SettingsOverlayColumn/SettingsBodyRow/SettingsNavigationPanel/SettingsNavigationMargin/SettingsNavigationColumn/AudioSettingsCategoryButton";
    private const string ControlsSettingsCategoryButtonPath = "HudLayer/HudRoot/SettingsOverlay/SettingsOverlayMargin/SettingsOverlayColumn/SettingsBodyRow/SettingsNavigationPanel/SettingsNavigationMargin/SettingsNavigationColumn/ControlsSettingsCategoryButton";
    private const string LanguageSettingsCategoryButtonPath = "HudLayer/HudRoot/SettingsOverlay/SettingsOverlayMargin/SettingsOverlayColumn/SettingsBodyRow/SettingsNavigationPanel/SettingsNavigationMargin/SettingsNavigationColumn/LanguageSettingsCategoryButton";
    private const string GraphicsSettingsCategoryButtonPath = "HudLayer/HudRoot/SettingsOverlay/SettingsOverlayMargin/SettingsOverlayColumn/SettingsBodyRow/SettingsNavigationPanel/SettingsNavigationMargin/SettingsNavigationColumn/GraphicsSettingsCategoryButton";
    private const string SettingsCategoryTitleLabelPath = "HudLayer/HudRoot/SettingsOverlay/SettingsOverlayMargin/SettingsOverlayColumn/SettingsBodyRow/SettingsContentPanel/SettingsContentMargin/SettingsContentColumn/SettingsCategoryTitleLabel";
    private const string SettingsCategorySummaryLabelPath = "HudLayer/HudRoot/SettingsOverlay/SettingsOverlayMargin/SettingsOverlayColumn/SettingsBodyRow/SettingsContentPanel/SettingsContentMargin/SettingsContentColumn/SettingsCategorySummaryLabel";
    private const string SettingsOptionButton1Path = "HudLayer/HudRoot/SettingsOverlay/SettingsOverlayMargin/SettingsOverlayColumn/SettingsBodyRow/SettingsContentPanel/SettingsContentMargin/SettingsContentColumn/SettingsOptionButton1";
    private const string SettingsOptionButton2Path = "HudLayer/HudRoot/SettingsOverlay/SettingsOverlayMargin/SettingsOverlayColumn/SettingsBodyRow/SettingsContentPanel/SettingsContentMargin/SettingsContentColumn/SettingsOptionButton2";
    private const string SettingsOptionButton3Path = "HudLayer/HudRoot/SettingsOverlay/SettingsOverlayMargin/SettingsOverlayColumn/SettingsBodyRow/SettingsContentPanel/SettingsContentMargin/SettingsContentColumn/SettingsOptionButton3";
    private const string SettingsOptionButton4Path = "HudLayer/HudRoot/SettingsOverlay/SettingsOverlayMargin/SettingsOverlayColumn/SettingsBodyRow/SettingsContentPanel/SettingsContentMargin/SettingsContentColumn/SettingsOptionButton4";
    private const string SettingsFooterLabelPath = "HudLayer/HudRoot/SettingsOverlay/SettingsOverlayMargin/SettingsOverlayColumn/SettingsBodyRow/SettingsContentPanel/SettingsContentMargin/SettingsContentColumn/SettingsFooterLabel";
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
    private Control? hudRoot;
    private Label? worldNameLabel;
    private Label? timeSummaryLabel;
    private Label? tickRateLabel;
    private Button? inspectButton;
    private Button? timeControlsButton;
    private Button? stepTickButton;
    private Button? encyclopediaButton;
    private Button? statisticsButton;
    private Button? settingsButton;
    private Button? encyclopediaFilterButton;
    private Button? encyclopediaSortButton;
    private Button? encyclopediaCompareButton;
    private Button? settingsOptionButton1;
    private Button? settingsOptionButton2;
    private Button? settingsOptionButton3;
    private Button? settingsOptionButton4;
    private PanelContainer? leftPanel;
    private PanelContainer? contextPanel;
    private PanelContainer? statisticsOverlay;
    private PanelContainer? encyclopediaOverlay;
    private PanelContainer? settingsOverlay;
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
    private Label? statisticsStatusLabel;
    private Label? statisticsPopulationLabel;
    private Label? statisticsClimateLabel;
    private Label? statisticsDiscoveryLabel;
    private Label? statisticsPerformanceLabel;
    private Label? statisticsHistoryLabel;
    private Label? entryListStatusLabel;
    private Label? entryDetailsStatusLabel;
    private Label? entryDetailsBodyLabel;
    private Label? entryDetailsStatisticsLabel;
    private Label? comparisonSummaryLabel;
    private Label? progressSummaryLabel;
    private Label? settingsCategoryTitleLabel;
    private Label? settingsCategorySummaryLabel;
    private Label? settingsFooterLabel;
    private LineEdit? encyclopediaSearchBar;
    private ItemList? encyclopediaEntryList;
    private ItemList? relatedEntriesList;
    private Button[]? encyclopediaCategoryButtons;
    private Button[]? settingsCategoryButtons;
    private PanelContainer[]? notificationCards;
    private Label[]? notificationTitleLabels;
    private Label[]? notificationBodyLabels;
    private HudNotificationQueue? notificationQueue;
    private double tickAccumulator;
    private HudViewSnapshot? lastSnapshot;
    private StatisticsViewSnapshot? lastStatisticsSnapshot;
    private EncyclopediaViewSnapshot? lastEncyclopediaSnapshot;
    private SettingsViewSnapshot? lastSettingsSnapshot;
    private RuntimeObservationSnapshot? lastObservedState;
    private FocusOverrideKind focusOverrideKind;
    private GaiaEngine.Domain.Identifiers.OrganismId? focusedOrganismId;
    private bool isSimulationPaused;
    private bool isStatisticsOverlayVisible;
    private bool isEncyclopediaOverlayVisible;
    private bool isSettingsOverlayVisible;
    private SimulationTickDiagnostics? lastDiagnostics;
    private readonly List<StatisticsHistorySample> statisticsHistory = new();
    private EncyclopediaCategory selectedEncyclopediaCategory = EncyclopediaCategory.Species;
    private EncyclopediaFilterMode encyclopediaFilterMode;
    private EncyclopediaSortMode encyclopediaSortMode;
    private string encyclopediaSearchText = string.Empty;
    private string? selectedEncyclopediaEntryId;
    private string? compareEncyclopediaEntryId;
    private SettingsCategory selectedSettingsCategory = SettingsCategory.Accessibility;

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
        hudRoot = GetNode<Control>(HudRootPath);
        worldNameLabel = GetNode<Label>(WorldNameLabelPath);
        timeSummaryLabel = GetNode<Label>(TimeSummaryLabelPath);
        tickRateLabel = GetNode<Label>(TickRateLabelPath);
        inspectButton = GetNode<Button>(InspectButtonPath);
        timeControlsButton = GetNode<Button>(TimeControlsButtonPath);
        stepTickButton = GetNode<Button>(StepTickButtonPath);
        encyclopediaButton = GetNode<Button>(EncyclopediaButtonPath);
        statisticsButton = GetNode<Button>(StatisticsButtonPath);
        settingsButton = GetNode<Button>(SettingsButtonPath);
        encyclopediaFilterButton = GetNode<Button>(EncyclopediaFilterButtonPath);
        encyclopediaSortButton = GetNode<Button>(EncyclopediaSortButtonPath);
        encyclopediaCompareButton = GetNode<Button>(EncyclopediaCompareButtonPath);
        settingsOptionButton1 = GetNode<Button>(SettingsOptionButton1Path);
        settingsOptionButton2 = GetNode<Button>(SettingsOptionButton2Path);
        settingsOptionButton3 = GetNode<Button>(SettingsOptionButton3Path);
        settingsOptionButton4 = GetNode<Button>(SettingsOptionButton4Path);
        leftPanel = GetNode<PanelContainer>(LeftPanelPath);
        contextPanel = GetNode<PanelContainer>(ContextPanelPath);
        statisticsOverlay = GetNode<PanelContainer>(StatisticsOverlayPath);
        encyclopediaOverlay = GetNode<PanelContainer>(EncyclopediaOverlayPath);
        settingsOverlay = GetNode<PanelContainer>(SettingsOverlayPath);
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
        statisticsStatusLabel = GetNode<Label>(StatisticsStatusLabelPath);
        statisticsPopulationLabel = GetNode<Label>(StatisticsPopulationLabelPath);
        statisticsClimateLabel = GetNode<Label>(StatisticsClimateLabelPath);
        statisticsDiscoveryLabel = GetNode<Label>(StatisticsDiscoveryLabelPath);
        statisticsPerformanceLabel = GetNode<Label>(StatisticsPerformanceLabelPath);
        statisticsHistoryLabel = GetNode<Label>(StatisticsHistoryLabelPath);
        entryListStatusLabel = GetNode<Label>(EntryListStatusLabelPath);
        entryDetailsStatusLabel = GetNode<Label>(EntryDetailsStatusLabelPath);
        entryDetailsBodyLabel = GetNode<Label>(EntryDetailsBodyLabelPath);
        entryDetailsStatisticsLabel = GetNode<Label>(EntryDetailsStatisticsLabelPath);
        comparisonSummaryLabel = GetNode<Label>(ComparisonSummaryLabelPath);
        progressSummaryLabel = GetNode<Label>(ProgressSummaryLabelPath);
        settingsCategoryTitleLabel = GetNode<Label>(SettingsCategoryTitleLabelPath);
        settingsCategorySummaryLabel = GetNode<Label>(SettingsCategorySummaryLabelPath);
        settingsFooterLabel = GetNode<Label>(SettingsFooterLabelPath);
        encyclopediaSearchBar = GetNode<LineEdit>(EncyclopediaSearchBarPath);
        encyclopediaEntryList = GetNode<ItemList>(EncyclopediaEntryListPath);
        relatedEntriesList = GetNode<ItemList>(RelatedEntriesListPath);
        encyclopediaCategoryButtons =
        [
            GetNode<Button>(SpeciesCategoryButtonPath),
            GetNode<Button>(TraitsCategoryButtonPath),
            GetNode<Button>(BiomesCategoryButtonPath),
            GetNode<Button>(ResourcesCategoryButtonPath),
            GetNode<Button>(ClimateCategoryButtonPath),
            GetNode<Button>(BehavioursCategoryButtonPath),
            GetNode<Button>(EvolutionCategoryButtonPath),
            GetNode<Button>(WorldHistoryCategoryButtonPath),
        ];
        settingsCategoryButtons =
        [
            GetNode<Button>(AccessibilitySettingsCategoryButtonPath),
            GetNode<Button>(AudioSettingsCategoryButtonPath),
            GetNode<Button>(ControlsSettingsCategoryButtonPath),
            GetNode<Button>(LanguageSettingsCategoryButtonPath),
            GetNode<Button>(GraphicsSettingsCategoryButtonPath),
        ];
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
        inspectButton.Disabled = false;
        inspectButton.Pressed += OnInspectPressed;
        timeControlsButton.Pressed += OnTimeControlsPressed;
        stepTickButton.Pressed += OnStepTickPressed;
        encyclopediaButton.Pressed += OnEncyclopediaPressed;
        statisticsButton.Pressed += OnStatisticsPressed;
        settingsButton.Pressed += OnSettingsPressed;
        encyclopediaFilterButton.Pressed += OnEncyclopediaFilterPressed;
        encyclopediaSortButton.Pressed += OnEncyclopediaSortPressed;
        encyclopediaCompareButton.Pressed += OnEncyclopediaComparePressed;
        settingsOptionButton1.Pressed += () => OnSettingsOptionPressed(0);
        settingsOptionButton2.Pressed += () => OnSettingsOptionPressed(1);
        settingsOptionButton3.Pressed += () => OnSettingsOptionPressed(2);
        settingsOptionButton4.Pressed += () => OnSettingsOptionPressed(3);
        encyclopediaSearchBar.TextChanged += OnEncyclopediaSearchTextChanged;
        encyclopediaEntryList.ItemSelected += OnEncyclopediaEntrySelected;
        relatedEntriesList.ItemSelected += OnRelatedEntrySelected;
        WireCategoryButtons();
        WireSettingsCategoryButtons();
        ApplyPresentationSettings();

        UpdateSimulationStatusText();
        UpdateStatisticsOverlay();
        UpdateEncyclopediaOverlay();
        UpdateSettingsOverlay();
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

        if (!isSimulationPaused)
        {
            double secondsPerTick = 1d / runtime.EngineConfiguration.TickRate;
            tickAccumulator += delta;
            while (tickAccumulator >= secondsPerTick)
            {
                AdvanceSimulationTick();
                tickAccumulator -= secondsPerTick;
            }
        }

        notificationQueue.Advance(delta);
        ApplyPresentationSettings();
        UpdateSimulationStatusText();
        UpdateStatisticsOverlay();
        UpdateEncyclopediaOverlay();
        UpdateSettingsOverlay();
        UpdateNotificationWidgets();
    }

    private void UpdateSimulationStatusText()
    {
        if (runtime is null
            || worldNameLabel is null
            || timeSummaryLabel is null
            || tickRateLabel is null
            || inspectButton is null
            || timeControlsButton is null
            || stepTickButton is null
            || encyclopediaButton is null
            || statisticsButton is null
            || settingsButton is null
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
        inspectButton.Disabled = aliveOrganisms == 0;
        timeControlsButton.Text = isSimulationPaused ? "Resume" : "Pause";
        stepTickButton.Disabled = !isSimulationPaused;
        encyclopediaButton.Disabled = runtime.PlayerProfile.Knowledge.Encyclopedia.Count == 0;
        encyclopediaButton.Text = isEncyclopediaOverlayVisible ? "Close Archive" : "Encyclopedia";
        statisticsButton.Disabled = false;
        statisticsButton.Text = isStatisticsOverlayVisible ? "Close Stats" : "Statistics";
        settingsButton.Disabled = false;
        settingsButton.Text = isSettingsOverlayVisible ? "Close Settings" : "Settings";
        GaiaEngine.Domain.World.Chunk primaryChunk = runtime.World.GetChunks()[0];
        HudViewSnapshot snapshot = new(
            runtime.World.Metadata.WorldName,
            $"Day {runtime.SimulationSession.CurrentTimeState.CurrentDay} - {runtime.SimulationSession.CurrentTimeState.CurrentSeason} - Year {runtime.SimulationSession.CurrentTimeState.CurrentYear} - Tick {runtime.SimulationSession.CurrentTimeState.CurrentTick} - {(isSimulationPaused ? "Paused" : "Running")}",
            $"Tick Rate: {runtime.EngineConfiguration.TickRate} ({(isSimulationPaused ? "Paused" : "Live")})",
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

    private void UpdateStatisticsOverlay()
    {
        if (runtime is null
            || statisticsOverlay is null
            || statisticsStatusLabel is null
            || statisticsPopulationLabel is null
            || statisticsClimateLabel is null
            || statisticsDiscoveryLabel is null
            || statisticsPerformanceLabel is null
            || statisticsHistoryLabel is null)
        {
            return;
        }

        GaiaEngine.Domain.World.Chunk primaryChunk = runtime.World.GetChunks()[0];
        int extinctSpecies = CountExtinctSpecies();
        bool populationGraphsUnlocked = runtime.PlayerProfile.Progression.Unlocks.Contains("statistics.population-graphs");
        string statusText = populationGraphsUnlocked
            ? "Statistics overlay is active. Advanced sampled history is unlocked for this profile."
            : "Statistics overlay is active. Advanced sampled history unlocks with Population Graphs at progression level 2.";
        string populationText =
            $"Population Summary\nTotal organisms: {runtime.Organisms.Count}\nAlive organisms: {CountAliveOrganisms()}\nSpecies tracked: {runtime.Species.Count}\nExtinct species: {extinctSpecies}";
        string climateText =
            $"Climate Summary\nBiome: {primaryChunk.Biome.Name}\nWeather: {primaryChunk.Climate.WeatherState}\nTemperature: {primaryChunk.Climate.Temperature.CurrentTemperature} C\nHumidity: {primaryChunk.Climate.Humidity.RelativeHumidity}%";
        string discoveryText =
            $"Discovery Progress\nDiscoveries: {runtime.PlayerProfile.Knowledge.Discoveries.Count}\nEncyclopedia entries: {runtime.PlayerProfile.Knowledge.Encyclopedia.Count}\nCompleted objectives: {runtime.PlayerProfile.Progression.CompletedObjectives}\nAchievements: {runtime.PlayerProfile.Achievements.Count}\nUnlock level: {runtime.PlayerProfile.Progression.UnlockLevel}";
        string performanceText = BuildPerformanceText();
        string historyText = BuildStatisticsHistoryText(populationGraphsUnlocked);

        StatisticsViewSnapshot snapshot = new(
            isStatisticsOverlayVisible,
            statusText,
            populationText,
            climateText,
            discoveryText,
            performanceText,
            historyText);

        if (snapshot == lastStatisticsSnapshot)
        {
            return;
        }

        lastStatisticsSnapshot = snapshot;
        statisticsOverlay.Visible = snapshot.IsVisible;
        statisticsStatusLabel.Text = snapshot.Status;
        statisticsPopulationLabel.Text = snapshot.Population;
        statisticsClimateLabel.Text = snapshot.Climate;
        statisticsDiscoveryLabel.Text = snapshot.Discovery;
        statisticsPerformanceLabel.Text = snapshot.Performance;
        statisticsHistoryLabel.Text = snapshot.History;
    }

    private void UpdateEncyclopediaOverlay()
    {
        if (runtime is null
            || encyclopediaOverlay is null
            || encyclopediaFilterButton is null
            || encyclopediaSortButton is null
            || encyclopediaCompareButton is null
            || encyclopediaSearchBar is null
            || encyclopediaEntryList is null
            || relatedEntriesList is null
            || entryListStatusLabel is null
            || entryDetailsStatusLabel is null
            || entryDetailsBodyLabel is null
            || entryDetailsStatisticsLabel is null
            || comparisonSummaryLabel is null
            || progressSummaryLabel is null
            || encyclopediaCategoryButtons is null)
        {
            return;
        }

        encyclopediaOverlay.Visible = isEncyclopediaOverlayVisible;
        encyclopediaSearchBar.Editable = true;
        encyclopediaFilterButton.Text = $"Filter: {GetFilterModeLabel(encyclopediaFilterMode)}";
        encyclopediaSortButton.Text = $"Sort: {GetSortModeLabel(encyclopediaSortMode)}";

        IReadOnlyList<EncyclopediaEntry> filteredEntries = GetFilteredEncyclopediaEntries();
        EnsureSelectedEncyclopediaEntry(filteredEntries);
        EncyclopediaEntry? selectedEntry = TryResolveEncyclopediaEntry(selectedEncyclopediaEntryId);
        EncyclopediaEntry? compareEntry = TryResolveEncyclopediaEntry(compareEncyclopediaEntryId);

        UpdateCategoryButtons();
        UpdateEntryListStatus(filteredEntries);
        UpdateEntryListItems(filteredEntries);
        UpdateRelatedEntryItems(selectedEntry);

        string detailStatus = selectedEntry is null
            ? "No encyclopedia entry is selected."
            : $"{selectedEntry.Title} ({GetCategoryLabel(selectedEntry.Category)})\nDiscovery Tick: {selectedEntry.DiscoveryDate}\nState: {selectedEntry.UnlockState}";
        string detailBody = selectedEntry?.Description ?? "Choose one entry from the filtered list to inspect its archived knowledge.";
        string detailStatistics = BuildEntryStatisticsText(selectedEntry);
        string comparisonText = BuildComparisonText(selectedEntry, compareEntry);
        string progressText = BuildEncyclopediaProgressText();
        string[] activeEntryIds = BuildEntryIdList(filteredEntries);
        string[] relatedEntryIds = selectedEntry is null ? Array.Empty<string>() : BuildRelatedEntryIdList(selectedEntry);

        EncyclopediaViewSnapshot snapshot = new(
            isEncyclopediaOverlayVisible,
            encyclopediaFilterButton.Text,
            encyclopediaSortButton.Text,
            selectedEncyclopediaCategory,
            encyclopediaSearchText,
            selectedEntry?.EntryId,
            compareEntry?.EntryId,
            entryListStatusLabel.Text,
            detailStatus,
            detailBody,
            detailStatistics,
            comparisonText,
            progressText,
            activeEntryIds,
            relatedEntryIds);

        if (snapshot == lastEncyclopediaSnapshot)
        {
            return;
        }

        lastEncyclopediaSnapshot = snapshot;
        entryDetailsStatusLabel.Text = snapshot.DetailsStatus;
        entryDetailsBodyLabel.Text = snapshot.DetailsBody;
        entryDetailsStatisticsLabel.Text = snapshot.DetailsStatistics;
        comparisonSummaryLabel.Text = snapshot.ComparisonSummary;
        progressSummaryLabel.Text = snapshot.ProgressSummary;
        encyclopediaCompareButton.Text = compareEntry is null ? "Compare" : "Clear Compare";
    }

    private void WireCategoryButtons()
    {
        if (encyclopediaCategoryButtons is null)
        {
            return;
        }

        EncyclopediaCategory[] categories =
        [
            EncyclopediaCategory.Species,
            EncyclopediaCategory.Traits,
            EncyclopediaCategory.Biomes,
            EncyclopediaCategory.Resources,
            EncyclopediaCategory.Climate,
            EncyclopediaCategory.Behaviours,
            EncyclopediaCategory.Evolution,
            EncyclopediaCategory.WorldHistory,
        ];
        for (int index = 0; index < encyclopediaCategoryButtons.Length && index < categories.Length; index++)
        {
            EncyclopediaCategory category = categories[index];
            encyclopediaCategoryButtons[index].Pressed += () => OnEncyclopediaCategoryPressed(category);
        }
    }

    private void UpdateCategoryButtons()
    {
        if (encyclopediaCategoryButtons is null)
        {
            return;
        }

        EncyclopediaCategory[] categories =
        [
            EncyclopediaCategory.Species,
            EncyclopediaCategory.Traits,
            EncyclopediaCategory.Biomes,
            EncyclopediaCategory.Resources,
            EncyclopediaCategory.Climate,
            EncyclopediaCategory.Behaviours,
            EncyclopediaCategory.Evolution,
            EncyclopediaCategory.WorldHistory,
        ];
        for (int index = 0; index < encyclopediaCategoryButtons.Length && index < categories.Length; index++)
        {
            Button button = encyclopediaCategoryButtons[index];
            bool isSelected = categories[index] == selectedEncyclopediaCategory;
            button.Disabled = isSelected;
            button.Text = isSelected ? $"> {GetCategoryLabel(categories[index])}" : GetCategoryLabel(categories[index]);
        }
    }

    private void UpdateEntryListStatus(IReadOnlyList<EncyclopediaEntry> filteredEntries)
    {
        if (entryListStatusLabel is null)
        {
            return;
        }

        entryListStatusLabel.Text =
            $"{GetCategoryLabel(selectedEncyclopediaCategory)} Entries\nVisible results: {filteredEntries.Count}\nSearch: {(string.IsNullOrWhiteSpace(encyclopediaSearchText) ? "none" : encyclopediaSearchText)}";
    }

    private void UpdateEntryListItems(IReadOnlyList<EncyclopediaEntry> filteredEntries)
    {
        if (encyclopediaEntryList is null)
        {
            return;
        }

        encyclopediaEntryList.Clear();
        int selectedIndex = -1;
        for (int index = 0; index < filteredEntries.Count; index++)
        {
            EncyclopediaEntry entry = filteredEntries[index];
            encyclopediaEntryList.AddItem($"{entry.Title} [{entry.DiscoveryDate}]");
            if (entry.EntryId == selectedEncyclopediaEntryId)
            {
                selectedIndex = index;
            }
        }

        if (selectedIndex >= 0)
        {
            encyclopediaEntryList.Select(selectedIndex);
        }
    }

    private void UpdateRelatedEntryItems(EncyclopediaEntry? selectedEntry)
    {
        if (relatedEntriesList is null)
        {
            return;
        }

        relatedEntriesList.Clear();
        if (selectedEntry is null)
        {
            return;
        }

        IReadOnlyList<string> relatedEntries = selectedEntry.GetRelatedEntries();
        if (relatedEntries.Count == 0)
        {
            relatedEntriesList.AddItem("No related entries yet");
            return;
        }

        foreach (string relatedEntryId in relatedEntries)
        {
            EncyclopediaEntry? relatedEntry = TryResolveEncyclopediaEntry(relatedEntryId);
            relatedEntriesList.AddItem(relatedEntry?.Title ?? relatedEntryId);
        }
    }

    private IReadOnlyList<EncyclopediaEntry> GetFilteredEncyclopediaEntries()
    {
        if (runtime is null)
        {
            return Array.Empty<EncyclopediaEntry>();
        }

        List<EncyclopediaEntry> filteredEntries = new();
        foreach (EncyclopediaEntry entry in runtime.PlayerProfile.Knowledge.Encyclopedia.GetAll())
        {
            if (entry.Category != selectedEncyclopediaCategory)
            {
                continue;
            }

            if (!MatchesSearch(entry) || !MatchesFilter(entry))
            {
                continue;
            }

            filteredEntries.Add(entry);
        }

        filteredEntries.Sort(CompareEncyclopediaEntries);
        return filteredEntries.AsReadOnly();
    }

    private void EnsureSelectedEncyclopediaEntry(IReadOnlyList<EncyclopediaEntry> filteredEntries)
    {
        if (filteredEntries.Count == 0)
        {
            selectedEncyclopediaEntryId = null;
            if (compareEncyclopediaEntryId is not null && TryResolveEncyclopediaEntry(compareEncyclopediaEntryId)?.Category == selectedEncyclopediaCategory)
            {
                compareEncyclopediaEntryId = null;
            }

            return;
        }

        foreach (EncyclopediaEntry entry in filteredEntries)
        {
            if (entry.EntryId == selectedEncyclopediaEntryId)
            {
                return;
            }
        }

        selectedEncyclopediaEntryId = filteredEntries[0].EntryId;
    }

    private bool MatchesSearch(EncyclopediaEntry entry)
    {
        if (string.IsNullOrWhiteSpace(encyclopediaSearchText))
        {
            return true;
        }

        return entry.Title.Contains(encyclopediaSearchText, StringComparison.OrdinalIgnoreCase)
            || entry.Description.Contains(encyclopediaSearchText, StringComparison.OrdinalIgnoreCase)
            || entry.EntryId.Contains(encyclopediaSearchText, StringComparison.OrdinalIgnoreCase)
            || GetCategoryLabel(entry.Category).Contains(encyclopediaSearchText, StringComparison.OrdinalIgnoreCase);
    }

    private bool MatchesFilter(EncyclopediaEntry entry)
    {
        return encyclopediaFilterMode switch
        {
            EncyclopediaFilterMode.All => true,
            EncyclopediaFilterMode.Discovered => entry.UnlockState == EncyclopediaUnlockState.Discovered || entry.UnlockState == EncyclopediaUnlockState.Complete,
            EncyclopediaFilterMode.Incomplete => entry.UnlockState != EncyclopediaUnlockState.Complete,
            EncyclopediaFilterMode.RecentlyUpdated => IsRecentlyUpdated(entry),
            _ => true,
        };
    }

    private static bool IsRecentlyUpdated(EncyclopediaEntry entry)
    {
        return int.TryParse(entry.DiscoveryDate, out int discoveryTick) && discoveryTick >= 0 && discoveryTick >= 50;
    }

    private int CompareEncyclopediaEntries(EncyclopediaEntry left, EncyclopediaEntry right)
    {
        return encyclopediaSortMode switch
        {
            EncyclopediaSortMode.Alphabetical => string.CompareOrdinal(left.Title, right.Title),
            EncyclopediaSortMode.DiscoveryDate => CompareNumericText(left.DiscoveryDate, right.DiscoveryDate),
            EncyclopediaSortMode.ObservationCount => GetStatisticValue(right, "TimesObserved").CompareTo(GetStatisticValue(left, "TimesObserved")),
            EncyclopediaSortMode.CompletionPercentage => GetCompletionPercentage(right).CompareTo(GetCompletionPercentage(left)),
            _ => string.CompareOrdinal(left.Title, right.Title),
        };
    }

    private static int CompareNumericText(string left, string right)
    {
        bool leftParsed = int.TryParse(left, out int leftValue);
        bool rightParsed = int.TryParse(right, out int rightValue);
        if (leftParsed && rightParsed)
        {
            return leftValue.CompareTo(rightValue);
        }

        return string.CompareOrdinal(left, right);
    }

    private EncyclopediaEntry? TryResolveEncyclopediaEntry(string? entryId)
    {
        if (runtime is null || string.IsNullOrWhiteSpace(entryId))
        {
            return null;
        }

        foreach (EncyclopediaEntry entry in runtime.PlayerProfile.Knowledge.Encyclopedia.GetAll())
        {
            if (entry.EntryId == entryId)
            {
                return entry;
            }
        }

        return null;
    }

    private static int GetStatisticValue(EncyclopediaEntry entry, string key)
    {
        foreach (EncyclopediaStatistic statistic in entry.GetStatistics())
        {
            if (statistic.Key == key)
            {
                return statistic.Value;
            }
        }

        return 0;
    }

    private static int GetCompletionPercentage(EncyclopediaEntry entry)
    {
        return entry.UnlockState switch
        {
            EncyclopediaUnlockState.Hidden => 0,
            EncyclopediaUnlockState.Discovered => 50,
            EncyclopediaUnlockState.Complete => 100,
            _ => 0,
        };
    }

    private string BuildEntryStatisticsText(EncyclopediaEntry? entry)
    {
        if (entry is null)
        {
            return "Statistics\nNo entry statistics are available without a selection.";
        }

        StringBuilder builder = new("Statistics\n");
        foreach (EncyclopediaStatistic statistic in entry.GetStatistics())
        {
            builder.Append(HumanizeIdentifier(statistic.Key))
                .Append(": ")
                .Append(statistic.Value)
                .AppendLine();
        }

        builder.Append("Completion: ").Append(GetCompletionPercentage(entry)).Append('%');
        return builder.ToString().TrimEnd();
    }

    private string BuildComparisonText(EncyclopediaEntry? selectedEntry, EncyclopediaEntry? compareEntry)
    {
        if (selectedEntry is null)
        {
            return "Comparison is inactive. Select one entry to start.";
        }

        if (compareEntry is null)
        {
            return "Comparison is inactive. Press Compare to pin the current entry, then choose another entry from the same category.";
        }

        if (compareEntry.EntryId == selectedEntry.EntryId)
        {
            return $"Comparison anchor set to {compareEntry.Title}. Select another entry in {GetCategoryLabel(compareEntry.Category)} to compare.";
        }

        if (compareEntry.Category != selectedEntry.Category)
        {
            return "Comparison requires two entries from the same category.";
        }

        return
            $"Comparison\n{compareEntry.Title} vs {selectedEntry.Title}\nDiscovery ticks: {compareEntry.DiscoveryDate} / {selectedEntry.DiscoveryDate}\nTimes observed: {GetStatisticValue(compareEntry, "TimesObserved")} / {GetStatisticValue(selectedEntry, "TimesObserved")}\nWorlds found: {GetStatisticValue(compareEntry, "WorldsFound")} / {GetStatisticValue(selectedEntry, "WorldsFound")}\nCompletion: {GetCompletionPercentage(compareEntry)}% / {GetCompletionPercentage(selectedEntry)}%";
    }

    private string BuildEncyclopediaProgressText()
    {
        if (runtime is null)
        {
            return "Progress Summary\nNo encyclopedia data is available.";
        }

        StringBuilder builder = new("Progress Summary\n");
        foreach (EncyclopediaCategory category in Enum.GetValues<EncyclopediaCategory>())
        {
            int count = 0;
            foreach (EncyclopediaEntry entry in runtime.PlayerProfile.Knowledge.Encyclopedia.GetAll())
            {
                if (entry.Category == category)
                {
                    count++;
                }
            }

            builder.Append(GetCategoryLabel(category))
                .Append(": ")
                .Append(count)
                .AppendLine();
        }

        builder.Append("Total entries: ").Append(runtime.PlayerProfile.Knowledge.Encyclopedia.Count);
        return builder.ToString().TrimEnd();
    }

    private static string[] BuildEntryIdList(IReadOnlyList<EncyclopediaEntry> entries)
    {
        string[] identifiers = new string[entries.Count];
        for (int index = 0; index < entries.Count; index++)
        {
            identifiers[index] = entries[index].EntryId;
        }

        return identifiers;
    }

    private static string[] BuildRelatedEntryIdList(EncyclopediaEntry entry)
    {
        IReadOnlyList<string> relatedEntries = entry.GetRelatedEntries();
        string[] identifiers = new string[relatedEntries.Count];
        for (int index = 0; index < relatedEntries.Count; index++)
        {
            identifiers[index] = relatedEntries[index];
        }

        return identifiers;
    }

    private void UpdateSettingsOverlay()
    {
        if (runtime is null
            || settingsOverlay is null
            || settingsCategoryTitleLabel is null
            || settingsCategorySummaryLabel is null
            || settingsFooterLabel is null
            || settingsOptionButton1 is null
            || settingsOptionButton2 is null
            || settingsOptionButton3 is null
            || settingsOptionButton4 is null
            || settingsCategoryButtons is null)
        {
            return;
        }

        settingsOverlay.Visible = isSettingsOverlayVisible;
        UpdateSettingsCategoryButtons();
        string categoryTitle = GetSettingsCategoryLabel(selectedSettingsCategory);
        string categorySummary = BuildSettingsCategorySummary(runtime.PlayerProfile.Settings, selectedSettingsCategory);
        string footerText = "Changes affect presentation only and never modify simulation determinism.";
        string option1Text = GetSettingsOptionLabel(runtime.PlayerProfile.Settings, selectedSettingsCategory, 0);
        string option2Text = GetSettingsOptionLabel(runtime.PlayerProfile.Settings, selectedSettingsCategory, 1);
        string option3Text = GetSettingsOptionLabel(runtime.PlayerProfile.Settings, selectedSettingsCategory, 2);
        string option4Text = GetSettingsOptionLabel(runtime.PlayerProfile.Settings, selectedSettingsCategory, 3);

        SettingsViewSnapshot snapshot = new(
            isSettingsOverlayVisible,
            selectedSettingsCategory,
            categoryTitle,
            categorySummary,
            footerText,
            option1Text,
            option2Text,
            option3Text,
            option4Text);

        if (snapshot == lastSettingsSnapshot)
        {
            return;
        }

        lastSettingsSnapshot = snapshot;
        settingsCategoryTitleLabel.Text = snapshot.CategoryTitle;
        settingsCategorySummaryLabel.Text = snapshot.CategorySummary;
        settingsFooterLabel.Text = snapshot.Footer;
        settingsOptionButton1.Text = snapshot.Option1;
        settingsOptionButton2.Text = snapshot.Option2;
        settingsOptionButton3.Text = snapshot.Option3;
        settingsOptionButton4.Text = snapshot.Option4;
    }

    private void WireSettingsCategoryButtons()
    {
        if (settingsCategoryButtons is null)
        {
            return;
        }

        SettingsCategory[] categories =
        [
            SettingsCategory.Accessibility,
            SettingsCategory.Audio,
            SettingsCategory.Controls,
            SettingsCategory.Language,
            SettingsCategory.Graphics,
        ];

        for (int index = 0; index < settingsCategoryButtons.Length && index < categories.Length; index++)
        {
            SettingsCategory category = categories[index];
            settingsCategoryButtons[index].Pressed += () => selectedSettingsCategory = category;
        }
    }

    private void UpdateSettingsCategoryButtons()
    {
        if (settingsCategoryButtons is null)
        {
            return;
        }

        SettingsCategory[] categories =
        [
            SettingsCategory.Accessibility,
            SettingsCategory.Audio,
            SettingsCategory.Controls,
            SettingsCategory.Language,
            SettingsCategory.Graphics,
        ];

        for (int index = 0; index < settingsCategoryButtons.Length && index < categories.Length; index++)
        {
            bool isSelected = categories[index] == selectedSettingsCategory;
            settingsCategoryButtons[index].Disabled = isSelected;
            settingsCategoryButtons[index].Text = isSelected
                ? $"> {GetSettingsCategoryLabel(categories[index])}"
                : GetSettingsCategoryLabel(categories[index]);
        }
    }

    private void ApplyPresentationSettings()
    {
        if (runtime is null || hudRoot is null)
        {
            return;
        }

        PlayerSettings settings = runtime.PlayerProfile.Settings;
        float brightnessFactor = settings.BrightnessPercent / 100f;
        float scaleFactor = settings.Accessibility.UiScalePercent / 100f;
        if (settings.Accessibility.LargeText)
        {
            scaleFactor *= 1.15f;
        }

        Color accessibilityTint = settings.Accessibility.ColorProfile switch
        {
            AccessibilityColorProfile.Protanopia => new Color(0.92f, 1.0f, 1.08f, 1f),
            AccessibilityColorProfile.Deuteranopia => new Color(1.0f, 0.96f, 1.08f, 1f),
            AccessibilityColorProfile.Tritanopia => new Color(1.06f, 0.98f, 0.92f, 1f),
            _ => Colors.White,
        };
        Color brightnessColor = new(
            brightnessFactor * accessibilityTint.R,
            brightnessFactor * accessibilityTint.G,
            brightnessFactor * accessibilityTint.B,
            1f);

        hudRoot.Scale = new Vector2(scaleFactor, scaleFactor);
        hudRoot.Modulate = settings.Accessibility.HighContrastMode ? brightnessColor.Lightened(0.15f) : brightnessColor;

        float minimumButtonHeight = settings.Accessibility.LargeTouchTargets ? 52f : 40f;
        ApplyButtonSize(inspectButton, minimumButtonHeight);
        ApplyButtonSize(timeControlsButton, minimumButtonHeight);
        ApplyButtonSize(stepTickButton, minimumButtonHeight);
        ApplyButtonSize(encyclopediaButton, minimumButtonHeight);
        ApplyButtonSize(statisticsButton, minimumButtonHeight);
        ApplyButtonSize(settingsButton, minimumButtonHeight);
        ApplyButtonSize(encyclopediaFilterButton, minimumButtonHeight);
        ApplyButtonSize(encyclopediaSortButton, minimumButtonHeight);
        ApplyButtonSize(encyclopediaCompareButton, minimumButtonHeight);
        ApplyButtonSize(settingsOptionButton1, minimumButtonHeight + 2f);
        ApplyButtonSize(settingsOptionButton2, minimumButtonHeight + 2f);
        ApplyButtonSize(settingsOptionButton3, minimumButtonHeight + 2f);
        ApplyButtonSize(settingsOptionButton4, minimumButtonHeight + 2f);
    }

    private static void ApplyButtonSize(Button? button, float minimumHeight)
    {
        if (button is null)
        {
            return;
        }

        Vector2 current = button.CustomMinimumSize;
        button.CustomMinimumSize = new Vector2(current.X, minimumHeight);
    }

    private string BuildSettingsCategorySummary(PlayerSettings settings, SettingsCategory category)
    {
        return category switch
        {
            SettingsCategory.Accessibility => $"UI Scale: {settings.Accessibility.UiScalePercent}%\nHigh Contrast: {OnOff(settings.Accessibility.HighContrastMode)}\nReduced Motion: {OnOff(settings.Accessibility.ReducedMotion)}\nSimplified Notifications: {OnOff(settings.Accessibility.SimplifiedNotifications)}",
            SettingsCategory.Audio => $"Master Volume: {settings.MasterVolumePercent}%\nMusic Volume: {settings.MusicVolumePercent}%\nEffects Volume: {settings.EffectsVolumePercent}%\nVisual Event Indicators: {OnOff(settings.Accessibility.VisualEventIndicators)}",
            SettingsCategory.Controls => $"Large Touch Targets: {OnOff(settings.Accessibility.LargeTouchTargets)}\nToggle Instead Of Hold: {OnOff(settings.Accessibility.ToggleInsteadOfHold)}\nHold Duration: {settings.Accessibility.HoldDurationMilliseconds} ms\nController Support: {OnOff(settings.ControllerSupportEnabled)}",
            SettingsCategory.Language => $"Language: {settings.Language}\nLarge Text: {OnOff(settings.Accessibility.LargeText)}\nSubtitle Size: {settings.Accessibility.SubtitleSizePercent}%\nColor Profile: {settings.Accessibility.ColorProfile}",
            SettingsCategory.Graphics => $"Brightness: {settings.BrightnessPercent}%\nHigh Contrast: {OnOff(settings.Accessibility.HighContrastMode)}\nColor Profile: {settings.Accessibility.ColorProfile}\nReduced Motion: {OnOff(settings.Accessibility.ReducedMotion)}",
            _ => string.Empty,
        };
    }

    private string GetSettingsOptionLabel(PlayerSettings settings, SettingsCategory category, int optionIndex)
    {
        return (category, optionIndex) switch
        {
            (SettingsCategory.Accessibility, 0) => $"UI Scale: {settings.Accessibility.UiScalePercent}%",
            (SettingsCategory.Accessibility, 1) => $"High Contrast: {OnOff(settings.Accessibility.HighContrastMode)}",
            (SettingsCategory.Accessibility, 2) => $"Color Profile: {settings.Accessibility.ColorProfile}",
            (SettingsCategory.Accessibility, 3) => $"Reduced Motion: {OnOff(settings.Accessibility.ReducedMotion)}",
            (SettingsCategory.Audio, 0) => $"Master Volume: {settings.MasterVolumePercent}%",
            (SettingsCategory.Audio, 1) => $"Music Volume: {settings.MusicVolumePercent}%",
            (SettingsCategory.Audio, 2) => $"Effects Volume: {settings.EffectsVolumePercent}%",
            (SettingsCategory.Audio, 3) => $"Visual Indicators: {OnOff(settings.Accessibility.VisualEventIndicators)}",
            (SettingsCategory.Controls, 0) => $"Large Touch Targets: {OnOff(settings.Accessibility.LargeTouchTargets)}",
            (SettingsCategory.Controls, 1) => $"Toggle Instead Of Hold: {OnOff(settings.Accessibility.ToggleInsteadOfHold)}",
            (SettingsCategory.Controls, 2) => $"Hold Duration: {settings.Accessibility.HoldDurationMilliseconds} ms",
            (SettingsCategory.Controls, 3) => $"Controller Support: {OnOff(settings.ControllerSupportEnabled)}",
            (SettingsCategory.Language, 0) => $"Language: {settings.Language}",
            (SettingsCategory.Language, 1) => $"Large Text: {OnOff(settings.Accessibility.LargeText)}",
            (SettingsCategory.Language, 2) => $"Subtitle Size: {settings.Accessibility.SubtitleSizePercent}%",
            (SettingsCategory.Language, 3) => $"Simplified Notifications: {OnOff(settings.Accessibility.SimplifiedNotifications)}",
            (SettingsCategory.Graphics, 0) => $"Brightness: {settings.BrightnessPercent}%",
            (SettingsCategory.Graphics, 1) => $"High Contrast: {OnOff(settings.Accessibility.HighContrastMode)}",
            (SettingsCategory.Graphics, 2) => $"Color Profile: {settings.Accessibility.ColorProfile}",
            (SettingsCategory.Graphics, 3) => $"Reduced Motion: {OnOff(settings.Accessibility.ReducedMotion)}",
            _ => string.Empty,
        };
    }

    private static string GetSettingsCategoryLabel(SettingsCategory category)
    {
        return category switch
        {
            SettingsCategory.Accessibility => "Accessibility",
            SettingsCategory.Audio => "Audio",
            SettingsCategory.Controls => "Controls",
            SettingsCategory.Language => "Language",
            SettingsCategory.Graphics => "Graphics",
            _ => "Settings",
        };
    }

    private static string OnOff(bool value)
    {
        return value ? "On" : "Off";
    }

    private static string GetCategoryLabel(EncyclopediaCategory category)
    {
        return category switch
        {
            EncyclopediaCategory.Species => "Species",
            EncyclopediaCategory.Traits => "Traits",
            EncyclopediaCategory.Biomes => "Biomes",
            EncyclopediaCategory.Resources => "Resources",
            EncyclopediaCategory.Climate => "Climate",
            EncyclopediaCategory.Behaviours => "Behaviours",
            EncyclopediaCategory.Evolution => "Evolution",
            EncyclopediaCategory.WorldHistory => "World History",
            _ => "Unknown",
        };
    }

    private static string GetFilterModeLabel(EncyclopediaFilterMode mode)
    {
        return mode switch
        {
            EncyclopediaFilterMode.All => "All",
            EncyclopediaFilterMode.Discovered => "Discovered",
            EncyclopediaFilterMode.Incomplete => "Incomplete",
            EncyclopediaFilterMode.RecentlyUpdated => "Recent",
            _ => "All",
        };
    }

    private static string GetSortModeLabel(EncyclopediaSortMode mode)
    {
        return mode switch
        {
            EncyclopediaSortMode.Alphabetical => "A-Z",
            EncyclopediaSortMode.DiscoveryDate => "Discovery",
            EncyclopediaSortMode.ObservationCount => "Observed",
            EncyclopediaSortMode.CompletionPercentage => "Completion",
            _ => "A-Z",
        };
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
            notificationBodyLabels[index].Text = runtime is not null && runtime.PlayerProfile.Settings.Accessibility.SimplifiedNotifications
                ? string.Empty
                : entry.Message;
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

    private int CountExtinctSpecies()
    {
        if (runtime is null)
        {
            return 0;
        }

        int extinctSpecies = 0;
        foreach (GaiaEngine.Domain.Genetics.Species species in runtime.Species.GetAll())
        {
            if (species.IsExtinct)
            {
                extinctSpecies++;
            }
        }

        return extinctSpecies;
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
                SelectionHint: focusOverrideKind == FocusOverrideKind.Automatic
                    ? "Observed focus updates automatically. Use Inspect to cycle through organisms."
                    : "Manual focus is active. Use Inspect to move to the next observed organism.",
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
            SelectionHint: focusOverrideKind == FocusOverrideKind.Chunk
                ? "Chunk focus is active. Use Inspect to return to the first available organism."
                : "No living organism is available, so the HUD is observing the primary world chunk.",
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

        List<GaiaEngine.Domain.Organisms.Organism> liveOrganisms = GetLiveOrganisms();
        if (focusOverrideKind == FocusOverrideKind.Organism && focusedOrganismId.HasValue)
        {
            foreach (GaiaEngine.Domain.Organisms.Organism liveOrganism in liveOrganisms)
            {
                if (liveOrganism.Id == focusedOrganismId.Value)
                {
                    return liveOrganism;
                }
            }
        }

        if (focusOverrideKind == FocusOverrideKind.Chunk)
        {
            return null;
        }

        foreach (GaiaEngine.Domain.Organisms.Organism organism in liveOrganisms)
        {
            return organism;
        }

        return null;
    }

    private List<GaiaEngine.Domain.Organisms.Organism> GetLiveOrganisms()
    {
        List<GaiaEngine.Domain.Organisms.Organism> liveOrganisms = new();
        if (runtime is null)
        {
            return liveOrganisms;
        }

        foreach (GaiaEngine.Domain.Organisms.Organism organism in runtime.Organisms.GetAll())
        {
            if (organism.Lifecycle.IsAlive)
            {
                liveOrganisms.Add(organism);
            }
        }

        return liveOrganisms;
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

    private void OnInspectPressed()
    {
        if (runtime is null)
        {
            return;
        }

        List<GaiaEngine.Domain.Organisms.Organism> liveOrganisms = GetLiveOrganisms();
        if (liveOrganisms.Count == 0)
        {
            focusOverrideKind = FocusOverrideKind.Chunk;
            focusedOrganismId = null;
            return;
        }

        GaiaEngine.Domain.Organisms.Organism? currentOrganism = TryResolveObservedOrganism();
        if (currentOrganism is null)
        {
            focusOverrideKind = FocusOverrideKind.Organism;
            focusedOrganismId = liveOrganisms[0].Id;
            return;
        }

        int currentIndex = -1;
        for (int index = 0; index < liveOrganisms.Count; index++)
        {
            if (liveOrganisms[index].Id == currentOrganism.Id)
            {
                currentIndex = index;
                break;
            }
        }

        if (currentIndex >= 0 && currentIndex < liveOrganisms.Count - 1)
        {
            focusOverrideKind = FocusOverrideKind.Organism;
            focusedOrganismId = liveOrganisms[currentIndex + 1].Id;
            return;
        }

        focusOverrideKind = FocusOverrideKind.Chunk;
        focusedOrganismId = null;
    }

    private void OnTimeControlsPressed()
    {
        isSimulationPaused = !isSimulationPaused;
        if (!isSimulationPaused)
        {
            tickAccumulator = 0d;
        }
    }

    private void OnStepTickPressed()
    {
        if (runtime is null || !isSimulationPaused)
        {
            return;
        }

        AdvanceSimulationTick();
    }

    private void OnEncyclopediaPressed()
    {
        if (runtime is null || runtime.PlayerProfile.Knowledge.Encyclopedia.Count == 0)
        {
            return;
        }

        isEncyclopediaOverlayVisible = !isEncyclopediaOverlayVisible;
        if (isEncyclopediaOverlayVisible)
        {
            isStatisticsOverlayVisible = false;
            isSettingsOverlayVisible = false;
        }
    }

    private void OnEncyclopediaFilterPressed()
    {
        encyclopediaFilterMode = encyclopediaFilterMode switch
        {
            EncyclopediaFilterMode.All => EncyclopediaFilterMode.Discovered,
            EncyclopediaFilterMode.Discovered => EncyclopediaFilterMode.Incomplete,
            EncyclopediaFilterMode.Incomplete => EncyclopediaFilterMode.RecentlyUpdated,
            _ => EncyclopediaFilterMode.All,
        };
    }

    private void OnEncyclopediaSortPressed()
    {
        encyclopediaSortMode = encyclopediaSortMode switch
        {
            EncyclopediaSortMode.Alphabetical => EncyclopediaSortMode.DiscoveryDate,
            EncyclopediaSortMode.DiscoveryDate => EncyclopediaSortMode.ObservationCount,
            EncyclopediaSortMode.ObservationCount => EncyclopediaSortMode.CompletionPercentage,
            _ => EncyclopediaSortMode.Alphabetical,
        };
    }

    private void OnEncyclopediaComparePressed()
    {
        EncyclopediaEntry? selectedEntry = TryResolveEncyclopediaEntry(selectedEncyclopediaEntryId);
        if (selectedEntry is null)
        {
            compareEncyclopediaEntryId = null;
            return;
        }

        compareEncyclopediaEntryId = compareEncyclopediaEntryId is null ? selectedEntry.EntryId : null;
    }

    private void OnEncyclopediaSearchTextChanged(string text)
    {
        encyclopediaSearchText = text ?? string.Empty;
        selectedEncyclopediaEntryId = null;
    }

    private void OnEncyclopediaEntrySelected(long index)
    {
        IReadOnlyList<EncyclopediaEntry> filteredEntries = GetFilteredEncyclopediaEntries();
        if (index < 0 || index >= filteredEntries.Count)
        {
            return;
        }

        selectedEncyclopediaEntryId = filteredEntries[(int)index].EntryId;
    }

    private void OnRelatedEntrySelected(long index)
    {
        EncyclopediaEntry? selectedEntry = TryResolveEncyclopediaEntry(selectedEncyclopediaEntryId);
        if (selectedEntry is null)
        {
            return;
        }

        IReadOnlyList<string> relatedEntries = selectedEntry.GetRelatedEntries();
        if (index < 0 || index >= relatedEntries.Count)
        {
            return;
        }

        EncyclopediaEntry? relatedEntry = TryResolveEncyclopediaEntry(relatedEntries[(int)index]);
        if (relatedEntry is null)
        {
            return;
        }

        selectedEncyclopediaCategory = relatedEntry.Category;
        selectedEncyclopediaEntryId = relatedEntry.EntryId;
    }

    private void OnEncyclopediaCategoryPressed(EncyclopediaCategory category)
    {
        selectedEncyclopediaCategory = category;
        selectedEncyclopediaEntryId = null;
        compareEncyclopediaEntryId = null;
    }

    private void OnStatisticsPressed()
    {
        isStatisticsOverlayVisible = !isStatisticsOverlayVisible;
        if (isStatisticsOverlayVisible)
        {
            isEncyclopediaOverlayVisible = false;
            isSettingsOverlayVisible = false;
        }
    }

    private void OnSettingsPressed()
    {
        if (runtime is null)
        {
            return;
        }

        isSettingsOverlayVisible = !isSettingsOverlayVisible;
        if (isSettingsOverlayVisible)
        {
            isStatisticsOverlayVisible = false;
            isEncyclopediaOverlayVisible = false;
        }
    }

    private void OnSettingsOptionPressed(int optionIndex)
    {
        if (runtime is null)
        {
            return;
        }

        runtime.UpdatePlayerSettings(CreateUpdatedSettings(runtime.PlayerProfile.Settings, selectedSettingsCategory, optionIndex));
    }

    private PlayerSettings CreateUpdatedSettings(PlayerSettings settings, SettingsCategory category, int optionIndex)
    {
        AccessibilitySettings accessibility = settings.Accessibility;
        AccessibilitySettings updatedAccessibility = accessibility;
        int brightnessPercent = settings.BrightnessPercent;
        int masterVolumePercent = settings.MasterVolumePercent;
        int musicVolumePercent = settings.MusicVolumePercent;
        int effectsVolumePercent = settings.EffectsVolumePercent;
        bool controllerSupportEnabled = settings.ControllerSupportEnabled;
        string language = settings.Language;

        switch (category)
        {
            case SettingsCategory.Accessibility:
                updatedAccessibility = optionIndex switch
                {
                    0 => CreateAccessibilitySettings(accessibility, uiScalePercent: CycleUiScale(accessibility.UiScalePercent)),
                    1 => CreateAccessibilitySettings(accessibility, highContrastMode: !accessibility.HighContrastMode),
                    2 => CreateAccessibilitySettings(accessibility, colorProfile: CycleColorProfile(accessibility.ColorProfile)),
                    3 => CreateAccessibilitySettings(accessibility, reducedMotion: !accessibility.ReducedMotion),
                    _ => accessibility,
                };
                break;

            case SettingsCategory.Audio:
                if (optionIndex == 0)
                {
                    masterVolumePercent = CycleVolume(settings.MasterVolumePercent);
                }
                else if (optionIndex == 1)
                {
                    musicVolumePercent = CycleVolume(settings.MusicVolumePercent);
                }
                else if (optionIndex == 2)
                {
                    effectsVolumePercent = CycleVolume(settings.EffectsVolumePercent);
                }
                else if (optionIndex == 3)
                {
                    updatedAccessibility = CreateAccessibilitySettings(accessibility, visualEventIndicators: !accessibility.VisualEventIndicators);
                }

                break;

            case SettingsCategory.Controls:
                if (optionIndex == 0)
                {
                    updatedAccessibility = CreateAccessibilitySettings(accessibility, largeTouchTargets: !accessibility.LargeTouchTargets);
                }
                else if (optionIndex == 1)
                {
                    updatedAccessibility = CreateAccessibilitySettings(accessibility, toggleInsteadOfHold: !accessibility.ToggleInsteadOfHold);
                }
                else if (optionIndex == 2)
                {
                    updatedAccessibility = CreateAccessibilitySettings(accessibility, holdDurationMilliseconds: CycleHoldDuration(accessibility.HoldDurationMilliseconds));
                }
                else if (optionIndex == 3)
                {
                    controllerSupportEnabled = !settings.ControllerSupportEnabled;
                }

                break;

            case SettingsCategory.Language:
                if (optionIndex == 0)
                {
                    language = CycleLanguage(settings.Language);
                }
                else if (optionIndex == 1)
                {
                    updatedAccessibility = CreateAccessibilitySettings(accessibility, largeText: !accessibility.LargeText);
                }
                else if (optionIndex == 2)
                {
                    updatedAccessibility = CreateAccessibilitySettings(accessibility, subtitleSizePercent: CycleSubtitleScale(accessibility.SubtitleSizePercent));
                }
                else if (optionIndex == 3)
                {
                    updatedAccessibility = CreateAccessibilitySettings(accessibility, simplifiedNotifications: !accessibility.SimplifiedNotifications);
                }

                break;

            case SettingsCategory.Graphics:
                if (optionIndex == 0)
                {
                    brightnessPercent = CycleBrightness(settings.BrightnessPercent);
                }
                else if (optionIndex == 1)
                {
                    updatedAccessibility = CreateAccessibilitySettings(accessibility, highContrastMode: !accessibility.HighContrastMode);
                }
                else if (optionIndex == 2)
                {
                    updatedAccessibility = CreateAccessibilitySettings(accessibility, colorProfile: CycleColorProfile(accessibility.ColorProfile));
                }
                else if (optionIndex == 3)
                {
                    updatedAccessibility = CreateAccessibilitySettings(accessibility, reducedMotion: !accessibility.ReducedMotion);
                }

                break;
        }

        return new PlayerSettings(
            language,
            updatedAccessibility,
            brightnessPercent,
            masterVolumePercent,
            musicVolumePercent,
            effectsVolumePercent,
            controllerSupportEnabled);
    }

    private static AccessibilitySettings CreateAccessibilitySettings(
        AccessibilitySettings source,
        bool? highContrastMode = null,
        bool? largeText = null,
        int? uiScalePercent = null,
        AccessibilityColorProfile? colorProfile = null,
        bool? reducedMotion = null,
        int? subtitleSizePercent = null,
        bool? simplifiedNotifications = null,
        bool? visualEventIndicators = null,
        bool? largeTouchTargets = null,
        bool? toggleInsteadOfHold = null,
        int? holdDurationMilliseconds = null)
    {
        return new AccessibilitySettings(
            highContrastMode ?? source.HighContrastMode,
            largeText ?? source.LargeText,
            uiScalePercent ?? source.UiScalePercent,
            colorProfile ?? source.ColorProfile,
            reducedMotion ?? source.ReducedMotion,
            subtitleSizePercent ?? source.SubtitleSizePercent,
            simplifiedNotifications ?? source.SimplifiedNotifications,
            visualEventIndicators ?? source.VisualEventIndicators,
            largeTouchTargets ?? source.LargeTouchTargets,
            toggleInsteadOfHold ?? source.ToggleInsteadOfHold,
            holdDurationMilliseconds ?? source.HoldDurationMilliseconds);
    }

    private static int CycleUiScale(int currentScale)
    {
        return currentScale switch
        {
            75 => 100,
            100 => 125,
            125 => 150,
            150 => 200,
            _ => 75,
        };
    }

    private static int CycleSubtitleScale(int currentScale)
    {
        return currentScale switch
        {
            75 => 100,
            100 => 125,
            125 => 150,
            150 => 200,
            _ => 75,
        };
    }

    private static int CycleVolume(int currentVolume)
    {
        return currentVolume switch
        {
            0 => 25,
            25 => 50,
            50 => 75,
            75 => 100,
            _ => 0,
        };
    }

    private static int CycleBrightness(int currentBrightness)
    {
        return currentBrightness switch
        {
            50 => 75,
            75 => 100,
            100 => 125,
            125 => 150,
            _ => 50,
        };
    }

    private static int CycleHoldDuration(int currentDuration)
    {
        return currentDuration switch
        {
            0 => 250,
            250 => 500,
            500 => 750,
            750 => 1000,
            _ => 0,
        };
    }

    private static string CycleLanguage(string currentLanguage)
    {
        return currentLanguage switch
        {
            "en" => "es",
            "es" => "fr",
            _ => "en",
        };
    }

    private static AccessibilityColorProfile CycleColorProfile(AccessibilityColorProfile currentProfile)
    {
        return currentProfile switch
        {
            AccessibilityColorProfile.None => AccessibilityColorProfile.Protanopia,
            AccessibilityColorProfile.Protanopia => AccessibilityColorProfile.Deuteranopia,
            AccessibilityColorProfile.Deuteranopia => AccessibilityColorProfile.Tritanopia,
            _ => AccessibilityColorProfile.None,
        };
    }

    private void AdvanceSimulationTick()
    {
        if (runtime is null || notificationQueue is null)
        {
            return;
        }

        SimulationTickResult result = runtime.AdvanceTick();
        CaptureStatisticsSample(result);
        IReadOnlyList<HudNotificationEntry> entries = BuildNotificationsFromObservedChanges();
        notificationQueue.EnqueueRange(entries);
    }

    private void CaptureStatisticsSample(SimulationTickResult result)
    {
        if (runtime is null || result.Diagnostics is null)
        {
            return;
        }

        lastDiagnostics = result.Diagnostics;
        StatisticsHistorySample sample = new(
            result.Diagnostics.Tick,
            result.Diagnostics.Day,
            result.Diagnostics.Season,
            result.Diagnostics.Year,
            runtime.Organisms.Count,
            CountAliveOrganisms(),
            runtime.Species.Count,
            CountExtinctSpecies(),
            runtime.World.GetChunks()[0].Climate.WeatherState.ToString(),
            runtime.World.GetChunks()[0].Climate.Temperature.CurrentTemperature,
            runtime.World.GetChunks()[0].Climate.Humidity.RelativeHumidity,
            runtime.PlayerProfile.Knowledge.Discoveries.Count,
            runtime.PlayerProfile.Knowledge.Encyclopedia.Count,
            runtime.PlayerProfile.Progression.CompletedObjectives,
            runtime.PlayerProfile.Achievements.Count,
            result.Diagnostics.PublishedEventCount,
            result.Diagnostics.ProcessedEventCount,
            result.Diagnostics.ExecutedPhaseCount,
            result.Diagnostics.ScheduledSystemCount);
        if (statisticsHistory.Count >= 8)
        {
            statisticsHistory.RemoveAt(0);
        }

        statisticsHistory.Add(sample);
    }

    private string BuildPerformanceText()
    {
        if (lastDiagnostics is null)
        {
            return "Performance Snapshot\nWaiting for the first scheduled statistics sample at tick 100.";
        }

        return
            $"Performance Snapshot\nLast sampled tick: {lastDiagnostics.Tick}\nExecuted phases: {lastDiagnostics.ExecutedPhaseCount}\nScheduled systems: {lastDiagnostics.ScheduledSystemCount}\nPublished events: {lastDiagnostics.PublishedEventCount}\nProcessed events: {lastDiagnostics.ProcessedEventCount}";
    }

    private string BuildStatisticsHistoryText(bool populationGraphsUnlocked)
    {
        if (!populationGraphsUnlocked)
        {
            return "Sampled History\nPopulation graph history is locked until the corresponding progression unlock is earned.";
        }

        if (statisticsHistory.Count == 0)
        {
            return "Sampled History\nNo scheduled statistics samples are available yet.";
        }

        StringBuilder builder = new("Sampled History\n");
        foreach (StatisticsHistorySample sample in statisticsHistory)
        {
            builder.Append("Tick ")
                .Append(sample.Tick)
                .Append(" | Day ")
                .Append(sample.Day)
                .Append(" | ")
                .Append(sample.Season)
                .Append(" Y")
                .Append(sample.Year)
                .Append(" | Alive ")
                .Append(sample.AlivePopulation)
                .Append('/')
                .Append(sample.TotalPopulation)
                .Append(" | Species ")
                .Append(sample.SpeciesCount)
                .Append(" | Extinct ")
                .Append(sample.ExtinctSpecies)
                .Append(" | ")
                .Append(sample.Weather)
                .Append(" ")
                .Append(sample.Temperature)
                .Append(" C")
                .Append(" | Humidity ")
                .Append(sample.Humidity)
                .Append('%')
                .Append(" | Discoveries ")
                .Append(sample.Discoveries)
                .Append(" | Encyclopedia ")
                .Append(sample.EncyclopediaEntries)
                .Append(" | Objectives ")
                .Append(sample.CompletedObjectives)
                .Append(" | Achievements ")
                .Append(sample.Achievements)
                .Append(" | Events ")
                .Append(sample.PublishedEvents)
                .Append('/')
                .Append(sample.ProcessedEvents)
                .Append(" | Systems ")
                .Append(sample.ScheduledSystems)
                .AppendLine();
        }

        return builder.ToString().TrimEnd();
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

    private sealed record StatisticsViewSnapshot(
        bool IsVisible,
        string Status,
        string Population,
        string Climate,
        string Discovery,
        string Performance,
        string History);

    private sealed record EncyclopediaViewSnapshot(
        bool IsVisible,
        string FilterLabel,
        string SortLabel,
        EncyclopediaCategory Category,
        string SearchText,
        string? SelectedEntryId,
        string? CompareEntryId,
        string EntryListStatus,
        string DetailsStatus,
        string DetailsBody,
        string DetailsStatistics,
        string ComparisonSummary,
        string ProgressSummary,
        string[] ActiveEntryIds,
        string[] RelatedEntryIds);

    private sealed record SettingsViewSnapshot(
        bool IsVisible,
        SettingsCategory Category,
        string CategoryTitle,
        string CategorySummary,
        string Footer,
        string Option1,
        string Option2,
        string Option3,
        string Option4);

    private sealed record StatisticsHistorySample(
        long Tick,
        int Day,
        string Season,
        int Year,
        int TotalPopulation,
        int AlivePopulation,
        int SpeciesCount,
        int ExtinctSpecies,
        string Weather,
        int Temperature,
        int Humidity,
        int Discoveries,
        int EncyclopediaEntries,
        int CompletedObjectives,
        int Achievements,
        int PublishedEvents,
        int ProcessedEvents,
        int ExecutedPhases,
        int ScheduledSystems);

    private enum SettingsCategory
    {
        Accessibility = 0,
        Audio = 1,
        Controls = 2,
        Language = 3,
        Graphics = 4,
    }

    private enum EncyclopediaFilterMode
    {
        All = 0,
        Discovered = 1,
        Incomplete = 2,
        RecentlyUpdated = 3,
    }

    private enum EncyclopediaSortMode
    {
        Alphabetical = 0,
        DiscoveryDate = 1,
        ObservationCount = 2,
        CompletionPercentage = 3,
    }

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

    private enum FocusOverrideKind
    {
        Automatic = 0,
        Organism = 1,
        Chunk = 2,
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

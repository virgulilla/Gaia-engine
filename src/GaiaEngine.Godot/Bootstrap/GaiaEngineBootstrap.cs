using GaiaEngine.App.Bootstrap;
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
    private const string NotificationTitleLabelPath = "HudLayer/HudRoot/NotificationArea/NotificationCard/NotificationMargin/NotificationColumn/NotificationTitleLabel";
    private const string NotificationBodyLabelPath = "HudLayer/HudRoot/NotificationArea/NotificationCard/NotificationMargin/NotificationColumn/NotificationBodyLabel";

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
    private Label? notificationTitleLabel;
    private Label? notificationBodyLabel;
    private double tickAccumulator;
    private HudViewSnapshot? lastSnapshot;

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
        notificationTitleLabel = GetNode<Label>(NotificationTitleLabelPath);
        notificationBodyLabel = GetNode<Label>(NotificationBodyLabelPath);

        UpdateSimulationStatusText();
        GD.Print($"Gaia Engine initialized with tick rate {runtime.EngineConfiguration.TickRate}.");
    }

    /// <summary>
    /// Advances the minimal simulation session and refreshes the HUD.
    /// </summary>
    /// <param name="delta">The real elapsed frame time.</param>
    public override void _Process(double delta)
    {
        if (runtime is null)
        {
            return;
        }

        double secondsPerTick = 1d / runtime.EngineConfiguration.TickRate;
        tickAccumulator += delta;

        while (tickAccumulator >= secondsPerTick)
        {
            runtime.AdvanceTick();
            tickAccumulator -= secondsPerTick;
        }

        UpdateSimulationStatusText();
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
            || achievementCountLabel is null
            || notificationTitleLabel is null
            || notificationBodyLabel is null)
        {
            return;
        }

        int aliveOrganisms = 0;
        foreach (GaiaEngine.Domain.Organisms.Organism organism in runtime.Organisms.GetAll())
        {
            if (organism.Lifecycle.IsAlive)
            {
                aliveOrganisms++;
            }
        }

        int memoryEntries = 0;
        foreach (GaiaEngine.Domain.AI.OrganismMemory memory in runtime.Memories.GetAll())
        {
            memoryEntries += memory.Count;
        }

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
            BuildNotificationTitle(primaryChunk),
            BuildNotificationBody(primaryChunk, aliveOrganisms));

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
        notificationTitleLabel.Text = snapshot.NotificationTitle;
        notificationBodyLabel.Text = snapshot.NotificationBody;
    }

    private static string BuildNotificationTitle(GaiaEngine.Domain.World.Chunk primaryChunk)
    {
        return primaryChunk.Climate.WeatherState switch
        {
            GaiaEngine.Domain.World.WeatherState.Storm => "Weather Warning",
            GaiaEngine.Domain.World.WeatherState.Drought => "Climate Warning",
            GaiaEngine.Domain.World.WeatherState.Rain => "Seasonal Update",
            _ => "Observation Mode",
        };
    }

    private static string BuildNotificationBody(GaiaEngine.Domain.World.Chunk primaryChunk, int aliveOrganisms)
    {
        return primaryChunk.Climate.WeatherState switch
        {
            GaiaEngine.Domain.World.WeatherState.Storm => $"A storm is active over the observed biome. {aliveOrganisms} organisms remain alive in the current simulation snapshot.",
            GaiaEngine.Domain.World.WeatherState.Drought => $"Dry conditions are active in {primaryChunk.Biome.Name}. Monitor resources as the ecosystem evolves.",
            GaiaEngine.Domain.World.WeatherState.Rain => $"Rain is currently falling over {primaryChunk.Biome.Name}. The simulation continues in read-only observation mode.",
            _ => $"The simulation is running in {primaryChunk.Biome.Name}. Interactive inspection, filters and notifications will expand in later UI steps.",
        };
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
        string NotificationTitle,
        string NotificationBody);
}

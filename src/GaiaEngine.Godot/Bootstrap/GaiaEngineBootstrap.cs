using GaiaEngine.App.Bootstrap;
using GaiaEngine.Gameplay.Discovery;
using Godot;

namespace GaiaEngine.Godot.Bootstrap;

/// <summary>
/// Adapts the Godot scene lifecycle to the Gaia Engine application bootstrap.
/// </summary>
public sealed partial class GaiaEngineBootstrap : Node
{
    private const string TickLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/TickLabel";
    private const string DayLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/DayLabel";
    private const string SeasonLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/SeasonLabel";
    private const string YearLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/YearLabel";
    private const string TickRateLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/TickRateLabel";
    private const string PlayerLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/PlayerLabel";
    private const string DiscoveryCountLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/DiscoveryCountLabel";
    private const string SpeciesDiscoveryLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/SpeciesDiscoveryLabel";
    private const string BiomeDiscoveryLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/BiomeDiscoveryLabel";
    private const string ResourceDiscoveryLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/ResourceDiscoveryLabel";
    private const string BehaviourDiscoveryLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/BehaviourDiscoveryLabel";
    private const string OrganismCountLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/OrganismCountLabel";
    private const string AliveOrganismCountLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/AliveOrganismCountLabel";
    private const string SpeciesCountLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/SpeciesCountLabel";
    private const string MemoryCountLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/MemoryCountLabel";
    private const string ActionCountLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/ActionCountLabel";
    private const string WeatherLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/WeatherLabel";
    private const string BiomeLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/BiomeLabel";
    private const string ExperienceLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/ExperienceLabel";
    private const string EncyclopediaCountLabelPath = "DiagnosticsLayer/SimulationStatusPanel/SimulationStatusMargin/SimulationStatusRows/EncyclopediaCountLabel";

    private GaiaEngineApplication? application;
    private GaiaEngineRuntime? runtime;
    private Label? tickLabel;
    private Label? dayLabel;
    private Label? seasonLabel;
    private Label? yearLabel;
    private Label? tickRateLabel;
    private Label? playerLabel;
    private Label? discoveryCountLabel;
    private Label? speciesDiscoveryLabel;
    private Label? biomeDiscoveryLabel;
    private Label? resourceDiscoveryLabel;
    private Label? behaviourDiscoveryLabel;
    private Label? organismCountLabel;
    private Label? aliveOrganismCountLabel;
    private Label? speciesCountLabel;
    private Label? memoryCountLabel;
    private Label? actionCountLabel;
    private Label? weatherLabel;
    private Label? biomeLabel;
    private Label? experienceLabel;
    private Label? encyclopediaCountLabel;
    private double tickAccumulator;

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
        tickLabel = GetNode<Label>(TickLabelPath);
        dayLabel = GetNode<Label>(DayLabelPath);
        seasonLabel = GetNode<Label>(SeasonLabelPath);
        yearLabel = GetNode<Label>(YearLabelPath);
        tickRateLabel = GetNode<Label>(TickRateLabelPath);
        playerLabel = GetNode<Label>(PlayerLabelPath);
        discoveryCountLabel = GetNode<Label>(DiscoveryCountLabelPath);
        speciesDiscoveryLabel = GetNode<Label>(SpeciesDiscoveryLabelPath);
        biomeDiscoveryLabel = GetNode<Label>(BiomeDiscoveryLabelPath);
        resourceDiscoveryLabel = GetNode<Label>(ResourceDiscoveryLabelPath);
        behaviourDiscoveryLabel = GetNode<Label>(BehaviourDiscoveryLabelPath);
        organismCountLabel = GetNode<Label>(OrganismCountLabelPath);
        aliveOrganismCountLabel = GetNode<Label>(AliveOrganismCountLabelPath);
        speciesCountLabel = GetNode<Label>(SpeciesCountLabelPath);
        memoryCountLabel = GetNode<Label>(MemoryCountLabelPath);
        actionCountLabel = GetNode<Label>(ActionCountLabelPath);
        weatherLabel = GetNode<Label>(WeatherLabelPath);
        biomeLabel = GetNode<Label>(BiomeLabelPath);
        experienceLabel = GetNode<Label>(ExperienceLabelPath);
        encyclopediaCountLabel = GetNode<Label>(EncyclopediaCountLabelPath);

        UpdateSimulationStatusText();
        GD.Print($"Gaia Engine initialized with tick rate {runtime.EngineConfiguration.TickRate}.");
    }

    /// <summary>
    /// Advances the minimal simulation session and refreshes the diagnostics panel.
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
            || tickLabel is null
            || dayLabel is null
            || seasonLabel is null
            || yearLabel is null
            || tickRateLabel is null
            || playerLabel is null
            || discoveryCountLabel is null
            || speciesDiscoveryLabel is null
            || biomeDiscoveryLabel is null
            || resourceDiscoveryLabel is null
            || behaviourDiscoveryLabel is null
            || organismCountLabel is null
            || aliveOrganismCountLabel is null
            || speciesCountLabel is null
            || memoryCountLabel is null
            || actionCountLabel is null
            || weatherLabel is null
            || biomeLabel is null
            || experienceLabel is null
            || encyclopediaCountLabel is null)
        {
            return;
        }

        int aliveOrganisms = 0;
        foreach (var organism in runtime.Organisms.GetAll())
        {
            if (organism.Lifecycle.IsAlive)
            {
                aliveOrganisms++;
            }
        }

        int memoryEntries = 0;
        foreach (var memory in runtime.Memories.GetAll())
        {
            memoryEntries += memory.Count;
        }

        var primaryChunk = runtime.World.GetChunks()[0];
        tickLabel.Text = $"Tick: {runtime.SimulationSession.CurrentTimeState.CurrentTick}";
        dayLabel.Text = $"Day: {runtime.SimulationSession.CurrentTimeState.CurrentDay}";
        seasonLabel.Text = $"Season: {runtime.SimulationSession.CurrentTimeState.CurrentSeason}";
        yearLabel.Text = $"Year: {runtime.SimulationSession.CurrentTimeState.CurrentYear}";
        tickRateLabel.Text = $"Tick Rate: {runtime.EngineConfiguration.TickRate}";
        playerLabel.Text = $"Player: {runtime.PlayerProfile.Identity.ProfileName}";
        discoveryCountLabel.Text = $"Discoveries: {runtime.PlayerProfile.Knowledge.Discoveries.Count}";
        speciesDiscoveryLabel.Text = $"Species: {runtime.CountDiscoveries(DiscoveryCategory.Species)}";
        biomeDiscoveryLabel.Text = $"Biomes: {runtime.CountDiscoveries(DiscoveryCategory.Biomes)}";
        resourceDiscoveryLabel.Text = $"Resources: {runtime.CountDiscoveries(DiscoveryCategory.Resources)}";
        behaviourDiscoveryLabel.Text = $"Behaviours: {runtime.CountDiscoveries(DiscoveryCategory.Behaviours)}";
        organismCountLabel.Text = $"Organisms: {runtime.Organisms.Count}";
        aliveOrganismCountLabel.Text = $"Alive: {aliveOrganisms}";
        speciesCountLabel.Text = $"Species Total: {runtime.Species.Count}";
        memoryCountLabel.Text = $"Memory Entries: {memoryEntries}";
        actionCountLabel.Text = $"Actions: {runtime.ActionRequests.Count}";
        weatherLabel.Text = $"Weather: {primaryChunk.Climate.WeatherState}";
        biomeLabel.Text = $"Biome: {primaryChunk.Biome.Name}";
        experienceLabel.Text = $"XP: {runtime.PlayerProfile.Progression.Experience}";
        encyclopediaCountLabel.Text = $"Encyclopedia: {runtime.PlayerProfile.Knowledge.Encyclopedia.Count}";
    }
}

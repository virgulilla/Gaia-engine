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
            || behaviourDiscoveryLabel is null)
        {
            return;
        }

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
    }
}

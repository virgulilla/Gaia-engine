using GaiaEngine.App.Bootstrap;
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

    private GaiaEngineApplication? application;
    private GaiaEngineRuntime? runtime;
    private Label? tickLabel;
    private Label? dayLabel;
    private Label? seasonLabel;
    private Label? yearLabel;
    private Label? tickRateLabel;
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
            runtime.SimulationSession.AdvanceTick();
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
            || tickRateLabel is null)
        {
            return;
        }

        tickLabel.Text = $"Tick: {runtime.SimulationSession.CurrentTimeState.CurrentTick}";
        dayLabel.Text = $"Day: {runtime.SimulationSession.CurrentTimeState.CurrentDay}";
        seasonLabel.Text = $"Season: {runtime.SimulationSession.CurrentTimeState.CurrentSeason}";
        yearLabel.Text = $"Year: {runtime.SimulationSession.CurrentTimeState.CurrentYear}";
        tickRateLabel.Text = $"Tick Rate: {runtime.EngineConfiguration.TickRate}";
    }
}

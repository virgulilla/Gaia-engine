using GaiaEngine.App.Bootstrap;
using GaiaEngine.App.Configuration;
using Godot;

namespace GaiaEngine.Godot.Bootstrap;

/// <summary>
/// Adapts the Godot scene lifecycle to the Gaia Engine application bootstrap.
/// </summary>
public sealed partial class GaiaEngineBootstrap : Node
{
    private GaiaEngineApplication? application;

    /// <summary>
    /// Initializes the application when the root scene enters the tree.
    /// </summary>
    public override void _Ready()
    {
        string configurationPath = ProjectSettings.GlobalizePath("res://Configuration/Engine/EngineConfiguration.json");
        application = GaiaEngineCompositionRoot.CreateApplication(configurationPath);

        EngineConfiguration configuration = application.Initialize();
        GD.Print($"Gaia Engine initialized with tick rate {configuration.TickRate}.");
    }
}

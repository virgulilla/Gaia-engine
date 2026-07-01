using System;
using System.IO;
using GaiaEngine.App.Bootstrap;
using GaiaEngine.App.Configuration;
using GaiaEngine.App.SaveGames;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Versioning;
using GaiaEngine.Gameplay.Player;
using GaiaEngine.Serialization.Profiles;
using GaiaEngine.Serialization.SaveGames;
using Xunit;

namespace GaiaEngine.App.Tests.SaveGames;

public sealed class GaiaEngineSaveSlotManagerTests
{
    [Fact]
    public void SaveManual_AndLoad_ShouldRestoreDeterministicRuntimeState()
    {
        string saveDirectory = Path.Combine(Path.GetTempPath(), "GaiaEngine.App.Tests", Guid.NewGuid().ToString("N"));
        try
        {
            GaiaEngineApplication savingApplication = CreateApplication();
            GaiaEngineRuntime savingRuntime = savingApplication.Initialize();
            savingRuntime.AdvanceTick();
            savingRuntime.AdvanceTick();
            savingRuntime.UpdatePlayerSettings(
                new PlayerSettings(
                    "es",
                    new AccessibilitySettings(
                        highContrastMode: true,
                        largeText: true,
                        uiScalePercent: 125,
                        AccessibilityColorProfile.Deuteranopia,
                        reducedMotion: true,
                        subtitleSizePercent: 125,
                        simplifiedNotifications: true,
                        visualEventIndicators: true,
                        largeTouchTargets: true,
                        toggleInsteadOfHold: true,
                        holdDurationMilliseconds: 500),
                    brightnessPercent: 125,
                    masterVolumePercent: 75,
                    musicVolumePercent: 50,
                    effectsVolumePercent: 75,
                    controllerSupportEnabled: true));

            GaiaEngineSaveSlotManager saveSlotManager = new(saveDirectory, new JsonWorldSaveGameSerializer(), new JsonPlayerProfileSerializer());
            SaveSlotSummary savedSlot = saveSlotManager.SaveManual(savingRuntime, "Integration Save", "2026-07-01T12:00:00.0000000Z");

            Assert.Equal(SaveSlotType.Manual, savedSlot.SlotType);
            Assert.Single(saveSlotManager.ListSlots());

            LoadedSaveSlot loadedSlot = saveSlotManager.Load(savedSlot.SlotId);
            GaiaEngineApplication loadingApplication = CreateApplication();
            GaiaEngineRuntime loadedRuntime = loadingApplication.InitializeFromSaveGame(loadedSlot.WorldSaveGame, loadedSlot.PlayerProfile);

            Assert.Equal(savingRuntime.World.TimeState.CurrentTick, loadedRuntime.World.TimeState.CurrentTick);
            Assert.Equal(savingRuntime.World.TimeState.CurrentDay, loadedRuntime.World.TimeState.CurrentDay);
            Assert.Equal(savingRuntime.World.TimeState.CurrentYear, loadedRuntime.World.TimeState.CurrentYear);
            Assert.Equal(savingRuntime.World.Metadata.Seed, loadedRuntime.World.Metadata.Seed);
            Assert.Equal(savingRuntime.Organisms.Count, loadedRuntime.Organisms.Count);
            Assert.Equal(savingRuntime.Species.Count, loadedRuntime.Species.Count);
            Assert.Equal(savingRuntime.Memories.Count, loadedRuntime.Memories.Count);
            Assert.Equal(savingRuntime.ActionRequests.Count, loadedRuntime.ActionRequests.Count);
            Assert.Equal("es", loadedRuntime.PlayerProfile.Settings.Language);
            Assert.True(loadedRuntime.PlayerProfile.Settings.Accessibility.HighContrastMode);
            Assert.Equal(125, loadedRuntime.PlayerProfile.Settings.Accessibility.UiScalePercent);
            Assert.Equal(loadedSlot.PlayerProfile.Knowledge.Discoveries.Count, loadedRuntime.PlayerProfile.Knowledge.Discoveries.Count);
            Assert.Equal(loadedSlot.PlayerProfile.Knowledge.Encyclopedia.Count, loadedRuntime.PlayerProfile.Knowledge.Encyclopedia.Count);
        }
        finally
        {
            if (Directory.Exists(saveDirectory))
            {
                Directory.Delete(saveDirectory, recursive: true);
            }
        }
    }

    private static GaiaEngineApplication CreateApplication()
    {
        return new GaiaEngineApplication(
            new FakeEngineConfigurationProvider(),
            new FakeSimulationConfigurationProvider(),
            new FakeWorldConfigurationProvider());
    }

    private sealed class FakeEngineConfigurationProvider : IEngineConfigurationProvider
    {
        private readonly EngineConfiguration configuration =
            new(new ConfigurationVersion("2026.06.28"), new EngineVersion(1, 0, 0), 30, 1, "Info");

        public EngineConfiguration Load()
        {
            return configuration;
        }
    }

    private sealed class FakeSimulationConfigurationProvider : ISimulationConfigurationProvider
    {
        private readonly SimulationConfiguration configuration = new(300, 12, 0, "Spring", 0);

        public SimulationConfiguration Load()
        {
            return configuration;
        }
    }

    private sealed class FakeWorldConfigurationProvider : IWorldConfigurationProvider
    {
        private readonly WorldConfiguration configuration = new("Gaia", 42, 2, 2, 16, 200, ClimateZone.Temperate);

        public WorldConfiguration Load()
        {
            return configuration;
        }
    }
}

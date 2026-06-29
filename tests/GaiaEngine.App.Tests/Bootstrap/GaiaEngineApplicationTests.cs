using GaiaEngine.App.Bootstrap;
using GaiaEngine.App.Configuration;
using GaiaEngine.Domain.World;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Versioning;
using Xunit;

namespace GaiaEngine.App.Tests.Bootstrap;

public sealed class GaiaEngineApplicationTests
{
    [Fact]
    public void Initialize_ShouldCacheLoadedConfiguration()
    {
        FakeEngineConfigurationProvider configurationProvider = new();
        FakeSimulationConfigurationProvider simulationConfigurationProvider = new();
        FakeWorldConfigurationProvider worldConfigurationProvider = new();
        GaiaEngineApplication application = new(configurationProvider, simulationConfigurationProvider, worldConfigurationProvider);

        GaiaEngineRuntime first = application.Initialize();
        GaiaEngineRuntime second = application.Initialize();

        Assert.Same(first, second);
        Assert.Equal(1, configurationProvider.LoadCallCount);
        Assert.Equal(1, simulationConfigurationProvider.LoadCallCount);
        Assert.Equal(1, worldConfigurationProvider.LoadCallCount);
        Assert.Equal(0, first.SimulationSession.CurrentTimeState.CurrentTick);
        Assert.Equal("Spring", first.SimulationSession.CurrentTimeState.CurrentSeason);
        Assert.Equal("Gaia", first.World.Metadata.WorldName);
        Assert.Equal(4, first.World.ChunkCount);
        Assert.True(first.World.GetChunks()[0].Terrain.Elevation.Height >= 0);
        Assert.False(string.IsNullOrWhiteSpace(first.World.GetChunks()[0].Biome.Name));
        Assert.True(first.World.GetChunks()[0].Water.SurfaceWater.WaterLevel >= 0);
        Assert.Equal(3, first.World.GetChunks()[0].Resources.Count);
        Assert.Equal(first.World.ChunkCount, first.Organisms.Count);
        Assert.Equal(first.Organisms.Count, first.Genomes.Count);
        Assert.Single(first.Species.GetAll());
        Assert.Equal(first.Organisms.Count, first.Species.GetAll()[0].GetFounderPopulation().Count);
        Assert.True(first.Genomes.TryGet(first.Organisms.GetAll()[0].GenomeId, out _));
        Assert.Empty(first.ActionRequests.GetAll());
        Assert.Single(first.World.GetChunks()[0].OrganismIds);
        Assert.Empty(first.SimulationSession.CurrentActionRequests.GetAll());
        Assert.Empty(first.SimulationSession.AdvanceTick().MovementRequests.GetAll());
        Assert.Empty(first.SimulationSession.CurrentFeedingRequests.GetAll());
        Assert.Empty(first.SimulationSession.CurrentHydrationRequests.GetAll());
    }

    private sealed class FakeEngineConfigurationProvider : IEngineConfigurationProvider
    {
        private readonly EngineConfiguration configuration =
            new(new ConfigurationVersion("2026.06.28"), new EngineVersion(1, 0, 0), 30, 1, "Info");

        public int LoadCallCount { get; private set; }

        public EngineConfiguration Load()
        {
            LoadCallCount++;
            return configuration;
        }
    }

    private sealed class FakeSimulationConfigurationProvider : ISimulationConfigurationProvider
    {
        private readonly SimulationConfiguration configuration = new(300, 12, 0, "Spring", 0);

        public int LoadCallCount { get; private set; }

        public SimulationConfiguration Load()
        {
            LoadCallCount++;
            return configuration;
        }
    }

    private sealed class FakeWorldConfigurationProvider : IWorldConfigurationProvider
    {
        private readonly WorldConfiguration configuration = new("Gaia", 42, 2, 2, 16, 200, ClimateZone.Temperate);

        public int LoadCallCount { get; private set; }

        public WorldConfiguration Load()
        {
            LoadCallCount++;
            return configuration;
        }
    }
}

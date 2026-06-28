using GaiaEngine.App.Bootstrap;
using GaiaEngine.App.Configuration;
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
        GaiaEngineApplication application = new(configurationProvider, simulationConfigurationProvider);

        GaiaEngineRuntime first = application.Initialize();
        GaiaEngineRuntime second = application.Initialize();

        Assert.Same(first, second);
        Assert.Equal(1, configurationProvider.LoadCallCount);
        Assert.Equal(1, simulationConfigurationProvider.LoadCallCount);
        Assert.Equal(0, first.SimulationSession.CurrentTimeState.CurrentTick);
        Assert.Equal("Spring", first.SimulationSession.CurrentTimeState.CurrentSeason);
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
}

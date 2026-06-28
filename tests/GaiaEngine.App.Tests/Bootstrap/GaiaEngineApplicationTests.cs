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
        GaiaEngineApplication application = new(configurationProvider);

        EngineConfiguration first = application.Initialize();
        EngineConfiguration second = application.Initialize();

        Assert.Same(first, second);
        Assert.Equal(1, configurationProvider.LoadCallCount);
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
}

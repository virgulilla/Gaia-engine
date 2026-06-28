using System;
using System.IO;
using GaiaEngine.App.Configuration;
using Xunit;

namespace GaiaEngine.App.Tests.Configuration;

public sealed class JsonEngineConfigurationProviderTests
{
    [Fact]
    public void Load_ShouldReadConfigurationValues()
    {
        string configurationPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.json");

        try
        {
            File.WriteAllText(
                configurationPath,
                """
                {
                  "configurationVersion": "2026.06.28",
                  "engineVersion": "1.2.3",
                  "tickRate": 30,
                  "threadCount": 1,
                  "loggingLevel": "Info"
                }
                """);

            JsonEngineConfigurationProvider provider = new(configurationPath);

            EngineConfiguration configuration = provider.Load();

            Assert.Equal("2026.06.28", configuration.ConfigurationVersion.Value);
            Assert.Equal("1.2.3", configuration.EngineVersion.ToString());
            Assert.Equal(30, configuration.TickRate);
            Assert.Equal(1, configuration.ThreadCount);
            Assert.Equal("Info", configuration.LoggingLevel);
        }
        finally
        {
            if (File.Exists(configurationPath))
            {
                File.Delete(configurationPath);
            }
        }
    }

    [Fact]
    public void Load_ShouldRejectMissingFile()
    {
        string configurationPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.json");
        JsonEngineConfigurationProvider provider = new(configurationPath);

        Assert.Throws<FileNotFoundException>(() => provider.Load());
    }
}

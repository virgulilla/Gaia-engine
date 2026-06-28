using System;
using System.IO;
using GaiaEngine.App.Configuration;
using Xunit;

namespace GaiaEngine.App.Tests.Configuration;

public sealed class JsonSimulationConfigurationProviderTests
{
    [Fact]
    public void Load_ShouldReadSimulationConfigurationValues()
    {
        string configurationPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.json");

        try
        {
            File.WriteAllText(
                configurationPath,
                """
                {
                  "ticksPerDay": 300,
                  "daysPerSeason": 12,
                  "startingDay": 0,
                  "startingSeason": "Spring",
                  "startingYear": 3
                }
                """);

            JsonSimulationConfigurationProvider provider = new(configurationPath);

            SimulationConfiguration configuration = provider.Load();

            Assert.Equal(300, configuration.TicksPerDay);
            Assert.Equal(12, configuration.DaysPerSeason);
            Assert.Equal(0, configuration.StartingDay);
            Assert.Equal("Spring", configuration.StartingSeason);
            Assert.Equal(3, configuration.StartingYear);
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
        JsonSimulationConfigurationProvider provider = new(configurationPath);

        Assert.Throws<FileNotFoundException>(() => provider.Load());
    }
}

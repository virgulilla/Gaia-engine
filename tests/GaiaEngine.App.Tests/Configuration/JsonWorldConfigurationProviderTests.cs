using System;
using System.IO;
using GaiaEngine.App.Configuration;
using Xunit;

namespace GaiaEngine.App.Tests.Configuration;

public sealed class JsonWorldConfigurationProviderTests
{
    [Fact]
    public void Load_ShouldReadWorldConfigurationValues()
    {
        string configurationPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.json");

        try
        {
            File.WriteAllText(
                configurationPath,
                """
                {
                  "worldName": "Gaia",
                  "worldSeed": 42,
                  "chunkColumns": 2,
                  "chunkRows": 3,
                  "chunkSize": 16,
                  "maximumElevation": 200,
                  "defaultClimateZone": "Temperate"
                }
                """);

            JsonWorldConfigurationProvider provider = new(configurationPath);

            WorldConfiguration configuration = provider.Load();

            Assert.Equal("Gaia", configuration.WorldName);
            Assert.Equal(42, configuration.WorldSeed);
            Assert.Equal(2, configuration.ChunkColumns);
            Assert.Equal(3, configuration.ChunkRows);
            Assert.Equal(16, configuration.ChunkSize);
            Assert.Equal(200, configuration.MaximumElevation);
            Assert.Equal(GaiaEngine.Domain.World.ClimateZone.Temperate, configuration.DefaultClimateZone);
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
        JsonWorldConfigurationProvider provider = new(configurationPath);

        Assert.Throws<FileNotFoundException>(() => provider.Load());
    }
}

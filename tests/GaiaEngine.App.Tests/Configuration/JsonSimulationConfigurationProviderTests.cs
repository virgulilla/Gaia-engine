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
                  "startingYear": 3,
                  "mutation": {
                    "globalMutationChance": 120,
                    "mutationStrength": 240,
                    "maxMutationsPerGenome": 3,
                    "parameterMutationWeight": 600,
                    "dominanceMutationWeight": 150,
                    "activationMutationWeight": 80,
                    "structuralMutationWeight": 40,
                    "morphologyGroupWeight": 700,
                    "physiologyGroupWeight": 650,
                    "reproductionGroupWeight": 300,
                    "sensesGroupWeight": 450,
                    "adaptationGroupWeight": 500,
                    "appearanceGroupWeight": 350,
                    "behaviourBiasGroupWeight": 200,
                    "mutationVersion": 2
                  },
                  "speciesRecognition": {
                    "evaluationFrequency": 120,
                    "minimumGenomeSimilarity": 790,
                    "minimumTraitSimilarity": 770,
                    "minimumMorphologySimilarity": 730,
                    "minimumReproductiveCompatibility": 710,
                    "requiredFailedMetricCount": 3
                  }
                }
                """);

            JsonSimulationConfigurationProvider provider = new(configurationPath);

            SimulationConfiguration configuration = provider.Load();

            Assert.Equal(300, configuration.TicksPerDay);
            Assert.Equal(12, configuration.DaysPerSeason);
            Assert.Equal(0, configuration.StartingDay);
            Assert.Equal("Spring", configuration.StartingSeason);
            Assert.Equal(3, configuration.StartingYear);
            Assert.Equal(120, configuration.Mutation.GlobalMutationChance);
            Assert.Equal(240, configuration.Mutation.MutationStrength);
            Assert.Equal(3, configuration.Mutation.MaxMutationsPerGenome);
            Assert.Equal(2, configuration.Mutation.MutationVersion);
            Assert.Equal(120, configuration.SpeciesRecognition.EvaluationFrequency);
            Assert.Equal(790, configuration.SpeciesRecognition.MinimumGenomeSimilarity);
            Assert.Equal(710, configuration.SpeciesRecognition.MinimumReproductiveCompatibility);
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

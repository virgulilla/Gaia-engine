using System;
using GaiaEngine.Foundation.Configuration;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Foundation.Versioning;
using Xunit;

namespace GaiaEngine.Foundation.Tests.Determinism;

public sealed class DeterministicExecutionContextTests
{
    [Fact]
    public void Constructor_ShouldStoreDeterministicInputs()
    {
        WorldSeed worldSeed = new(42);
        EngineVersion engineVersion = new(1, 0, 0);
        ConfigurationVersion configurationVersion = new("2026.06.27");

        DeterministicExecutionContext context = new(worldSeed, engineVersion, configurationVersion, 128);

        Assert.Equal(worldSeed, context.WorldSeed);
        Assert.Equal(engineVersion, context.EngineVersion);
        Assert.Equal(configurationVersion, context.ConfigurationVersion);
        Assert.Equal(128, context.SimulationTick);
    }

    [Fact]
    public void Constructor_ShouldRejectNegativeSimulationTick()
    {
        WorldSeed worldSeed = new(42);
        EngineVersion engineVersion = new(1, 0, 0);
        ConfigurationVersion configurationVersion = new("2026.06.27");

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new DeterministicExecutionContext(worldSeed, engineVersion, configurationVersion, -1));
    }
}

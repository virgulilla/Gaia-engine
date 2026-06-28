using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Pipeline;
using GaiaEngine.Simulation.Scheduling;
using Xunit;

namespace GaiaEngine.Simulation.Tests.Pipeline;

public sealed class PostUpdateStatisticsPhaseTests
{
    [Fact]
    public void Execute_ShouldCaptureDiagnosticsWhenStatisticsSystemIsScheduled()
    {
        SimulationTickContext context = new(CreateWorldState(100, 2, "Summer", 1), 4);
        context.ApplySchedule(
            new SimulationTickSchedule(
                100,
                new[]
                {
                    new ScheduledSimulationSystem(SimulationSystemNames.Statistics, SimulationTickPhase.PostUpdate, 0, 100),
                }));
        context.ApplyEventPublicationResult(
            new SimulationEventPublicationResult(
                5,
                new IEvent[]
                {
                    new NewDaySimulationEvent(GaiaEngine.Domain.Identifiers.EventId.FromSequence(new GaiaEngine.Domain.Identifiers.EntitySequence(4)), 100, 100, 2, "Summer", 1),
                }));
        context.ApplyEventDispatchResult(new EventDispatchResult(1, 0, System.Array.Empty<EventDispatchFailure>()));
        context.RegisterExecutedPhase(SimulationTickPhase.InputCollection);
        context.RegisterExecutedPhase(SimulationTickPhase.PreUpdate);
        context.RegisterExecutedPhase(SimulationTickPhase.WorldUpdate);
        context.RegisterExecutedPhase(SimulationTickPhase.EventDispatch);

        PostUpdateStatisticsPhase phase = new(new SimulationDiagnosticsCollector());

        phase.Execute(context);

        Assert.NotNull(context.Diagnostics);
        Assert.Equal(100, context.Diagnostics!.Tick);
        Assert.Equal(1, context.Diagnostics.ProcessedEventCount);
    }

    [Fact]
    public void Execute_ShouldSkipDiagnosticsWhenStatisticsSystemIsNotScheduled()
    {
        SimulationTickContext context = new(CreateWorldState(99, 2, "Summer", 1), 4);
        context.ApplySchedule(new SimulationTickSchedule(99, System.Array.Empty<ScheduledSimulationSystem>()));

        PostUpdateStatisticsPhase phase = new(new SimulationDiagnosticsCollector());

        phase.Execute(context);

        Assert.Null(context.Diagnostics);
    }

    private static GaiaEngine.Domain.World.World CreateWorldState(long tick, int day, string season, int year)
    {
        GaiaEngine.Domain.Identifiers.WorldId worldId = GaiaEngine.Domain.Identifiers.WorldId.FromSequence(new GaiaEngine.Domain.Identifiers.EntitySequence(1));
        return new GaiaEngine.Domain.World.World(
            new GaiaEngine.Domain.World.WorldMetadata(
                worldId,
                "Gaia",
                new GaiaEngine.Foundation.Determinism.WorldSeed(42),
                "2026-06-28",
                new GaiaEngine.Foundation.Versioning.EngineVersion(1, 0, 0),
                new GaiaEngine.Foundation.Configuration.ConfigurationVersion("2026.06.28")),
            new GaiaEngine.Domain.World.WorldDimensions(32, 32, 16, 1, 200),
            new WorldTimeState(tick, day, season, year),
            new[]
            {
                new GaiaEngine.Domain.World.Chunk(
                    new GaiaEngine.Domain.World.ChunkMetadata(
                        GaiaEngine.Domain.Identifiers.ChunkId.FromSequence(new GaiaEngine.Domain.Identifiers.EntitySequence(2)),
                        worldId,
                        new GaiaEngine.Domain.World.ChunkCoordinates(0, 0),
                        new GaiaEngine.Foundation.Determinism.WorldSeed(100),
                        16),
                    GaiaEngine.Domain.World.ChunkState.Active,
                    new GaiaEngine.Domain.World.ClimateState(
                        GaiaEngine.Domain.World.ClimateZone.Temperate,
                        GaiaEngine.Domain.World.WeatherState.Clear,
                        new GaiaEngine.Domain.World.TemperatureState(18, 18, 18, 0),
                        new GaiaEngine.Domain.World.HumidityState(55, 3, 2),
                        new GaiaEngine.Domain.World.WindState(90, 4, 6),
                        new GaiaEngine.Domain.World.PrecipitationState(GaiaEngine.Domain.World.PrecipitationType.None, 0, 0, 0),
                        new GaiaEngine.Domain.World.PressureState(1012)),
                    System.Array.Empty<GaiaEngine.Domain.Identifiers.OrganismId>()),
            });
    }
}

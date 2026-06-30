using GaiaEngine.Audio.Events;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Engine.Events;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.Events;
using Xunit;

namespace GaiaEngine.Audio.Tests.Events;

public sealed class DeterministicAudioEventSystemTests
{
    [Fact]
    public void Translate_ShouldCreatePlaybackRequestsFromSimulationAndGameplayInputs()
    {
        DeterministicAudioEventSystem system = new(DefaultAudioEventCatalogFactory.Create());

        AudioEventBatchResult result = system.Translate(
            new IEvent[]
            {
                new ActionCompletedSimulationEvent(
                    EventId.FromSequence(new EntitySequence(1)),
                    tick: 20,
                    timestamp: 20,
                    ActionId.FromSequence(new EntitySequence(10)),
                    OrganismId.FromSequence(new EntitySequence(11)),
                    SimulationActionType.Eat,
                    new SimulationActionTarget(ActionTargetKind.Chunk, "144115188075855874")),
                new NewSeasonSimulationEvent(
                    EventId.FromSequence(new EntitySequence(2)),
                    tick: 20,
                    timestamp: 20,
                    currentDay: 1,
                    currentSeason: "Summer",
                    currentYear: 0),
            },
            new[]
            {
                new GameplayAudioSignal(
                    EventId.FromSequence(new EntitySequence(3)),
                    GameplayAudioSignalKind.AchievementUnlocked,
                    timestamp: 20),
            });

        Assert.Equal(3, result.AudioEvents.Count);
        Assert.Equal(3, result.PlaybackRequests.Count);
        Assert.Contains(result.AudioEvents, audioEvent => audioEvent.AudioClipId == "organism.eat");
        Assert.Contains(result.AudioEvents, audioEvent => audioEvent.AudioClipId == "environment.season.change");
        Assert.Contains(result.AudioEvents, audioEvent => audioEvent.AudioClipId == "gameplay.achievement.unlocked");
    }

    [Fact]
    public void Translate_ShouldMergeIdenticalAudioEventsInTheSameBatch()
    {
        DeterministicAudioEventSystem system = new(DefaultAudioEventCatalogFactory.Create());

        AudioEventBatchResult result = system.Translate(
            new IEvent[]
            {
                new ActionCompletedSimulationEvent(
                    EventId.FromSequence(new EntitySequence(1)),
                    tick: 20,
                    timestamp: 20,
                    ActionId.FromSequence(new EntitySequence(10)),
                    OrganismId.FromSequence(new EntitySequence(11)),
                    SimulationActionType.Eat,
                    new SimulationActionTarget(ActionTargetKind.Chunk, "144115188075855874")),
                new ActionCompletedSimulationEvent(
                    EventId.FromSequence(new EntitySequence(2)),
                    tick: 20,
                    timestamp: 20,
                    ActionId.FromSequence(new EntitySequence(12)),
                    OrganismId.FromSequence(new EntitySequence(13)),
                    SimulationActionType.Eat,
                    new SimulationActionTarget(ActionTargetKind.Chunk, "144115188075855875")),
            },
            System.Array.Empty<GameplayAudioSignal>());

        Assert.Single(result.AudioEvents);
        Assert.Single(result.PlaybackRequests);
        Assert.Equal(1, result.MergedEventCount);
    }
}

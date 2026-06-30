using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Foundation.Determinism;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.Events;
using Xunit;

namespace GaiaEngine.Gameplay.Tests.Discovery;

public sealed class SimulationDiscoverySignalFactoryTests
{
    [Fact]
    public void CreateSignals_ShouldTranslateActionAndTimeEventsIntoDiscoverySignals()
    {
        EventId actionEventId = EventId.FromSequence(new EntitySequence(1));
        EventId timeEventId = EventId.FromSequence(new EntitySequence(2));

        IReadOnlyList<DiscoverySignal> signals = SimulationDiscoverySignalFactory.CreateSignals(
            new GaiaEngine.Engine.Events.IEvent[]
            {
                new ActionCompletedSimulationEvent(
                    actionEventId,
                    tick: 40,
                    timestamp: 40,
                    ActionId.FromSequence(new EntitySequence(10)),
                    OrganismId.FromSequence(new EntitySequence(11)),
                    SimulationActionType.Eat,
                    new SimulationActionTarget(ActionTargetKind.Chunk, "144115188075855874")),
                new NewSeasonSimulationEvent(
                    timeEventId,
                    tick: 40,
                    timestamp: 40,
                    currentDay: 3,
                    currentSeason: "Summer",
                    currentYear: 0),
            });

        Assert.Equal(2, signals.Count);
        Assert.Contains(signals, signal => signal.Category == DiscoveryCategory.Behaviours && signal.Key == SimulationActionType.Eat.ToString());
        Assert.Contains(signals, signal => signal.Category == DiscoveryCategory.WorldEvents && signal.Key == nameof(NewSeasonSimulationEvent));
    }
}

using System.Collections.Generic;
using GaiaEngine.Audio.Events;
using GaiaEngine.Domain.Organisms;

namespace GaiaEngine.Audio.Music;

/// <summary>
/// Defines a deterministic service that selects one primary music theme from current presentation and world state.
/// </summary>
public interface IMusicSystem
{
    /// <summary>
    /// Evaluates the current primary music selection.
    /// </summary>
    /// <param name="context">The requesting presentation context.</param>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism collection.</param>
    /// <param name="simulationEvents">The current simulation events available to audio.</param>
    /// <param name="gameplaySignals">The current gameplay signals available to audio.</param>
    /// <returns>The deterministic music selection snapshot.</returns>
    public MusicSelectionSnapshot Evaluate(
        MusicPresentationContext context,
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        IReadOnlyList<GaiaEngine.Engine.Events.IEvent> simulationEvents,
        IReadOnlyList<GameplayAudioSignal> gameplaySignals);
}

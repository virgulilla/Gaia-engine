using System.Collections.Generic;
using GaiaEngine.Engine.Events;

namespace GaiaEngine.Audio.Events;

/// <summary>
/// Translates simulation and gameplay activity into runtime audio playback requests.
/// </summary>
public interface IAudioEventSystem
{
    /// <summary>
    /// Translates one batch of simulation and gameplay inputs into runtime audio playback requests.
    /// </summary>
    /// <param name="simulationEvents">The simulation events to translate.</param>
    /// <param name="gameplaySignals">The gameplay signals to translate.</param>
    /// <returns>The deterministic translation result.</returns>
    public AudioEventBatchResult Translate(IReadOnlyList<IEvent> simulationEvents, IReadOnlyList<GameplayAudioSignal> gameplaySignals);
}

using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Gameplay.Player;

namespace GaiaEngine.Gameplay.Discovery;

/// <summary>
/// Detects deterministic player discoveries from observed gameplay signals.
/// </summary>
public interface IDiscoverySystem
{
    /// <summary>
    /// Evaluates one discovery pass for the supplied player profile.
    /// </summary>
    /// <param name="profile">The current player profile.</param>
    /// <param name="worldId">The world associated with the evaluation pass.</param>
    /// <param name="tick">The current simulation tick.</param>
    /// <param name="signals">The observed discovery signals to evaluate.</param>
    /// <returns>The deterministic discovery evaluation result.</returns>
    public DiscoveryEvaluationResult Evaluate(PlayerProfile profile, WorldId worldId, long tick, IReadOnlyList<DiscoverySignal> signals);
}

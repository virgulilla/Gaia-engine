using System.Collections.Generic;
using GaiaEngine.Gameplay.Player;

namespace GaiaEngine.Gameplay.Objectives;

/// <summary>
/// Evaluates deterministic gameplay objectives for one player profile.
/// </summary>
public interface IObjectiveSystem
{
    /// <summary>
    /// Evaluates one objective pass for the supplied player profile.
    /// </summary>
    /// <param name="profile">The current player profile.</param>
    /// <param name="tick">The current simulation tick.</param>
    /// <param name="signals">The ordered gameplay signals observed during the pass.</param>
    /// <returns>The deterministic objective evaluation result.</returns>
    public ObjectiveEvaluationResult Evaluate(PlayerProfile profile, long tick, IReadOnlyList<ObjectiveSignal> signals);
}

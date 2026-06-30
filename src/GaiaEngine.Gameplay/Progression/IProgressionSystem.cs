using GaiaEngine.Gameplay.Player;

namespace GaiaEngine.Gameplay.Progression;

/// <summary>
/// Evaluates deterministic player progression from persistent gameplay state.
/// </summary>
public interface IProgressionSystem
{
    /// <summary>
    /// Evaluates one progression pass for the supplied player profile.
    /// </summary>
    /// <param name="profile">The current player profile.</param>
    /// <returns>The deterministic progression evaluation result.</returns>
    public ProgressionEvaluationResult Evaluate(PlayerProfile profile);
}

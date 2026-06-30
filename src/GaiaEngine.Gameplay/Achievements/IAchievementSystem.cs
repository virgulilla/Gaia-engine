using GaiaEngine.Gameplay.Player;

namespace GaiaEngine.Gameplay.Achievements;

/// <summary>
/// Evaluates deterministic player achievements from persistent gameplay state.
/// </summary>
public interface IAchievementSystem
{
    /// <summary>
    /// Evaluates one achievement pass for the supplied player profile.
    /// </summary>
    /// <param name="profile">The current player profile.</param>
    /// <returns>The deterministic achievement evaluation result.</returns>
    public AchievementEvaluationResult Evaluate(PlayerProfile profile);
}

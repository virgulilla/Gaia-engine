using System.Collections.Generic;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.AI.Decision;

namespace GaiaEngine.Simulation.AI.Execution;

/// <summary>
/// Translates selected deterministic decisions into immutable simulation action requests.
/// </summary>
public interface IBehaviourExecutionSystem
{
    /// <summary>
    /// Applies one or more selected decisions to the current action request collection.
    /// </summary>
    /// <param name="currentActionRequests">The current action request collection.</param>
    /// <param name="decisions">The selected decisions to translate.</param>
    /// <returns>The deterministic translation result.</returns>
    public BehaviourExecutionResult Execute(
        SimulationActionRequestCollection currentActionRequests,
        IReadOnlyList<SelectedDecision> decisions);
}

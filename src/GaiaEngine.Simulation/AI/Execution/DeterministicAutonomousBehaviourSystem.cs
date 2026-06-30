using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Simulation.Actions;
using GaiaEngine.Simulation.AI.Decision;
using GaiaEngine.Simulation.AI.Perception;
using GaiaEngine.Simulation.AI.Utility;

namespace GaiaEngine.Simulation.AI.Execution;

/// <summary>
/// Coordinates perception, utility evaluation, decision making, and behaviour execution for autonomous organisms.
/// </summary>
public sealed class DeterministicAutonomousBehaviourSystem : IAutonomousBehaviourSystem
{
    private readonly IPerceptionSystem perceptionSystem;
    private readonly IUtilityEvaluationSystem utilityEvaluationSystem;
    private readonly IDecisionMakingSystem decisionMakingSystem;
    private readonly IBehaviourExecutionSystem behaviourExecutionSystem;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicAutonomousBehaviourSystem"/> class.
    /// </summary>
    /// <param name="perceptionSystem">The deterministic perception system.</param>
    /// <param name="utilityEvaluationSystem">The deterministic utility evaluation system.</param>
    /// <param name="decisionMakingSystem">The deterministic decision making system.</param>
    /// <param name="behaviourExecutionSystem">The deterministic behaviour execution system.</param>
    /// <exception cref="ArgumentNullException">Thrown when any dependency is <see langword="null"/>.</exception>
    public DeterministicAutonomousBehaviourSystem(
        IPerceptionSystem perceptionSystem,
        IUtilityEvaluationSystem utilityEvaluationSystem,
        IDecisionMakingSystem decisionMakingSystem,
        IBehaviourExecutionSystem behaviourExecutionSystem)
    {
        this.perceptionSystem = perceptionSystem ?? throw new ArgumentNullException(nameof(perceptionSystem));
        this.utilityEvaluationSystem = utilityEvaluationSystem ?? throw new ArgumentNullException(nameof(utilityEvaluationSystem));
        this.decisionMakingSystem = decisionMakingSystem ?? throw new ArgumentNullException(nameof(decisionMakingSystem));
        this.behaviourExecutionSystem = behaviourExecutionSystem ?? throw new ArgumentNullException(nameof(behaviourExecutionSystem));
    }

    /// <summary>
    /// Updates the current action request collection from the current world and organism state.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism state.</param>
    /// <param name="actionRequests">The current action request collection.</param>
    /// <returns>The updated deterministic behaviour execution result.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/>, <paramref name="organisms"/>, or <paramref name="actionRequests"/> is <see langword="null"/>.
    /// </exception>
    public BehaviourExecutionResult Update(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        SimulationActionRequestCollection actionRequests)
    {
        ArgumentNullException.ThrowIfNull(world);
        ArgumentNullException.ThrowIfNull(organisms);
        ArgumentNullException.ThrowIfNull(actionRequests);

        List<SelectedDecision> decisions = new();
        foreach (Organism organism in organisms.GetAll())
        {
            if (!organism.Lifecycle.IsAlive)
            {
                continue;
            }

            PerceptionResult perception = perceptionSystem.Evaluate(world, organisms, organism.Id);
            UtilityEvaluationResult utilityResult = utilityEvaluationSystem.Evaluate(world, organisms, perception, organism.Id);
            SelectedDecision decision = decisionMakingSystem.Select(world, organisms, utilityResult, organism.Id);
            decisions.Add(decision);
        }

        return behaviourExecutionSystem.Execute(actionRequests, decisions.AsReadOnly());
    }
}

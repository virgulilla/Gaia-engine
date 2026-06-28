using System;
using System.Collections.Generic;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Executes the fixed deterministic phase order defined by the Gaia Engine simulation specifications.
/// </summary>
public sealed class DeterministicSimulationTickPipeline : ISimulationTickPipeline
{
    private static readonly SimulationTickPhase[] RequiredPhaseOrder =
    {
        SimulationTickPhase.InputCollection,
        SimulationTickPhase.PreUpdate,
        SimulationTickPhase.WorldUpdate,
        SimulationTickPhase.OrganismUpdate,
        SimulationTickPhase.InteractionSystems,
        SimulationTickPhase.EnvironmentUpdate,
        SimulationTickPhase.EventDispatch,
        SimulationTickPhase.PostUpdate,
    };

    private readonly IReadOnlyList<ISimulationTickPhase> phases;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeterministicSimulationTickPipeline"/> class.
    /// </summary>
    /// <param name="phases">The ordered deterministic phases executed by every simulation tick.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="phases"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when the supplied phase order is incomplete or invalid.</exception>
    public DeterministicSimulationTickPipeline(IReadOnlyList<ISimulationTickPhase> phases)
    {
        ArgumentNullException.ThrowIfNull(phases);

        if (phases.Count != RequiredPhaseOrder.Length)
        {
            throw new ArgumentException("The simulation tick pipeline must contain the full deterministic phase set.", nameof(phases));
        }

        for (int index = 0; index < RequiredPhaseOrder.Length; index++)
        {
            ISimulationTickPhase phase = phases[index] ?? throw new ArgumentNullException(nameof(phases), "The simulation tick pipeline cannot contain null phases.");
            if (phase.Phase != RequiredPhaseOrder[index])
            {
                throw new ArgumentException("The simulation tick pipeline phase order must match the approved deterministic specification.", nameof(phases));
            }
        }

        this.phases = phases;
    }

    /// <summary>
    /// Executes one deterministic simulation tick starting from the supplied world time state.
    /// </summary>
    /// <param name="timeState">The current world time state.</param>
    /// <returns>The deterministic result of the tick pipeline execution.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="timeState"/> is <see langword="null"/>.</exception>
    public SimulationTickResult Execute(WorldTimeState timeState)
    {
        SimulationTickContext context = new(timeState ?? throw new ArgumentNullException(nameof(timeState)));
        List<SimulationTickPhase> executedPhases = new(phases.Count);

        foreach (ISimulationTickPhase phase in phases)
        {
            phase.Execute(context);
            executedPhases.Add(phase.Phase);
        }

        return new SimulationTickResult(context.CurrentTimeState, executedPhases.AsReadOnly(), context.TimeAdvanceResult);
    }
}

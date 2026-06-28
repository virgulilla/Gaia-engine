using System;
using System.Collections.Generic;
using GaiaEngine.Domain.World;
using GaiaEngine.Engine.Events;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Scheduling;
using GaiaEngine.Simulation.Time;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Represents the mutable runtime context shared across deterministic tick phases.
/// </summary>
public sealed class SimulationTickContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationTickContext"/> class.
    /// </summary>
    /// <param name="timeState">The initial world time state for the tick.</param>
    /// <param name="nextEventSequence">The next deterministic event sequence value to use.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="timeState"/> is <see langword="null"/>.</exception>
    public SimulationTickContext(WorldTimeState timeState, ulong nextEventSequence)
    {
        CurrentTimeState = timeState ?? throw new ArgumentNullException(nameof(timeState));
        Schedule = new SimulationTickSchedule(timeState.CurrentTick, Array.Empty<ScheduledSimulationSystem>());
        if (nextEventSequence == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(nextEventSequence), "The next event sequence value must be greater than zero.");
        }

        NextEventSequence = nextEventSequence;
        EventPublicationResult = new SimulationEventPublicationResult(nextEventSequence, Array.Empty<IEvent>());
    }

    /// <summary>
    /// Gets the current world time state being updated by the pipeline.
    /// </summary>
    public WorldTimeState CurrentTimeState { get; private set; }

    /// <summary>
    /// Gets the time advancement produced during the current tick, when available.
    /// </summary>
    public TimeAdvanceResult? TimeAdvanceResult { get; private set; }

    /// <summary>
    /// Gets the deterministic event publication result produced during the current tick.
    /// </summary>
    public SimulationEventPublicationResult EventPublicationResult { get; private set; }

    /// <summary>
    /// Gets the deterministic event dispatch result produced during the current tick, when available.
    /// </summary>
    public EventDispatchResult? EventDispatchResult { get; private set; }

    /// <summary>
    /// Gets the deterministic diagnostics snapshot captured during the current tick, when available.
    /// </summary>
    public SimulationTickDiagnostics? Diagnostics { get; private set; }

    /// <summary>
    /// Gets the deterministic schedule selected for the current tick, when available.
    /// </summary>
    public SimulationTickSchedule Schedule { get; private set; }

    /// <summary>
    /// Gets the next deterministic event sequence value to use.
    /// </summary>
    public ulong NextEventSequence { get; private set; }

    /// <summary>
    /// Gets the deterministic list of phases already executed during the current tick.
    /// </summary>
    public IReadOnlyList<SimulationTickPhase> ExecutedPhases => executedPhases.AsReadOnly();

    private readonly List<SimulationTickPhase> executedPhases = new();

    /// <summary>
    /// Applies the time advancement produced by the Time System to the current context.
    /// </summary>
    /// <param name="result">The time advancement result to apply.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public void ApplyTimeAdvance(TimeAdvanceResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        TimeAdvanceResult = result;
        CurrentTimeState = result.TimeState;
    }

    /// <summary>
    /// Applies the deterministic schedule created for the current tick.
    /// </summary>
    /// <param name="schedule">The schedule to apply.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="schedule"/> is <see langword="null"/>.</exception>
    public void ApplySchedule(SimulationTickSchedule schedule)
    {
        Schedule = schedule ?? throw new ArgumentNullException(nameof(schedule));
    }

    /// <summary>
    /// Applies the deterministic event publication result produced during the current tick.
    /// </summary>
    /// <param name="result">The event publication result to apply.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public void ApplyEventPublicationResult(SimulationEventPublicationResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        EventPublicationResult = result;
        NextEventSequence = result.NextEventSequence;
    }

    /// <summary>
    /// Applies the deterministic event dispatch result produced during the current tick.
    /// </summary>
    /// <param name="result">The event dispatch result to apply.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="result"/> is <see langword="null"/>.</exception>
    public void ApplyEventDispatchResult(EventDispatchResult result)
    {
        EventDispatchResult = result ?? throw new ArgumentNullException(nameof(result));
    }

    /// <summary>
    /// Registers one executed phase in deterministic order.
    /// </summary>
    /// <param name="phase">The executed phase.</param>
    public void RegisterExecutedPhase(SimulationTickPhase phase)
    {
        executedPhases.Add(phase);
    }

    /// <summary>
    /// Applies the deterministic diagnostics snapshot captured during the current tick.
    /// </summary>
    /// <param name="diagnostics">The diagnostics snapshot to apply.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="diagnostics"/> is <see langword="null"/>.</exception>
    public void ApplyDiagnostics(SimulationTickDiagnostics diagnostics)
    {
        Diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
    }
}

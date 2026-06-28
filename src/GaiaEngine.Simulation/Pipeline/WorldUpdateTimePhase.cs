using System;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Time;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Advances deterministic world time during the world update phase of the simulation tick pipeline.
/// </summary>
public sealed class WorldUpdateTimePhase : ISimulationTickPhase
{
    private readonly ITimeSystem timeSystem;
    private readonly ISimulationEventPublisher eventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldUpdateTimePhase"/> class.
    /// </summary>
    /// <param name="timeSystem">The Time System used to advance deterministic world time.</param>
    /// <param name="eventPublisher">The simulation event publisher used to enqueue time events.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="timeSystem"/> or <paramref name="eventPublisher"/> is <see langword="null"/>.
    /// </exception>
    public WorldUpdateTimePhase(ITimeSystem timeSystem, ISimulationEventPublisher eventPublisher)
    {
        this.timeSystem = timeSystem ?? throw new ArgumentNullException(nameof(timeSystem));
        this.eventPublisher = eventPublisher ?? throw new ArgumentNullException(nameof(eventPublisher));
    }

    /// <summary>
    /// Gets the deterministic phase represented by the implementation.
    /// </summary>
    public SimulationTickPhase Phase => SimulationTickPhase.WorldUpdate;

    /// <summary>
    /// Executes deterministic world time advancement for the current tick.
    /// </summary>
    /// <param name="context">The mutable context shared by the current tick.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <see langword="null"/>.</exception>
    public void Execute(SimulationTickContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        TimeAdvanceResult timeAdvanceResult = timeSystem.Advance(context.CurrentTimeState);
        context.ApplyTimeAdvance(timeAdvanceResult);
        context.ApplyEventPublicationResult(eventPublisher.PublishTimeEvents(timeAdvanceResult, context.NextEventSequence));
    }
}

using System;
using GaiaEngine.Engine.Events;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Dispatches deterministic simulation events during the event dispatch phase of the tick pipeline.
/// </summary>
public sealed class EventDispatchPhase : ISimulationTickPhase
{
    private readonly IEventBus eventBus;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventDispatchPhase"/> class.
    /// </summary>
    /// <param name="eventBus">The engine event bus used for deterministic dispatch.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="eventBus"/> is <see langword="null"/>.</exception>
    public EventDispatchPhase(IEventBus eventBus)
    {
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    /// <summary>
    /// Gets the deterministic phase represented by the implementation.
    /// </summary>
    public SimulationTickPhase Phase => SimulationTickPhase.EventDispatch;

    /// <summary>
    /// Dispatches all queued events scheduled up to the current simulation tick.
    /// </summary>
    /// <param name="context">The mutable context shared by the current tick.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <see langword="null"/>.</exception>
    public void Execute(SimulationTickContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.ApplyEventDispatchResult(eventBus.Dispatch(context.CurrentTimeState.CurrentTick));
    }
}

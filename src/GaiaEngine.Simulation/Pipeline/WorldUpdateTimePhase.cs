using System;
using GaiaEngine.Simulation.Diagnostics;
using GaiaEngine.Simulation.Events;
using GaiaEngine.Simulation.Scheduling;
using GaiaEngine.Simulation.Time;
using GaiaEngine.Simulation.World.Climate;
using GaiaEngine.Simulation.World.Resources;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Advances deterministic world time during the world update phase of the simulation tick pipeline.
/// </summary>
public sealed class WorldUpdateTimePhase : ISimulationTickPhase
{
    private readonly ITimeSystem timeSystem;
    private readonly ISimulationScheduler scheduler;
    private readonly IClimateSystem climateSystem;
    private readonly IResourceSystem resourceSystem;
    private readonly ISimulationEventPublisher eventPublisher;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorldUpdateTimePhase"/> class.
    /// </summary>
    /// <param name="timeSystem">The Time System used to advance deterministic world time.</param>
    /// <param name="scheduler">The scheduler used to create the deterministic tick schedule.</param>
    /// <param name="climateSystem">The climate system used to update world climate state.</param>
    /// <param name="resourceSystem">The resource system used to update world resource state.</param>
    /// <param name="eventPublisher">The simulation event publisher used to enqueue time events.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when any supplied dependency is <see langword="null"/>.
    /// </exception>
    public WorldUpdateTimePhase(
        ITimeSystem timeSystem,
        ISimulationScheduler scheduler,
        IClimateSystem climateSystem,
        IResourceSystem resourceSystem,
        ISimulationEventPublisher eventPublisher)
    {
        this.timeSystem = timeSystem ?? throw new ArgumentNullException(nameof(timeSystem));
        this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        this.climateSystem = climateSystem ?? throw new ArgumentNullException(nameof(climateSystem));
        this.resourceSystem = resourceSystem ?? throw new ArgumentNullException(nameof(resourceSystem));
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
        context.ApplySchedule(scheduler.CreateSchedule(context.CurrentTimeState.CurrentTick));

        foreach (ScheduledSimulationSystem scheduledSystem in context.Schedule.GetSystemsForPhase(SimulationTickPhase.WorldUpdate))
        {
            if (scheduledSystem.SystemName == SimulationSystemNames.Climate)
            {
                context.ApplyWorld(climateSystem.UpdateWorld(context.CurrentWorld));
                continue;
            }

            if (scheduledSystem.SystemName == SimulationSystemNames.Resources)
            {
                context.ApplyWorld(resourceSystem.UpdateWorld(context.CurrentWorld));
            }
        }

        context.ApplyEventPublicationResult(eventPublisher.PublishTimeEvents(timeAdvanceResult, context.NextEventSequence));
    }
}

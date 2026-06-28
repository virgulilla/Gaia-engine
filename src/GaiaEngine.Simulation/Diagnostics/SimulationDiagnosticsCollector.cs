using System;
using GaiaEngine.Simulation.Pipeline;

namespace GaiaEngine.Simulation.Diagnostics;

/// <summary>
/// Captures deterministic diagnostics snapshots from the current simulation tick context.
/// </summary>
public sealed class SimulationDiagnosticsCollector : ISimulationDiagnosticsCollector
{
    /// <summary>
    /// Captures a deterministic diagnostics snapshot from the supplied tick context.
    /// </summary>
    /// <param name="context">The mutable context shared by the current tick.</param>
    /// <returns>The captured diagnostics snapshot.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <see langword="null"/>.</exception>
    public SimulationTickDiagnostics Capture(SimulationTickContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        return new SimulationTickDiagnostics(
            context.CurrentTimeState.CurrentTick,
            context.CurrentTimeState.CurrentDay,
            context.CurrentTimeState.CurrentSeason,
            context.CurrentTimeState.CurrentYear,
            context.ExecutedPhases.Count,
            context.Schedule.Systems.Count,
            context.EventPublicationResult.PublishedEvents.Count,
            context.EventDispatchResult?.ProcessedEventCount ?? 0);
    }
}

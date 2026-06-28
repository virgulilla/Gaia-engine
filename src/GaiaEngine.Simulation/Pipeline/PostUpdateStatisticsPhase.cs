using System;
using GaiaEngine.Simulation.Diagnostics;

namespace GaiaEngine.Simulation.Pipeline;

/// <summary>
/// Captures deterministic diagnostics during post-update when the statistics system is scheduled for the current tick.
/// </summary>
public sealed class PostUpdateStatisticsPhase : ISimulationTickPhase
{
    private readonly ISimulationDiagnosticsCollector diagnosticsCollector;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostUpdateStatisticsPhase"/> class.
    /// </summary>
    /// <param name="diagnosticsCollector">The diagnostics collector used to capture the post-update snapshot.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="diagnosticsCollector"/> is <see langword="null"/>.</exception>
    public PostUpdateStatisticsPhase(ISimulationDiagnosticsCollector diagnosticsCollector)
    {
        this.diagnosticsCollector = diagnosticsCollector ?? throw new ArgumentNullException(nameof(diagnosticsCollector));
    }

    /// <summary>
    /// Gets the deterministic phase represented by the implementation.
    /// </summary>
    public SimulationTickPhase Phase => SimulationTickPhase.PostUpdate;

    /// <summary>
    /// Captures a diagnostics snapshot when the statistics system is scheduled for the current tick.
    /// </summary>
    /// <param name="context">The mutable context shared by the current tick.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="context"/> is <see langword="null"/>.</exception>
    public void Execute(SimulationTickContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        foreach (Scheduling.ScheduledSimulationSystem scheduledSystem in context.Schedule.GetSystemsForPhase(SimulationTickPhase.PostUpdate))
        {
            if (scheduledSystem.SystemName == SimulationSystemNames.Statistics)
            {
                context.ApplyDiagnostics(diagnosticsCollector.Capture(context));
                break;
            }
        }
    }
}

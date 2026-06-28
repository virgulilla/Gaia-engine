namespace GaiaEngine.Simulation.Scheduling;

/// <summary>
/// Creates deterministic per-tick schedules for simulation systems.
/// </summary>
public interface ISimulationScheduler
{
    /// <summary>
    /// Creates the deterministic schedule for the supplied simulation tick.
    /// </summary>
    /// <param name="executingTick">The simulation tick to schedule.</param>
    /// <returns>The deterministic schedule for the supplied tick.</returns>
    public SimulationTickSchedule CreateSchedule(long executingTick);
}

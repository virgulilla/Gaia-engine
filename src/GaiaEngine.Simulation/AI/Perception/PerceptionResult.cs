using System;
using System.Collections.Generic;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Simulation.AI.Perception;

/// <summary>
/// Represents the deterministic observation set produced for one organism.
/// </summary>
public sealed class PerceptionResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PerceptionResult"/> class.
    /// </summary>
    /// <param name="observerId">The observing organism identifier.</param>
    /// <param name="detectionTick">The simulation tick used during perception.</param>
    /// <param name="observations">The generated observations in deterministic order.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="observations"/> is <see langword="null"/>.</exception>
    public PerceptionResult(OrganismId observerId, long detectionTick, IReadOnlyList<PerceivedObject> observations)
    {
        if (detectionTick < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(detectionTick), "The detection tick must be zero or greater.");
        }

        ObserverId = observerId;
        DetectionTick = detectionTick;
        Observations = observations ?? throw new ArgumentNullException(nameof(observations));
    }

    /// <summary>
    /// Gets the observing organism identifier.
    /// </summary>
    public OrganismId ObserverId { get; }

    /// <summary>
    /// Gets the simulation tick used during perception.
    /// </summary>
    public long DetectionTick { get; }

    /// <summary>
    /// Gets the generated observations in deterministic order.
    /// </summary>
    public IReadOnlyList<PerceivedObject> Observations { get; }
}

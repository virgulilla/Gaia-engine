using System;

namespace GaiaEngine.Simulation.AI.Perception;

/// <summary>
/// Defines the deterministic configurable sensor settings used by the perception system.
/// </summary>
public sealed class PerceptionSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PerceptionSettings"/> class.
    /// </summary>
    /// <param name="visionRange">The maximum vision range in chunk distance.</param>
    /// <param name="hearingRange">The maximum hearing range in chunk distance.</param>
    /// <param name="smellRange">The maximum smell range in chunk distance.</param>
    /// <param name="touchRange">The maximum touch range in chunk distance.</param>
    /// <param name="minimumConfidence">
    /// The minimum confidence scaled to the inclusive range [0, 1000],
    /// which represents the specification range [0.0, 1.0].
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when any range is negative or when <paramref name="minimumConfidence"/> is outside [0, 1000].
    /// </exception>
    public PerceptionSettings(
        int visionRange,
        int hearingRange,
        int smellRange,
        int touchRange,
        int minimumConfidence)
    {
        if (visionRange < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(visionRange), "The vision range must be zero or greater.");
        }

        if (hearingRange < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(hearingRange), "The hearing range must be zero or greater.");
        }

        if (smellRange < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(smellRange), "The smell range must be zero or greater.");
        }

        if (touchRange < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(touchRange), "The touch range must be zero or greater.");
        }

        if (minimumConfidence < 0 || minimumConfidence > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumConfidence), "The minimum confidence must be between 0 and 1000.");
        }

        VisionRange = visionRange;
        HearingRange = hearingRange;
        SmellRange = smellRange;
        TouchRange = touchRange;
        MinimumConfidence = minimumConfidence;
    }

    /// <summary>
    /// Gets a shared default perception configuration.
    /// </summary>
    public static PerceptionSettings Default { get; } = new(visionRange: 3, hearingRange: 2, smellRange: 2, touchRange: 0, minimumConfidence: 250);

    /// <summary>
    /// Gets the maximum vision range in chunk distance.
    /// </summary>
    public int VisionRange { get; }

    /// <summary>
    /// Gets the maximum hearing range in chunk distance.
    /// </summary>
    public int HearingRange { get; }

    /// <summary>
    /// Gets the maximum smell range in chunk distance.
    /// </summary>
    public int SmellRange { get; }

    /// <summary>
    /// Gets the maximum touch range in chunk distance.
    /// </summary>
    public int TouchRange { get; }

    /// <summary>
    /// Gets the minimum accepted confidence scaled to the inclusive range [0, 1000].
    /// </summary>
    public int MinimumConfidence { get; }

    /// <summary>
    /// Gets the maximum configured sensor range in chunk distance.
    /// </summary>
    public int MaximumRange => Math.Max(Math.Max(VisionRange, HearingRange), Math.Max(SmellRange, TouchRange));
}

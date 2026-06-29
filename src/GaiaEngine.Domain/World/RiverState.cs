using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the optional deterministic river state stored by one chunk.
/// </summary>
public sealed record RiverState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RiverState"/> class.
    /// </summary>
    /// <param name="riverId">The deterministic river identifier.</param>
    /// <param name="width">The river width.</param>
    /// <param name="depth">The river depth.</param>
    /// <param name="flowRate">The river flow rate.</param>
    /// <param name="currentVelocity">The river current velocity.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="riverId"/> is empty.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when one or more numeric values are negative.</exception>
    public RiverState(string riverId, int width, int depth, int flowRate, int currentVelocity)
    {
        if (string.IsNullOrWhiteSpace(riverId))
        {
            throw new ArgumentException("The river identifier must contain a value.", nameof(riverId));
        }

        if (width < 0 || depth < 0 || flowRate < 0 || currentVelocity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width), "The river values must be zero or greater.");
        }

        RiverId = riverId;
        Width = width;
        Depth = depth;
        FlowRate = flowRate;
        CurrentVelocity = currentVelocity;
    }

    /// <summary>
    /// Gets the deterministic river identifier.
    /// </summary>
    public string RiverId { get; }

    /// <summary>
    /// Gets the river width.
    /// </summary>
    public int Width { get; }

    /// <summary>
    /// Gets the river depth.
    /// </summary>
    public int Depth { get; }

    /// <summary>
    /// Gets the river flow rate.
    /// </summary>
    public int FlowRate { get; }

    /// <summary>
    /// Gets the river current velocity.
    /// </summary>
    public int CurrentVelocity { get; }
}

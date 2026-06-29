using System;
using GaiaEngine.Domain.Organisms;

namespace GaiaEngine.Simulation.Interactions.Feeding;

/// <summary>
/// Represents the deterministic result produced by one feeding system execution.
/// </summary>
public sealed record FeedingSystemResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FeedingSystemResult"/> class.
    /// </summary>
    /// <param name="world">The updated world state.</param>
    /// <param name="organisms">The updated organism state.</param>
    /// <param name="remainingRequests">The feeding requests not executed during the current tick.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/>, <paramref name="organisms"/>, or <paramref name="remainingRequests"/> is <see langword="null"/>.
    /// </exception>
    public FeedingSystemResult(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        FeedingRequestCollection remainingRequests)
    {
        World = world ?? throw new ArgumentNullException(nameof(world));
        Organisms = organisms ?? throw new ArgumentNullException(nameof(organisms));
        RemainingRequests = remainingRequests ?? throw new ArgumentNullException(nameof(remainingRequests));
    }

    /// <summary>
    /// Gets the updated world state.
    /// </summary>
    public GaiaEngine.Domain.World.World World { get; }

    /// <summary>
    /// Gets the updated organism state.
    /// </summary>
    public OrganismCollection Organisms { get; }

    /// <summary>
    /// Gets the feeding requests not executed during the current tick.
    /// </summary>
    public FeedingRequestCollection RemainingRequests { get; }
}

using System;
using GaiaEngine.Domain.Organisms;

namespace GaiaEngine.Simulation.Interactions.Movement;

/// <summary>
/// Represents the deterministic result produced by one movement system execution.
/// </summary>
public sealed record MovementSystemResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MovementSystemResult"/> class.
    /// </summary>
    /// <param name="world">The updated world state.</param>
    /// <param name="organisms">The updated organism state.</param>
    /// <param name="remainingRequests">The requests not executed during the current tick.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="world"/>, <paramref name="organisms"/>, or <paramref name="remainingRequests"/> is <see langword="null"/>.
    /// </exception>
    public MovementSystemResult(
        GaiaEngine.Domain.World.World world,
        OrganismCollection organisms,
        MovementRequestCollection remainingRequests)
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
    /// Gets the requests not executed during the current tick.
    /// </summary>
    public MovementRequestCollection RemainingRequests { get; }
}

using System;
using GaiaEngine.Domain.Organisms;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Represents the deterministic bootstrap state produced for the organism module.
/// </summary>
public sealed record OrganismBootstrapState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrganismBootstrapState"/> class.
    /// </summary>
    /// <param name="world">The world updated with organism references.</param>
    /// <param name="organisms">The initial organism collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="world"/> or <paramref name="organisms"/> is <see langword="null"/>.</exception>
    public OrganismBootstrapState(GaiaEngine.Domain.World.World world, OrganismCollection organisms)
    {
        World = world ?? throw new ArgumentNullException(nameof(world));
        Organisms = organisms ?? throw new ArgumentNullException(nameof(organisms));
    }

    /// <summary>
    /// Gets the world updated with organism references.
    /// </summary>
    public GaiaEngine.Domain.World.World World { get; }

    /// <summary>
    /// Gets the initial organism collection.
    /// </summary>
    public OrganismCollection Organisms { get; }
}

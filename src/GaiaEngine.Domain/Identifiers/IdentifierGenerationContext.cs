using GaiaEngine.Foundation.Determinism;

namespace GaiaEngine.Domain.Identifiers;

/// <summary>
/// Represents the immutable deterministic context associated with identifier generation.
/// </summary>
public readonly record struct IdentifierGenerationContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="IdentifierGenerationContext"/> struct.
    /// </summary>
    /// <param name="worldSeed">The world seed associated with the current simulation.</param>
    /// <param name="simulationTick">The simulation tick associated with the identifier request.</param>
    /// <param name="sequence">The deterministic sequence value for the identifier request.</param>
    public IdentifierGenerationContext(WorldSeed worldSeed, long simulationTick, EntitySequence sequence)
    {
        WorldSeed = worldSeed;
        SimulationTick = simulationTick;
        Sequence = sequence;
    }

    /// <summary>
    /// Gets the world seed associated with the request.
    /// </summary>
    public WorldSeed WorldSeed { get; }

    /// <summary>
    /// Gets the simulation tick associated with the request.
    /// </summary>
    public long SimulationTick { get; }

    /// <summary>
    /// Gets the deterministic sequence value.
    /// </summary>
    public EntitySequence Sequence { get; }
}

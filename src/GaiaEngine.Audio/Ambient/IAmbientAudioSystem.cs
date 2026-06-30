using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Domain.Organisms;
using GaiaEngine.Domain.World;

namespace GaiaEngine.Audio.Ambient;

/// <summary>
/// Defines a deterministic service that reconstructs ambient audio from current world state.
/// </summary>
public interface IAmbientAudioSystem
{
    /// <summary>
    /// Evaluates the active ambient layers for the supplied focus chunk.
    /// </summary>
    /// <param name="world">The current world state.</param>
    /// <param name="organisms">The current organism collection.</param>
    /// <param name="focusChunkId">The focus chunk used to resolve local ambience.</param>
    /// <returns>The deterministic ambient mix snapshot.</returns>
    public AmbientMixSnapshot Evaluate(GaiaEngine.Domain.World.World world, OrganismCollection organisms, ChunkId focusChunkId);
}

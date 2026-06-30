namespace GaiaEngine.Audio.Music;

/// <summary>
/// Defines the supported music transition kinds.
/// </summary>
public enum MusicTransitionKind
{
    /// <summary>
    /// Identifies crossfade transitions.
    /// </summary>
    Crossfade = 0,

    /// <summary>
    /// Identifies fade-in transitions.
    /// </summary>
    FadeIn = 1,

    /// <summary>
    /// Identifies fade-out transitions.
    /// </summary>
    FadeOut = 2,

    /// <summary>
    /// Identifies layer-blend transitions.
    /// </summary>
    LayerBlend = 3,
}

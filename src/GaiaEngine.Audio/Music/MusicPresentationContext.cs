namespace GaiaEngine.Audio.Music;

/// <summary>
/// Defines the supported high-level presentation contexts for music selection.
/// </summary>
public enum MusicPresentationContext
{
    /// <summary>
    /// Identifies main-menu or pause-menu presentation.
    /// </summary>
    Menu = 0,

    /// <summary>
    /// Identifies world loading or intro presentation.
    /// </summary>
    Loading = 1,

    /// <summary>
    /// Identifies in-game presentation.
    /// </summary>
    InGame = 2,

    /// <summary>
    /// Identifies credits presentation.
    /// </summary>
    Credits = 3,
}

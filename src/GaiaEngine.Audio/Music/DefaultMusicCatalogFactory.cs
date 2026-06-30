namespace GaiaEngine.Audio.Music;

/// <summary>
/// Creates the default deterministic music catalog used by the audio module.
/// </summary>
public static class DefaultMusicCatalogFactory
{
    /// <summary>
    /// Creates the default deterministic music catalog.
    /// </summary>
    /// <returns>The initialized music catalog.</returns>
    public static MusicCatalog Create()
    {
        return new MusicCatalog(
            new[]
            {
                new MusicThemeDefinition(
                    "music.main-theme",
                    MusicThemeKind.MainTheme,
                    MusicPrimaryState.Menu,
                    priority: 0,
                    "music.main",
                    new MusicPlaybackRules(looping: true, canInterruptLowerPriority: true, MusicTransitionKind.FadeIn, transitionTicks: 18)),
                new MusicThemeDefinition(
                    "music.exploration",
                    MusicThemeKind.Exploration,
                    MusicPrimaryState.Exploration,
                    priority: 4,
                    "music.exploration",
                    new MusicPlaybackRules(looping: true, canInterruptLowerPriority: false, MusicTransitionKind.LayerBlend, transitionTicks: 12)),
                new MusicThemeDefinition(
                    "music.discovery",
                    MusicThemeKind.Discovery,
                    MusicPrimaryState.Discovery,
                    priority: 1,
                    "music.discovery",
                    new MusicPlaybackRules(looping: false, canInterruptLowerPriority: true, MusicTransitionKind.Crossfade, transitionTicks: 8)),
                new MusicThemeDefinition(
                    "music.tension",
                    MusicThemeKind.Tension,
                    MusicPrimaryState.Tension,
                    priority: 2,
                    "music.tension",
                    new MusicPlaybackRules(looping: true, canInterruptLowerPriority: true, MusicTransitionKind.Crossfade, transitionTicks: 6)),
                new MusicThemeDefinition(
                    "music.world-event",
                    MusicThemeKind.WorldEvent,
                    MusicPrimaryState.Event,
                    priority: 0,
                    "music.event",
                    new MusicPlaybackRules(looping: false, canInterruptLowerPriority: true, MusicTransitionKind.Crossfade, transitionTicks: 5)),
                new MusicThemeDefinition(
                    "music.menu",
                    MusicThemeKind.Menu,
                    MusicPrimaryState.Menu,
                    priority: 0,
                    "music.menu",
                    new MusicPlaybackRules(looping: true, canInterruptLowerPriority: true, MusicTransitionKind.FadeIn, transitionTicks: 14)),
                new MusicThemeDefinition(
                    "music.credits",
                    MusicThemeKind.Credits,
                    MusicPrimaryState.Menu,
                    priority: 0,
                    "music.credits",
                    new MusicPlaybackRules(looping: false, canInterruptLowerPriority: true, MusicTransitionKind.FadeIn, transitionTicks: 20)),
            });
    }
}

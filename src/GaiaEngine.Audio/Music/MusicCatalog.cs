using System;
using System.Collections.Generic;

namespace GaiaEngine.Audio.Music;

/// <summary>
/// Represents the deterministic catalog of available music themes.
/// </summary>
public sealed class MusicCatalog
{
    private readonly Dictionary<MusicThemeKind, MusicThemeDefinition> definitions;

    /// <summary>
    /// Initializes a new instance of the <see cref="MusicCatalog"/> class.
    /// </summary>
    /// <param name="definitions">The music theme definitions contained by the catalog.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="definitions"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when duplicate theme kinds are supplied.</exception>
    public MusicCatalog(IReadOnlyList<MusicThemeDefinition> definitions)
    {
        ArgumentNullException.ThrowIfNull(definitions);

        this.definitions = new Dictionary<MusicThemeKind, MusicThemeDefinition>(definitions.Count);
        foreach (MusicThemeDefinition definition in definitions)
        {
            ArgumentNullException.ThrowIfNull(definition);
            if (!this.definitions.TryAdd(definition.ThemeKind, definition))
            {
                throw new ArgumentException("Music theme kinds must be unique.", nameof(definitions));
            }
        }
    }

    /// <summary>
    /// Tries to resolve one music theme by kind.
    /// </summary>
    /// <param name="themeKind">The requested theme kind.</param>
    /// <param name="definition">The resolved definition when present.</param>
    /// <returns><see langword="true"/> when the definition exists; otherwise <see langword="false"/>.</returns>
    public bool TryGet(MusicThemeKind themeKind, out MusicThemeDefinition? definition)
    {
        return definitions.TryGetValue(themeKind, out definition);
    }

    /// <summary>
    /// Gets one music theme by kind.
    /// </summary>
    /// <param name="themeKind">The requested theme kind.</param>
    /// <returns>The resolved theme definition.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the theme kind does not exist.</exception>
    public MusicThemeDefinition Get(MusicThemeKind themeKind)
    {
        if (!TryGet(themeKind, out MusicThemeDefinition? definition))
        {
            throw new KeyNotFoundException("The requested music theme does not exist in the catalog.");
        }

        return definition!;
    }
}

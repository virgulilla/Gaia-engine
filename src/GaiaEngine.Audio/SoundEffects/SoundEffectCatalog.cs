using System;
using System.Collections.Generic;

namespace GaiaEngine.Audio.SoundEffects;

/// <summary>
/// Represents the configurable sound effect library keyed by logical audio clip identifier.
/// </summary>
public sealed class SoundEffectCatalog
{
    private readonly Dictionary<string, SoundEffectDefinition> definitionsByClipId;

    /// <summary>
    /// Initializes a new instance of the <see cref="SoundEffectCatalog"/> class.
    /// </summary>
    /// <param name="definitions">The stored sound effect definitions.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="definitions"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when a duplicate clip identifier is detected.</exception>
    public SoundEffectCatalog(IReadOnlyList<SoundEffectDefinition> definitions)
    {
        ArgumentNullException.ThrowIfNull(definitions);

        definitionsByClipId = new Dictionary<string, SoundEffectDefinition>(StringComparer.Ordinal);
        foreach (SoundEffectDefinition definition in definitions)
        {
            ArgumentNullException.ThrowIfNull(definition);
            foreach (SoundEffectVariant variant in definition.Variants)
            {
                if (!definitionsByClipId.TryAdd(variant.AudioClipId, definition))
                {
                    throw new ArgumentException($"The sound clip '{variant.AudioClipId}' is duplicated.", nameof(definitions));
                }
            }
        }
    }

    /// <summary>
    /// Attempts to resolve one sound effect definition by logical audio clip identifier.
    /// </summary>
    /// <param name="audioClipId">The logical audio clip identifier to resolve.</param>
    /// <param name="definition">The resolved definition when found.</param>
    /// <returns><see langword="true"/> when the definition exists; otherwise <see langword="false"/>.</returns>
    public bool TryResolveByClipId(string audioClipId, out SoundEffectDefinition? definition)
    {
        if (string.IsNullOrWhiteSpace(audioClipId))
        {
            definition = null;
            return false;
        }

        return definitionsByClipId.TryGetValue(audioClipId, out definition);
    }
}

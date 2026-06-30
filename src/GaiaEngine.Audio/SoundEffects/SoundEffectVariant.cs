using System;

namespace GaiaEngine.Audio.SoundEffects;

/// <summary>
/// Represents one deterministic sound effect clip variant.
/// </summary>
public sealed record SoundEffectVariant
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SoundEffectVariant"/> class.
    /// </summary>
    /// <param name="variantId">The stable variant identifier.</param>
    /// <param name="audioClipId">The logical audio clip identifier.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="variantId"/> or <paramref name="audioClipId"/> is empty.</exception>
    public SoundEffectVariant(string variantId, string audioClipId)
    {
        if (string.IsNullOrWhiteSpace(variantId))
        {
            throw new ArgumentException("The sound effect variant identifier must contain a value.", nameof(variantId));
        }

        if (string.IsNullOrWhiteSpace(audioClipId))
        {
            throw new ArgumentException("The sound effect variant clip identifier must contain a value.", nameof(audioClipId));
        }

        VariantId = variantId;
        AudioClipId = audioClipId;
    }

    /// <summary>
    /// Gets the stable variant identifier.
    /// </summary>
    public string VariantId { get; }

    /// <summary>
    /// Gets the logical audio clip identifier.
    /// </summary>
    public string AudioClipId { get; }
}

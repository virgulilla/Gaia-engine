using GaiaEngine.Audio.Events;

namespace GaiaEngine.Audio.SoundEffects;

/// <summary>
/// Selects deterministic sound effect variants from translated audio events.
/// </summary>
public interface ISoundEffectLibrary
{
    /// <summary>
    /// Attempts to resolve one sound effect selection for the supplied audio event.
    /// </summary>
    /// <param name="audioEvent">The translated audio event to resolve.</param>
    /// <param name="selection">The selected sound effect when found.</param>
    /// <returns><see langword="true"/> when one selection is produced; otherwise <see langword="false"/>.</returns>
    public bool TrySelect(AudioEvent audioEvent, out SoundEffectSelection? selection);
}

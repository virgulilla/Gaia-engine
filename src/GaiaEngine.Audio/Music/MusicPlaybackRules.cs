using System;

namespace GaiaEngine.Audio.Music;

/// <summary>
/// Represents deterministic playback rules attached to one music theme.
/// </summary>
public sealed record MusicPlaybackRules
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MusicPlaybackRules"/> class.
    /// </summary>
    /// <param name="looping">Whether the selected music should loop.</param>
    /// <param name="canInterruptLowerPriority">Whether the theme can interrupt lower-priority themes.</param>
    /// <param name="transitionKind">The preferred transition kind.</param>
    /// <param name="transitionTicks">The preferred transition duration in ticks.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="transitionTicks"/> is negative.</exception>
    public MusicPlaybackRules(bool looping, bool canInterruptLowerPriority, MusicTransitionKind transitionKind, int transitionTicks)
    {
        if (transitionTicks < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(transitionTicks), "The music transition duration must be zero or greater.");
        }

        Looping = looping;
        CanInterruptLowerPriority = canInterruptLowerPriority;
        TransitionKind = transitionKind;
        TransitionTicks = transitionTicks;
    }

    /// <summary>
    /// Gets a value indicating whether the selected music should loop.
    /// </summary>
    public bool Looping { get; }

    /// <summary>
    /// Gets a value indicating whether the theme can interrupt lower-priority themes.
    /// </summary>
    public bool CanInterruptLowerPriority { get; }

    /// <summary>
    /// Gets the preferred transition kind.
    /// </summary>
    public MusicTransitionKind TransitionKind { get; }

    /// <summary>
    /// Gets the preferred transition duration in ticks.
    /// </summary>
    public int TransitionTicks { get; }
}

using System;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the passive wind values stored for one chunk climate state.
/// </summary>
public sealed record WindState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WindState"/> class.
    /// </summary>
    /// <param name="direction">The deterministic wind direction in degrees.</param>
    /// <param name="speed">The deterministic wind speed.</param>
    /// <param name="gustStrength">The deterministic gust strength.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="direction"/> is outside the inclusive range [0, 359]
    /// or when <paramref name="speed"/> or <paramref name="gustStrength"/> is negative.
    /// </exception>
    public WindState(int direction, int speed, int gustStrength)
    {
        if (direction < 0 || direction > 359)
        {
            throw new ArgumentOutOfRangeException(nameof(direction), "The wind direction must be between 0 and 359 degrees.");
        }

        if (speed < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(speed), "The wind speed must be zero or greater.");
        }

        if (gustStrength < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(gustStrength), "The gust strength must be zero or greater.");
        }

        Direction = direction;
        Speed = speed;
        GustStrength = gustStrength;
    }

    /// <summary>
    /// Gets the deterministic wind direction in degrees.
    /// </summary>
    public int Direction { get; }

    /// <summary>
    /// Gets the deterministic wind speed.
    /// </summary>
    public int Speed { get; }

    /// <summary>
    /// Gets the deterministic gust strength.
    /// </summary>
    public int GustStrength { get; }
}

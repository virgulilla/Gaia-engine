namespace GaiaEngine.Audio.Events;

/// <summary>
/// Represents one deterministic spatial audio position.
/// </summary>
public sealed record AudioPosition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioPosition"/> class.
    /// </summary>
    /// <param name="x">The world x position.</param>
    /// <param name="y">The world y position.</param>
    /// <param name="z">The world z position.</param>
    public AudioPosition(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    /// <summary>
    /// Gets the world x position.
    /// </summary>
    public float X { get; }

    /// <summary>
    /// Gets the world y position.
    /// </summary>
    public float Y { get; }

    /// <summary>
    /// Gets the world z position.
    /// </summary>
    public float Z { get; }
}

namespace GaiaEngine.Foundation.Determinism;

/// <summary>
/// Represents the immutable world seed that drives deterministic simulation setup.
/// </summary>
public readonly record struct WorldSeed
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WorldSeed"/> struct.
    /// </summary>
    /// <param name="value">The seed value.</param>
    public WorldSeed(long value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets the seed value.
    /// </summary>
    public long Value { get; }

    /// <summary>
    /// Returns the seed as a string for diagnostics.
    /// </summary>
    /// <returns>The seed value string.</returns>
    public override string ToString()
    {
        return Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
    }
}

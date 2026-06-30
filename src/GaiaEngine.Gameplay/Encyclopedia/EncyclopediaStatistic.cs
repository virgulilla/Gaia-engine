using System;

namespace GaiaEngine.Gameplay.Encyclopedia;

/// <summary>
/// Represents one immutable encyclopedia statistic entry.
/// </summary>
public sealed record EncyclopediaStatistic
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EncyclopediaStatistic"/> class.
    /// </summary>
    /// <param name="key">The stable statistic key.</param>
    /// <param name="value">The deterministic statistic value.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="key"/> is empty.</exception>
    public EncyclopediaStatistic(string key, int value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentException("The encyclopedia statistic key must contain a value.", nameof(key));
        }

        Key = key;
        Value = value;
    }

    /// <summary>
    /// Gets the stable statistic key.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the deterministic statistic value.
    /// </summary>
    public int Value { get; }
}

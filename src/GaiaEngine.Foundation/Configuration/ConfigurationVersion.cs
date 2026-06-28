using System;

namespace GaiaEngine.Foundation.Configuration;

/// <summary>
/// Represents the immutable version of the configuration set used by a deterministic simulation.
/// </summary>
public readonly record struct ConfigurationVersion
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationVersion"/> struct.
    /// </summary>
    /// <param name="value">The configuration version value.</param>
    /// <exception cref="ArgumentException">Thrown when the version is null, empty, or whitespace.</exception>
    public ConfigurationVersion(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("The configuration version must contain a value.", nameof(value));
        }

        Value = value;
    }

    /// <summary>
    /// Gets the configuration version value.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Returns the configuration version string.
    /// </summary>
    /// <returns>The configuration version.</returns>
    public override string ToString()
    {
        return Value;
    }
}

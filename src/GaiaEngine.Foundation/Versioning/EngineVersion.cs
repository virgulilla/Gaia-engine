using System;
using System.Globalization;

namespace GaiaEngine.Foundation.Versioning;

/// <summary>
/// Represents the immutable engine version used to reproduce deterministic simulations.
/// </summary>
public readonly record struct EngineVersion
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EngineVersion"/> struct.
    /// </summary>
    /// <param name="major">The major version number.</param>
    /// <param name="minor">The minor version number.</param>
    /// <param name="patch">The patch version number.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when any version component is negative.
    /// </exception>
    public EngineVersion(int major, int minor, int patch)
    {
        if (major < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(major), "The major version must be zero or greater.");
        }

        if (minor < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minor), "The minor version must be zero or greater.");
        }

        if (patch < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(patch), "The patch version must be zero or greater.");
        }

        Major = major;
        Minor = minor;
        Patch = patch;
    }

    /// <summary>
    /// Gets the major version number.
    /// </summary>
    public int Major { get; }

    /// <summary>
    /// Gets the minor version number.
    /// </summary>
    public int Minor { get; }

    /// <summary>
    /// Gets the patch version number.
    /// </summary>
    public int Patch { get; }

    /// <summary>
    /// Parses a semantic engine version string.
    /// </summary>
    /// <param name="value">The semantic version string.</param>
    /// <returns>The parsed engine version.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="value"/> is empty.</exception>
    /// <exception cref="FormatException">Thrown when <paramref name="value"/> is not a valid semantic version.</exception>
    public static EngineVersion Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("The engine version must contain a value.", nameof(value));
        }

        string[] parts = value.Split('.', StringSplitOptions.TrimEntries);
        if (parts.Length != 3)
        {
            throw new FormatException("The engine version must contain exactly three numeric components.");
        }

        bool majorParsed = int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out int major);
        bool minorParsed = int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out int minor);
        bool patchParsed = int.TryParse(parts[2], NumberStyles.None, CultureInfo.InvariantCulture, out int patch);

        if (!majorParsed || !minorParsed || !patchParsed)
        {
            throw new FormatException("The engine version must contain only numeric components.");
        }

        return new EngineVersion(major, minor, patch);
    }

    /// <summary>
    /// Returns the version as a semantic version string.
    /// </summary>
    /// <returns>The semantic version string.</returns>
    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }
}

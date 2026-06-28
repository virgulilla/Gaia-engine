using System;
using GaiaEngine.Foundation.Versioning;

namespace GaiaEngine.Serialization.SaveGames;

/// <summary>
/// Represents the version information embedded in a serialized save document.
/// </summary>
public sealed record SaveVersionInfo
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SaveVersionInfo"/> class.
    /// </summary>
    /// <param name="formatVersion">The save format version string.</param>
    /// <param name="engineVersion">The engine version used by the save.</param>
    /// <param name="contentVersion">The content version string.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="formatVersion"/> or <paramref name="contentVersion"/> is empty.
    /// </exception>
    public SaveVersionInfo(string formatVersion, EngineVersion engineVersion, string contentVersion)
    {
        if (string.IsNullOrWhiteSpace(formatVersion))
        {
            throw new ArgumentException("The format version must contain a value.", nameof(formatVersion));
        }

        if (string.IsNullOrWhiteSpace(contentVersion))
        {
            throw new ArgumentException("The content version must contain a value.", nameof(contentVersion));
        }

        FormatVersion = formatVersion;
        EngineVersion = engineVersion;
        ContentVersion = contentVersion;
    }

    /// <summary>
    /// Gets the save format version string.
    /// </summary>
    public string FormatVersion { get; }

    /// <summary>
    /// Gets the engine version used by the save.
    /// </summary>
    public EngineVersion EngineVersion { get; }

    /// <summary>
    /// Gets the content version string.
    /// </summary>
    public string ContentVersion { get; }
}

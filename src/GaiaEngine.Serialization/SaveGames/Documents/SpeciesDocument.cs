using System.Collections.Generic;

namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized species section.
/// </summary>
internal sealed class SpeciesDocument
{
    /// <summary>
    /// Gets or sets the serialized species identifier.
    /// </summary>
    public string SpeciesId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the serialized parent species identifier.
    /// </summary>
    public string? ParentSpeciesId { get; set; }

    /// <summary>
    /// Gets or sets the serialized origin tick.
    /// </summary>
    public long OriginTick { get; set; }

    /// <summary>
    /// Gets or sets the serialized extinction tick.
    /// </summary>
    public long? ExtinctionTick { get; set; }

    /// <summary>
    /// Gets or sets the serialized founder organism identifiers.
    /// </summary>
    public List<string> FounderPopulation { get; set; } = new();
}

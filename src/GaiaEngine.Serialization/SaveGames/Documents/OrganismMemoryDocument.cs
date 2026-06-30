namespace GaiaEngine.Serialization.SaveGames.Documents;

using System.Collections.Generic;

/// <summary>
/// Represents the serialized memory set of one organism.
/// </summary>
internal sealed class OrganismMemoryDocument
{
    /// <summary>
    /// Gets or sets the organism identifier.
    /// </summary>
    public string OrganismId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the memory entries owned by the organism.
    /// </summary>
    public List<MemoryEntryDocument> Entries { get; set; } = new();
}

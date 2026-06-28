using System.Collections.Generic;

namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized world section of a save game.
/// </summary>
internal sealed class WorldDocument
{
    public string WorldId { get; set; } = string.Empty;

    public string WorldName { get; set; } = string.Empty;

    public long Seed { get; set; }

    public string CreationDate { get; set; } = string.Empty;

    public string EngineVersion { get; set; } = string.Empty;

    public string ConfigurationVersion { get; set; } = string.Empty;

    public int Width { get; set; }

    public int Height { get; set; }

    public int ChunkSize { get; set; }

    public int ChunkCount { get; set; }

    public int MaximumElevation { get; set; }

    public long CurrentTick { get; set; }

    public int CurrentDay { get; set; }

    public string CurrentSeason { get; set; } = string.Empty;

    public int CurrentYear { get; set; }

    public List<ChunkDocument> Chunks { get; set; } = new();
}

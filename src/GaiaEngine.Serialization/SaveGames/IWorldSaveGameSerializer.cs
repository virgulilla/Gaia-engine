namespace GaiaEngine.Serialization.SaveGames;

/// <summary>
/// Serializes and deserializes world save documents.
/// </summary>
public interface IWorldSaveGameSerializer
{
    /// <summary>
    /// Serializes a world save game into a deterministic payload.
    /// </summary>
    /// <param name="saveGame">The save game to serialize.</param>
    /// <returns>The serialized payload.</returns>
    public string Serialize(WorldSaveGame saveGame);

    /// <summary>
    /// Deserializes a world save game from a payload.
    /// </summary>
    /// <param name="payload">The serialized payload.</param>
    /// <returns>The deserialized save game.</returns>
    public WorldSaveGame Deserialize(string payload);
}

using GaiaEngine.Gameplay.Player;

namespace GaiaEngine.Serialization.Profiles;

/// <summary>
/// Serializes player profiles independently from world save data.
/// </summary>
public interface IPlayerProfileSerializer
{
    /// <summary>
    /// Serializes one player profile into a deterministic payload.
    /// </summary>
    /// <param name="profile">The player profile to serialize.</param>
    /// <returns>The serialized payload.</returns>
    public string Serialize(PlayerProfile profile);

    /// <summary>
    /// Deserializes one player profile from a payload.
    /// </summary>
    /// <param name="payload">The serialized payload.</param>
    /// <returns>The restored player profile.</returns>
    public PlayerProfile Deserialize(string payload);
}

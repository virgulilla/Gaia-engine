using GaiaEngine.Gameplay.Player;
using GaiaEngine.Serialization.SaveGames;

namespace GaiaEngine.App.SaveGames;

/// <summary>
/// Represents one loaded save slot together with its independent player profile.
/// </summary>
public sealed record LoadedSaveSlot(
    SaveSlotSummary Summary,
    WorldSaveGame WorldSaveGame,
    PlayerProfile PlayerProfile);

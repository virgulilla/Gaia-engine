using GaiaEngine.Foundation.Determinism;

namespace GaiaEngine.App.SaveGames;

/// <summary>
/// Represents lightweight metadata about one persistent save slot.
/// </summary>
public sealed record SaveSlotSummary(
    string SlotId,
    SaveSlotType SlotType,
    string SaveName,
    string WorldName,
    string LastModified,
    WorldSeed WorldSeed);

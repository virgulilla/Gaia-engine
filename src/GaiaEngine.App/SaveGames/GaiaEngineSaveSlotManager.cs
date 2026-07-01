using System;
using System.Collections.Generic;
using System.IO;
using GaiaEngine.App.Bootstrap;
using GaiaEngine.Gameplay.Player;
using GaiaEngine.Serialization.Profiles;
using GaiaEngine.Serialization.SaveGames;

namespace GaiaEngine.App.SaveGames;

/// <summary>
/// Manages persistent world save slots and independent player profile snapshots.
/// </summary>
public sealed class GaiaEngineSaveSlotManager
{
    private const string SaveFormatVersion = "1.0.0";
    private readonly string rootDirectory;
    private readonly IWorldSaveGameSerializer worldSaveGameSerializer;
    private readonly IPlayerProfileSerializer playerProfileSerializer;

    /// <summary>
    /// Initializes a new instance of the <see cref="GaiaEngineSaveSlotManager"/> class.
    /// </summary>
    /// <param name="rootDirectory">The absolute root directory used for save slot storage.</param>
    /// <param name="worldSaveGameSerializer">The serializer used for world save payloads.</param>
    /// <param name="playerProfileSerializer">The serializer used for player profile payloads.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="rootDirectory"/> is empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when a serializer dependency is <see langword="null"/>.</exception>
    public GaiaEngineSaveSlotManager(
        string rootDirectory,
        IWorldSaveGameSerializer worldSaveGameSerializer,
        IPlayerProfileSerializer playerProfileSerializer)
    {
        if (string.IsNullOrWhiteSpace(rootDirectory))
        {
            throw new ArgumentException("The save slot root directory must contain a value.", nameof(rootDirectory));
        }

        this.rootDirectory = rootDirectory;
        this.worldSaveGameSerializer = worldSaveGameSerializer ?? throw new ArgumentNullException(nameof(worldSaveGameSerializer));
        this.playerProfileSerializer = playerProfileSerializer ?? throw new ArgumentNullException(nameof(playerProfileSerializer));
    }

    /// <summary>
    /// Saves the supplied runtime to one manual slot.
    /// </summary>
    /// <param name="runtime">The runtime to persist.</param>
    /// <param name="saveName">The user-visible save name.</param>
    /// <param name="timestamp">The timestamp string used for metadata.</param>
    /// <returns>The resulting slot summary.</returns>
    public SaveSlotSummary SaveManual(GaiaEngineRuntime runtime, string saveName, string timestamp)
    {
        return Save(runtime, SaveSlotType.Manual, $"manual-{SanitizeSlotKey(timestamp)}", saveName, timestamp);
    }

    /// <summary>
    /// Saves the supplied runtime to the shared quick save slot.
    /// </summary>
    /// <param name="runtime">The runtime to persist.</param>
    /// <param name="timestamp">The timestamp string used for metadata.</param>
    /// <returns>The resulting slot summary.</returns>
    public SaveSlotSummary SaveQuick(GaiaEngineRuntime runtime, string timestamp)
    {
        return Save(runtime, SaveSlotType.Quick, "quick-save", "Quick Save", timestamp);
    }

    /// <summary>
    /// Saves the supplied runtime to the shared auto save slot.
    /// </summary>
    /// <param name="runtime">The runtime to persist.</param>
    /// <param name="timestamp">The timestamp string used for metadata.</param>
    /// <returns>The resulting slot summary.</returns>
    public SaveSlotSummary SaveAuto(GaiaEngineRuntime runtime, string timestamp)
    {
        return Save(runtime, SaveSlotType.Auto, "auto-save", "Auto Save", timestamp);
    }

    /// <summary>
    /// Lists all readable save slots in deterministic order.
    /// </summary>
    /// <returns>The ordered save slot summaries.</returns>
    public IReadOnlyList<SaveSlotSummary> ListSlots()
    {
        List<SaveSlotSummary> summaries = new();
        foreach ((SaveSlotType slotType, string folderName) in GetSlotFolders())
        {
            string directoryPath = Path.Combine(rootDirectory, folderName);
            if (!Directory.Exists(directoryPath))
            {
                continue;
            }

            string[] worldSaveFiles = Directory.GetFiles(directoryPath, "*.world.json", SearchOption.TopDirectoryOnly);
            Array.Sort(worldSaveFiles, StringComparer.Ordinal);
            foreach (string worldSaveFile in worldSaveFiles)
            {
                try
                {
                    WorldSaveGame worldSaveGame = worldSaveGameSerializer.Deserialize(File.ReadAllText(worldSaveFile));
                    string slotId = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(worldSaveFile));
                    summaries.Add(
                        new SaveSlotSummary(
                            slotId,
                            slotType,
                            worldSaveGame.Metadata.SaveName,
                            worldSaveGame.World.Metadata.WorldName,
                            worldSaveGame.Metadata.LastModified,
                            worldSaveGame.Metadata.WorldSeed));
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        summaries.Sort(static (left, right) =>
        {
            int modifiedComparison = string.CompareOrdinal(right.LastModified, left.LastModified);
            return modifiedComparison != 0 ? modifiedComparison : string.CompareOrdinal(left.SlotId, right.SlotId);
        });
        return summaries.AsReadOnly();
    }

    /// <summary>
    /// Loads one persisted save slot together with its player profile.
    /// </summary>
    /// <param name="slotId">The stable slot identifier.</param>
    /// <returns>The loaded slot data.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="slotId"/> is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the slot cannot be resolved.</exception>
    public LoadedSaveSlot Load(string slotId)
    {
        if (string.IsNullOrWhiteSpace(slotId))
        {
            throw new ArgumentException("The slot identifier must contain a value.", nameof(slotId));
        }

        foreach ((SaveSlotType slotType, string folderName) in GetSlotFolders())
        {
            string directoryPath = Path.Combine(rootDirectory, folderName);
            string worldPath = Path.Combine(directoryPath, $"{slotId}.world.json");
            string profilePath = Path.Combine(directoryPath, $"{slotId}.profile.json");
            if (!File.Exists(worldPath) || !File.Exists(profilePath))
            {
                continue;
            }

            WorldSaveGame worldSaveGame = worldSaveGameSerializer.Deserialize(File.ReadAllText(worldPath));
            ValidateCompatibility(worldSaveGame);
            PlayerProfile playerProfile = playerProfileSerializer.Deserialize(File.ReadAllText(profilePath));
            SaveSlotSummary summary = new(
                slotId,
                slotType,
                worldSaveGame.Metadata.SaveName,
                worldSaveGame.World.Metadata.WorldName,
                worldSaveGame.Metadata.LastModified,
                worldSaveGame.Metadata.WorldSeed);
            return new LoadedSaveSlot(summary, worldSaveGame, playerProfile);
        }

        throw new InvalidOperationException($"The save slot '{slotId}' could not be found.");
    }

    private static void ValidateCompatibility(WorldSaveGame worldSaveGame)
    {
        ArgumentNullException.ThrowIfNull(worldSaveGame);

        if (!string.Equals(worldSaveGame.Metadata.SaveVersion, SaveFormatVersion, StringComparison.Ordinal)
            || !string.Equals(worldSaveGame.Version.FormatVersion, SaveFormatVersion, StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"The save format version '{worldSaveGame.Version.FormatVersion}' is incompatible with the supported version '{SaveFormatVersion}'.");
        }

        if (!string.Equals(worldSaveGame.Version.ContentVersion, worldSaveGame.ConfigurationVersion.ToString(), StringComparison.Ordinal))
        {
            throw new InvalidOperationException("The save content version does not match the serialized configuration version.");
        }
    }

    private SaveSlotSummary Save(GaiaEngineRuntime runtime, SaveSlotType slotType, string slotId, string saveName, string timestamp)
    {
        ArgumentNullException.ThrowIfNull(runtime);
        if (string.IsNullOrWhiteSpace(slotId))
        {
            throw new ArgumentException("The slot identifier must contain a value.", nameof(slotId));
        }

        if (string.IsNullOrWhiteSpace(saveName))
        {
            throw new ArgumentException("The save name must contain a value.", nameof(saveName));
        }

        if (string.IsNullOrWhiteSpace(timestamp))
        {
            throw new ArgumentException("The timestamp must contain a value.", nameof(timestamp));
        }

        string directoryPath = Path.Combine(rootDirectory, GetSlotFolderName(slotType));
        Directory.CreateDirectory(directoryPath);

        WorldSaveGame saveGame = runtime.CreateSaveGame(saveName, timestamp, SaveFormatVersion);
        string worldPath = Path.Combine(directoryPath, $"{slotId}.world.json");
        string profilePath = Path.Combine(directoryPath, $"{slotId}.profile.json");
        File.WriteAllText(worldPath, worldSaveGameSerializer.Serialize(saveGame));
        File.WriteAllText(profilePath, playerProfileSerializer.Serialize(runtime.PlayerProfile));
        return new SaveSlotSummary(slotId, slotType, saveName, runtime.World.Metadata.WorldName, timestamp, runtime.World.Metadata.Seed);
    }

    private static IEnumerable<(SaveSlotType SlotType, string FolderName)> GetSlotFolders()
    {
        yield return (SaveSlotType.Manual, GetSlotFolderName(SaveSlotType.Manual));
        yield return (SaveSlotType.Auto, GetSlotFolderName(SaveSlotType.Auto));
        yield return (SaveSlotType.Quick, GetSlotFolderName(SaveSlotType.Quick));
    }

    private static string GetSlotFolderName(SaveSlotType slotType)
    {
        return slotType switch
        {
            SaveSlotType.Manual => "manual",
            SaveSlotType.Auto => "auto",
            _ => "quick",
        };
    }

    private static string SanitizeSlotKey(string value)
    {
        string sanitized = value.Replace(":", string.Empty, StringComparison.Ordinal)
            .Replace("-", string.Empty, StringComparison.Ordinal)
            .Replace(" ", "_", StringComparison.Ordinal);
        return sanitized;
    }
}

using System;
using System.Collections.Generic;
using System.Text.Json;
using GaiaEngine.Domain.Identifiers;
using GaiaEngine.Gameplay.Discovery;
using GaiaEngine.Gameplay.Encyclopedia;
using GaiaEngine.Gameplay.Player;
using GaiaEngine.Serialization.Profiles.Documents;

namespace GaiaEngine.Serialization.Profiles;

/// <summary>
/// Serializes player profiles using deterministic JSON payloads.
/// </summary>
public sealed class JsonPlayerProfileSerializer : IPlayerProfileSerializer
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false,
    };

    /// <summary>
    /// Serializes one player profile into a deterministic payload.
    /// </summary>
    /// <param name="profile">The player profile to serialize.</param>
    /// <returns>The serialized payload.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="profile"/> is <see langword="null"/>.</exception>
    public string Serialize(PlayerProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        return JsonSerializer.Serialize(CreateDocument(profile), SerializerOptions);
    }

    /// <summary>
    /// Deserializes one player profile from a payload.
    /// </summary>
    /// <param name="payload">The serialized payload.</param>
    /// <returns>The restored player profile.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="payload"/> is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the payload is invalid or incomplete.</exception>
    public PlayerProfile Deserialize(string payload)
    {
        if (string.IsNullOrWhiteSpace(payload))
        {
            throw new ArgumentException("The payload must contain a value.", nameof(payload));
        }

        PlayerProfileDocument? document = JsonSerializer.Deserialize<PlayerProfileDocument>(payload, SerializerOptions);
        if (document is null)
        {
            throw new InvalidOperationException("The player profile document could not be deserialized.");
        }

        return CreateProfile(document);
    }

    private static PlayerProfileDocument CreateDocument(PlayerProfile profile)
    {
        List<DiscoveryEntryDocument> discoveries = new(profile.Knowledge.Discoveries.Count);
        List<EncyclopediaEntryDocument> encyclopedia = new(profile.Knowledge.Encyclopedia.Count);
        foreach (DiscoveryEntry entry in profile.Knowledge.Discoveries.GetAll())
        {
            discoveries.Add(
                new DiscoveryEntryDocument
                {
                    DiscoveryId = entry.DiscoveryId,
                    Category = entry.Category.ToString(),
                    Name = entry.Name,
                    Description = entry.Description,
                    UnlockTick = entry.UnlockTick,
                    WorldId = entry.WorldId.ToString(),
                    PlayerId = entry.PlayerId,
                });
        }

        foreach (EncyclopediaEntry entry in profile.Knowledge.Encyclopedia.GetAll())
        {
            List<EncyclopediaStatisticDocument> statistics = new(entry.GetStatistics().Count);
            foreach (EncyclopediaStatistic statistic in entry.GetStatistics())
            {
                statistics.Add(
                    new EncyclopediaStatisticDocument
                    {
                        Key = statistic.Key,
                        Value = statistic.Value,
                    });
            }

            encyclopedia.Add(
                new EncyclopediaEntryDocument
                {
                    EntryId = entry.EntryId,
                    Category = entry.Category.ToString(),
                    Title = entry.Title,
                    Description = entry.Description,
                    UnlockState = entry.UnlockState.ToString(),
                    DiscoveryDate = entry.DiscoveryDate,
                    RelatedEntries = new List<string>(entry.GetRelatedEntries()),
                    Statistics = statistics,
                });
        }

        return new PlayerProfileDocument
        {
            Identity = new PlayerIdentityDocument
            {
                PlayerId = profile.Identity.PlayerId,
                ProfileName = profile.Identity.ProfileName,
                CreationDate = profile.Identity.CreationDate,
            },
            Discoveries = discoveries,
            Encyclopedia = encyclopedia,
            Progression = new PlayerProgressionDocument
            {
                Experience = profile.Progression.Experience,
                Discoveries = profile.Progression.Discoveries,
                UnlockLevel = profile.Progression.UnlockLevel,
            },
            Statistics = new PlayerStatisticsDocument
            {
                TotalDiscoveriesUnlocked = profile.Statistics.TotalDiscoveriesUnlocked,
                DuplicateDiscoveryObservations = profile.Statistics.DuplicateDiscoveryObservations,
            },
        };
    }

    private static PlayerProfile CreateProfile(PlayerProfileDocument document)
    {
        if (document.Identity is null)
        {
            throw new InvalidOperationException("The player identity section is required.");
        }

        if (document.Progression is null)
        {
            throw new InvalidOperationException("The player progression section is required.");
        }

        if (document.Statistics is null)
        {
            throw new InvalidOperationException("The player statistics section is required.");
        }

        List<DiscoveryEntry> discoveries = new(document.Discoveries.Count);
        foreach (DiscoveryEntryDocument entry in document.Discoveries)
        {
            discoveries.Add(
                new DiscoveryEntry(
                    entry.DiscoveryId,
                    Enum.Parse<DiscoveryCategory>(entry.Category, ignoreCase: false),
                    entry.Name,
                    entry.Description,
                    entry.UnlockTick,
                    WorldId.Parse(entry.WorldId),
                    entry.PlayerId));
        }

        List<EncyclopediaEntry> encyclopediaEntries = new(document.Encyclopedia.Count);
        foreach (EncyclopediaEntryDocument entry in document.Encyclopedia)
        {
            List<EncyclopediaStatistic> statistics = new(entry.Statistics.Count);
            foreach (EncyclopediaStatisticDocument statistic in entry.Statistics)
            {
                statistics.Add(new EncyclopediaStatistic(statistic.Key, statistic.Value));
            }

            encyclopediaEntries.Add(
                new EncyclopediaEntry(
                    entry.EntryId,
                    Enum.Parse<EncyclopediaCategory>(entry.Category, ignoreCase: false),
                    entry.Title,
                    entry.Description,
                    Enum.Parse<EncyclopediaUnlockState>(entry.UnlockState, ignoreCase: false),
                    entry.DiscoveryDate,
                    entry.RelatedEntries.AsReadOnly(),
                    statistics.AsReadOnly()));
        }

        return new PlayerProfile(
            new PlayerIdentity(document.Identity.PlayerId, document.Identity.ProfileName, document.Identity.CreationDate),
            new PlayerKnowledge(
                new DiscoveryCollection(discoveries.AsReadOnly()),
                new EncyclopediaCollection(encyclopediaEntries.AsReadOnly())),
            new PlayerProgression(document.Progression.Experience, document.Progression.Discoveries, document.Progression.UnlockLevel),
            new PlayerStatistics(document.Statistics.TotalDiscoveriesUnlocked, document.Statistics.DuplicateDiscoveryObservations));
    }
}

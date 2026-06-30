using System;
using System.Collections.Generic;

namespace GaiaEngine.Audio.Events;

/// <summary>
/// Represents the configurable audio translation catalog.
/// </summary>
public sealed class AudioEventCatalog
{
    private readonly Dictionary<string, AudioEventDefinition> simulationDefinitions;
    private readonly Dictionary<GameplayAudioSignalKind, AudioEventDefinition> gameplayDefinitions;

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioEventCatalog"/> class.
    /// </summary>
    /// <param name="simulationDefinitions">The simulation event definitions keyed by source key.</param>
    /// <param name="gameplayDefinitions">The gameplay signal definitions keyed by signal kind.</param>
    /// <exception cref="ArgumentNullException">Thrown when one argument is <see langword="null"/>.</exception>
    public AudioEventCatalog(
        IReadOnlyDictionary<string, AudioEventDefinition> simulationDefinitions,
        IReadOnlyDictionary<GameplayAudioSignalKind, AudioEventDefinition> gameplayDefinitions)
    {
        ArgumentNullException.ThrowIfNull(simulationDefinitions);
        ArgumentNullException.ThrowIfNull(gameplayDefinitions);

        this.simulationDefinitions = new Dictionary<string, AudioEventDefinition>(simulationDefinitions, StringComparer.Ordinal);
        this.gameplayDefinitions = new Dictionary<GameplayAudioSignalKind, AudioEventDefinition>(gameplayDefinitions);
    }

    /// <summary>
    /// Attempts to resolve one simulation event definition.
    /// </summary>
    /// <param name="sourceKey">The source key to resolve.</param>
    /// <param name="definition">The resolved definition when found.</param>
    /// <returns><see langword="true"/> when the definition exists; otherwise <see langword="false"/>.</returns>
    public bool TryResolveSimulation(string sourceKey, out AudioEventDefinition? definition)
    {
        if (string.IsNullOrWhiteSpace(sourceKey))
        {
            definition = null;
            return false;
        }

        return simulationDefinitions.TryGetValue(sourceKey, out definition);
    }

    /// <summary>
    /// Attempts to resolve one gameplay signal definition.
    /// </summary>
    /// <param name="kind">The gameplay signal kind to resolve.</param>
    /// <param name="definition">The resolved definition when found.</param>
    /// <returns><see langword="true"/> when the definition exists; otherwise <see langword="false"/>.</returns>
    public bool TryResolveGameplay(GameplayAudioSignalKind kind, out AudioEventDefinition? definition)
    {
        return gameplayDefinitions.TryGetValue(kind, out definition);
    }
}

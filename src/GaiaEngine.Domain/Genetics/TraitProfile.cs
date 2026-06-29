using System;
using System.Collections.Generic;

namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Stores the deterministic expressed trait set derived from one genome.
/// </summary>
public sealed class TraitProfile
{
    private readonly IReadOnlyList<ExpressedTrait> orderedTraits;
    private readonly Dictionary<TraitKey, ExpressedTrait> traitsByKey;

    /// <summary>
    /// Initializes a new instance of the <see cref="TraitProfile"/> class.
    /// </summary>
    /// <param name="traits">The expressed traits to store.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="traits"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">Thrown when duplicated trait identifiers are detected.</exception>
    public TraitProfile(IReadOnlyList<ExpressedTrait> traits)
    {
        ArgumentNullException.ThrowIfNull(traits);

        List<ExpressedTrait> ordered = new(traits.Count);
        traitsByKey = new Dictionary<TraitKey, ExpressedTrait>(traits.Count);
        foreach (ExpressedTrait trait in traits)
        {
            ArgumentNullException.ThrowIfNull(trait);
            if (!traitsByKey.TryAdd(trait.Key, trait))
            {
                throw new ArgumentException($"The trait '{trait.Key}' is duplicated.", nameof(traits));
            }

            ordered.Add(trait);
        }

        ordered.Sort(static (left, right) => left.Key.CompareTo(right.Key));
        orderedTraits = ordered.AsReadOnly();
    }

    /// <summary>
    /// Gets the stored traits in deterministic order.
    /// </summary>
    /// <returns>The ordered expressed traits.</returns>
    public IReadOnlyList<ExpressedTrait> GetAll()
    {
        return orderedTraits;
    }

    /// <summary>
    /// Resolves one trait value by identifier.
    /// </summary>
    /// <param name="key">The trait identifier to resolve.</param>
    /// <returns>The resolved normalized trait value.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the requested trait does not exist.</exception>
    public int GetValue(TraitKey key)
    {
        if (!traitsByKey.TryGetValue(key, out ExpressedTrait? trait))
        {
            throw new InvalidOperationException($"The trait '{key}' is required but was not found.");
        }

        return trait.Value.ScaledValue;
    }
}

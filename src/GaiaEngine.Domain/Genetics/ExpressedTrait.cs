using System;

namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Represents one deterministic expressed biological trait.
/// </summary>
public sealed record ExpressedTrait
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExpressedTrait"/> class.
    /// </summary>
    /// <param name="key">The trait identifier.</param>
    /// <param name="value">The normalized trait value.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <see langword="null"/>.</exception>
    public ExpressedTrait(TraitKey key, NormalizedGeneValue value)
    {
        Key = key;
        Value = value;
    }

    /// <summary>
    /// Gets the trait identifier.
    /// </summary>
    public TraitKey Key { get; }

    /// <summary>
    /// Gets the normalized trait value.
    /// </summary>
    public NormalizedGeneValue Value { get; }
}

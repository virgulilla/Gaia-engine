using System;

namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Represents one immutable deterministic genome gene.
/// </summary>
public sealed record GenomeGene
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenomeGene"/> class.
    /// </summary>
    /// <param name="key">The gene identifier.</param>
    /// <param name="value">The normalized gene value.</param>
    /// <param name="dominance">The gene dominance mode.</param>
    /// <param name="isActive">A value indicating whether the gene is active.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="value"/> is <see langword="null"/>.</exception>
    public GenomeGene(GenomeGeneKey key, NormalizedGeneValue value, GeneDominance dominance, bool isActive)
    {
        Key = key;
        Value = value;
        Dominance = dominance;
        IsActive = isActive;
    }

    /// <summary>
    /// Gets the gene identifier.
    /// </summary>
    public GenomeGeneKey Key { get; }

    /// <summary>
    /// Gets the normalized deterministic value.
    /// </summary>
    public NormalizedGeneValue Value { get; }

    /// <summary>
    /// Gets the dominance mode.
    /// </summary>
    public GeneDominance Dominance { get; }

    /// <summary>
    /// Gets a value indicating whether the gene is active.
    /// </summary>
    public bool IsActive { get; }
}

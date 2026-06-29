using GaiaEngine.Domain.Genetics;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Translates immutable genomes into deterministic expressed biological traits.
/// </summary>
public interface ITraitExpressionService
{
    /// <summary>
    /// Evaluates one genome and produces its expressed trait profile.
    /// </summary>
    /// <param name="genome">The immutable genome to interpret.</param>
    /// <returns>The expressed trait profile.</returns>
    public TraitProfile Evaluate(Genome genome);
}

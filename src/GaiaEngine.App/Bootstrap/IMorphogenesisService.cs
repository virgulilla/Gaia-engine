using GaiaEngine.Domain.Genetics;

namespace GaiaEngine.App.Bootstrap;

/// <summary>
/// Transforms deterministic genetic data into a phenotype and initialized physiology.
/// </summary>
public interface IMorphogenesisService
{
    /// <summary>
    /// Executes one deterministic morphogenesis pass.
    /// </summary>
    /// <param name="genome">The immutable genome to interpret.</param>
    /// <param name="developmentConditions">The environmental development conditions.</param>
    /// <returns>The generated phenotype and initialized physiology.</returns>
    public MorphogenesisResult Generate(Genome genome, DevelopmentConditions developmentConditions);
}

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
    /// <param name="traits">The expressed trait profile to interpret.</param>
    /// <param name="developmentConditions">The environmental development conditions.</param>
    /// <returns>The generated phenotype and initialized physiology.</returns>
    public MorphogenesisResult Generate(TraitProfile traits, DevelopmentConditions developmentConditions);
}

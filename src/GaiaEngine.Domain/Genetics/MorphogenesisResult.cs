using System;
using GaiaEngine.Domain.Organisms;

namespace GaiaEngine.Domain.Genetics;

/// <summary>
/// Represents the deterministic result produced by one morphogenesis pass.
/// </summary>
public sealed record MorphogenesisResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MorphogenesisResult"/> class.
    /// </summary>
    /// <param name="bodyPlan">The generated phenotype description.</param>
    /// <param name="physiology">The initialized physiology derived from the phenotype.</param>
    /// <param name="maturityAgeTicks">The initialized maturity age in ticks.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="bodyPlan"/> or <paramref name="physiology"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="maturityAgeTicks"/> is not greater than zero.</exception>
    public MorphogenesisResult(BodyPlan bodyPlan, PhysiologyComponent physiology, int maturityAgeTicks)
    {
        BodyPlan = bodyPlan ?? throw new ArgumentNullException(nameof(bodyPlan));
        Physiology = physiology ?? throw new ArgumentNullException(nameof(physiology));
        if (maturityAgeTicks <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maturityAgeTicks), "The maturity age must be greater than zero.");
        }

        MaturityAgeTicks = maturityAgeTicks;
    }

    /// <summary>
    /// Gets the generated phenotype description.
    /// </summary>
    public BodyPlan BodyPlan { get; }

    /// <summary>
    /// Gets the initialized physiology derived from the phenotype.
    /// </summary>
    public PhysiologyComponent Physiology { get; }

    /// <summary>
    /// Gets the initialized maturity age in ticks.
    /// </summary>
    public int MaturityAgeTicks { get; }
}

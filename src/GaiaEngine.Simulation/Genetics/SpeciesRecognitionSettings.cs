using System;

namespace GaiaEngine.Simulation.Genetics;

/// <summary>
/// Stores the immutable deterministic settings used by the species recognition subsystem.
/// </summary>
public sealed record SpeciesRecognitionSettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SpeciesRecognitionSettings"/> class.
    /// </summary>
    /// <param name="evaluationFrequency">The tick frequency used to evaluate species recognition.</param>
    /// <param name="minimumGenomeSimilarity">The minimum genome similarity required to remain within the current species.</param>
    /// <param name="minimumTraitSimilarity">The minimum trait similarity required to remain within the current species.</param>
    /// <param name="minimumMorphologySimilarity">The minimum morphology similarity required to remain within the current species.</param>
    /// <param name="minimumReproductiveCompatibility">The minimum reproductive compatibility required to remain within the current species.</param>
    /// <param name="requiredFailedMetricCount">The number of failed metrics required before a new species can be recognized.</param>
    public SpeciesRecognitionSettings(
        int evaluationFrequency,
        int minimumGenomeSimilarity,
        int minimumTraitSimilarity,
        int minimumMorphologySimilarity,
        int minimumReproductiveCompatibility,
        int requiredFailedMetricCount)
    {
        if (evaluationFrequency <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(evaluationFrequency), "The evaluation frequency must be greater than zero.");
        }

        MinimumGenomeSimilarity = ValidateNormalized(minimumGenomeSimilarity, nameof(minimumGenomeSimilarity));
        MinimumTraitSimilarity = ValidateNormalized(minimumTraitSimilarity, nameof(minimumTraitSimilarity));
        MinimumMorphologySimilarity = ValidateNormalized(minimumMorphologySimilarity, nameof(minimumMorphologySimilarity));
        MinimumReproductiveCompatibility = ValidateNormalized(minimumReproductiveCompatibility, nameof(minimumReproductiveCompatibility));
        if (requiredFailedMetricCount < 1 || requiredFailedMetricCount > 4)
        {
            throw new ArgumentOutOfRangeException(nameof(requiredFailedMetricCount), "The required failed metric count must be between 1 and 4.");
        }

        EvaluationFrequency = evaluationFrequency;
        RequiredFailedMetricCount = requiredFailedMetricCount;
    }

    /// <summary>
    /// Gets the tick frequency used to evaluate species recognition.
    /// </summary>
    public int EvaluationFrequency { get; }

    /// <summary>
    /// Gets the minimum genome similarity required to remain within the current species.
    /// </summary>
    public int MinimumGenomeSimilarity { get; }

    /// <summary>
    /// Gets the minimum trait similarity required to remain within the current species.
    /// </summary>
    public int MinimumTraitSimilarity { get; }

    /// <summary>
    /// Gets the minimum morphology similarity required to remain within the current species.
    /// </summary>
    public int MinimumMorphologySimilarity { get; }

    /// <summary>
    /// Gets the minimum reproductive compatibility required to remain within the current species.
    /// </summary>
    public int MinimumReproductiveCompatibility { get; }

    /// <summary>
    /// Gets the number of failed metrics required before a new species can be recognized.
    /// </summary>
    public int RequiredFailedMetricCount { get; }

    private static int ValidateNormalized(int value, string parameterName)
    {
        if (value < 0 || value > 1000)
        {
            throw new ArgumentOutOfRangeException(parameterName, "Species recognition thresholds must be between 0 and 1000.");
        }

        return value;
    }
}

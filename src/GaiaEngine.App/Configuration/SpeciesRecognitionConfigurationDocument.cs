namespace GaiaEngine.App.Configuration;

/// <summary>
/// Represents the raw JSON document used to deserialize species recognition configuration.
/// </summary>
internal sealed class SpeciesRecognitionConfigurationDocument
{
    /// <summary>
    /// Gets or sets the evaluation frequency in ticks.
    /// </summary>
    public int EvaluationFrequency { get; set; } = SpeciesRecognitionConfiguration.Default.EvaluationFrequency;

    /// <summary>
    /// Gets or sets the minimum genome similarity threshold.
    /// </summary>
    public int MinimumGenomeSimilarity { get; set; } = SpeciesRecognitionConfiguration.Default.MinimumGenomeSimilarity;

    /// <summary>
    /// Gets or sets the minimum trait similarity threshold.
    /// </summary>
    public int MinimumTraitSimilarity { get; set; } = SpeciesRecognitionConfiguration.Default.MinimumTraitSimilarity;

    /// <summary>
    /// Gets or sets the minimum morphology similarity threshold.
    /// </summary>
    public int MinimumMorphologySimilarity { get; set; } = SpeciesRecognitionConfiguration.Default.MinimumMorphologySimilarity;

    /// <summary>
    /// Gets or sets the minimum reproductive compatibility threshold.
    /// </summary>
    public int MinimumReproductiveCompatibility { get; set; } = SpeciesRecognitionConfiguration.Default.MinimumReproductiveCompatibility;

    /// <summary>
    /// Gets or sets the required failed metric count.
    /// </summary>
    public int RequiredFailedMetricCount { get; set; } = SpeciesRecognitionConfiguration.Default.RequiredFailedMetricCount;
}

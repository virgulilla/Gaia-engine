namespace GaiaEngine.Serialization.Profiles.Documents;

/// <summary>
/// Represents one serialized encyclopedia statistic.
/// </summary>
internal sealed class EncyclopediaStatisticDocument
{
    /// <summary>
    /// Gets or sets the statistic key.
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the statistic value.
    /// </summary>
    public int Value { get; set; }
}

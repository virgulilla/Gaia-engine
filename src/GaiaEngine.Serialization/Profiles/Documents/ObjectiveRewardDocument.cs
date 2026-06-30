using System.Collections.Generic;

namespace GaiaEngine.Serialization.Profiles.Documents;

/// <summary>
/// Represents the serialized objective reward section.
/// </summary>
internal sealed class ObjectiveRewardDocument
{
    /// <summary>
    /// Gets or sets the granted experience.
    /// </summary>
    public int Experience { get; set; }

    /// <summary>
    /// Gets or sets the granted unlock identifiers.
    /// </summary>
    public List<string> Unlocks { get; set; } = new();
}

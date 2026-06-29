namespace GaiaEngine.Serialization.SaveGames.Documents;

/// <summary>
/// Represents the serialized organism section stored in a save game document.
/// </summary>
internal sealed class OrganismDocument
{
    /// <summary>
    /// Gets or sets the organism identifier.
    /// </summary>
    public string OrganismId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the species identifier.
    /// </summary>
    public string SpeciesId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the genome identifier.
    /// </summary>
    public string GenomeId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current chunk identifier.
    /// </summary>
    public string CurrentChunkId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the metabolism rate.
    /// </summary>
    public int MetabolismRate { get; set; }

    /// <summary>
    /// Gets or sets the growth rate.
    /// </summary>
    public int GrowthRate { get; set; }

    /// <summary>
    /// Gets or sets the lifespan in ticks.
    /// </summary>
    public int LifespanTicks { get; set; }

    /// <summary>
    /// Gets or sets the water efficiency.
    /// </summary>
    public int WaterEfficiency { get; set; }

    /// <summary>
    /// Gets or sets the digestion efficiency.
    /// </summary>
    public int DigestionEfficiency { get; set; }

    /// <summary>
    /// Gets or sets the body temperature value.
    /// </summary>
    public int BodyTemperature { get; set; }

    /// <summary>
    /// Gets or sets the hunger urgency.
    /// </summary>
    public int Hunger { get; set; }

    /// <summary>
    /// Gets or sets the hydration urgency.
    /// </summary>
    public int Hydration { get; set; }

    /// <summary>
    /// Gets or sets the rest urgency.
    /// </summary>
    public int Rest { get; set; }

    /// <summary>
    /// Gets or sets the reproduction urgency.
    /// </summary>
    public int ReproductionUrge { get; set; }

    /// <summary>
    /// Gets or sets the birth tick.
    /// </summary>
    public long BirthTick { get; set; }

    /// <summary>
    /// Gets or sets the age in ticks.
    /// </summary>
    public long AgeTicks { get; set; }

    /// <summary>
    /// Gets or sets the maturity age in ticks.
    /// </summary>
    public long MaturityAgeTicks { get; set; }

    /// <summary>
    /// Gets or sets the lifecycle stage.
    /// </summary>
    public string Stage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the organism is alive.
    /// </summary>
    public bool IsAlive { get; set; }

    /// <summary>
    /// Gets or sets the current health value.
    /// </summary>
    public int CurrentHealth { get; set; }

    /// <summary>
    /// Gets or sets the maximum health value.
    /// </summary>
    public int MaximumHealth { get; set; }
}

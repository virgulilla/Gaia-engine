using System;

namespace GaiaEngine.Simulation.AI.Memory;

/// <summary>
/// Defines the configurable deterministic settings used by the memory system.
/// </summary>
public sealed class MemorySettings
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MemorySettings"/> class.
    /// </summary>
    public MemorySettings(
        int organismCapacity,
        int resourceCapacity,
        int locationCapacity,
        int hazardCapacity,
        int eventCapacity,
        int organismExpirationTicks,
        int resourceExpirationTicks,
        int locationExpirationTicks,
        int hazardExpirationTicks,
        int eventExpirationTicks,
        int organismDecayPerTick,
        int resourceDecayPerTick,
        int locationDecayPerTick,
        int hazardDecayPerTick,
        int eventDecayPerTick,
        int confidenceRefreshBonus,
        int minimumConfidence)
    {
        ValidatePositive(organismCapacity, nameof(organismCapacity));
        ValidatePositive(resourceCapacity, nameof(resourceCapacity));
        ValidatePositive(locationCapacity, nameof(locationCapacity));
        ValidatePositive(hazardCapacity, nameof(hazardCapacity));
        ValidatePositive(eventCapacity, nameof(eventCapacity));
        ValidatePositive(organismExpirationTicks, nameof(organismExpirationTicks));
        ValidatePositive(resourceExpirationTicks, nameof(resourceExpirationTicks));
        ValidatePositive(locationExpirationTicks, nameof(locationExpirationTicks));
        ValidatePositive(hazardExpirationTicks, nameof(hazardExpirationTicks));
        ValidatePositive(eventExpirationTicks, nameof(eventExpirationTicks));
        ValidateNonNegative(organismDecayPerTick, nameof(organismDecayPerTick));
        ValidateNonNegative(resourceDecayPerTick, nameof(resourceDecayPerTick));
        ValidateNonNegative(locationDecayPerTick, nameof(locationDecayPerTick));
        ValidateNonNegative(hazardDecayPerTick, nameof(hazardDecayPerTick));
        ValidateNonNegative(eventDecayPerTick, nameof(eventDecayPerTick));

        if (confidenceRefreshBonus < 0 || confidenceRefreshBonus > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(confidenceRefreshBonus), "The confidence refresh bonus must be between 0 and 1000.");
        }

        if (minimumConfidence < 0 || minimumConfidence > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumConfidence), "The minimum confidence must be between 0 and 1000.");
        }

        OrganismCapacity = organismCapacity;
        ResourceCapacity = resourceCapacity;
        LocationCapacity = locationCapacity;
        HazardCapacity = hazardCapacity;
        EventCapacity = eventCapacity;
        OrganismExpirationTicks = organismExpirationTicks;
        ResourceExpirationTicks = resourceExpirationTicks;
        LocationExpirationTicks = locationExpirationTicks;
        HazardExpirationTicks = hazardExpirationTicks;
        EventExpirationTicks = eventExpirationTicks;
        OrganismDecayPerTick = organismDecayPerTick;
        ResourceDecayPerTick = resourceDecayPerTick;
        LocationDecayPerTick = locationDecayPerTick;
        HazardDecayPerTick = hazardDecayPerTick;
        EventDecayPerTick = eventDecayPerTick;
        ConfidenceRefreshBonus = confidenceRefreshBonus;
        MinimumConfidence = minimumConfidence;
    }

    /// <summary>
    /// Gets a shared default memory configuration.
    /// </summary>
    public static MemorySettings Default { get; } = new(
        organismCapacity: 16,
        resourceCapacity: 16,
        locationCapacity: 8,
        hazardCapacity: 8,
        eventCapacity: 8,
        organismExpirationTicks: 600,
        resourceExpirationTicks: 900,
        locationExpirationTicks: 1200,
        hazardExpirationTicks: 1200,
        eventExpirationTicks: 600,
        organismDecayPerTick: 2,
        resourceDecayPerTick: 1,
        locationDecayPerTick: 1,
        hazardDecayPerTick: 2,
        eventDecayPerTick: 3,
        confidenceRefreshBonus: 100,
        minimumConfidence: 150);

    /// <summary>
    /// Gets the maximum number of organism memories stored per organism.
    /// </summary>
    public int OrganismCapacity { get; }

    /// <summary>
    /// Gets the maximum number of resource memories stored per organism.
    /// </summary>
    public int ResourceCapacity { get; }

    /// <summary>
    /// Gets the maximum number of location memories stored per organism.
    /// </summary>
    public int LocationCapacity { get; }

    /// <summary>
    /// Gets the maximum number of hazard memories stored per organism.
    /// </summary>
    public int HazardCapacity { get; }

    /// <summary>
    /// Gets the maximum number of event memories stored per organism.
    /// </summary>
    public int EventCapacity { get; }

    /// <summary>
    /// Gets the expiration time, in ticks, for organism memories.
    /// </summary>
    public int OrganismExpirationTicks { get; }

    /// <summary>
    /// Gets the expiration time, in ticks, for resource memories.
    /// </summary>
    public int ResourceExpirationTicks { get; }

    /// <summary>
    /// Gets the expiration time, in ticks, for location memories.
    /// </summary>
    public int LocationExpirationTicks { get; }

    /// <summary>
    /// Gets the expiration time, in ticks, for hazard memories.
    /// </summary>
    public int HazardExpirationTicks { get; }

    /// <summary>
    /// Gets the expiration time, in ticks, for event memories.
    /// </summary>
    public int EventExpirationTicks { get; }

    /// <summary>
    /// Gets the confidence decay applied per tick to organism memories.
    /// </summary>
    public int OrganismDecayPerTick { get; }

    /// <summary>
    /// Gets the confidence decay applied per tick to resource memories.
    /// </summary>
    public int ResourceDecayPerTick { get; }

    /// <summary>
    /// Gets the confidence decay applied per tick to location memories.
    /// </summary>
    public int LocationDecayPerTick { get; }

    /// <summary>
    /// Gets the confidence decay applied per tick to hazard memories.
    /// </summary>
    public int HazardDecayPerTick { get; }

    /// <summary>
    /// Gets the confidence decay applied per tick to event memories.
    /// </summary>
    public int EventDecayPerTick { get; }

    /// <summary>
    /// Gets the confidence bonus applied when an existing memory is refreshed.
    /// </summary>
    public int ConfidenceRefreshBonus { get; }

    /// <summary>
    /// Gets the minimum confidence required to keep a memory entry.
    /// </summary>
    public int MinimumConfidence { get; }

    private static void ValidatePositive(int value, string parameterName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, "The value must be greater than zero.");
        }
    }

    private static void ValidateNonNegative(int value, string parameterName)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(parameterName, "The value must be zero or greater.");
        }
    }
}

using System;
using GaiaEngine.Domain.Identifiers;

namespace GaiaEngine.Domain.World;

/// <summary>
/// Represents the passive stored state of one chunk resource.
/// </summary>
public sealed record ResourceState
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ResourceState"/> class.
    /// </summary>
    /// <param name="resourceId">The deterministic resource identifier.</param>
    /// <param name="type">The deterministic resource type.</param>
    /// <param name="category">The deterministic resource category.</param>
    /// <param name="currentAmount">The current stored amount.</param>
    /// <param name="maximumCapacity">The maximum supported amount.</param>
    /// <param name="regenerationRate">The deterministic regeneration rate.</param>
    /// <param name="quality">The deterministic quality value in the inclusive range [0, 100].</param>
    /// <param name="availability">The deterministic availability value in the inclusive range [0, 1000].</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when amounts are negative, when current amount exceeds capacity,
    /// when quality is outside [0, 100], or when availability is outside [0, 1000].
    /// </exception>
    public ResourceState(
        ResourceId resourceId,
        ResourceType type,
        ResourceCategory category,
        int currentAmount,
        int maximumCapacity,
        int regenerationRate,
        int quality,
        int availability)
    {
        if (currentAmount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(currentAmount), "The current resource amount must be zero or greater.");
        }

        if (maximumCapacity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumCapacity), "The maximum resource capacity must be greater than zero.");
        }

        if (currentAmount > maximumCapacity)
        {
            throw new ArgumentOutOfRangeException(nameof(currentAmount), "The current resource amount cannot exceed the maximum capacity.");
        }

        if (regenerationRate < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(regenerationRate), "The regeneration rate must be zero or greater.");
        }

        if (quality < 0 || quality > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(quality), "The resource quality must be between 0 and 100.");
        }

        if (availability < 0 || availability > 1000)
        {
            throw new ArgumentOutOfRangeException(nameof(availability), "The resource availability must be between 0 and 1000.");
        }

        ResourceId = resourceId;
        Type = type;
        Category = category;
        CurrentAmount = currentAmount;
        MaximumCapacity = maximumCapacity;
        RegenerationRate = regenerationRate;
        Quality = quality;
        Availability = availability;
    }

    /// <summary>
    /// Gets the deterministic resource identifier.
    /// </summary>
    public ResourceId ResourceId { get; }

    /// <summary>
    /// Gets the deterministic resource type.
    /// </summary>
    public ResourceType Type { get; }

    /// <summary>
    /// Gets the deterministic resource category.
    /// </summary>
    public ResourceCategory Category { get; }

    /// <summary>
    /// Gets the current stored amount.
    /// </summary>
    public int CurrentAmount { get; }

    /// <summary>
    /// Gets the maximum supported amount.
    /// </summary>
    public int MaximumCapacity { get; }

    /// <summary>
    /// Gets the deterministic regeneration rate.
    /// </summary>
    public int RegenerationRate { get; }

    /// <summary>
    /// Gets the deterministic quality value.
    /// </summary>
    public int Quality { get; }

    /// <summary>
    /// Gets the deterministic availability value scaled to the inclusive range [0, 1000],
    /// which represents the continuous specification range [0.0, 1.0].
    /// </summary>
    public int Availability { get; }
}

namespace GaiaEngine.Engine.Events;

/// <summary>
/// Defines the deterministic event processing priorities.
/// </summary>
public enum EventPriority
{
    /// <summary>
    /// Represents low-priority events.
    /// </summary>
    Low = 0,

    /// <summary>
    /// Represents normal-priority events.
    /// </summary>
    Normal = 1,

    /// <summary>
    /// Represents high-priority events.
    /// </summary>
    High = 2,

    /// <summary>
    /// Represents critical-priority events.
    /// </summary>
    Critical = 3,
}

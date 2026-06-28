namespace GaiaEngine.Engine.Events;

/// <summary>
/// Defines the supported event categories used by the engine.
/// </summary>
public enum EventCategory
{
    /// <summary>
    /// Identifies simulation-level events.
    /// </summary>
    Simulation,

    /// <summary>
    /// Identifies world-level events.
    /// </summary>
    World,

    /// <summary>
    /// Identifies organism-level events.
    /// </summary>
    Organism,

    /// <summary>
    /// Identifies AI-level events.
    /// </summary>
    AI,

    /// <summary>
    /// Identifies gameplay-level events.
    /// </summary>
    Gameplay,

    /// <summary>
    /// Identifies UI-level events.
    /// </summary>
    UI,

    /// <summary>
    /// Identifies audio-level events.
    /// </summary>
    Audio,

    /// <summary>
    /// Identifies engine system events.
    /// </summary>
    System,
}

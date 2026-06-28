using System.Collections.Generic;

namespace GaiaEngine.Engine.Events;

/// <summary>
/// Represents the result of a deterministic event dispatch pass.
/// </summary>
public sealed record EventDispatchResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EventDispatchResult"/> class.
    /// </summary>
    /// <param name="processedEventCount">The number of processed events.</param>
    /// <param name="failureCount">The number of subscriber failures.</param>
    /// <param name="failures">The captured subscriber failures.</param>
    public EventDispatchResult(int processedEventCount, int failureCount, IReadOnlyList<EventDispatchFailure> failures)
    {
        ProcessedEventCount = processedEventCount;
        FailureCount = failureCount;
        Failures = failures;
    }

    /// <summary>
    /// Gets the number of processed events.
    /// </summary>
    public int ProcessedEventCount { get; }

    /// <summary>
    /// Gets the number of subscriber failures.
    /// </summary>
    public int FailureCount { get; }

    /// <summary>
    /// Gets the captured subscriber failures.
    /// </summary>
    public IReadOnlyList<EventDispatchFailure> Failures { get; }
}

using System;

namespace GaiaEngine.Simulation.Actions;

/// <summary>
/// Represents the immutable target metadata referenced by one action request.
/// </summary>
public readonly record struct SimulationActionTarget
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SimulationActionTarget"/> struct.
    /// </summary>
    /// <param name="kind">The target kind.</param>
    /// <param name="targetId">The serialized target identifier.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="targetId"/> is empty.</exception>
    public SimulationActionTarget(ActionTargetKind kind, string targetId)
    {
        if (string.IsNullOrWhiteSpace(targetId))
        {
            throw new ArgumentException("The action target identifier must contain a value.", nameof(targetId));
        }

        Kind = kind;
        TargetId = targetId;
    }

    /// <summary>
    /// Gets the target kind.
    /// </summary>
    public ActionTargetKind Kind { get; }

    /// <summary>
    /// Gets the serialized target identifier.
    /// </summary>
    public string TargetId { get; }
}
